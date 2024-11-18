using ExtensionMethods;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Models.Marks;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.Static.Pdf.Imposition.Services
{
    public static class SaveLoadService
    {
        static string RootPath;
        static string SheetPath;
        static string MarksPath;
        static string SheetTemplatesPath;

        static SaveLoadService()
        {
            RootPath = Path.Combine(Directory.GetCurrentDirectory(), "Impos");
            SheetPath = Path.Combine(RootPath, "Sheets");
            MarksPath = Path.Combine(RootPath, "Marks");
            SheetTemplatesPath = Path.Combine(RootPath, "SheetTemplates");

            if (!Directory.Exists(RootPath)) Directory.CreateDirectory(RootPath);
            if (!Directory.Exists(SheetPath)) Directory.CreateDirectory(SheetPath);
            if (!Directory.Exists(MarksPath)) Directory.CreateDirectory(MarksPath);
            if (!Directory.Exists(SheetTemplatesPath)) Directory.CreateDirectory(SheetTemplatesPath);

        }
        public static void SaveSheet(TemplateSheet sheet)
        {
            string sheetStr = JsonSerializer.Serialize(sheet);
            string fileName = sheet.Description.Transliteration();
            string filePath = Path.Combine(SheetPath, fileName + ".json");
            File.WriteAllText(filePath, sheetStr);
        }

        public static List<TemplateSheet> LoadSheets()
        {
            var files = Directory.GetFiles(SheetPath, "*.json");

            if (files.Length == 0) return new List<TemplateSheet>();

            var sheets = new List<TemplateSheet>();

            foreach (var file in files)
            {
                var sheetstr = File.ReadAllText(file);
                var sheet = JsonSerializer.Deserialize<TemplateSheet>(sheetstr);
                sheets.Add(sheet);
            }
            return sheets;
        }

        public static bool DeleteSheet(TemplateSheet sheet)
        {
            string fileName = sheet.Description.Transliteration();
            string filePath = Path.Combine(SheetPath, fileName + ".json");
            try
            {
                File.Delete(filePath);
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }

        public static void SaveResourceMarks(List<MarksContainer> marks)
        {
            string filePath = Path.Combine(MarksPath, "resource_marks.json");
            string marksStr = JsonSerializer.Serialize(marks);
            File.WriteAllText(filePath, marksStr);
        }

        public static List<MarksContainer> LoadResourceMarks()
        {
            string filePath = Path.Combine(MarksPath, "resource_marks.json");
            if (!File.Exists(filePath)) return new List<MarksContainer>();

            string marksStr = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<MarksContainer>>(marksStr);
        }

        public static void SaveSheetTemplates(TemplateSheet sheet)
        {
            using (var form = new SaveFileDialog())
            {
                string fileName = sheet.Description.Transliteration() + ".json";

                form.InitialDirectory = SheetTemplatesPath;
                form.FileName = fileName;
                if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string str = JsonSerializer.Serialize(sheet);
                    File.WriteAllText(form.FileName, str);
                }
            }
        }

        public static TemplateSheet LoadSheetTemplate()
        {

            using (var form = new Ookii.Dialogs.WinForms.VistaOpenFileDialog())
            {
                form.InitialDirectory = SheetTemplatesPath;
                form.CheckFileExists = true;

                if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string str = File.ReadAllText(form.FileName);

                    TemplateSheet sheet = JsonSerializer.Deserialize<TemplateSheet>(str);
                    
                    return sheet;
                }
            }

            return null;
        }
    }
}
