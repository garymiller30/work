using System.Runtime.Serialization;

namespace ActiveWorks.Licensing
{
    [DataContract]
    internal sealed class LicenseFeatureSet
    {
        [DataMember(Name = "updates", EmitDefaultValue = false)]
        public bool Updates { get; set; }

        [DataMember(Name = "exportPdf", EmitDefaultValue = false)]
        public bool ExportPdf { get; set; }

        [DataMember(Name = "advancedReports", EmitDefaultValue = false)]
        public bool AdvancedReports { get; set; }

        [DataMember(Name = "sync", EmitDefaultValue = false)]
        public bool Sync { get; set; }

        [DataMember(Name = "threeDPreview", EmitDefaultValue = false)]
        public bool ThreeDPreview { get; set; }

        [DataMember(Name = "maxProjects", EmitDefaultValue = false)]
        public int MaxProjects { get; set; }
    }
}
