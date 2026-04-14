namespace Web_ActiveWorks.Models;

public sealed class WebProfileDefinition
{
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string MongoConnectionString { get; set; } = string.Empty;
    public string MongoDatabaseName { get; set; } = string.Empty;
}
