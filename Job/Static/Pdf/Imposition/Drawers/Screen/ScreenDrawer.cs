﻿using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition.Drawers.Services.Screen;
using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.Static.Pdf.Imposition.Drawers.Screen
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

        public Bitmap Draw(TemplateSheet sheet)
        {
            switch (sheet.SheetPlaceType)
            {
                case TemplateSheetPlaceType.SingleSide:
                    return DrawSingleSideService.Draw(sheet);

                case TemplateSheetPlaceType.Sheetwise:
                    return DrawSingleSideService.Draw(sheet);

                case TemplateSheetPlaceType.WorkAndTurn:
                    return DrawSingleSideService.Draw(sheet);

                case TemplateSheetPlaceType.Perfecting:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}