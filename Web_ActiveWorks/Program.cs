using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.Security.Claims;
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
                    [FromBody] LicenseActivateRequest request,
                    LicenseStore store,
                    LicenseTokenService tokenService) =>
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

                    return Results.Ok(new LicenseTokenResponse
                    {
                        Token = tokenService.CreateToken(license, request.MachineId)
                    });
                })
            .DisableAntiforgery();

        app.MapPost(
                "/api/license/refresh",
                async Task<IResult> (
                    [FromBody] LicenseRefreshRequest request,
                    LicenseStore store,
                    LicenseTokenService tokenService) =>
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

                    return Results.Ok(new LicenseTokenResponse
                    {
                        Token = tokenService.CreateToken(license, request.MachineId)
                    });
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
                var fullPath = Path.GetFullPath(Path.Combine(updateRoot, path.Replace('/', Path.DirectorySeparatorChar)));
                if (!fullPath.StartsWith(Path.GetFullPath(updateRoot), StringComparison.OrdinalIgnoreCase) || !System.IO.File.Exists(fullPath))
                {
                    return Results.NotFound();
                }

                return Results.File(fullPath, "application/octet-stream", Path.GetFileName(fullPath));
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

    private static string ResolvePath(string contentRootPath, string path)
    {
        return Path.IsPathRooted(path)
            ? path
            : Path.GetFullPath(Path.Combine(contentRootPath, path));
    }
}
