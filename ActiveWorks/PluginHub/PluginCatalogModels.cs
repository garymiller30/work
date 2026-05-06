using Interfaces;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ActiveWorks.PluginHub
{
    [DataContract]
    internal sealed class PluginCatalogItem
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "version")]
        public string Version { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "changelog")]
        public string Changelog { get; set; }

        [DataMember(Name = "pluginFilesCount")]
        public int PluginFilesCount { get; set; }

        [DataMember(Name = "dependencyFilesCount")]
        public int DependencyFilesCount { get; set; }
    }

    [DataContract]
    internal sealed class PluginCatalogResponse
    {
        [DataMember(Name = "items")]
        public List<PluginCatalogItem> Items { get; set; }
    }

    internal sealed class PluginManagementItem
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string LocalVersion { get; set; }

        public string ServerVersion { get; set; }

        public string AssemblyFileName { get; set; }

        public IPluginBase Plugin { get; set; }

        public PluginCatalogItem CatalogItem { get; set; }

        public bool IsDisabled { get; set; }

        public bool IsLoaded { get { return Plugin != null; } }

        public bool HasCatalogPackage { get { return CatalogItem != null; } }

        public string StatusText
        {
            get
            {
                if (IsDisabled && IsLoaded)
                {
                    return "Вимкнеться після перезапуску";
                }

                if (IsDisabled)
                {
                    return "Вимкнено";
                }

                if (IsLoaded &&
                    HasCatalogPackage &&
                    !string.IsNullOrWhiteSpace(ServerVersion) &&
                    !string.Equals(LocalVersion, ServerVersion, System.StringComparison.OrdinalIgnoreCase))
                {
                    return "Встановлено, доступне оновлення";
                }

                if (IsLoaded)
                {
                    return "Встановлено";
                }

                if (HasCatalogPackage)
                {
                    return "Доступно для встановлення";
                }

                return string.Empty;
            }
        }

        public static PluginManagementItem FromPlugin(IPluginBase plugin, bool isDisabled)
        {
            var assembly = plugin.GetType().Assembly;
            var version = assembly.GetName().Version;

            return new PluginManagementItem
            {
                Id = plugin.PluginName,
                Name = plugin.PluginName,
                Description = plugin.PluginDescription,
                LocalVersion = version == null ? string.Empty : version.ToString(),
                AssemblyFileName = System.IO.Path.GetFileName(assembly.Location),
                Plugin = plugin,
                IsDisabled = isDisabled
            };
        }

        public static PluginManagementItem FromDisabledFile(string assemblyFileName)
        {
            return new PluginManagementItem
            {
                Id = System.IO.Path.GetFileNameWithoutExtension(assemblyFileName),
                Name = System.IO.Path.GetFileNameWithoutExtension(assemblyFileName),
                Description = "Плагін вимкнено для цього профілю.",
                AssemblyFileName = assemblyFileName,
                IsDisabled = true
            };
        }

        public static PluginManagementItem FromCatalog(PluginCatalogItem catalogItem)
        {
            return new PluginManagementItem
            {
                Id = catalogItem.Id,
                Name = string.IsNullOrWhiteSpace(catalogItem.Name) ? catalogItem.Id : catalogItem.Name,
                Description = catalogItem.Description,
                ServerVersion = catalogItem.Version,
                CatalogItem = catalogItem
            };
        }

        public void AttachCatalog(PluginCatalogItem catalogItem)
        {
            CatalogItem = catalogItem;
            ServerVersion = catalogItem.Version;
            if (string.IsNullOrWhiteSpace(Description))
            {
                Description = catalogItem.Description;
            }
        }
    }
}
