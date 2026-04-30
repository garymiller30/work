using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services.Impos.Binding.Loose.Perfecting
{
    public static class LooseBindingWorkAndTumble
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

        private static TemplatePageContainer LooseBindingMaxRotated(LooseBindingParameters parameters)
        {
            return CreateTemplatePageContainerWithExtraBlocks(parameters, 90);
        }

        private static TemplatePageContainer LooseBindingMaxNormal(LooseBindingParameters parameters)
        {
            return CreateTemplatePageContainerWithExtraBlocks(parameters, 0);
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

            double frontSideHeight = printFieldFormat.H / 2;

            int CntX = (int)(printFieldFormat.W / pageW);
            int CntY = (int)(frontSideHeight / pageH);

            if (CntX == 0 || CntY == 0) return templatePageContainer;

            double blockWidth = CntX * pageW;
            double blockHeight = CntY * pageH;

            GetExtraBlocks(
                printFieldFormat.W,
                frontSideHeight,
                blockWidth,
                blockHeight,
                rotatedPageW,
                rotatedPageH,
                out int extraCntRightX,
                out int extraCntRightY,
                out int extraCntBottomX,
                out int extraCntBottomY);

            double layoutWidth = Math.Max(blockWidth + extraCntRightX * rotatedPageW, extraCntBottomX * rotatedPageW);
            double layoutSideHeight = Math.Max(blockHeight, Math.Max(extraCntRightY * rotatedPageH, blockHeight + extraCntBottomY * rotatedPageH));

            GetStartCoord(parameters, parameters.Sheet, layoutWidth, layoutSideHeight * 2, out double x, out double y);

            LooseBindingSingleSide.PlacePages(templatePageContainer, masterPage, CntX, CntY, x, y, angle, 1, 0);

            double extraAngle = angle == 0 ? 90 : 0;
            if (extraCntRightX > 0 && extraCntRightY > 0)
            {
                LooseBindingSingleSide.PlacePages(templatePageContainer, masterPage, extraCntRightX, extraCntRightY, x + blockWidth, y, extraAngle, 1, 0);
            }

            if (extraCntBottomX > 0 && extraCntBottomY > 0)
            {
                LooseBindingSingleSide.PlacePages(templatePageContainer, masterPage, extraCntBottomX, extraCntBottomY, x, y + blockHeight, extraAngle, 1, 0);
            }

            CalcBackCoord(parameters, templatePageContainer);
            LooseBindingSingleSide.ApplyFixes(parameters, templatePageContainer);

            return templatePageContainer;
        }

        private static void GetExtraBlocks(
            double printFieldWidth,
            double frontSideHeight,
            double blockWidth,
            double blockHeight,
            double rotatedPageW,
            double rotatedPageH,
            out int extraCntRightX,
            out int extraCntRightY,
            out int extraCntBottomX,
            out int extraCntBottomY)
        {
            CalculateExtraBlocks(printFieldWidth - blockWidth, frontSideHeight, rotatedPageW, rotatedPageH, out int rightFullX, out int rightFullY);
            CalculateExtraBlocks(blockWidth, frontSideHeight - blockHeight, rotatedPageW, rotatedPageH, out int bottomBaseX, out int bottomBaseY);

            CalculateExtraBlocks(printFieldWidth - blockWidth, blockHeight, rotatedPageW, rotatedPageH, out int rightBaseX, out int rightBaseY);
            CalculateExtraBlocks(printFieldWidth, frontSideHeight - blockHeight, rotatedPageW, rotatedPageH, out int bottomFullX, out int bottomFullY);

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

        private static TemplatePageContainer LooseBindingRotated(LooseBindingParameters parameters)
        {
            TemplatePageContainer templatePageContainer = new TemplatePageContainer();

            (double W, double H) printFieldFormat = GetPrintFieldFormat(parameters);
            TemplatePage masterPage = parameters.Sheet.MasterPage;

            double pageW = masterPage.W + masterPage.Margins.Left + masterPage.Margins.Right;
            double pageH = masterPage.H + masterPage.Margins.Bottom + masterPage.Margins.Top;

            int CntX = (int)(printFieldFormat.W / pageH);
            int CntY = (int)(printFieldFormat.H / pageW);

            if (CntX == 0 || CntY == 0) return templatePageContainer;
            //кількість по Y має бути парною. Інакше віднімаємо 1, щоб було парна кількість
            if (CntY % 2 != 0) CntY--;
            //парна кількість не виходить
            if (CntY == 0) return templatePageContainer;

            double blockWidth = CntX * pageH;
            double blockHeight = CntY * pageW;
            double x, y;

            GetStartCoord(parameters, parameters.Sheet, blockWidth, blockHeight, out x, out y);
            LooseBindingSingleSide.PlacePages(templatePageContainer, masterPage, CntX, CntY / 2, x, y, 90, 1, 0);
            CalcBackCoord(parameters, templatePageContainer);

            LooseBindingSingleSide.ApplyFixes(parameters, templatePageContainer);
            return templatePageContainer;
        }

        private static TemplatePageContainer LooseBindingNormal(LooseBindingParameters parameters)
        {
            TemplatePageContainer templatePageContainer = new TemplatePageContainer();
            
            (double W, double H) printFieldFormat = GetPrintFieldFormat(parameters);
            TemplatePage masterPage = parameters.Sheet.MasterPage;
            
            double pageW = masterPage.W + masterPage.Margins.Left + masterPage.Margins.Right;
            double pageH = masterPage.H + masterPage.Margins.Bottom + masterPage.Margins.Top;
            
            int CntX = (int)(printFieldFormat.W / pageW);
            int CntY = (int)(printFieldFormat.H / pageH);

            if (CntX == 0 || CntY == 0) return templatePageContainer;

            //кількість по Y має бути парною. Інакше віднімаємо 1, щоб було парна кількість
            if (CntY % 2 != 0) CntY--;
            //парна кількість не виходить
            if (CntY == 0) return templatePageContainer;

            double blockWidth = CntX * pageW;
            double blockHeight = CntY * pageH;

            double x, y;

            GetStartCoord(parameters, parameters.Sheet, blockWidth, blockHeight, out x, out y);
            
            LooseBindingSingleSide.PlacePages(templatePageContainer, masterPage, CntX, CntY/2, x, y, 0, 1, 0);
            CalcBackCoord(parameters, templatePageContainer);
            
            LooseBindingSingleSide.ApplyFixes(parameters, templatePageContainer);
            
            return templatePageContainer;
        }

        private static void CalcBackCoord(LooseBindingParameters parameters, TemplatePageContainer tc)
        {
            var sheet = parameters.Sheet;

            foreach (TemplatePage tp in tc.TemplatePages)
            {
                tp.Back.X = tp.Front.X;
                tp.Back.Y = sheet.H - tp.Front.Y - tp.GetClippedHByRotate();

                tp.Back.MasterIdx = 2;

                tp.Back.Angle = GetBackAngle(tp.Front.Angle);
            }
        }

        public static double GetBackAngle(double angle)
        {
            switch (angle)
            {
                case 0:
                case 180:
                    return (angle + 180) % 360;
                case 90:
                case 270:
                    return angle;
                default:
                    throw new NotImplementedException();
            }
        }

        private static void GetStartCoord(LooseBindingParameters parameters, TemplateSheet sheet, double blockWidth, double blockHeight, out double x, out double y)
        {
            x = sheet.SafeFields.Left + parameters.Xofs;
            y = sheet.SafeFields.GetMaxFieldH() + parameters.Yofs;
            if (parameters.IsCenterHorizontal)
                x = (sheet.W - sheet.SafeFields.Left - sheet.SafeFields.Right - blockWidth) / 2 + sheet.SafeFields.Left;

            if (parameters.IsCenterVertical)
                y = (sheet.H - sheet.SafeFields.GetMaxFieldH()*2 - blockHeight) / 2 + sheet.SafeFields.Bottom;
        }

        private static (double W, double H) GetPrintFieldFormat(LooseBindingParameters parameters)
        {
            var _sheet = parameters.Sheet;

            double sheetW = _sheet.W - _sheet.SafeFields.Left - _sheet.SafeFields.Right;
            double sheetH = _sheet.H - _sheet.SafeFields.GetMaxFieldH() * 2;
            if (!parameters.IsCenterHorizontal) sheetW -= parameters.Xofs;
            if (!parameters.IsCenterVertical) sheetH -= parameters.Yofs;
            return (sheetW, sheetH);
        }

        public static void FixBackPagePosition(TemplateSheet sheet, TemplatePage page)
        {
            page.Back.X = page.Front.X;
            page.Back.Y = sheet.H - page.Front.Y - page.GetClippedHByRotate();
        }
    }
}
