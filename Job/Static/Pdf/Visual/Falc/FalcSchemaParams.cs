using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Create.Falc
{
    public class FalcSchemaParams
    {
        public bool Mirrored { get; set; } = true;
        public decimal[] PartsWidth { get;set; } 
        
        public bool CreateSchema { get; set; }
        public bool CreateFileAndSchema { get; set; }

        public MarkColor Color { get; set; }
        public double LineLen { get; set; }
        public double LineDistance { get; set; }

        public decimal[] RawPartsWidth { get; set; }
        public int FalcCnt { get; set; }

    }
}
