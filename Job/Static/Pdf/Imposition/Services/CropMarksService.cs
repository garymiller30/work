using JobSpace.Static.Pdf.Imposition.Models.Marks;
using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobSpace.Static.Pdf.Imposition.Drawers.Services.Screen;

namespace JobSpace.Static.Pdf.Imposition.Services
{
    public static class CropMarksService
    {
        public static void FixCropMarks(TemplateSheet sheet)
        {
            if (sheet == null) return;

            switch (sheet.SheetPlaceType)
            {
                case TemplateSheetPlaceType.SingleSide:
                    FixCropMarksFront(sheet.TemplatePageContainer);
                    break;
                case TemplateSheetPlaceType.Sheetwise:
                    FixCropMarksFront(sheet.TemplatePageContainer);
                    FixCropMarksBack(sheet);
                    break;
                case TemplateSheetPlaceType.WorkAndTurn:
                    FixCropMarksWorkAndTurn(sheet);
                    break;
                case TemplateSheetPlaceType.WorkAndTumble:
                    FixCropMarksWorkAndTumble(sheet);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        static void FixCropMarksFront(TemplatePageContainer templateContainer)
        {
            RecalcCropsFront(templateContainer);
            RemoveCropsFront(templateContainer);
        }

        static void FixCropMarksBack(TemplateSheet sheet)
        {

            RecalcCropsBack(sheet);
            RemoveCropsBack(sheet);
        }

        static void FixCropMarksWorkAndTurn(TemplateSheet sheet)
        {

            RecalcCropsFront(sheet.TemplatePageContainer);
            RecalcCropsBack(sheet);
            RemoveCropsWorkAndTurn(sheet);
        }

        static void FixCropMarksWorkAndTumble(TemplateSheet sheet)
        {
            if (sheet == null) return;
            RecalcCropsFront(sheet.TemplatePageContainer);
            RecalcCropsWorkandTumbleBack(sheet);
            RemoveCropsWorkAndTurn(sheet);
        }

        private static void RemoveCropsBack(TemplateSheet sheet)
        {
            foreach (TemplatePage page in sheet.TemplatePageContainer.TemplatePages)
            {
                CropMarksController crops = page.CropMarksController;

                foreach (TemplatePage pageToCompare in sheet.TemplatePageContainer.TemplatePages)
                {
                    if (page != pageToCompare)
                    {

                        var rect = ScreenDrawCommons.GetPageDraw(pageToCompare, pageToCompare.Back);

                        RectangleD pageRect = new RectangleD
                        {
                            X1 = rect.page_x,
                            Y1 = rect.page_y,
                            X2 = rect.page_x + rect.page_w,
                            Y2 = rect.page_y + rect.page_h
                        };



                        List<CropMark> forDeleteCrops = new List<CropMark>();

                        foreach (var cropMark in page.CropMarksController.CropMarks.Where(x => x.IsBack))
                        {
                            if (pageRect.IsInsideMe(cropMark)) forDeleteCrops.Add(cropMark);
                        }

                        page.CropMarksController.CropMarks = page.CropMarksController.CropMarks.Except(forDeleteCrops).ToList();
                    }
                }
            }
        }

        static void RemoveCropsFront(TemplatePageContainer templateContainer)
        {

            foreach (TemplatePage page in templateContainer.TemplatePages)
            {
                CropMarksController crops = page.CropMarksController;

                foreach (TemplatePage pageToCompare in templateContainer.TemplatePages)
                {
                    if (page != pageToCompare)
                    {
                        var rect = ScreenDrawCommons.GetPageDraw(pageToCompare, pageToCompare.Front);

                        RectangleD pageRect = new RectangleD
                        {
                            X1 = rect.page_x,
                            Y1 = rect.page_y,
                            X2 = rect.page_x + rect.page_w,
                            Y2 = rect.page_y + rect.page_h
                        };

                        List<CropMark> forDeleteCrops = new List<CropMark>();

                        foreach (var cropMark in page.CropMarksController.CropMarks.Where(x => x.IsFront))
                        {
                            if (pageRect.IsInsideMe(cropMark)) forDeleteCrops.Add(cropMark);
                        }

                        page.CropMarksController.CropMarks = page.CropMarksController.CropMarks.Except(forDeleteCrops).ToList();
                    }
                }
            }
        }

        static void RecalcCropsFront(TemplatePageContainer templateContainer)
        {
            foreach (var page in templateContainer.TemplatePages)
            {

                CropMarksController crops = page.CropMarksController;

                var delList = crops.CropMarks.Where(c => c.IsFront).ToList();

                crops.CropMarks = crops.CropMarks.Except(delList).ToList();

                double len = crops.Parameters.Len;
                double dist = crops.Parameters.Distance;

                CropDirection[] direction = crops.GetDrawDirectionFront(page.Front.Angle);
                AnchorOfset[] ofsets = crops.GetAnchorOfsetsFront(page, page.Front.Angle);

                double x = 0;
                double y = 0;

                for (int i = 0; i < ofsets.Length; i++)
                {
                    x += ofsets[i].X;
                    y += ofsets[i].Y;

                    for (int j = 0; j < 2; j++)
                    {
                        int idx = i * 2 + j;

                        var cropMark = new CropMarkCreator(x, y).From(direction[idx].X * dist, direction[idx].Y * dist).To(direction[idx].X * len, direction[idx].Y * len);
                        crops.CropMarks.Add(cropMark);

                    }
                }
            }
        }

        static void RecalcCropsBack(TemplateSheet sheet)
        {

            foreach (var page in sheet.TemplatePageContainer.TemplatePages)
            {

                CropMarksController crops = page.CropMarksController;

                var delList = crops.CropMarks.Where(c => c.IsBack).ToList();

                crops.CropMarks = crops.CropMarks.Except(delList).ToList();

                double len = crops.Parameters.Len;
                double dist = crops.Parameters.Distance;

                CropDirection[] direction = crops.GetDrawDirectionBack(page.Front.Angle);
                AnchorOfset[] ofsets = crops.GetAnchorOfsetsBack(page, sheet, page.Back.Angle);

                double x = 0;
                double y = 0;

                for (int i = 0; i < ofsets.Length; i++)
                {
                    x += ofsets[i].X;
                    y += ofsets[i].Y;

                    for (int j = 0; j < 2; j++)
                    {
                        int idx = i * 2 + j;

                        var cropMark = new CropMarkCreator(x, y).From(direction[idx].X * dist, direction[idx].Y * dist).To(direction[idx].X * len, direction[idx].Y * len);
                        cropMark.IsFront = false;
                        cropMark.IsBack = true;
                        crops.CropMarks.Add(cropMark);
                    }
                }
            }
        }

        static void RecalcCropsWorkandTumbleBack(TemplateSheet sheet)
        {
            foreach (var page in sheet.TemplatePageContainer.TemplatePages)
            {
                CropMarksController crops = page.CropMarksController;

                var delList = crops.CropMarks.Where(c => c.IsBack).ToList();

                crops.CropMarks = crops.CropMarks.Except(delList).ToList();

                double len = crops.Parameters.Len;
                double dist = crops.Parameters.Distance;

                CropDirection[] direction = crops.GetDrawDirectionWorkandTumbleBack(page.Back.Angle);
                AnchorOfset[] ofsets = crops.GetAnchorOfsetsWorkandTumbleBack(page, sheet, page.Back.Angle);

                double x = 0;
                double y = 0;

                for (int i = 0; i < ofsets.Length; i++)
                {
                    x += ofsets[i].X;
                    y += ofsets[i].Y;
                    for (int j = 0; j < 2; j++)
                    {
                        int idx = i * 2 + j;
                        var cropMark = new CropMarkCreator(x, y).From(direction[idx].X * dist, direction[idx].Y * dist).To(direction[idx].X * len, direction[idx].Y * len);
                        cropMark.IsFront = false;
                        cropMark.IsBack = true;
                        crops.CropMarks.Add(cropMark);
                    }
                }
            }
        }

        private static void RemoveCropsWorkAndTurn(TemplateSheet sheet)
        {
            foreach (TemplatePage page in sheet.TemplatePageContainer.TemplatePages)
            {
                CropMarksController crops = page.CropMarksController;

                foreach (TemplatePage pageToCompare in sheet.TemplatePageContainer.TemplatePages)
                {

                    var r_front = ScreenDrawCommons.GetPageDraw(pageToCompare, pageToCompare.Front);
                    var r_back = ScreenDrawCommons.GetPageDraw(pageToCompare, pageToCompare.Back);

                    RectangleD pageRectFront = new RectangleD
                    {
                        X1 = r_front.page_x,
                        Y1 = r_front.page_y,
                        X2 = r_front.page_x + r_front.page_w,
                        Y2 = r_front.page_y + r_front.page_h
                    };

                    RectangleD pageRectBack = new RectangleD
                    {
                        X1 = r_back.page_x,
                        Y1 = r_back.page_y,
                        X2 = r_back.page_x + r_back.page_w,
                        Y2 = r_back.page_y + r_back.page_h
                    };

                    //RectangleD pageRectFront = new RectangleD
                    //{
                    //    X1 = pageToCompare.Front.X,
                    //    Y1 = pageToCompare.Front.Y,
                    //    X2 = pageToCompare.Front.X + pageToCompare.GetClippedWByRotate(),
                    //    Y2 = pageToCompare.Front.Y + pageToCompare.GetClippedHByRotate()
                    //};

                    //RectangleD pageRectBack = new RectangleD
                    //{
                    //    X1 = pageToCompare.Back.X,
                    //    Y1 = pageToCompare.Back.Y,
                    //    X2 = pageToCompare.Back.X + pageToCompare.GetClippedWByRotate(),
                    //    Y2 = pageToCompare.Back.Y + pageToCompare.GetClippedHByRotate()
                    //};

                    HashSet<CropMark> forDeleteCrops = new HashSet<CropMark>();

                    foreach (var cropMark in crops.CropMarks)
                    {
                        if (page == pageToCompare)
                        {
                            if (cropMark.IsFront)
                            {
                                if (pageRectBack.IsInsideMe(cropMark))
                                {
                                    forDeleteCrops.Add(cropMark);
                                }
                            }
                            else
                            {
                                if (pageRectFront.IsInsideMe(cropMark))
                                {
                                    forDeleteCrops.Add(cropMark);
                                }
                            }
                        }
                        else
                        {
                            if (pageRectFront.IsInsideMe(cropMark))
                            {
                                forDeleteCrops.Add(cropMark);
                            }
                            if (pageRectBack.IsInsideMe(cropMark))
                            {
                                forDeleteCrops.Add(cropMark);
                            }

                        }
                    }

                    crops.CropMarks = crops.CropMarks.Except(forDeleteCrops).ToList();
                }
            }
        }


    }
}
