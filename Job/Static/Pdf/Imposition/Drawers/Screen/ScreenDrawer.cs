using Job.Static.Pdf.Common;
using Job.Static.Pdf.Imposition.Drawers.Services.Screen;
using Job.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Job.Static.Pdf.Imposition.Drawers.Screen
{
    public class ScreenDrawer
    {

        public Bitmap Draw(ProductPart impos)
        {
            switch (impos.Sheet.SheetPlaceType)
            {
                case TemplateSheetPlaceType.SingleSide:
                     return DrawSingleSideService.Draw(impos);
                    
                case TemplateSheetPlaceType.Sheetwise:
                    return DrawSingleSideService.Draw(impos);
                
                case TemplateSheetPlaceType.WorkAndTurn:
                    return DrawSingleSideService.Draw(impos);
               
                case TemplateSheetPlaceType.Perfecting:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
