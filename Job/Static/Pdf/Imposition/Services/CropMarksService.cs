using JobSpace.Static.Pdf.Imposition.Models.Marks;
using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services
{
    public static class CropMarksService
    {
        public static void FixCropMarksFront(TemplatePageContainer templateContainer)
        {
            if (templateContainer == null) return;

            RecalcCropsFront(templateContainer);
            RemoveCropsFront(templateContainer);

        }

        public static void FixCropMarksBack(TemplateSheet sheet)
        {
            if (sheet == null) return;
            RecalcCropsBack(sheet);
            RemoveCropsBack(sheet);
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
                        RectangleD pageRect = new RectangleD
                        {
                            X1 = sheet.W - pageToCompare.Front.X - pageToCompare.GetClippedWByRotate(),
                            Y1 = pageToCompare.Front.Y,
                            X2 = sheet.W - pageToCompare.Front.X,
                            Y2 = pageToCompare.Front.Y + pageToCompare.GetClippedHByRotate()
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
                        RectangleD pageRect = new RectangleD
                        {
                            X1 = pageToCompare.Front.X,
                            Y1 = pageToCompare.Front.Y,
                            X2 = pageToCompare.Front.X + pageToCompare.GetClippedWByRotate(),
                            Y2 = pageToCompare.Front.Y + pageToCompare.GetClippedHByRotate()
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
                AnchorOfset[] ofsets = crops.GetAnchorOfsetsBack(page, sheet, page.Front.Angle);

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

        public static void FixCropMarksWorkAndTurn(TemplateSheet sheet)
        {
            if (sheet == null) return;
            RecalcCropsFront(sheet.TemplatePageContainer);
            RecalcCropsBack(sheet);
            RemoveCropsWorkAndTurn(sheet);
        }

        private static void RemoveCropsWorkAndTurn(TemplateSheet sheet)
        {
            foreach (TemplatePage page in sheet.TemplatePageContainer.TemplatePages)
            {
                CropMarksController crops = page.CropMarksController;

                foreach (TemplatePage pageToCompare in sheet.TemplatePageContainer.TemplatePages)
                {
                    RectangleD pageRectFront = new RectangleD
                    {
                        X1 = pageToCompare.Front.X,
                        Y1 = pageToCompare.Front.Y,
                        X2 = pageToCompare.Front.X + pageToCompare.GetClippedWByRotate(),
                        Y2 = pageToCompare.Front.Y + pageToCompare.GetClippedHByRotate()
                    };

                    RectangleD pageRectBack = new RectangleD
                    {
                        X1 = pageToCompare.Back.X,
                        Y1 = pageToCompare.Back.Y,
                        X2 = pageToCompare.Back.X + pageToCompare.GetClippedWByRotate(),
                        Y2 = pageToCompare.Back.Y + pageToCompare.GetClippedHByRotate()
                    };

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
