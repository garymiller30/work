using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Services.Impos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services.Impos.Binding.Loose.WorkAndTurn
{
    public static class LooseBindingWorkAndTurn
    {
        public static TemplatePageContainer Impos(LooseBindingParameters parameters)
        {
            TemplatePageContainer templatePageContainer = new TemplatePageContainer();

            TemplateSheet sheet = parameters.Sheet;
            TemplatePage page = sheet.MasterPage;

            AbstractPlaceVariant nonRotated = new PlaceVariantWorkAndTurnNonRotated(parameters);
            AbstractPlaceVariant rotated = new PlaceVariantWorkAndTurnRotated(parameters);

            AbstractPlaceVariant selVariant;
            if (nonRotated.Total > rotated.Total)
            {
                selVariant = nonRotated;
            }
            else if (nonRotated.Total < rotated.Total)
            {
                selVariant = rotated;
            }
            else if (nonRotated.FreeSpace >= rotated.FreeSpace)
            {
                selVariant = nonRotated;
            }
            else
            {
                selVariant = rotated;
            }

            double angle_front = selVariant.IsRotated ? 90 : 0;
            double angle_back = selVariant.IsRotated ? 270 : 0;

            double x = sheet.W / 2 - selVariant.BlockWidth;
            double y = sheet.SafeFields.Bottom + parameters.Yofs;

            if (parameters.IsCenterHorizontal)
                x = (sheet.W/2 - selVariant.BlockWidth) / 2 + sheet.SafeFields.Left;


            if (parameters.IsCenterVertical)
                y = (sheet.H - sheet.SafeFields.Top - sheet.SafeFields.Bottom - selVariant.BlockHeight) / 2 + sheet.SafeFields.Bottom;

            double xOfs = x;

            for (int cy = 0; cy < selVariant.CntY; cy++)
            {
                TemplatePage templatePage = new TemplatePage();
                for (int cx = 0; cx < selVariant.CntX; cx++)
                {
                    templatePage = new TemplatePage(xOfs, y, page.W, page.H, angle_front);
                    templatePage.Margins.Set(page.Margins);
                    templatePage.FrontIdx = 1;
                    templatePage.BackIdx = 0;
                    templatePageContainer.AddPage(templatePage);

                    double x_back = sheet.W - xOfs - templatePage.GetClippedWByRotate();
                    templatePage = new TemplatePage(x_back, y, page.W, page.H, angle_back);
                    templatePage.Margins.Set(page.Margins);
                    templatePage.FrontIdx = 2;
                    templatePage.BackIdx = 0;
                    templatePageContainer.AddPage(templatePage);

                    xOfs += templatePage.GetClippedWByRotate();
                }
                xOfs = x;
                y += templatePage.GetClippedHByRotate();
            }
            CropMarksService.FixCropMarksFront(templatePageContainer);
            return templatePageContainer;
        }
    }
}
