using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Services.Impos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services.Impos.Binding.Loose.Sheetwise
{
    public static class LooseBindingSheetwise
    {
        public static TemplatePageContainer Impos(LooseBindingParameters parameters)
        {

            if (parameters.IsOneCut)
            {
                parameters.Sheet.MasterPage.Margins.Set(0d);
            }
            else
            {
                parameters.Sheet.MasterPage.SetMarginsLikeBleed();
            }

            TemplatePageContainer tc;

            switch (parameters.BindingPlace)
            {
                case Binding.BindingPlaceEnum.Normal:

                    tc = LooseBindingSingleSide.LooseBindingNormal(parameters);
                    break;

                case Binding.BindingPlaceEnum.Rotated:
                    tc = LooseBindingSingleSide.LooseBindingRotated(parameters);
                    break;

                case Binding.BindingPlaceEnum.MaxNormal:
                    tc = LooseBindingSingleSide.LooseBindingMaxNormal(parameters);
                    break;

                case Binding.BindingPlaceEnum.MaxRotated:
                    tc = LooseBindingSingleSide.LooseBindingMaxRotated(parameters);
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (tc == null) throw new NotImplementedException();


            tc.TemplatePages.ForEach(p => { p.MasterFrontIdx = 1; p.MasterBackIdx = 2; });
            return tc;

        }

        public static void FixBleedsBack(TemplatePageContainer templatePageContainer)
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
