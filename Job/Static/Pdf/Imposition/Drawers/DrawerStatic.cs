using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Drawers
{
    public static class DrawerStatic
    {
        public static ProductPart CurProductPart { get; set; }
        public static DrawerSideEnum CurSide { get; set; } 
    }
}
