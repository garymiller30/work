using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Visual.BlocknoteSpiral
{
    public class BlockNoteSettings
    {
        public bool UseCustomSize { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public SpiralPlaceEnum SpiralPlace { get; set; } = SpiralPlaceEnum.top;
        /// <summary>
        /// відстань від краю листа до кінця пружини
        /// </summary>
        public double DistanceFromEdge { get; set; } = SpiralGlobalSettings.DistanceFromEdge;
        public double DistanceBetween { get; set; } = SpiralGlobalSettings.DistanceBetween;
        public SpiralTypeEnum SpiralType { get; set; } = SpiralGlobalSettings.SpiralType;
        public int CountHoles { get; set; } = SpiralGlobalSettings.CountHoles;
    }
}
