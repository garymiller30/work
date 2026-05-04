using System.Runtime.Serialization;

namespace ActiveWorks.Licensing
{
    [DataContract]
    internal sealed class LicenseTokenPayload
    {
        [DataMember(Name = "licenseId")]
        public string LicenseId { get; set; }

        [DataMember(Name = "customerId")]
        public string CustomerId { get; set; }

        [DataMember(Name = "machineId")]
        public string MachineId { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "expiresAtUtc")]
        public string ExpiresAtUtc { get; set; }

        [DataMember(Name = "paidUntilUtc", EmitDefaultValue = false)]
        public string PaidUntilUtc { get; set; }

        [DataMember(Name = "features")]
        public LicenseFeatureSet Features { get; set; }
    }
}
