using System.Runtime.Serialization;

namespace ActiveWorks.Licensing
{
    [DataContract]
    internal sealed class LicenseStorageState
    {
        [DataMember(Name = "licenseKey", EmitDefaultValue = false)]
        public string LicenseKey { get; set; }

        [DataMember(Name = "licenseToken", EmitDefaultValue = false)]
        public string LicenseToken { get; set; }

        [DataMember(Name = "updatedAtUtc", EmitDefaultValue = false)]
        public string UpdatedAtUtc { get; set; }
    }
}
