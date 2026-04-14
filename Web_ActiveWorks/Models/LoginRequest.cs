namespace Web_ActiveWorks.Models;

public sealed class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ReturnUrl { get; set; } = "/";

    public static string NormalizeReturnUrl(string? returnUrl)
    {
        if (string.IsNullOrWhiteSpace(returnUrl) || !returnUrl.StartsWith('/'))
        {
            return "/";
        }

        return returnUrl;
    }

    public static string BuildFailureUrl(string? returnUrl)
    {
        var normalizedReturnUrl = Uri.EscapeDataString(NormalizeReturnUrl(returnUrl));
        return $"/login?error=1&returnUrl={normalizedReturnUrl}";
    }
}
