using Interfaces.Licensing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.Licensing
{
    public static class LicenseUiGate
    {
        public static bool RequireFor(Control owner, Type type, string methodName)
        {
            if (LicenseFeatureGate.RequireFor(type, methodName, out var requirement))
            {
                return true;
            }

            MessageBox.Show(
                owner,
                LicenseFeatureMessages.GetDeniedMessage(requirement.Feature),
                "Потрібна підписка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            return false;
        }
    }

}
