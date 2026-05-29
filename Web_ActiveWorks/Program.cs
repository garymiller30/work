using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.Text.Json;
using System.Security.Claims;
using System.Security.Cryptography;
using Web_ActiveWorks.Components;
using Web_ActiveWorks.Models;
using Web_ActiveWorks.Services;

namespace Web_ActiveWorks;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/login";
                options.AccessDeniedPath = "/access-denied";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromHours(12);
            });
        builder.Services.AddAuthorization();
        builder.Services.AddCascadingAuthenticationState();

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        builder.Services.AddScoped<CurrentUserService>();
        builder.Services.AddSingleton<WebAccessSettingsStore>();
        builder.Services.AddSingleton<MongoProfileDataService>();
        builder.Services.AddSingleton<LicenseStore>();
        builder.Services.AddSingleton<LicenseTokenService>();
        builder.Services.AddSingleton<PluginCatalogService>();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
        }
        app.UseStaticFiles();
        app.UseStaticFiles(new StaticFileOptions
        {
            RequestPath = "/aw",
            FileProvider = new PhysicalFileProvider(
                Path.Combine(app.Environment.WebRootPath, "aw")
            ),
            ServeUnknownFileTypes = true
        });
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseAntiforgery();

        app.MapPost(
                "/api/license/activate",
                async Task<IResult> (
                    HttpContext httpContext,
                    [FromBody] LicenseActivateRequest request,
                    LicenseStore store,
                    LicenseTokenService tokenService,
                    ILogger<Program> logger) =>
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(request.LicenseKey) || string.IsNullOrWhiteSpace(request.MachineId))
                        {
                            return Results.BadRequest("License key and machine id are required.");
                        }

                        var license = await store.FindByKeyAsync(request.LicenseKey);
                        if (license is null)
                        {
                            return Results.NotFound("License was not found.");
                        }

                        if (!await store.RegisterMachineAsync(license, request.MachineId))
                        {
                            return Results.Conflict("Device limit is reached for this license.");
                        }

                        return CreateLicenseTokenResult(license, request.MachineId, tokenService, logger);
                    }
                    catch (Exception ex) when (IsLicenseStoreException(ex))
                    {
                        logger.LogError(ex, "License activation failed while reading or writing the license store.");
                        return Results.Problem(
                            detail: "License server could not update the license store. Check write permissions for Licensing:StorePath.",
                            statusCode: StatusCodes.Status500InternalServerError);
                    }
                })
            .DisableAntiforgery();

        app.MapPost(
                "/api/license/refresh",
                async Task<IResult> (
                    HttpContext httpContext,
                    [FromBody] LicenseRefreshRequest request,
                    LicenseStore store,
                    LicenseTokenService tokenService,
                    ILogger<Program> logger) =>
                {
                    try
                    {
                        var payload = tokenService.ValidateToken(request.Token);
                        if (payload is null || !string.Equals(payload.MachineId, request.MachineId, StringComparison.OrdinalIgnoreCase))
                        {
                            return Results.Unauthorized();
                        }

                        var license = await store.FindByIdAsync(payload.LicenseId);
                        if (license is null)
                        {
                            return Results.NotFound("License was not found.");
                        }

                        return CreateLicenseTokenResult(license, request.MachineId, tokenService, logger);
                    }
                    catch (Exception ex) when (IsLicenseStoreException(ex))
                    {
                        logger.LogError(ex, "License refresh failed while reading the license store.");
                        return Results.Problem(
                            detail: "License server could not read the license store. Check Licensing:StorePath.",
                            statusCode: StatusCodes.Status500InternalServerError);
                    }
                })
            .DisableAntiforgery();

        app.MapGet(
            "/api/updates/manifest",
            (HttpContext httpContext, IWebHostEnvironment environment, IConfiguration configuration, LicenseTokenService tokenService) =>
            {
                if (!TryAuthorizeUpdates(httpContext, tokenService))
                {
                    return Results.StatusCode(StatusCodes.Status403Forbidden);
                }

                var options = configuration.GetSection("Licensing").Get<LicenseOptions>() ?? new LicenseOptions();
                var updateRoot = ResolvePath(environment.ContentRootPath, options.UpdateRootPath);
                var manifestPath = Path.Combine(updateRoot, options.ManifestFileName);
                return File.Exists(manifestPath)
                    ? Results.File(manifestPath, "application/json")
                    : Results.NotFound();
            });

        app.MapGet(
            "/api/updates/download/{**path}",
            (string path, HttpContext httpContext, IWebHostEnvironment environment, IConfiguration configuration, LicenseTokenService tokenService) =>
            {
                if (!TryAuthorizeUpdates(httpContext, tokenService))
                {
                    return Results.StatusCode(StatusCodes.Status403Forbidden);
                }

                var options = configuration.GetSection("Licensing").Get<LicenseOptions>() ?? new LicenseOptions();
                var updateRoot = ResolvePath(environment.ContentRootPath, options.UpdateRootPath);
                if (!TryResolveUpdateDownloadPath(updateRoot, path, out var fullPath) || !System.IO.File.Exists(fullPath))
                {
                    return Results.NotFound();
                }

                return Results.File(fullPath, "application/octet-stream", Path.GetFileName(fullPath));
            });

        app.MapGet(
            "/api/plugins",
            async (HttpContext httpContext, PluginCatalogService pluginCatalogService, LicenseTokenService tokenService) =>
            {
                if (!TryAuthorizeDownloads(httpContext, tokenService))
                {
                    return Results.StatusCode(StatusCodes.Status403Forbidden);
                }

                return Results.Ok(await pluginCatalogService.GetCatalogAsync());
            });

        app.MapGet(
            "/api/plugins/{id}/manifest",
            async (string id, HttpContext httpContext, PluginCatalogService pluginCatalogService, LicenseTokenService tokenService) =>
            {
                if (!TryAuthorizeDownloads(httpContext, tokenService))
                {
                    return Results.StatusCode(StatusCodes.Status403Forbidden);
                }

                var manifest = await pluginCatalogService.FindManifestAsync(id);
                return manifest is null
                    ? Results.NotFound()
                    : Results.Json(manifest, new JsonSerializerOptions(JsonSerializerDefaults.Web)
                    {
                        WriteIndented = true
                    });
            });

        app.MapGet(
            "/api/plugins/{id}/download",
            async (string id, HttpContext httpContext, PluginCatalogService pluginCatalogService, LicenseTokenService tokenService) =>
            {
                if (!TryAuthorizeDownloads(httpContext, tokenService))
                {
                    return Results.StatusCode(StatusCodes.Status403Forbidden);
                }

                var package = await pluginCatalogService.BuildDownloadPackageAsync(id);
                return package is null
                    ? Results.NotFound()
                    : Results.File(package.Content, "application/zip", package.FileName);
            });

        app.MapPost(
                "/account/login",
                async Task<IResult> (
                    HttpContext httpContext,
                    [FromForm] LoginRequest request,
                    WebAccessSettingsStore settingsStore) =>
                {
                    var user = await settingsStore.ValidateCredentialsAsync(request.Username, request.Password);
                    if (user is null)
                    {
                        return Results.LocalRedirect(LoginRequest.BuildFailureUrl(request.ReturnUrl));
                    }

                    var claims = new List<Claim>
                    {
                        new(ClaimTypes.Name, user.Username),
                        new("display_name", user.DisplayName ?? user.Username),
                        new("profile_key", user.ProfileKey)
                    };

                    if (user.IsAdmin)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, Roles.Admin));
                    }

                    var identity = new ClaimsIdentity(
                        claims,
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    await httpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(identity));

                    return Results.LocalRedirect(LoginRequest.NormalizeReturnUrl(request.ReturnUrl));
                })
            .DisableAntiforgery();

        app.MapPost(
                "/account/logout",
                async Task<IResult> (HttpContext httpContext) =>
                {
                    await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    return Results.LocalRedirect("/login");
                })
            .DisableAntiforgery();
        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }

    private static bool TryAuthorizeUpdates(HttpContext httpContext, LicenseTokenService tokenService)
    {
        var authorization = httpContext.Request.Headers.Authorization.ToString();
        const string bearerPrefix = "Bearer ";
        if (!authorization.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return tokenService.AllowsUpdates(authorization[bearerPrefix.Length..].Trim());
    }

    private static bool TryAuthorizeDownloads(HttpContext httpContext, LicenseTokenService tokenService)
    {
        if (httpContext.User.Identity?.IsAuthenticated == true)
        {
            return true;
        }

        return TryAuthorizeUpdates(httpContext, tokenService);
    }

    private static string ResolvePath(string contentRootPath, string path)
    {
        return Path.IsPathRooted(path)
            ? path
            : Path.GetFullPath(Path.Combine(contentRootPath, path));
    }

    private static bool TryResolveUpdateDownloadPath(string updateRoot, string requestPath, out string fullPath)
    {
        fullPath = string.Empty;
        if (string.IsNullOrWhiteSpace(requestPath))
        {
            return false;
        }

        var segments = requestPath
            .Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length == 0 || segments.Any(IsUnsafePathSegment))
        {
            return false;
        }

        var updateRootFullPath = Path.GetFullPath(updateRoot);
        var candidate = Path.GetFullPath(Path.Combine(new[] { updateRootFullPath }.Concat(segments).ToArray()));
        var rootPrefix = updateRootFullPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
        if (!candidate.StartsWith(rootPrefix, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        fullPath = candidate;
        return true;
    }

    private static bool IsUnsafePathSegment(string segment) =>
        string.IsNullOrWhiteSpace(segment) ||
        segment == "." ||
        segment == ".." ||
        segment.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0;

    private static bool IsLicenseStoreException(Exception ex) =>
        ex is IOException ||
        ex is UnauthorizedAccessException ||
        ex is JsonException;

    private static IResult CreateLicenseTokenResult(
        LicenseSubscription license,
        string machineId,
        LicenseTokenService tokenService,
        ILogger logger)
    {
        try
        {
            return Results.Ok(new LicenseTokenResponse
            {
                Token = tokenService.CreateToken(license, machineId)
            });
        }
        catch (InvalidOperationException ex)
        {
            logger.LogError(ex, "License token signing is not configured correctly.");
            return Results.Problem(
                detail: "License server could not sign the token. Check Licensing:PrivateKeyPem.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
        catch (CryptographicException ex)
        {
            logger.LogError(ex, "License private key is invalid.");
            return Results.Problem(
                detail: "License server private key is invalid. Check Licensing:PrivateKeyPem PEM format.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
