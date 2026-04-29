using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UpdateHub
{
    [DataContract]
    public sealed class UpdateManifest
    {
        [DataMember(Name = "Version", Order = 1)]
        public string Version { get; set; }

        [DataMember(Name = "UpdateType", Order = 2)]
        public string UpdateType { get; set; }

        [DataMember(Name = "Changelog", Order = 3, EmitDefaultValue = false)]
        public string Changelog { get; set; }

        [DataMember(Name = "ChangelogHistory", Order = 4, EmitDefaultValue = false)]
        public List<UpdateChangelogEntry> ChangelogHistory { get; set; } = new List<UpdateChangelogEntry>();

        [DataMember(Name = "PackagePath", Order = 5, EmitDefaultValue = false)]
        public string PackagePath { get; set; }

        [DataMember(Name = "IgnoredFiles", Order = 6, EmitDefaultValue = false)]
        public List<string> IgnoredFiles { get; set; } = new List<string>();

        [DataMember(Name = "Files", Order = 7)]
        public List<UpdateManifestFile> Files { get; set; } = new List<UpdateManifestFile>();

        [DataMember(Name = "PublishedAtUtc", Order = 8, EmitDefaultValue = false)]
        public string PublishedAtUtc { get; set; }
    }

    [DataContract]
    public sealed class UpdateChangelogEntry
    {
        [DataMember(Name = "Version", Order = 1)]
        public string Version { get; set; }

        [DataMember(Name = "UpdateType", Order = 2, EmitDefaultValue = false)]
        public string UpdateType { get; set; }

        [DataMember(Name = "PublishedAtUtc", Order = 3, EmitDefaultValue = false)]
        public string PublishedAtUtc { get; set; }

        [DataMember(Name = "Changelog", Order = 4, EmitDefaultValue = false)]
        public string Changelog { get; set; }
    }

    [DataContract]
    public sealed class UpdateManifestFile
    {
        [DataMember(Name = "Path", Order = 1)]
        public string Path { get; set; }

        [DataMember(Name = "Hash", Order = 2)]
        public string Hash { get; set; }

        [DataMember(Name = "Size", Order = 3)]
        public long Size { get; set; }
    }
}
