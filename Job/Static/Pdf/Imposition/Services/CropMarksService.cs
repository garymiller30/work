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
            RecalcCropsFront(templateContainer);
            RemoveCropsFront(templateContainer);

        }

        public static void FixCropMarksBack(TemplateSheet sheet)
        {
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
                            X1 = sheet.W - pageToCompare.X - pageToCompare.GetClippedWByRotate(),
                            Y1 = pageToCompare.Y,
                            X2 = sheet.W - pageToCompare.X,
                            Y2 = pageToCompare.Y + pageToCompare.GetClippedHByRotate()
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
                            X1 = pageToCompare.X,
                            Y1 = pageToCompare.Y,
                            X2 = pageToCompare.X + pageToCompare.GetClippedWByRotate(),
                            Y2 = pageToCompare.Y + pageToCompare.GetClippedHByRotate()
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

                CropDirection[] direction = crops.GetDrawDirectionFront(page.Angle);
                AnchorOfset[] ofsets = crops.GetAnchorOfsetsFront(page, page.Angle);

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

                CropDirection[] direction = crops.GetDrawDirectionBack(page.Angle);
                AnchorOfset[] ofsets = crops.GetAnchorOfsetsBack(page, sheet, page.Angle);

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

    }
}
