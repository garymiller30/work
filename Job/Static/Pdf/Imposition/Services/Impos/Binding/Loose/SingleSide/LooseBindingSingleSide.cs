using JobSpace.Static.Pdf.Imposition.Models;
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
            
            if (parameters.IsOneCut) page.Margins.Set(0);

            TemplatePageContainer templatePageContainer = new TemplatePageContainer();

            AbstractPlaceVariant nonRotated = new PlaceVariantNonRotated(parameters);
            AbstractPlaceVariant rotated = new PlaceVariantRotated(parameters);

            AbstractPlaceVariant selVariant;

            if (parameters.BindingPlace == Binding.BindingPlaceEnum.Normal)
            {
                selVariant = nonRotated;
            }
            else if (parameters.BindingPlace == Binding.BindingPlaceEnum.Rotated)
            {
                selVariant = rotated;
            }
            else
            {
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
                    templatePage.Bleeds.SetDefault(page.Bleeds.Default);
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
                FixBleedsFront(templatePageContainer);
            }

            CropMarksService.FixCropMarksFront(templatePageContainer);
            return templatePageContainer;
        }

        public static void FixBleedsFront(TemplatePageContainer templatePageContainer)
        {
            templatePageContainer.TemplatePages.ForEach(x => x.Bleeds.Set(x.Bleeds.Default));

            foreach (var page in templatePageContainer.TemplatePages)
            {
                RectangleD left = page.GetDrawBleedLeft();
                RectangleD right = page.GetDrawBleedRight();
                RectangleD top = page.GetDrawBleedTop();
                RectangleD bottom = page.GetDrawBleedBottom();

                foreach (var pageTarget in templatePageContainer.TemplatePages)
                {
                    if (page != pageTarget)
                    {
                        RectangleD pageRect = new RectangleD
                        {
                            X1 = pageTarget.GetPageDrawX(),
                            Y1 = pageTarget.GetPageDrawY(),
                            X2 = pageTarget.GetPageDrawX() + pageTarget.GetPageDrawW(),
                            Y2 = pageTarget.GetPageDrawY() + pageTarget.GetPageDrawH()
                        };

                        List<RectangleD> rects = new List<RectangleD>(){
                            pageTarget.GetDrawBleedLeft(),
                            pageTarget.GetDrawBleedRight(),
                            pageTarget.GetDrawBleedTop(),
                            pageTarget.GetDrawBleedBottom()
                        };

                        foreach (var rect in rects)
                        {
                            if (left.IntersectsWith(rect) || left.IntersectsWith(pageRect))
                            {
                                page.Bleeds.Left = 0;

                            }
                            if (right.IntersectsWith(rect) || right.IntersectsWith(pageRect))
                            {
                                page.Bleeds.Right = 0;

                            }
                            if (top.IntersectsWith(rect) || top.IntersectsWith(pageRect))
                            {
                                page.Bleeds.Top = 0;
                            }
                            if (bottom.IntersectsWith(rect) || bottom.IntersectsWith(pageRect))
                            {
                                page.Bleeds.Bottom = 0;
                            }
                        }
                    }
                }
               
            }
        }
    }
}