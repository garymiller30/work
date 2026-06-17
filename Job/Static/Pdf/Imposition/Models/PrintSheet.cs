using Interfaces.Pdf.Imposition;
using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models
{
    public class PrintSheet : TemplateSheet , IPrintSheet
    {
        public static int printId = 1;

        public int TemplateId { get; set; } = 0;

        public TemplatePlate TemplatePlate { get; set; }

        public int Count { get; set; } = 1;

        public PrintSheet()
        {
            
        }
        public static PrintSheet ConvertTemplateSheetToPrintSheet(TemplateSheet sheet)
        {
            // Перетворюємо об'єкт у дерево нод і одразу десеріалізуємо у новий тип
            var node = JsonSerializer.SerializeToNode(sheet);
            var print = node.Deserialize<PrintSheet>();

            if (print == null) throw new Exception("Can't convert TemplateSheet to PrintSheet");

            print.TemplateId = sheet.Id;
            print.Id = Interlocked.Increment(ref printId);
            return print;
        }

        public static void ResetId()
        {
            printId = 1;
        }

        new public PrintSheet Copy()
        {
            var str = JsonSerializer.Serialize(this);
            var sheet = JsonSerializer.Deserialize<PrintSheet>(str);

            if (sheet == null) throw new Exception("Can't copy PrintSheet");

            sheet.Id = Interlocked.Increment(ref SheetId); 
            return sheet;
        }

        public string GetFormatStr()
        {
            return $"{W:N1} x {H:N1}";
        }
    }
}
