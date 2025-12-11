using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Create.Falc
{
    public class FalcSchemaParams
    {
        public bool IsMarkFile { get; set; }
        public bool Mirrored { get; set; } = true;
        public decimal[] PartsWidth { get;set; } 
    }
}
