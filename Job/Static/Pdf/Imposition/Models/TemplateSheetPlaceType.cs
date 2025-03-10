using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models
{
    public enum TemplateSheetPlaceType
    {
        [Description("без звороту")]
        SingleSide,
        [Description("лице-зворот")]
        Sheetwise,
        [Description("свій зворот")]
        WorkAndTurn,
        [Description("клапан-хвіст")]
        WorkAndTumble
    }
}
