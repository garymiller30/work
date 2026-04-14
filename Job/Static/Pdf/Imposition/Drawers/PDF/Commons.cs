using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Drawers.PDF
{
    public static partial class Commons
    {
        public static Dictionary<double, string> Orientate = new Dictionary<double, string>
        {
            {0,"north" },
            {90,"west" },
            {180,"south" },
            { 270,"east"}
        };
    }
}
