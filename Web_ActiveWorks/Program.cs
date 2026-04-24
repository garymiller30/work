using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
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

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
        }
        app.UseStaticFiles();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseAntiforgery();

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
}
