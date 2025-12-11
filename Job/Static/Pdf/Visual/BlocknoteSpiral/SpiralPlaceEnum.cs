using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Visual.BlocknoteSpiral
{
    public enum SpiralPlaceEnum
    {
        [Description("Вгорі")]
        top,
        [Description("Внизу")]
        bottom,
        [Description("Зліва")]
        left,
        [Description("Справа")]
        right,
        [Description("Розворот горизонтальний")]
        spread_horizontal,
        [Description("Розворот вертикальний")]
        spread_vertical,
        [Description("Вгорі і внизу")]
        top_bottom,
        [Description("Зліва і справа")]
        left_right

    }
}
