using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Services.Impos.Binding.Loose.Sheetwise;
using JobSpace.Static.Pdf.Imposition.Services.Impos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services.Impos.Binding.Loose.WorkAndTurn
{
    public static class LooseBindingWorkAndTurn
    {
        public static TemplatePageContainer Impos(LooseBindingParameters parameters)
        {
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
            TemplatePageContainer templatePageContainer = new TemplatePageContainer();

            (double W, double H) printFieldFormat = GetPrintFieldFormat(parameters);
            TemplatePage masterPage = parameters.Sheet.MasterPage;

            double pageW = masterPage.W + masterPage.Margins.Left + masterPage.Margins.Right;
            double pageH = masterPage.H + masterPage.Margins.Bottom + masterPage.Margins.Top;

            int CntX = (int)(printFieldFormat.W / pageW);
            int CntY = (int)(printFieldFormat.H / pageH);

            if (CntX == 0 || CntY == 0) return templatePageContainer;
            //кількість по X має бути парною. Інакше віднімаємо 1, щоб було парна кількість
            if (CntX % 2 != 0) CntX--;

            //парна кількість не виходить
            if (CntX == 0) return templatePageContainer;

            double blockWidth = CntX * pageW;
            double blockHeight = CntY * pageH;

            double x, y;

            GetStartCoord(parameters, parameters.Sheet, blockWidth, blockHeight, out x, out y);

            LooseBindingSingleSide.PlacePages(templatePageContainer, masterPage, CntX / 2, CntY, x, y, 0, 1, 2);
            LooseBindingSheetwise.CalcBackCoord(parameters, templatePageContainer);

            LooseBindingSingleSide.ApplyFixes(parameters, templatePageContainer);

            return templatePageContainer;
        }

        private static void GetStartCoord(LooseBindingParameters parameters, TemplateSheet sheet, double blockWidth, double blockHeight, out double x, out double y)
        {
            x = sheet.SafeFields.GetMaxFieldW() + parameters.Xofs;
            y = sheet.SafeFields.Bottom + parameters.Yofs;
            if (parameters.IsCenterHorizontal)
                x = (sheet.W - sheet.SafeFields.Left - sheet.SafeFields.Right - blockWidth) / 2 + sheet.SafeFields.Left;

            if (parameters.IsCenterVertical)
                y = (sheet.H - sheet.SafeFields.Top - sheet.SafeFields.Bottom - blockHeight) / 2 + sheet.SafeFields.Bottom;
        }

        private static (double W, double H) GetPrintFieldFormat(LooseBindingParameters parameters)
        {
            var _sheet = parameters.Sheet;

            double sheetW = _sheet.W - _sheet.SafeFields.GetMaxFieldW() * 2;
            double sheetH = _sheet.H - _sheet.SafeFields.Top - _sheet.SafeFields.Bottom;
            if (!parameters.IsCenterHorizontal) sheetW -= parameters.Xofs;
            if (!parameters.IsCenterVertical) sheetH -= parameters.Yofs;
            return (sheetW, sheetH);
        }

        public static TemplatePageContainer LooseBindingRotated(LooseBindingParameters parameters)
        {
            TemplatePageContainer templatePageContainer = new TemplatePageContainer();

            (double W, double H) printFieldFormat = GetPrintFieldFormat(parameters);
            TemplatePage masterPage = parameters.Sheet.MasterPage;

            double pageW = masterPage.W + masterPage.Margins.Left + masterPage.Margins.Right;
            double pageH = masterPage.H + masterPage.Margins.Bottom + masterPage.Margins.Top;

            int CntX = (int)(printFieldFormat.W / pageH);
            int CntY = (int)(printFieldFormat.H / pageW);

            if (CntX == 0 || CntY == 0) return templatePageContainer;
            //кількість по X має бути парною. Інакше віднімаємо 1, щоб було парна кількість
            if (CntX % 2 != 0) CntX--;

            //парна кількість не виходить
            if (CntX == 0) return templatePageContainer;

            double blockWidth = CntX * pageH;
            double blockHeight = CntY * pageW;

            double x, y;

            GetStartCoord(parameters, parameters.Sheet, blockWidth, blockHeight, out x, out y);

            LooseBindingSingleSide.PlacePages(templatePageContainer, masterPage, CntX / 2, CntY, x, y, 270, 1, 2);
            LooseBindingSheetwise.CalcBackCoord(parameters, templatePageContainer);
           
            LooseBindingSingleSide.ApplyFixes(parameters, templatePageContainer);

            return templatePageContainer;
        }
        public static TemplatePageContainer LooseBindingMaxNormal(LooseBindingParameters parameters)
        {
            return CreateTemplatePageContainerWithExtraBlocks(parameters, 0);
        }
        public static TemplatePageContainer LooseBindingMaxRotated(LooseBindingParameters parameters)
        {
            return CreateTemplatePageContainerWithExtraBlocks(parameters, 270);
        }

        private static TemplatePageContainer CreateTemplatePageContainerWithExtraBlocks(LooseBindingParameters parameters, double angle)
        {
            TemplatePageContainer templatePageContainer = new TemplatePageContainer();

            (double W, double H) printFieldFormat = GetPrintFieldFormat(parameters);
            TemplatePage masterPage = parameters.Sheet.MasterPage;

            double pageW = angle == 0 ? masterPage.W + masterPage.Margins.Left + masterPage.Margins.Right : masterPage.H + masterPage.Margins.Bottom + masterPage.Margins.Top;
            double pageH = angle == 0 ? masterPage.H + masterPage.Margins.Bottom + masterPage.Margins.Top : masterPage.W + masterPage.Margins.Left + masterPage.Margins.Right;
            double rotatedPageW = pageH;
            double rotatedPageH = pageW;

            double frontSideWidth = printFieldFormat.W / 2;

            int CntX = (int)(frontSideWidth / pageW);
            int CntY = (int)(printFieldFormat.H / pageH);

            if (CntX == 0 || CntY == 0) return templatePageContainer;

            double blockWidth = CntX * pageW;
            double blockHeight = CntY * pageH;

            GetExtraBlocks(
                frontSideWidth,
                printFieldFormat.H,
                blockWidth,
                blockHeight,
                rotatedPageW,
                rotatedPageH,
                out int extraCntRightX,
                out int extraCntRightY,
                out int extraCntBottomX,
                out int extraCntBottomY);

            double layoutSideWidth = Math.Max(blockWidth + extraCntRightX * rotatedPageW, extraCntBottomX * rotatedPageW);
            double layoutHeight = Math.Max(blockHeight, Math.Max(extraCntRightY * rotatedPageH, blockHeight + extraCntBottomY * rotatedPageH));

            GetStartCoord(parameters, parameters.Sheet, layoutSideWidth * 2, layoutHeight, out double x, out double y);

            LooseBindingSingleSide.PlacePages(templatePageContainer, masterPage, CntX, CntY, x, y, angle, 1, 2);

            double extraAngle = angle == 0 ? 270 : 0;
            if (extraCntRightX > 0 && extraCntRightY > 0)
            {
                LooseBindingSingleSide.PlacePages(templatePageContainer, masterPage, extraCntRightX, extraCntRightY, x + blockWidth, y, extraAngle, 1, 2);
            }

            if (extraCntBottomX > 0 && extraCntBottomY > 0)
            {
                LooseBindingSingleSide.PlacePages(templatePageContainer, masterPage, extraCntBottomX, extraCntBottomY, x, y + blockHeight, extraAngle, 1, 2);
            }

            LooseBindingSheetwise.CalcBackCoord(parameters, templatePageContainer);
            LooseBindingSingleSide.ApplyFixes(parameters, templatePageContainer);

            return templatePageContainer;
        }

        private static void GetExtraBlocks(
            double frontSideWidth,
            double printFieldHeight,
            double blockWidth,
            double blockHeight,
            double rotatedPageW,
            double rotatedPageH,
            out int extraCntRightX,
            out int extraCntRightY,
            out int extraCntBottomX,
            out int extraCntBottomY)
        {
            CalculateExtraBlocks(frontSideWidth - blockWidth, printFieldHeight, rotatedPageW, rotatedPageH, out int rightFullX, out int rightFullY);
            CalculateExtraBlocks(blockWidth, printFieldHeight - blockHeight, rotatedPageW, rotatedPageH, out int bottomBaseX, out int bottomBaseY);

            CalculateExtraBlocks(frontSideWidth - blockWidth, blockHeight, rotatedPageW, rotatedPageH, out int rightBaseX, out int rightBaseY);
            CalculateExtraBlocks(frontSideWidth, printFieldHeight - blockHeight, rotatedPageW, rotatedPageH, out int bottomFullX, out int bottomFullY);

            int rightFirstTotal = rightFullX * rightFullY + bottomBaseX * bottomBaseY;
            int bottomFirstTotal = rightBaseX * rightBaseY + bottomFullX * bottomFullY;

            if (rightFirstTotal >= bottomFirstTotal)
            {
                extraCntRightX = rightFullX;
                extraCntRightY = rightFullY;
                extraCntBottomX = bottomBaseX;
                extraCntBottomY = bottomBaseY;
            }
            else
            {
                extraCntRightX = rightBaseX;
                extraCntRightY = rightBaseY;
                extraCntBottomX = bottomFullX;
                extraCntBottomY = bottomFullY;
            }
        }

        private static bool CalculateExtraBlocks(double extraWidth, double extraHeight, double pageW, double pageH, out int extraCntX, out int extraCntY)
        {
            extraCntX = (int)(extraWidth / pageW);
            extraCntY = (int)(extraHeight / pageH);
            return extraCntX > 0 && extraCntY > 0;
        }

    }
}
