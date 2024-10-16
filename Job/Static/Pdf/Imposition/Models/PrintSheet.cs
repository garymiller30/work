﻿using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models
{
    public class PrintSheet : TemplateSheet 
    {
        public static int printId = 1;

        public int TemplateId { get; set; } = 0;
        public int RunPageIdx { get;set; } = 0;

        public PrintSheet()
        {
            
        }
        public static PrintSheet ConvertTemplateSheetToPrintSheet(TemplateSheet sheet)
        {
            var str = JsonSerializer.Serialize(sheet);
            PrintSheet print = JsonSerializer.Deserialize<PrintSheet>(str);
            print.TemplateId = sheet.Id;
            print.Id = printId++;
            return print;
        }

        public static void ResetId()
        {
            printId = 1;
        }
    }
}
