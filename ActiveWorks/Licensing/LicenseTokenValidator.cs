using System;
using System.Security.Cryptography;
using System.Text;

namespace ActiveWorks.Licensing
{
    internal sealed class LicenseTokenValidator
    {
        private readonly string _publicKeyXml;
        private readonly string _machineId;

        public LicenseTokenValidator(string publicKeyXml, string machineId)
        {
            _publicKeyXml = publicKeyXml;
            _machineId = machineId;
        }

        public LicenseTokenState Validate(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return new LicenseTokenState(false, null, token, "License token is empty.");
            }

            if (string.IsNullOrWhiteSpace(_publicKeyXml))
            {
                return new LicenseTokenState(false, null, token, "License public key is not configured.");
            }

            var parts = token.Split('.');
            if (parts.Length != 2)
            {
                return new LicenseTokenState(false, null, token, "License token has invalid format.");
            }

            try
            {
                var signedBytes = Encoding.UTF8.GetBytes(parts[0]);
                var signature = Base64Url.Decode(parts[1]);

                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(_publicKeyXml);
                    if (!rsa.VerifyData(signedBytes, CryptoConfig.MapNameToOID("SHA256"), signature))
                    {
                        return new LicenseTokenState(false, null, token, "License token signature is invalid.");
                    }
                }

                var payloadJson = Encoding.UTF8.GetString(Base64Url.Decode(parts[0]));
                var payload = LicenseJsonSerializer.Deserialize<LicenseTokenPayload>(payloadJson);

                if (!string.Equals(payload.MachineId, _machineId, StringComparison.OrdinalIgnoreCase))
                {
                    return new LicenseTokenState(false, payload, token, "License token belongs to another machine.");
                }

                DateTime expiresAtUtc;
                if (!DateTime.TryParse(payload.ExpiresAtUtc, null, System.Globalization.DateTimeStyles.RoundtripKind, out expiresAtUtc))
                {
                    return new LicenseTokenState(false, payload, token, "License token expiration is invalid.");
                }

                if (expiresAtUtc <= DateTime.UtcNow)
                {
                    return new LicenseTokenState(false, payload, token, "License token is expired.");
                }

                if (!string.Equals(payload.Status, "active", StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(payload.Status, "grace", StringComparison.OrdinalIgnoreCase))
                {
                    return new LicenseTokenState(false, payload, token, "License subscription is not active.");
                }

                return new LicenseTokenState(true, payload, token, string.Empty);
            }
            catch (Exception ex)
            {
                return new LicenseTokenState(false, null, token, ex.Message);
            }
        }
    }
}
