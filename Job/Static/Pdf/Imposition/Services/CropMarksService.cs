using JobSpace.Static.Pdf.Imposition.Models.Marks;
using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobSpace.Static.Pdf.Imposition.Drawers.Services.Screen;
using Interfaces.Pdf.Imposition;

namespace JobSpace.Static.Pdf.Imposition.Services
{
    public static class CropMarksService
    {
        public static double delta = 0.3;
        private const double ClipEpsilon = 0.000001;

        public static void FixCropMarks(TemplateSheet sheet,GlobalImposParameters imposParam)
        {
            if (sheet == null) return;

            switch (sheet.SheetPlaceType)
            {
                case TemplateSheetPlaceType.SingleSide:
                    FixCropMarksFront(sheet.TemplatePageContainer, imposParam);
                    break;
                case TemplateSheetPlaceType.Sheetwise:
                    FixCropMarksFront(sheet.TemplatePageContainer, imposParam);
                    FixCropMarksBack(sheet, imposParam);
                    break;
                case TemplateSheetPlaceType.WorkAndTurn:
                    FixCropMarksWorkAndTurn(sheet, imposParam);
                    break;
                case TemplateSheetPlaceType.WorkAndTumble:
                    FixCropMarksWorkAndTumble(sheet, imposParam);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        static void FixCropMarksFront(TemplatePageContainer templateContainer, GlobalImposParameters imposParam)
        {
            RecalcCropsFront(templateContainer, imposParam);
            RemoveCropsFront(templateContainer);
        }

        static void FixCropMarksBack(TemplateSheet sheet, GlobalImposParameters imposParam)
        {

            RecalcCropsBack(sheet, imposParam);
            RemoveCropsBack(sheet);
        }

        static void FixCropMarksWorkAndTurn(TemplateSheet sheet, GlobalImposParameters imposParam)
        {

            RecalcCropsFront(sheet.TemplatePageContainer, imposParam);
            RecalcCropsBack(sheet, imposParam);
            RemoveCropsWorkAndTurn(sheet);
        }

        static void FixCropMarksWorkAndTumble(TemplateSheet sheet, GlobalImposParameters imposParam)
        {
            if (sheet == null) return;
            RecalcCropsFront(sheet.TemplatePageContainer, imposParam);
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

                        RectangleD pageRect = GetPageRectWithBleedsBack(sheet, pageToCompare);
                        ClipCrops(crops, cropMark => cropMark.IsBack, pageRect);
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
                        RectangleD pageRect = GetPageRectWithBleedsFront(pageToCompare);
                        ClipCrops(crops, cropMark => cropMark.IsFront, pageRect);
                    }
                }
            }
        }

        static void RecalcCropsFront(TemplatePageContainer templateContainer, GlobalImposParameters imposParam)
        {
            foreach (var page in templateContainer.TemplatePages)
            {

                CropMarksController crops = page.CropMarksController;

                var delList = crops.CropMarks.Where(c => c.IsFront).ToList();

                crops.CropMarks = crops.CropMarks.Except(delList).ToList();

                double len = imposParam.ImposTools.CropMarksParameters.Len;
                double dist = imposParam.ImposTools.CropMarksParameters.Distance;

                if (len == 0) continue;
                
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

                        CropMark cropMark = new CropMarkCreator(x, y).From(direction[idx].X * dist, direction[idx].Y * dist).To(direction[idx].X * len, direction[idx].Y * len);
                        crops.CropMarks.Add(cropMark);

                    }
                }
            }
        }

        static void RecalcCropsBack(TemplateSheet sheet, GlobalImposParameters imposParam)
        {

            foreach (var page in sheet.TemplatePageContainer.TemplatePages)
            {

                CropMarksController crops = page.CropMarksController;

                var delList = crops.CropMarks.Where(c => c.IsBack).ToList();

                crops.CropMarks = crops.CropMarks.Except(delList).ToList();

                double len = imposParam.ImposTools.CropMarksParameters.Len;
                double dist = imposParam.ImposTools.CropMarksParameters.Distance;

                if (len == 0) continue;

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

                if (len == 0) continue;

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

                    RectangleD pageRectFront = GetPageRectWithBleedsFront(pageToCompare);
                    RectangleD pageRectBack = GetPageRectWithBleedsBack(sheet, pageToCompare);

                    if (page == pageToCompare)
                    {
                        ClipCrops(crops, cropMark => cropMark.IsFront, pageRectBack);
                        ClipCrops(crops, cropMark => cropMark.IsBack, pageRectFront);
                    }
                    else
                    {
                        ClipCrops(crops, cropMark => true, pageRectFront);
                        ClipCrops(crops, cropMark => true, pageRectBack);
                    }
                }
            }
        }

        private static RectangleD GetPageRectWithBleedsFront(TemplatePage page)
        {
            var rect = ScreenDrawCommons.GetPageDraw(page, page.Front);
            double left = ScreenDrawCommons.GetLeftBleedByAngleFront(page, page.Front);
            double bottom = ScreenDrawCommons.GetBottomBleedByAngleFront(page, page.Front);

            double horizontalBleed = GetHorizontalBleed(page, page.Front);
            double verticalBleed = GetVerticalBleed(page, page.Front);

            return new RectangleD
            {
                X1 = rect.page_x - left - delta,
                Y1 = rect.page_y - bottom - delta,
                X2 = rect.page_x + rect.page_w + horizontalBleed - left + delta,
                Y2 = rect.page_y + rect.page_h + verticalBleed - bottom + delta
            };
        }

        private static RectangleD GetPageRectWithBleedsBack(TemplateSheet sheet, TemplatePage page)
        {
            var rect = ScreenDrawCommons.GetPageDrawBack(sheet, page, page.Back);
            double left = ScreenDrawCommons.GetLeftBleedByAngleBack(sheet, page, page.Back);
            double bottom = ScreenDrawCommons.GetBottomBleedByAngleBack(sheet, page, page.Back);

            double horizontalBleed = GetHorizontalBleed(page, page.Back);
            double verticalBleed = GetVerticalBleed(page, page.Back);

            return new RectangleD
            {
                X1 = rect.page_x - left - delta,
                Y1 = rect.page_y - bottom - delta,
                X2 = rect.page_x + rect.page_w + horizontalBleed - left + delta,
                Y2 = rect.page_y + rect.page_h + verticalBleed - bottom + delta
            };
        }

        private static double GetHorizontalBleed(TemplatePage page, PageSide side)
        {
            if (side.Angle == 0 || side.Angle == 180)
            {
                return page.Bleeds.Left + page.Bleeds.Right;
            }

            return page.Bleeds.Top + page.Bleeds.Bottom;
        }

        private static double GetVerticalBleed(TemplatePage page, PageSide side)
        {
            if (side.Angle == 0 || side.Angle == 180)
            {
                return page.Bleeds.Top + page.Bleeds.Bottom;
            }

            return page.Bleeds.Left + page.Bleeds.Right;
        }

        private static void ClipCrops(CropMarksController crops, Func<CropMark, bool> cropSelector, RectangleD pageRect)
        {
            var result = new List<CropMark>();

            foreach (CropMark cropMark in crops.CropMarks)
            {
                if (cropSelector(cropMark))
                {
                    result.AddRange(CutCropMarkByRectangle(cropMark, pageRect));
                }
                else
                {
                    result.Add(cropMark);
                }
            }

            crops.CropMarks = result;
        }

        private static IEnumerable<CropMark> CutCropMarkByRectangle(CropMark cropMark, RectangleD rect)
        {
            if (!TryGetIntersectionInterval(cropMark, rect, out double tIn, out double tOut))
            {
                yield return cropMark;
                yield break;
            }

            if (tIn > ClipEpsilon)
            {
                yield return CreateCropMarkSegment(cropMark, 0, tIn);
            }

            if (tOut < 1 - ClipEpsilon)
            {
                yield return CreateCropMarkSegment(cropMark, tOut, 1);
            }
        }

        private static bool TryGetIntersectionInterval(CropMark cropMark, RectangleD rect, out double tIn, out double tOut)
        {
            tIn = 0;
            tOut = 1;

            double dx = cropMark.To.X - cropMark.From.X;
            double dy = cropMark.To.Y - cropMark.From.Y;

            return Clip(-dx, cropMark.From.X - rect.X1, ref tIn, ref tOut)
                && Clip(dx, rect.X2 - cropMark.From.X, ref tIn, ref tOut)
                && Clip(-dy, cropMark.From.Y - rect.Y1, ref tIn, ref tOut)
                && Clip(dy, rect.Y2 - cropMark.From.Y, ref tIn, ref tOut)
                && tIn <= tOut;
        }

        private static bool Clip(double p, double q, ref double tIn, ref double tOut)
        {
            if (Math.Abs(p) < ClipEpsilon)
            {
                return q >= 0;
            }

            double r = q / p;

            if (p < 0)
            {
                if (r > tOut) return false;
                if (r > tIn) tIn = r;
            }
            else
            {
                if (r < tIn) return false;
                if (r < tOut) tOut = r;
            }

            return true;
        }

        private static CropMark CreateCropMarkSegment(CropMark source, double fromT, double toT)
        {
            return new CropMark
            {
                From = Interpolate(source.From, source.To, fromT),
                To = Interpolate(source.From, source.To, toT),
                Enable = source.Enable,
                IsFront = source.IsFront,
                IsBack = source.IsBack
            };
        }

        private static PointD Interpolate(PointD from, PointD to, double t)
        {
            return new PointD
            {
                X = from.X + (to.X - from.X) * t,
                Y = from.Y + (to.Y - from.Y) * t
            };
        }


    }
}
