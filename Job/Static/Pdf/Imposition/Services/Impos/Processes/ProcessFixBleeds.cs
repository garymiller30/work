using JobSpace.Static.Pdf.Imposition.Drawers.Services.Screen;
using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services.Impos.Processes
{
    public static class ProcessFixBleeds
    {
        public static void Front(TemplatePageContainer templatePageContainer)
        {
            templatePageContainer.TemplatePages.ForEach(x => x.Bleeds.Set(x.Bleeds.Default));

            foreach (var page in templatePageContainer.TemplatePages)
            {
                PageSide side = page.Front;
                var m = page.Margins;

                (RectangleD left, RectangleD right, RectangleD top, RectangleD bottom) = ScreenDrawCommons.GetDrawBleedsFront(page);

                foreach (var pageTarget in templatePageContainer.TemplatePages)
                {

                    if (page != pageTarget)
                    {
                        // координати сторінки в готовому вигляді
                        (double page_x, double page_y, double page_w, double page_h) = ScreenDrawCommons.GetPageDraw(pageTarget, pageTarget.Front);

                        RectangleD pageRect = new RectangleD
                        {
                            X1 = page_x,
                            Y1 = page_y,
                            X2 = page_x + page_w,
                            Y2 = page_y + page_h
                        };

                        List<RectangleD> rects = new List<RectangleD>() {
                            ScreenDrawCommons.GetDrawBleedLeftFront(pageTarget),
                            ScreenDrawCommons.GetDrawBleedRightFront(pageTarget),
                            ScreenDrawCommons.GetDrawBleedTopFront(pageTarget),
                            ScreenDrawCommons.GetDrawBleedBottomFront(pageTarget),
                        };

                        foreach (var rect in rects)
                        {
                            if (left.IntersectsWith(rect) || left.IntersectsWith(pageRect))// 
                            {
                                page.Bleeds.Left = m.Left;
                            }
                            if (right.IntersectsWith(rect) || right.IntersectsWith(pageRect))  //
                            {
                                page.Bleeds.Right = m.Right;
                            }
                            if (top.IntersectsWith(rect) || top.IntersectsWith(pageRect))// 
                            {
                                page.Bleeds.Top = m.Top;
                            }
                            if (bottom.IntersectsWith(rect) || bottom.IntersectsWith(pageRect))// 
                            {
                                page.Bleeds.Bottom = m.Bottom;
                            }
                        }
                    }
                }
            }
        }

        public static void Back(TemplatePageContainer templatePageContainer)
        {
            templatePageContainer.TemplatePages.ForEach(x => x.Bleeds.Set(x.Bleeds.Default));

            foreach (var page in templatePageContainer.TemplatePages)
            {

                (RectangleD left, RectangleD right, RectangleD top, RectangleD bottom) = ScreenDrawCommons.GetDrawBleedsFront(page);

                foreach (var pageTarget in templatePageContainer.TemplatePages)
                {
                    if (page != pageTarget)
                    {
                        (double page_x, double page_y, double page_w, double page_h) = ScreenDrawCommons.GetPageDraw(pageTarget, pageTarget.Back);

                        RectangleD pageRect = new RectangleD
                        {
                            X1 = page_x,
                            Y1 = page_y,
                            X2 = page_x + page_w,
                            Y2 = page_y + page_h
                        };

                        List<RectangleD> rects = new List<RectangleD>(){
                            ScreenDrawCommons.GetDrawBleedLeftFront(pageTarget),
                             ScreenDrawCommons.GetDrawBleedRightFront(pageTarget),
                              ScreenDrawCommons.GetDrawBleedTopFront(pageTarget),
                               ScreenDrawCommons.GetDrawBleedBottomFront(pageTarget),

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
