using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Services.Impos.Binding;
using JobSpace.Static.Pdf.Imposition.Services.Impos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services.Impos
{
    public static class LooseBindingSingleSide
    {
        public static TemplatePageContainer Impos(LooseBindingParameters parameters)
        {
            TemplateSheet sheet = parameters.Sheet;
            TemplatePage page = sheet.MasterPage;
            if (parameters.IsOneCut)
            {
                page.Margins.Set(0);
            }

            TemplatePageContainer templatePageContainer = new TemplatePageContainer();

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

            double angle = selVariant.IsRotated ? 90 : 0;

            double x = sheet.SafeFields.Left + parameters.Xofs;
            double y = sheet.SafeFields.Bottom + parameters.Yofs;

            if (parameters.IsCenterHorizontal)
                x = (sheet.W - sheet.SafeFields.Left - sheet.SafeFields.Right - selVariant.BlockWidth) / 2 + sheet.SafeFields.Left;

            if (parameters.IsCenterVertical)
                y = (sheet.H - sheet.SafeFields.Top - sheet.SafeFields.Bottom - selVariant.BlockHeight) / 2 + sheet.SafeFields.Bottom;

            double xOfs = x;

            for (int cy = 0; cy < selVariant.CntY; cy++)
            {
                TemplatePage templatePage = new TemplatePage();
                for (int cx = 0; cx < selVariant.CntX; cx++)
                {
                    templatePage = new TemplatePage(xOfs, y, page.W, page.H, angle);
                    templatePage.Bleeds = page.Bleeds;
                    templatePage.Margins.Set(page.Margins);
                    templatePage.FrontIdx = 1;
                    templatePage.BackIdx = 0;

                    templatePageContainer.AddPage(templatePage);
                    xOfs += templatePage.GetClippedWByRotate();
                }
                xOfs = x;
                y += templatePage.GetClippedHByRotate();
            }

            if (parameters.IsOneCut)
            {
                FixBleeds(templatePageContainer);
            }

            CropMarksService.FixCropMarksFront(templatePageContainer);
            return templatePageContainer;
        }

        private static void FixBleeds(TemplatePageContainer templatePageContainer)
        {
            foreach (var page in templatePageContainer.TemplatePages)
            {
                foreach (var pageTarget in templatePageContainer.TemplatePages)
                {
                    if (page != pageTarget)
                    {



                        switch (page.Angle)
                        {
                            case 0:
                                
                                
                                break;
                            case 90:
                                break;
                            case 180:
                                break;
                            case 270:
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                    }
                }
            }
        }
    }
}