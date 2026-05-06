using System.Security.Cryptography;
using System.Text.Json;
using Web_ActiveWorks.Models;

namespace Web_ActiveWorks.Services;

public sealed class PluginCatalogService
{
    private readonly IWebHostEnvironment _environment;
    private readonly IConfiguration _configuration;
    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public PluginCatalogService(IWebHostEnvironment environment, IConfiguration configuration)
    {
        _environment = environment;
        _configuration = configuration;
    }

    public async Task<IReadOnlyList<PluginCatalogItem>> GetCatalogAsync()
    {
        var manifests = await LoadManifestsAsync();

        return manifests
            .Select(x => ToCatalogItem(x.Manifest))
            .OrderBy(x => x.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    public async Task<PluginPackageManifest?> FindManifestAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return null;
        }

        var manifests = await LoadManifestsAsync();
        return manifests
            .Select(x => x.Manifest)
            .FirstOrDefault(x => string.Equals(x.Id, id, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<PluginDownloadPackage?> BuildDownloadPackageAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return null;
        }

        var manifestEntry = (await LoadManifestsAsync())
            .FirstOrDefault(x => string.Equals(x.Manifest.Id, id, StringComparison.OrdinalIgnoreCase));

        if (manifestEntry is null)
        {
            return null;
        }

        var manifest = manifestEntry.Manifest;
        if (!string.IsNullOrWhiteSpace(manifest.PackagePath) &&
            string.Equals(Path.GetExtension(manifest.PackagePath), ".zip", StringComparison.OrdinalIgnoreCase))
        {
            var zipPath = ResolveChildPath(manifestEntry.DirectoryPath, manifest.PackagePath.Replace('/', Path.DirectorySeparatorChar));
            if (!File.Exists(zipPath))
            {
                return null;
            }

            return new PluginDownloadPackage(
                File.OpenRead(zipPath),
                Path.GetFileName(zipPath));
        }

        return null;
    }

    private async Task<IReadOnlyList<PluginManifestEntry>> LoadManifestsAsync()
    {
        var pluginRoot = GetPluginRoot();
        if (!Directory.Exists(pluginRoot))
        {
            return Array.Empty<PluginManifestEntry>();
        }

        var manifestPaths = await GetManifestPathsAsync(pluginRoot);

        var result = new List<PluginManifestEntry>();
        foreach (var manifestPath in manifestPaths)
        {
            try
            {
                var manifest = await ReadManifestAsync(manifestPath);
                if (manifest is null)
                {
                    continue;
                }

                NormalizeManifest(manifest, manifestPath);
                UpdateFileMetadata(manifest, Path.GetDirectoryName(manifestPath) ?? pluginRoot);
                result.Add(new PluginManifestEntry(manifest, Path.GetDirectoryName(manifestPath) ?? pluginRoot));
            }
            catch
            {
                // A broken plugin manifest should not hide the rest of the catalog.
            }
        }

        return result;
    }

    private async Task<List<string>> GetManifestPathsAsync(string pluginRoot)
    {
        var options = GetOptions();
        var catalogPath = Path.Combine(pluginRoot, options.CatalogFileName);
        if (File.Exists(catalogPath))
        {
            try
            {
                await using var catalogStream = File.OpenRead(catalogPath);
                var catalog = await JsonSerializer.DeserializeAsync<PluginCatalogManifest>(catalogStream, _jsonOptions);
                var paths = catalog?.Plugins?
                    .Where(x => x is not null && !string.IsNullOrWhiteSpace(x.ManifestPath))
                    .Select(x => ResolveChildPath(pluginRoot, x.ManifestPath.Replace('/', Path.DirectorySeparatorChar)))
                    .Where(File.Exists)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
                    .ToList();

                if (paths is { Count: > 0 })
                {
                    return paths;
                }
            }
            catch
            {
                // Fall back to scanning plugin folders if the catalog index is broken.
            }
        }

        return Directory
            .GetFiles(pluginRoot, options.ManifestFileName, SearchOption.AllDirectories)
            .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private async Task<PluginPackageManifest?> ReadManifestAsync(string manifestPath)
    {
        await using var stream = File.OpenRead(manifestPath);
        return await JsonSerializer.DeserializeAsync<PluginPackageManifest>(stream, _jsonOptions);
    }

    private void NormalizeManifest(PluginPackageManifest manifest, string manifestPath)
    {
        var folderName = Path.GetFileName(Path.GetDirectoryName(manifestPath));
        if (string.IsNullOrWhiteSpace(manifest.Id))
        {
            manifest.Id = folderName ?? "";
        }

        if (string.IsNullOrWhiteSpace(manifest.Name))
        {
            manifest.Name = manifest.Id;
        }

        if (manifest.Files is null)
        {
            manifest.Files = new List<PluginPackageFile>();
        }

        manifest.Files = manifest.Files
            .Where(x => x is not null && !string.IsNullOrWhiteSpace(x.Path))
            .Select(NormalizeFile)
            .ToList();
    }

    private static PluginPackageFile NormalizeFile(PluginPackageFile file)
    {
        file.Path = NormalizeRelativePath(file.Path);
        file.TargetPath = NormalizeRelativePath(string.IsNullOrWhiteSpace(file.TargetPath) ? file.Path : file.TargetPath);

        if (!string.Equals(file.TargetRoot, PluginInstallTarget.Application, StringComparison.OrdinalIgnoreCase))
        {
            file.TargetRoot = PluginInstallTarget.ProfilePlugins;
        }
        else
        {
            file.TargetRoot = PluginInstallTarget.Application;
        }

        return file;
    }

    private void UpdateFileMetadata(PluginPackageManifest manifest, string manifestDirectory)
    {
        if (!string.IsNullOrWhiteSpace(manifest.PackagePath) &&
            string.Equals(Path.GetExtension(manifest.PackagePath), ".zip", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var packageRoot = ResolveChildPath(manifestDirectory, string.IsNullOrWhiteSpace(manifest.PackagePath)
            ? manifestDirectory
            : manifest.PackagePath.Replace('/', Path.DirectorySeparatorChar));

        foreach (var file in manifest.Files)
        {
            var sourcePath = ResolveChildPath(packageRoot, file.Path.Replace('/', Path.DirectorySeparatorChar));
            if (!File.Exists(sourcePath))
            {
                continue;
            }

            var fileInfo = new FileInfo(sourcePath);
            file.Size = fileInfo.Length;
            if (string.IsNullOrWhiteSpace(file.Hash))
            {
                file.Hash = ComputeSha256(sourcePath);
            }
        }
    }

    private string GetPluginRoot()
    {
        var rootPath = GetOptions().RootPath;
        return Path.IsPathRooted(rootPath)
            ? Path.GetFullPath(rootPath)
            : Path.GetFullPath(Path.Combine(_environment.ContentRootPath, rootPath));
    }

    private PluginCatalogOptions GetOptions() =>
        _configuration.GetSection("PluginCatalog").Get<PluginCatalogOptions>() ?? new PluginCatalogOptions();

    private static PluginCatalogItem ToCatalogItem(PluginPackageManifest manifest) => new()
    {
        Id = manifest.Id,
        Name = manifest.Name,
        Version = manifest.Version,
        Description = manifest.Description,
        Changelog = manifest.Changelog,
        PublishedAtUtc = manifest.PublishedAtUtc,
        PluginFilesCount = manifest.Files.Count(x => string.Equals(x.TargetRoot, PluginInstallTarget.ProfilePlugins, StringComparison.OrdinalIgnoreCase)),
        DependencyFilesCount = manifest.Files.Count(x => string.Equals(x.TargetRoot, PluginInstallTarget.Application, StringComparison.OrdinalIgnoreCase)),
        TotalSize = manifest.Files.Sum(x => x.Size)
    };

    private static string ResolveChildPath(string root, string childPath)
    {
        var rootFullPath = Path.GetFullPath(root);
        var fullPath = Path.GetFullPath(Path.IsPathRooted(childPath) ? childPath : Path.Combine(rootFullPath, childPath));

        if (!fullPath.StartsWith(rootFullPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(fullPath, rootFullPath, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Path is outside of the plugin catalog root.");
        }

        return fullPath;
    }

    private static string NormalizeRelativePath(string path) =>
        (path ?? string.Empty).Replace('\\', '/').TrimStart('/');

    private static string ComputeSha256(string path)
    {
        using var stream = File.OpenRead(path);
        var hash = SHA256.HashData(stream);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    private sealed record PluginManifestEntry(PluginPackageManifest Manifest, string DirectoryPath);
}

public sealed record PluginDownloadPackage(Stream Content, string FileName);
