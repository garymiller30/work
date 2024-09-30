using JobSpace.Static.Pdf.Imposition.Models.Marks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models
{
    public class TemplateSheet
    {
        public int Id { get;set; } = 0;
        public string Description { get; set; } = "sheet";

        public TemplatePage MasterPage { get; set; } = new TemplatePage();
        public TemplatePageContainer TemplatePageContainer { get; set; } = new TemplatePageContainer();

        public double W { get; set; }
        public double H { get; set; }

        public TemplateSheetPlaceType SheetPlaceType { get; set; } = TemplateSheetPlaceType.SingleSide;

        public TemplateSheet()
        {

        }

        public TemplateSheet(double w, double h)
        {
            W = w; H = h;
        }

        /// <summary>
        /// додаткове поле навколо сторінки
        /// </summary>
        public double ExtraSpace { get; set; }

        /// <summary>
        /// поля, що не задруковуються
        /// </summary>
        public ClipBox SafeFields { get; set; } = new ClipBox();

        public MarksContainer Marks { get; set; } = new MarksContainer();

        public static void Save(TemplateSheet sheet, string fileName)
        {
            var jsonStr = JsonSerializer.Serialize(sheet);
            File.WriteAllText(fileName, jsonStr);
        }

        public static TemplateSheet Load(string fileName)
        {
            var jsonStr = File.ReadAllText(fileName);
            return JsonSerializer.Deserialize<TemplateSheet>(jsonStr);
        }

        public TemplateSheet Copy()
        {
            return JsonSerializer.Deserialize<TemplateSheet>(JsonSerializer.Serialize(this));
        }
    }
}
