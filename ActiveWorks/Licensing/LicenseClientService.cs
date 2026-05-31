using ActiveWorks.Properties;
using Interfaces.Licensing;
using Logger;
using System;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ActiveWorks.Licensing
{
    internal sealed class LicenseClientService
    {
        private readonly string _machineId;
        private readonly LicenseTokenValidator _validator;
        private readonly LicenseStorage _storage;

        public LicenseClientService()
        {
            _machineId = LicenseMachine.GetMachineId();
            _validator = new LicenseTokenValidator(Settings.Default.LicensePublicKeyXml, _machineId);
            _storage = new LicenseStorage();
        }

        public bool IsConfigured =>
#if DEBUG
            Settings.Default.LicenseServerEnabled &&
#endif
            !string.IsNullOrWhiteSpace(Settings.Default.LicenseServerUrl);

        public LicenseTokenState CurrentToken => _validator.Validate(_storage.Load().LicenseToken);

        public string StoredLicenseKey => _storage.Load().LicenseKey;

        public bool IsFeatureEnabled(LicenseFeature feature)
        {
            if (!IsConfigured)
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }

            var state = CurrentToken;
            if (!state.IsValid)
            {
                return false;
            }

            var features = state.Payload?.Features;
            if (features == null)
            {
                return false;
            }

            switch (feature)
            {
                case LicenseFeature.Updates:
                    return features.Updates;
                case LicenseFeature.ExportPdf:
                    return features.ExportPdf;
                case LicenseFeature.AdvancedReports:
                    return features.AdvancedReports;
                case LicenseFeature.Sync:
                    return features.Sync;
                case LicenseFeature.ThreeDPreview:
                    return features.ThreeDPreview;
                default:
                    return false;
            }
        }

        public async Task<LicenseTokenState> GetUsableTokenAsync()
        {
            if (!IsConfigured)
            {
                return new LicenseTokenState(true, new LicenseTokenPayload
                {
                    MachineId = _machineId,
                    Status = "active",
                    Features = new LicenseFeatureSet { Updates = true, ExportPdf = true, AdvancedReports = true, Sync = true, ThreeDPreview = true, MaxProjects = int.MaxValue }
                }, null, string.Empty);
            }

            var current = CurrentToken;
            if (current.IsValid)
            {
                return current;
            }

            try
            {
                var stored = _storage.Load();
                if (!string.IsNullOrWhiteSpace(stored.LicenseToken))
                {
                    return await RefreshAsync().ConfigureAwait(false);
                }

                if (!string.IsNullOrWhiteSpace(stored.LicenseKey))
                {
                    return await ActivateAsync(stored.LicenseKey).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Licensing", "GetUsableTokenAsync", ex.ToString());
            }

            return current;
        }

        public async Task<LicenseTokenState> ActivateAsync(string licenseKey)
        {
            var request = new LicenseActivateRequest
            {
                LicenseKey = licenseKey,
                MachineId = _machineId,
                AppVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString()
            };

            return await PostTokenRequestAsync("api/license/activate", request, licenseKey).ConfigureAwait(false);
        }

        public async Task<LicenseTokenState> RefreshAsync()
        {
            var request = new LicenseRefreshRequest
            {
                Token = _storage.Load().LicenseToken,
                MachineId = _machineId,
                AppVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString()
            };

            return await PostTokenRequestAsync("api/license/refresh", request, _storage.Load().LicenseKey).ConfigureAwait(false);
        }

        private async Task<LicenseTokenState> PostTokenRequestAsync<T>(string relativePath, T request, string licenseKey)
        {
            using (var client = new HttpClient())
            {
                var uri = new Uri(new Uri(EnsureTrailingSlash(Settings.Default.LicenseServerUrl)), relativePath);
                var json = LicenseJsonSerializer.Serialize(request);
                using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    var response = await client.PostAsync(uri, content).ConfigureAwait(false);
                    var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new InvalidOperationException(
                            $"License server returned {(int)response.StatusCode} ({response.ReasonPhrase}). {BuildErrorMessage(responseJson)}");
                    }

                    var tokenResponse = LicenseJsonSerializer.Deserialize<LicenseTokenResponse>(responseJson);
                    var state = _validator.Validate(tokenResponse.Token);
                    if (state.IsValid)
                    {
                        _storage.Save(new LicenseStorageState
                        {
                            LicenseKey = licenseKey,
                            LicenseToken = tokenResponse.Token
                        });
                    }

                    return state;
                }
            }
        }

        private static string EnsureTrailingSlash(string value)
        {
            return value.EndsWith("/", StringComparison.Ordinal) ? value : value + "/";
        }

        private static string BuildErrorMessage(string responseText)
        {
            var problem = TryDeserializeProblem(responseText);
            if (!string.IsNullOrWhiteSpace(problem?.Detail))
            {
                return problem.Detail;
            }

            if (!string.IsNullOrWhiteSpace(problem?.Title))
            {
                return problem.Title;
            }

            if (LooksLikeHtml(responseText))
            {
                return "License server returned an HTML error page. Check the server logs for the request details.";
            }

            return string.IsNullOrWhiteSpace(responseText)
                ? "License server returned an empty error response."
                : responseText.Trim();
        }

        private static LicenseProblemResponse TryDeserializeProblem(string responseText)
        {
            if (string.IsNullOrWhiteSpace(responseText))
            {
                return null;
            }

            try
            {
                return LicenseJsonSerializer.Deserialize<LicenseProblemResponse>(responseText);
            }
            catch
            {
                return null;
            }
        }

        private static bool LooksLikeHtml(string responseText)
        {
            var trimmed = responseText?.TrimStart();
            return trimmed?.StartsWith("<!DOCTYPE html", StringComparison.OrdinalIgnoreCase) == true ||
                   trimmed?.StartsWith("<html", StringComparison.OrdinalIgnoreCase) == true;
        }
    }

    [System.Runtime.Serialization.DataContract]
    internal sealed class LicenseActivateRequest
    {
        [System.Runtime.Serialization.DataMember(Name = "licenseKey")]
        public string LicenseKey { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "machineId")]
        public string MachineId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "appVersion")]
        public string AppVersion { get; set; }
    }

    [System.Runtime.Serialization.DataContract]
    internal sealed class LicenseRefreshRequest
    {
        [System.Runtime.Serialization.DataMember(Name = "token")]
        public string Token { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "machineId")]
        public string MachineId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "appVersion")]
        public string AppVersion { get; set; }
    }

    [System.Runtime.Serialization.DataContract]
    internal sealed class LicenseTokenResponse
    {
        [System.Runtime.Serialization.DataMember(Name = "token")]
        public string Token { get; set; }
    }

    [DataContract]
    internal sealed class LicenseProblemResponse
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "detail")]
        public string Detail { get; set; }
    }
}
