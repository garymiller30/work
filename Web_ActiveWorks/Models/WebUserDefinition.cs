namespace Web_ActiveWorks.Models;

public sealed class WebUserDefinition
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public bool CanAccessJobsPage { get; set; } = true;
    public string ProfileKey { get; set; } = string.Empty;
    public List<int> VisibleStatusCodes { get; set; } = new();
}
