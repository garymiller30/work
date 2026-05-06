namespace Web_ActiveWorks.Models;

public sealed class PluginCatalogOptions
{
    public string RootPath { get; set; } = "Data/plugins";

    public string CatalogFileName { get; set; } = "plugins.json";

    public string ManifestFileName { get; set; } = "plugin.json";
}

public sealed class PluginCatalogManifest
{
    public List<PluginCatalogManifestItem> Plugins { get; set; } = new();
}

public sealed class PluginCatalogManifestItem
{
    public string Id { get; set; } = "";

    public string ManifestPath { get; set; } = "";
}

public sealed class PluginPackageManifest
{
    public string Id { get; set; } = "";

    public string Name { get; set; } = "";

    public string Version { get; set; } = "";

    public string Description { get; set; } = "";

    public string Changelog { get; set; } = "";

    public string PublishedAtUtc { get; set; } = "";

    public string PackagePath { get; set; } = "";

    public List<PluginPackageFile> Files { get; set; } = new();
}

public sealed class PluginPackageFile
{
    public string Path { get; set; } = "";

    public string TargetRoot { get; set; } = PluginInstallTarget.ProfilePlugins;

    public string TargetPath { get; set; } = "";

    public string Hash { get; set; } = "";

    public long Size { get; set; }
}

public static class PluginInstallTarget
{
    public const string Application = "Application";

    public const string ProfilePlugins = "ProfilePlugins";
}

public sealed class PluginCatalogItem
{
    public string Id { get; set; } = "";

    public string Name { get; set; } = "";

    public string Version { get; set; } = "";

    public string Description { get; set; } = "";

    public string Changelog { get; set; } = "";

    public string PublishedAtUtc { get; set; } = "";

    public int PluginFilesCount { get; set; }

    public int DependencyFilesCount { get; set; }

    public long TotalSize { get; set; }
}
