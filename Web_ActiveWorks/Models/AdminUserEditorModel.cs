namespace Web_ActiveWorks.Models;

public sealed class AdminUserEditorModel
{
    public string Username { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string ProfileName { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public bool CanAccessJobsPage { get; set; }
    public List<int> SelectedStatusCodes { get; set; } = new();
    public List<JobStatusOption> AvailableStatuses { get; set; } = new();
    public string? StatusLoadError { get; set; }
}
