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
        public string   OutputFilePath { get; private set; } = "";

        public void CreateOutputFileName()
        {
            string fileName = Path.GetFileNameWithoutExtension(OutputFileName);
            string folder = OutputFolder;
            int extra = 0;
            string ext = ".pdf";

            string filePath;

            if (UseTemplate)
            {
                fileName = TextVariablesService.ReplaceToRealValues(TemplateString).Transliteration();
            }

            if (UseCustomOutputFolder)
            {
                folder = CustomOutputFolder;
            }

            do
            {
                filePath = Path.Combine(folder, $"{fileName}{(extra == 0 ? "" : $"_{extra}")}{ext}");
                extra++;
            } while (File.Exists(filePath));

            

            OutputFilePath = filePath;
        }

      
    }
}
