using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Visual.BlocknoteSpiral
{
    public class SpiralSettings
    {
        public string SpiralFile { get; set; }
        public SpiralPlaceEnum SpiralPlace { get; set; } = SpiralPlaceEnum.top;
        public int CountHoles { get; set; } = 0;

        public int PageHandle { get; set; } = -1;
        public double SpiralWidth { get; set; } = 0;
        public double SpiralHeight { get; set; } = 0;
    }
}
