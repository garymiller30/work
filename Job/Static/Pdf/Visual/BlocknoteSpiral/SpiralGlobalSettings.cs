using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Visual.BlocknoteSpiral
{
    public static class SpiralGlobalSettings
    {
        /// <summary>
        /// відстань між пружинками
        /// </summary>
        public static double DistanceBetween = 8.46;
        public static double DistanceFromEdge = 8;
        public static double Diameter = 4;
        public static double Thickness = 0.8;
        public static SpiralTypeEnum SpiralType = SpiralTypeEnum.Circle;
        public static double HoleTransparency = 0.65;
        public static int CountHoles = 0;
    }
}
