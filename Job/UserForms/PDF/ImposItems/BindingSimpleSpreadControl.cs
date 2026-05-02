using Interfaces.Pdf.Imposition;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Services.Impos;
using JobSpace.Static.Pdf.Imposition.Services.Impos.Binding.Loose.Perfecting;
using JobSpace.Static.Pdf.Imposition.Services.Impos.Binding.Loose.Sheetwise;
using JobSpace.Static.Pdf.Imposition.Services.Impos.Processes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF.ImposItems
{
    public partial class BindingSimpleSpreadControl : BindingSimpleControl, IBindControl
    {
        public BindingSimpleSpreadControl() : base()
        {
            InitializeComponent();
        }

        public override void Calc()
        {
            if (_imposParam.ControlsBind.Sheet == null || _imposParam.ControlsBind.MasterPage == null) return;

            var par = CreateParameters();

            variantNormal = CreateTemplatePageContainer(par, 0, false);
            label_0.Text = variantNormal.TemplatePages.Count.ToString();

            var sel = variantNormal;

            variantRotated = CreateTemplatePageContainer(par, 90, false);
            label_90.Text = variantRotated.TemplatePages.Count.ToString();
            if (sel.TemplatePages.Count < variantRotated.TemplatePages.Count)
                sel = variantRotated;

            variantMaxNormal = CreateTemplatePageContainer(par, 0, true);
            label_max_0.Text = variantMaxNormal.TemplatePages.Count.ToString();
            if (sel.TemplatePages.Count < variantMaxNormal.TemplatePages.Count)
                sel = variantMaxNormal;

            variantMaxRotated = CreateTemplatePageContainer(par, 90, true);
            label_max_90.Text = variantMaxRotated.TemplatePages.Count.ToString();
            if (sel.TemplatePages.Count < variantMaxRotated.TemplatePages.Count)
                sel = variantMaxRotated;

            _imposParam.ControlsBind.Sheet.TemplatePageContainer.SetTemplatePages(sel.TemplatePages);
            _imposParam.ControlsBind.UpdateSheet();
        }

        public override void RearangePages(List<PrintSheet> sheets, List<ImposRunPage> pages)
        {
            pages.ForEach(p => p.IsAssumed = false);

            if (sheets.Count == 0) return;

            int positionsOnFront = sheets[0].TemplatePageContainer.TemplatePages.Count;
            int spreadPairsOnFront = positionsOnFront / 2;
            if (spreadPairsOnFront == 0) return;

            int sides = sheets[0].SheetPlaceType == TemplateSheetPlaceType.SingleSide ? 1 : 2;
            int pagesOnPrintSheet = spreadPairsOnFront * 2 * sides;
            int totalPages = sheets.Count * pagesOnPrintSheet;

            for (int sheetIdx = 0; sheetIdx < sheets.Count; sheetIdx++)
            {
                var sheet = sheets[sheetIdx];
                var templatePages = sheet.TemplatePageContainer.TemplatePages;

                for (int pairIdx = 0; pairIdx < spreadPairsOnFront; pairIdx++)
                {
                    int spreadIdx = sheetIdx * spreadPairsOnFront + pairIdx;
                    int lowPage = spreadIdx * 2 + 1;
                    int highPage = totalPages - spreadIdx * 2;

                    int leftIdx = pairIdx * 2;
                    int rightIdx = leftIdx + 1;

                    AssignPage(templatePages[leftIdx], true, highPage, pages);
                    AssignPage(templatePages[rightIdx], true, lowPage, pages);

                    if (sides == 2)
                    {
                        AssignPage(templatePages[leftIdx], false, lowPage + 1, pages);
                        AssignPage(templatePages[rightIdx], false, highPage - 1, pages);
                    }
                    else
                    {
                        AssignPage(templatePages[leftIdx], false, 0, pages);
                        AssignPage(templatePages[rightIdx], false, 0, pages);
                    }
                }

                for (int pageIdx = spreadPairsOnFront * 2; pageIdx < templatePages.Count; pageIdx++)
                {
                    AssignPage(templatePages[pageIdx], true, 0, pages);
                    AssignPage(templatePages[pageIdx], false, 0, pages);
                }
            }
        }

        private void AssignPage(TemplatePage templatePage, bool isFront, int printIdx, List<ImposRunPage> pages)
        {
            var side = isFront ? templatePage.Front : templatePage.Back;

            side.PrintIdx = printIdx;
            side.AssignedRunPage = null;

            if (printIdx <= 0) return;

            int runPageIdx = printIdx - 1;
            if (runPageIdx >= pages.Count) return;

            ImposRunPage runPage = pages[runPageIdx];
            runPage.IsAssumed = true;
            runPage.IsValidFormat = ValidateFormat(runPage, templatePage);
            side.AssignedRunPage = runPage;
        }

        private TemplatePageContainer CreateTemplatePageContainer(LooseBindingParameters parameters, double primaryAngle, bool allowExtraSpreads)
        {
            var templatePageContainer = new TemplatePageContainer();
            var printField = GetPrintFieldFormat(parameters);
            var masterPage = parameters.Sheet.MasterPage;
            
            var primarySpread = GetSpreadFit(printField.W, printField.H, masterPage, primaryAngle);

            int cntX = primarySpread.CntX;
            int cntY = primarySpread.CntY;

            if (cntX == 0 || cntY == 0) return templatePageContainer;

            double blockWidth = cntX * primarySpread.SpreadW;
            double blockHeight = cntY * primarySpread.SpreadH;
            double layoutWidth = blockWidth;
            double layoutHeight = blockHeight;

            ExtraSpreadBlocks extraBlocks = new ExtraSpreadBlocks();
            double extraAngle = IsHorizontalSpread(primaryAngle) ? 90 : 0;

            if (allowExtraSpreads)
            {
                var extraSpreadFormat = GetSpreadFormat(masterPage, extraAngle);
                extraBlocks = GetExtraSpreadBlocks(
                    printField.W,
                    printField.H,
                    blockWidth,
                    blockHeight,
                    masterPage,
                    extraAngle);

                layoutWidth = Math.Max(blockWidth + extraBlocks.RightCntX * extraSpreadFormat.W, extraBlocks.BottomCntX * extraSpreadFormat.W);
                layoutHeight = Math.Max(blockHeight, Math.Max(extraBlocks.RightCntY * extraSpreadFormat.H, blockHeight + extraBlocks.BottomCntY * extraSpreadFormat.H));
            }

            double placementWidth = layoutWidth;
            double placementHeight = layoutHeight;

            if (parameters.Sheet.SheetPlaceType == TemplateSheetPlaceType.WorkAndTurn)
            {
                placementWidth *= 2;
            }

            if (parameters.Sheet.SheetPlaceType == TemplateSheetPlaceType.WorkAndTumble)
            {
                placementHeight *= 2;
            }

            GetStartCoord(parameters, parameters.Sheet, placementWidth, placementHeight, out double x, out double y);

            PlaceSpreads(templatePageContainer, masterPage, cntX, cntY, x, y, primaryAngle);

            if (allowExtraSpreads)
            {
                if (extraBlocks.RightCntX > 0 && extraBlocks.RightCntY > 0)
                {
                    PlaceSpreads(templatePageContainer, masterPage, extraBlocks.RightCntX, extraBlocks.RightCntY, x + blockWidth, y, extraAngle);
                }

                if (extraBlocks.BottomCntX > 0 && extraBlocks.BottomCntY > 0)
                {
                    PlaceSpreads(templatePageContainer, masterPage, extraBlocks.BottomCntX, extraBlocks.BottomCntY, x, y + blockHeight, extraAngle);
                }
            }

            FixBackPages(parameters.Sheet, templatePageContainer);
            ProcessFixBleeds.Front(templatePageContainer);
            FixInnerSpreadBleeds(templatePageContainer);

            return templatePageContainer;
        }

        private void PlaceSpreads(TemplatePageContainer templatePageContainer, TemplatePage masterPage, int cntX, int cntY, double x, double y, double angle)
        {
            var spreadFormat = GetSpreadFormat(masterPage, angle);
            double yOfs = y;

            for (int cy = 0; cy < cntY; cy++)
            {
                double xOfs = x;
                for (int cx = 0; cx < cntX; cx++)
                {
                    AddSpread(templatePageContainer, xOfs, yOfs, masterPage, angle);
                    xOfs += spreadFormat.W;
                }

                yOfs += spreadFormat.H;
            }
        }

        private void AddSpread(TemplatePageContainer templatePageContainer, double x, double y, TemplatePage masterPage, double angle)
        {
            int pageIdx = templatePageContainer.TemplatePages.Count + 1;
            var page = GetOrientedPage(masterPage, angle);

            double firstDrawX = x + page.BeforeX;
            double firstDrawY = y + page.BeforeY;
            double secondDrawX = IsHorizontalSpread(angle) ? firstDrawX + page.DrawW : firstDrawX;
            double secondDrawY = IsHorizontalSpread(angle) ? firstDrawY : firstDrawY + page.DrawH;

            AddPage(templatePageContainer, masterPage, angle, firstDrawX, firstDrawY, pageIdx, pageIdx + 1);
            AddPage(templatePageContainer, masterPage, angle, secondDrawX, secondDrawY, pageIdx + 2, pageIdx + 3);
        }

        private void AddPage(TemplatePageContainer templatePageContainer, TemplatePage masterPage, double angle, double drawX, double drawY, int frontIdx, int backIdx)
        {
            var origin = GetPageOriginFromDrawPosition(masterPage, angle, drawX, drawY);
            var templatePage = new TemplatePage(origin.X, origin.Y, masterPage.W, masterPage.H, angle);
            templatePage.Bleeds.SetDefault(masterPage.Bleeds.Default);
            templatePage.Margins.Set(masterPage.Margins);
            templatePage.Front.MasterIdx = frontIdx;
            templatePage.Back.MasterIdx = backIdx;
            templatePageContainer.AddPage(templatePage);
        }

        private void FixInnerSpreadBleeds(TemplatePageContainer templatePageContainer)
        {
            for (int i = 0; i + 1 < templatePageContainer.TemplatePages.Count; i += 2)
            {
                var firstPage = templatePageContainer.TemplatePages[i];
                var secondPage = templatePageContainer.TemplatePages[i + 1];

                switch (firstPage.Front.Angle)
                {
                    case 0:
                        firstPage.Bleeds.Right = 0;
                        secondPage.Bleeds.Left = 0;
                        break;
                    case 90:
                        firstPage.Bleeds.Right = 0;
                        secondPage.Bleeds.Left = 0;
                        break;
                    case 180:
                        firstPage.Bleeds.Left = 0;
                        secondPage.Bleeds.Right = 0;
                        break;
                    case 270:
                        firstPage.Bleeds.Left = 0;
                        secondPage.Bleeds.Right = 0;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private void FixBackPages(TemplateSheet sheet, TemplatePageContainer templatePageContainer)
        {
            if (sheet.SheetPlaceType == TemplateSheetPlaceType.SingleSide)
            {
                templatePageContainer.TemplatePages.ForEach(p => p.Back.MasterIdx = 0);
                return;
            }
            else if (sheet.SheetPlaceType == TemplateSheetPlaceType.WorkAndTurn)
            {
                templatePageContainer.TemplatePages.ForEach(p => p.Back.Angle = LooseBindingSheetwise.GetBackAngle(p.Front.Angle));
            }
            else if (sheet.SheetPlaceType == TemplateSheetPlaceType.WorkAndTumble)
            {
                templatePageContainer.TemplatePages.ForEach(p => p.Back.Angle = LooseBindingWorkAndTumble.GetBackAngle(p.Front.Angle));
            }

            ProcessFixPageBackPosition.FixPosition(sheet, templatePageContainer);
        }

        private ExtraSpreadBlocks GetExtraSpreadBlocks(double printFieldW, double printFieldH, double blockW, double blockH, TemplatePage masterPage, double extraAngle)
        {
            CalculateExtraBlocks(printFieldW - blockW, printFieldH, masterPage, extraAngle, out int rightFullX, out int rightFullY);
            CalculateExtraBlocks(blockW, printFieldH - blockH, masterPage, extraAngle, out int bottomBaseX, out int bottomBaseY);

            CalculateExtraBlocks(printFieldW - blockW, blockH, masterPage, extraAngle, out int rightBaseX, out int rightBaseY);
            CalculateExtraBlocks(printFieldW, printFieldH - blockH, masterPage, extraAngle, out int bottomFullX, out int bottomFullY);

            int rightFirstTotal = rightFullX * rightFullY + bottomBaseX * bottomBaseY;
            int bottomFirstTotal = rightBaseX * rightBaseY + bottomFullX * bottomFullY;

            if (rightFirstTotal >= bottomFirstTotal)
            {
                return new ExtraSpreadBlocks
                {
                    RightCntX = rightFullX,
                    RightCntY = rightFullY,
                    BottomCntX = bottomBaseX,
                    BottomCntY = bottomBaseY
                };
            }

            return new ExtraSpreadBlocks
            {
                RightCntX = rightBaseX,
                RightCntY = rightBaseY,
                BottomCntX = bottomFullX,
                BottomCntY = bottomFullY
            };
        }

        private void CalculateExtraBlocks(double width, double height, TemplatePage masterPage, double angle, out int cntX, out int cntY)
        {
            var fit = GetSpreadFit(width, height, masterPage, angle);
            cntX = fit.CntX;
            cntY = fit.CntY;
        }

        private void GetStartCoord(LooseBindingParameters parameters, TemplateSheet sheet, double blockWidth, double blockHeight, out double x, out double y)
        {
            if (sheet.SheetPlaceType == TemplateSheetPlaceType.WorkAndTurn)
            {
                double safeX = sheet.SafeFields.GetMaxFieldW();
                x = safeX + parameters.Xofs;

                if (parameters.IsCenterHorizontal)
                {
                    x = (sheet.W - safeX * 2 - blockWidth) / 2 + safeX;
                }
            }
            else
            {
                x = sheet.SafeFields.Left + parameters.Xofs;

                if (parameters.IsCenterHorizontal)
                {
                    x = (sheet.W - sheet.SafeFields.Left - sheet.SafeFields.Right - blockWidth) / 2 + sheet.SafeFields.Left;
                }
            }

            if (sheet.SheetPlaceType == TemplateSheetPlaceType.WorkAndTumble)
            {
                double safeY = sheet.SafeFields.GetMaxFieldH();
                y = safeY + parameters.Yofs;

                if (parameters.IsCenterVertical)
                {
                    y = (sheet.H - safeY * 2 - blockHeight) / 2 + safeY;
                }
            }
            else
            {
                y = sheet.SafeFields.Bottom + parameters.Yofs;

                if (parameters.IsCenterVertical)
                {
                    y = (sheet.H - sheet.SafeFields.Top - sheet.SafeFields.Bottom - blockHeight) / 2 + sheet.SafeFields.Bottom;
                }
            }

            x = Math.Round(x, 1);
            y = Math.Round(y, 1);
        }

        private (double W, double H) GetPrintFieldFormat(LooseBindingParameters parameters)
        {
            var sheet = parameters.Sheet;
            double sheetW;
            double sheetH;

            switch (sheet.SheetPlaceType)
            {
                case TemplateSheetPlaceType.SingleSide:
                case TemplateSheetPlaceType.Sheetwise:
                    sheetW = sheet.W - sheet.SafeFields.Left - sheet.SafeFields.Right;
                    sheetH = sheet.H - sheet.SafeFields.Top - sheet.SafeFields.Bottom;
                    break;
                case TemplateSheetPlaceType.WorkAndTurn:
                    sheetW = (sheet.W - sheet.SafeFields.GetMaxFieldW() * 2) / 2;
                    sheetH = sheet.H - sheet.SafeFields.Top - sheet.SafeFields.Bottom;
                    break;
                case TemplateSheetPlaceType.WorkAndTumble:
                    sheetW = sheet.W - sheet.SafeFields.Left - sheet.SafeFields.Right;
                    sheetH = (sheet.H - sheet.SafeFields.GetMaxFieldH() * 2) / 2;
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (!parameters.IsCenterHorizontal) sheetW -= parameters.Xofs;
            if (!parameters.IsCenterVertical) sheetH -= parameters.Yofs;

            return (sheetW, sheetH);
        }

        private (double W, double H) GetSpreadFormat(TemplatePage masterPage, double angle)
        {
            var page = GetOrientedPage(masterPage, angle);
            if (IsHorizontalSpread(angle))
            {
                return (page.BeforeX + page.DrawW * 2 + page.AfterX, page.BeforeY + page.DrawH + page.AfterY);
            }

            return (page.BeforeX + page.DrawW + page.AfterX, page.BeforeY + page.DrawH * 2 + page.AfterY);
        }

        private SpreadFit GetSpreadFit(double width, double height, TemplatePage masterPage, double angle)
        {
            if (width <= 0 || height <= 0) return new SpreadFit();

            var page = GetOrientedPage(masterPage, angle);
            int cntX;
            int cntY;

            if (IsHorizontalSpread(angle))
            {
                int pagesX = (int)((width - page.BeforeX - page.AfterX) / page.DrawW);
                if (pagesX % 2 != 0) pagesX--;

                cntX = pagesX / 2;
                cntY = (int)(height / (page.BeforeY + page.DrawH + page.AfterY));
            }
            else
            {
                cntX = (int)(width / (page.BeforeX + page.DrawW + page.AfterX));

                int pagesY = (int)((height - page.BeforeY - page.AfterY) / page.DrawH);
                if (pagesY % 2 != 0) pagesY--;

                cntY = pagesY / 2;
            }

            if (cntX < 0) cntX = 0;
            if (cntY < 0) cntY = 0;

            var spreadFormat = GetSpreadFormat(masterPage, angle);

            return new SpreadFit
            {
                CntX = cntX,
                CntY = cntY,
                SpreadW = spreadFormat.W,
                SpreadH = spreadFormat.H
            };
        }

        private bool IsHorizontalSpread(double angle)
        {
            return angle == 0 || angle == 180;
        }

        private OrientedPage GetOrientedPage(TemplatePage page, double angle)
        {
            var result = new OrientedPage();
            if (angle == 0 || angle == 180)
            {
                result.DrawW = page.W;
                result.DrawH = page.H;
            }
            else
            {
                result.DrawW = page.H;
                result.DrawH = page.W;
            }

            switch (angle)
            {
                case 0:
                    result.BeforeX = page.Margins.Left;
                    result.AfterX = page.Margins.Right;
                    result.BeforeY = page.Margins.Bottom;
                    result.AfterY = page.Margins.Top;
                    break;
                case 90:
                    result.BeforeX = page.Margins.Top;
                    result.AfterX = page.Margins.Bottom;
                    result.BeforeY = page.Margins.Left;
                    result.AfterY = page.Margins.Right;
                    break;
                case 180:
                    result.BeforeX = page.Margins.Right;
                    result.AfterX = page.Margins.Left;
                    result.BeforeY = page.Margins.Top;
                    result.AfterY = page.Margins.Bottom;
                    break;
                case 270:
                    result.BeforeX = page.Margins.Bottom;
                    result.AfterX = page.Margins.Top;
                    result.BeforeY = page.Margins.Right;
                    result.AfterY = page.Margins.Left;
                    break;
                default:
                    throw new NotImplementedException();
            }

            return result;
        }

        private (double X, double Y) GetPageOriginFromDrawPosition(TemplatePage page, double angle, double drawX, double drawY)
        {
            switch (angle)
            {
                case 0:
                    return (drawX - page.Margins.Left, drawY - page.Margins.Bottom);
                case 90:
                    return (drawX - page.Margins.Top, drawY - page.Margins.Left);
                case 180:
                    return (drawX - page.Margins.Right, drawY - page.Margins.Top);
                case 270:
                    return (drawX - page.Margins.Bottom, drawY - page.Margins.Right);
                default:
                    throw new NotImplementedException();
            }
        }

        private class ExtraSpreadBlocks
        {
            public int RightCntX { get; set; }
            public int RightCntY { get; set; }
            public int BottomCntX { get; set; }
            public int BottomCntY { get; set; }
        }

        private class SpreadFit
        {
            public int CntX { get; set; }
            public int CntY { get; set; }
            public double SpreadW { get; set; }
            public double SpreadH { get; set; }
        }

        private class OrientedPage
        {
            public double DrawW { get; set; }
            public double DrawH { get; set; }
            public double BeforeX { get; set; }
            public double AfterX { get; set; }
            public double BeforeY { get; set; }
            public double AfterY { get; set; }
        }
    }
}
