namespace Web_ActiveWorks.Models;

public sealed class WebAccessSettings
{
    public List<WebProfileDefinition> Profiles { get; set; } = new();
    public List<WebUserDefinition> Users { get; set; } = new();
}
