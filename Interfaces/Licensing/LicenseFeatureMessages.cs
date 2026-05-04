using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Licensing
{
    public static class LicenseFeatureMessages
    {
        public static string GetDeniedMessage(LicenseFeature feature)
        {
            switch (feature)
            {
                case LicenseFeature.ThreeDPreview:
                    return "3D-перегляд доступний тільки з активною підпискою.";
                case LicenseFeature.ExportPdf:
                    return "PDF-інструмент доступний тільки з активною підпискою.";
                case LicenseFeature.AdvancedReports:
                    return "Розширені звіти доступні тільки з активною підпискою.";
                case LicenseFeature.Sync:
                    return "Синхронізація доступна тільки з активною підпискою.";
                default:
                    return "Ця функція доступна тільки з активною підпискою.";
            }
        }
    }
}
