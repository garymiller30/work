using System.Security.Claims;
using Web_ActiveWorks.Models;

namespace Web_ActiveWorks.Services;

public sealed class CurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly WebAccessSettingsStore _settingsStore;

    public CurrentUserService(
        IHttpContextAccessor httpContextAccessor,
        WebAccessSettingsStore settingsStore)
    {
        _httpContextAccessor = httpContextAccessor;
        _settingsStore = settingsStore;
    }

    public ClaimsPrincipal? Principal => _httpContextAccessor.HttpContext?.User;

    public string? Username => Principal?.Identity?.Name;

    public async Task<WebUserDefinition?> GetCurrentUserAsync()
    {
        return await _settingsStore.FindUserAsync(Username);
    }

    public async Task<WebProfileDefinition?> GetCurrentProfileAsync()
    {
        var user = await GetCurrentUserAsync();
        return user is null ? null : await _settingsStore.FindProfileAsync(user.ProfileKey);
    }
}
