using System;
using System.Linq;
using System.Reflection;

namespace Interfaces.Licensing
{
    public static class LicenseFeatureGate
    {
        public static Func<LicenseFeature, bool> IsFeatureEnabled { private get; set; } = feature => true;

        public static bool RequireFor(Type type)
        {
            return RequireFor(type, out var requirement);
        }

        public static bool RequireFor(Type type, out RequiresFeatureAttribute requirement)
        {
            requirement = GetRequirement(type);
            return requirement == null || IsFeatureEnabled(requirement.Feature);
        }

        public static bool RequireFor(Type type, string methodName)
        {
            return RequireFor(type, methodName, out var requirement);
        }

        public static bool RequireFor(Type type, string methodName, out RequiresFeatureAttribute requirement)
        {
            requirement = GetRequirement(type, methodName);
            return requirement == null || IsFeatureEnabled(requirement.Feature);
        }

        public static RequiresFeatureAttribute GetRequirement(Type type)
        {
            return type.GetCustomAttributes(typeof(RequiresFeatureAttribute), true)
                .OfType<RequiresFeatureAttribute>()
                .FirstOrDefault();
        }

        public static RequiresFeatureAttribute GetRequirement(Type type, string methodName)
        {
            var method = type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .FirstOrDefault(x => string.Equals(x.Name, methodName, StringComparison.Ordinal));

            var methodRequirement = method?.GetCustomAttributes(typeof(RequiresFeatureAttribute), true)
                .OfType<RequiresFeatureAttribute>()
                .FirstOrDefault();

            return methodRequirement ?? GetRequirement(type);
        }
    }
}
