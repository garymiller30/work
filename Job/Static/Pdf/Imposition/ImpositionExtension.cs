using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition
{
    public static class ImpositionExtension
    {
        public static bool IsProofColor(this MarkColor color)
        {
            return color.IsSpot && color.Name.Equals("ProofColor");
        }
    }
}
