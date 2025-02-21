using JobSpace.Static.Pdf.Imposition.Drawers.Services.Screen;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Services.Impos.Processes;
using System;
using System.Collections.Generic;
using System.Threading;

namespace JobSpace.Static.Pdf.Imposition.Services.Impos
{
    public static class LooseBindingSingleSide
    {
        public static TemplatePageContainer Impos(LooseBindingParameters parameters)
        {
            
            if (parameters.IsOneCut)
            {
                parameters.Sheet.MasterPage.Margins.Set(0d);
            }
           

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

        public static TemplatePageContainer CreateTemplatePageContainer(LooseBindingParameters parameters, double angle)
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
                double extraWidth = 0;
                if (isExtraBottom)
                {
                    int cntX = (int)((sheet.W - sheet.SafeFields.Left - sheet.SafeFields.Right) / pageH);
                    extraWidth = cntX * pageH;
                }

                if (extraWidth > blockWidth)
                {
                    x = (sheet.W - sheet.SafeFields.Left - sheet.SafeFields.Right - extraWidth) / 2 + sheet.SafeFields.Left;
                }
                else
                {
                    x = (sheet.W - sheet.SafeFields.Left - sheet.SafeFields.Right - blockWidth - extraX) / 2 + sheet.SafeFields.Left;
                }

            }

            if (parameters.IsCenterVertical)
            {
                double extraY = isExtraBottom ? extraCntBottomY * pageW : 0;
                double extraHeight = 0;
                if (isExtraRight)
                {
                    int cntY = (int)((sheet.H - sheet.SafeFields.Bottom - sheet.SafeFields.Top)/ pageW);
                    extraHeight = cntY * pageW;
                }
                if (extraHeight > blockHeight)
                {
                    y = (sheet.H - sheet.SafeFields.Top - sheet.SafeFields.Bottom - extraHeight) / 2 + sheet.SafeFields.Bottom;
                }
                else
                {
                    y = (sheet.H - sheet.SafeFields.Top - sheet.SafeFields.Bottom - blockHeight - extraY) / 2 + sheet.SafeFields.Bottom;
                }
               
            }
        }

        public static void ApplyFixes(LooseBindingParameters parameters, TemplatePageContainer templatePageContainer)
        {
            ProcessFixBleeds.Front(templatePageContainer);
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
            templatePage.Front.MasterIdx = frontIdx;
            templatePage.Back.MasterIdx = backIdx;
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
    }
}