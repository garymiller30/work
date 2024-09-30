using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models
{
    public class TemplatePlate
    {
        public double W {  get; set; }
        public double H { get; set; }
        public double Xofs { get;set; }
        public double Yofs { get;set; }

        public bool IsCenterHorizontal { get;set; } = true;
        public bool IsCenterVertical { get;set; }

        public bool IsLikePaperFormat { get;set; } = true;
    }
}
