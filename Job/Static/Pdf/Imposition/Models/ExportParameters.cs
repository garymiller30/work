using ExtensionMethods;
using JobSpace.Static.Pdf.Imposition.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models
{
    /// <summary>
    /// параметри для експорту в pdf
    /// </summary>
    public class ExportParameters
    {
        public string   OutputFolder { get; set; } = "";
        public string   OutputFileName { get; set; } = "";
        public bool     UseTemplate { get; set; } = false;
        public string   TemplateString { get; set; } = "$[orderNo]_$[customer]_$[orderDesc]";
        public bool     SavePrintSheetToOrderFolder { get; set; } = true;
        public bool     UseCustomOutputFolder { get; set; } = false;
        public string   CustomOutputFolder { get; set; } = "";

        public string GetOutputFilePath()
        {
            string fileName = OutputFileName;
            string folder = OutputFolder;

            if (UseTemplate)
            {
                fileName = TextVariablesService.ReplaceToRealValues(TemplateString).Transliteration() + ".pdf";
            }

            if (UseCustomOutputFolder)
            {
                folder = CustomOutputFolder;
            }

            string filePath = Path.Combine(folder, fileName);

            return filePath;
        }
    }
}
