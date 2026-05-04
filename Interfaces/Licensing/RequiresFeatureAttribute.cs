using System;

namespace Interfaces.Licensing
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class RequiresFeatureAttribute : Attribute
    {
        public RequiresFeatureAttribute(LicenseFeature feature)
        {
            Feature = feature;
        }

        public LicenseFeature Feature { get; }
    }
}
