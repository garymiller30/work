using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;

namespace JobSpace.Static.Pdf.Imposition.Services.Impos
{
    public static class LooseBindingSingleSide
    {
        public static TemplatePageContainer Impos(LooseBindingParameters parameters)
        {
            if (parameters.IsOneCut) parameters.Sheet.MasterPage.Margins.Set(0d);

            switch (parameters.BindingPlace)
            {
                case Binding.BindingPlaceEnum.Normal:
                    return LooseBindingNormal(parameters);

                case Binding.BindingPlaceEnum.Rotated:
                    return LooseBindingRotated(parameters);

                case Binding.BindingPlaceEnum.MaxNormal:
                    return LooseBindingMaxNormal(parameters);

                case Binding.BindingPlaceEnum.MaxRotated:
                    return LooseBindingMaxRotated(parameters);

                default:
                    throw new NotImplementedException();
            }
        }

        public static TemplatePageContainer LooseBindingNormal(LooseBindingParameters parameters)
        {
            return CreateTemplatePageContainer(parameters, 0);
        }

        public static TemplatePageContainer LooseBindingRotated(LooseBindingParameters parameters)
        {
            return CreateTemplatePageContainer(parameters, 90);
        }

        public static TemplatePageContainer LooseBindingMaxNormal(LooseBindingParameters parameters)
        {
            return CreateTemplatePageContainerWithExtraBlocks(parameters, 0);
        }

        public static TemplatePageContainer LooseBindingMaxRotated(LooseBindingParameters parameters)
        {
            return CreateTemplatePageContainerWithExtraBlocks(parameters, 90);
        }

        private static TemplatePageContainer CreateTemplatePageContainer(LooseBindingParameters parameters, double angle)
        {
            TemplatePageContainer templatePageContainer = new TemplatePageContainer();
            (double W, double H) printFieldFormat = GetPrintFieldFormat(parameters);
            TemplatePage masterPage = parameters.Sheet.MasterPage;

            double pageW = angle == 0 ? masterPage.W + masterPage.Margins.Left + masterPage.Margins.Right : masterPage.H + masterPage.Margins.Bottom + masterPage.Margins.Top;
            double pageH = angle == 0 ? masterPage.H + masterPage.Margins.Bottom + masterPage.Margins.Top : masterPage.W + masterPage.Margins.Left + masterPage.Margins.Right;

            int CntX = (int)(printFieldFormat.W / pageW);
            int CntY = (int)(printFieldFormat.H / pageH);

            if (CntX == 0 || CntY == 0) return templatePageContainer;

            double blockWidth = CntX * pageW;
            double blockHeight = CntY * pageH;

            GetStartCoord(parameters, parameters.Sheet, blockWidth, blockHeight, out double x, out double y);
            PlacePages(templatePageContainer, masterPage, CntX, CntY, x, y, angle, 1, 0);
            ApplyFixes(parameters, templatePageContainer);

            return templatePageContainer;
        }

        private static TemplatePageContainer CreateTemplatePageContainerWithExtraBlocks(LooseBindingParameters parameters, double angle)
        {
            TemplatePageContainer templatePageContainer = new TemplatePageContainer();
            (double W, double H) printFieldFormat = GetPrintFieldFormat(parameters);
            TemplatePage masterPage = parameters.Sheet.MasterPage;

            double pageW = angle == 0 ? masterPage.W + masterPage.Margins.Left + masterPage.Margins.Right : masterPage.H + masterPage.Margins.Bottom + masterPage.Margins.Top;
            double pageH = angle == 0 ? masterPage.H + masterPage.Margins.Bottom + masterPage.Margins.Top : masterPage.W + masterPage.Margins.Left + masterPage.Margins.Right;

            int CntX = (int)(printFieldFormat.W / pageW);
            int CntY = (int)(printFieldFormat.H / pageH);

            if (CntX == 0 || CntY == 0) return templatePageContainer;

            double blockWidth = CntX * pageW;
            double blockHeight = CntY * pageH;

            bool isExtraRight = CalculateExtraBlocks(printFieldFormat.W - blockWidth, printFieldFormat.H, pageH, pageW, out int extraCntRightX, out int extraCntRightY);
            bool isExtraBottom = CalculateExtraBlocks(printFieldFormat.W, printFieldFormat.H - blockHeight, pageH, pageW, out int extraCntBottomX, out int extraCntBottomY);

            GetStartCoord(parameters, parameters.Sheet, blockWidth, blockHeight, out double x, out double y, isExtraRight, isExtraBottom, extraCntRightX, extraCntBottomY, pageH, pageW);
            PlacePages(templatePageContainer, masterPage, CntX, CntY, x, y, angle, 1, 0);

            if (isExtraRight)
            {
                PlacePages(templatePageContainer, masterPage, extraCntRightX, extraCntRightY, x + blockWidth, y, angle == 0 ? 90 : 0, 1, 0);
            }

            if (isExtraBottom)
            {
                PlacePages(templatePageContainer, masterPage, extraCntBottomX, extraCntBottomY, x, y + blockHeight, angle == 0 ? 90 : 0, 1, 0);
            }

            ApplyFixes(parameters, templatePageContainer);

            return templatePageContainer;
        }

        private static bool CalculateExtraBlocks(double extraWidth, double extraHeight, double pageH, double pageW, out int extraCntX, out int extraCntY)
        {
            extraCntX = (int)(extraWidth / pageH);
            extraCntY = (int)(extraHeight / pageW);
            return extraCntX > 0 && extraCntY > 0;
        }

        private static void GetStartCoord(LooseBindingParameters parameters, TemplateSheet sheet, double blockWidth, double blockHeight, out double x, out double y, bool isExtraRight = false, bool isExtraBottom = false, int extraCntRightX = 0, int extraCntBottomY = 0, double pageH = 0, double pageW = 0)
        {
            x = sheet.SafeFields.Left + parameters.Xofs;
            y = sheet.SafeFields.Bottom + parameters.Yofs;

            if (parameters.IsCenterHorizontal)
            {
                double extraX = isExtraRight ? extraCntRightX * pageH : 0;
                x = (sheet.W - sheet.SafeFields.Left - sheet.SafeFields.Right - blockWidth - extraX) / 2 + sheet.SafeFields.Left;
            }

            if (parameters.IsCenterVertical)
            {
                double extraY = isExtraBottom ? extraCntBottomY * pageW : 0;
                y = (sheet.H - sheet.SafeFields.Top - sheet.SafeFields.Bottom - blockHeight - extraY) / 2 + sheet.SafeFields.Bottom;
            }
        }

        public static void ApplyFixes(LooseBindingParameters parameters, TemplatePageContainer templatePageContainer)
        {
            if (parameters.IsOneCut)
            {
                FixBleedsFront(templatePageContainer);
            }

            CropMarksService.FixCropMarksFront(templatePageContainer);
        }

        public static void PlacePages(TemplatePageContainer templatePageContainer, TemplatePage masterPage, int CntX, int CntY, double x, double y, double angle, int frontIdx, int backIdx)
        {
            double xOfs = x;
            double yOfs = y;

            for (int cy = 0; cy < CntY; cy++)
            {
                double tp_height = 0;
                for (int cx = 0; cx < CntX; cx++)
                {
                    (double w, double h) tp = AddPage(templatePageContainer, xOfs, yOfs, masterPage, angle, frontIdx, backIdx);
                    xOfs += tp.w;
                    tp_height = tp.h;
                }
                xOfs = x;
                yOfs += tp_height;
            }
        }

        private static (double w, double h) AddPage(TemplatePageContainer templatePageContainer, double xOfs, double yOfs, TemplatePage masterPage, double angle, int frontIdx, int backIdx)
        {
            TemplatePage templatePage = new TemplatePage(xOfs, yOfs, masterPage.W, masterPage.H, angle);
            templatePage.Bleeds.SetDefault(masterPage.Bleeds.Default);
            templatePage.Margins.Set(masterPage.Margins);
            templatePage.MasterFrontIdx = frontIdx;
            templatePage.MasterBackIdx = backIdx;
            templatePageContainer.AddPage(templatePage);

            return (templatePage.GetClippedWByRotate(), templatePage.GetClippedHByRotate());
        }

        private static (double W, double H) GetPrintFieldFormat(LooseBindingParameters parameters)
        {
            var _sheet = parameters.Sheet;
            double sheetW = _sheet.W - _sheet.SafeFields.Left - _sheet.SafeFields.Right;
            double sheetH = _sheet.H - _sheet.SafeFields.Top - _sheet.SafeFields.Bottom;
            if (!parameters.IsCenterHorizontal) sheetW -= parameters.Xofs;
            if (!parameters.IsCenterVertical) sheetH -= parameters.Yofs;
            return (sheetW, sheetH);
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

                        List<RectangleD> rects = new List<RectangleD>() {
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