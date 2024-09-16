using Job.Static.Pdf.Imposition.Models;
using Job.Static.Pdf.Imposition.Services.Impos.Binding;
using Job.Static.Pdf.Imposition.Services.Impos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Services.Impos
{
    public static class LooseBindingSingleSide
    {
        public static TemplatePageContainer Impos(LooseBindingParameters parameters)
        {
            TemplateSheet sheet = parameters.Sheet;
            TemplatePage page = parameters.TemplatePage;

            TemplatePageContainer templatePageContainer = new TemplatePageContainer();
            TemplatePage tPage = new TemplatePage(page.W, page.H);
            tPage.Margins.Set(page.Bleeds);

            AbstractPlaceVariant nonRotated = new PlaceVariantNonRotated(parameters);
            AbstractPlaceVariant rotated = new PlaceVariantRotated(parameters);

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

            tPage.Angle = selVariant.IsRotated ? 90 : 0;

            double x = sheet.SafeFields.Left + parameters.Xofs;
            double y = sheet.SafeFields.Bottom + parameters.Yofs;

            if (parameters.IsCenterHorizontal)
                x = (sheet.W - sheet.SafeFields.Left - sheet.SafeFields.Right - selVariant.BlockWidth) / 2;

            if (parameters.IsCenterVertical)
                y = (sheet.H - sheet.SafeFields.Top - sheet.SafeFields.Top - selVariant.BlockHeight) / 2;

            double xOfs = x;

            for (int cy = 0; cy < selVariant.CntY; cy++)
            {
                TemplatePage templatePage = new TemplatePage();
                for (int cx = 0; cx < selVariant.CntX; cx++)
                {
                    templatePage = new TemplatePage(xOfs, y, tPage.W, tPage.H, tPage.Angle);
                    templatePage.Margins.Set(page.Bleeds);
                    templatePage.FrontIdx = 1;
                    templatePage.BackIdx = 0;


                    templatePageContainer.AddPage(templatePage);
                    xOfs += templatePage.GetClippedWByRotate();
                }
                xOfs = x;
                y += templatePage.GetClippedHByRotate();
            }

            return templatePageContainer;
        }
    }
}
