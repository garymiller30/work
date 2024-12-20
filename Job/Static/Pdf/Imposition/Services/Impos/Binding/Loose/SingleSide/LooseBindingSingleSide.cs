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
            TemplatePageContainer templatePageContainer = new TemplatePageContainer();

            (double W, double H) printFieldFormat = GetPrintFieldFormat(parameters);

            TemplatePage masterPage = parameters.Sheet.MasterPage;

            double pageW = masterPage.W + masterPage.Margins.Left + masterPage.Margins.Right;
            double pageH = masterPage.H + masterPage.Margins.Bottom + masterPage.Margins.Top;

            int CntX = (int)(printFieldFormat.W / pageW);
            int CntY = (int)(printFieldFormat.H / pageH);

            if (CntX == 0 || CntY == 0) return templatePageContainer;

            double blockWidth = CntX * pageW;
            double blockHeight = CntY * pageH;

            double x, y;

            GetStartCoord(parameters, parameters.Sheet, blockWidth, blockHeight, out x, out y);
            PlacePages(templatePageContainer, masterPage, CntX, CntY, x, y,0);

            if (parameters.IsOneCut)
            {
                FixBleedsFront(templatePageContainer);
            }

            CropMarksService.FixCropMarksFront(templatePageContainer);

            return templatePageContainer;


        }

        private static void PlacePages(TemplatePageContainer templatePageContainer, TemplatePage masterPage, int CntX, int CntY, double x, double y,double angle)
        {
            double xOfs = x;
            double yOfs = y;

            for (int cy = 0; cy < CntY; cy++)
            {
                double tp_height = 0;
                for (int cx = 0; cx < CntX; cx++)
                {
                    (double w, double h) tp = AddPage(templatePageContainer, xOfs, yOfs, masterPage, angle, 1, 0);
                    xOfs += tp.w;
                    tp_height = tp.h;
                }
                xOfs = x;
                yOfs += tp_height;
            }
        }

        static (double w, double h) AddPage(TemplatePageContainer templatePageContainer, double xOfs, double yOfs, TemplatePage masterPage, double angle, int frontIdx,int backIdx)
        {
            TemplatePage templatePage = new TemplatePage(xOfs, yOfs, masterPage.W, masterPage.H, angle);
            templatePage.Bleeds.SetDefault(masterPage.Bleeds.Default);
            templatePage.Margins.Set(masterPage.Margins);
            templatePage.MasterFrontIdx = frontIdx;
            templatePage.MasterBackIdx = backIdx;
            templatePageContainer.AddPage(templatePage);

            return (templatePage.GetClippedWByRotate(), templatePage.GetClippedHByRotate());
        }


        private static void GetStartCoord(LooseBindingParameters parameters, TemplateSheet sheet, double BlockWidth, double BlockHeight, out double x, out double y)
        {
            x = sheet.SafeFields.Left + parameters.Xofs;
            y = sheet.SafeFields.Bottom + parameters.Yofs;
            if (parameters.IsCenterHorizontal)
                x = (sheet.W - sheet.SafeFields.Left - sheet.SafeFields.Right - BlockWidth) / 2 + sheet.SafeFields.Left;

            if (parameters.IsCenterVertical)
                y = (sheet.H - sheet.SafeFields.Top - sheet.SafeFields.Bottom - BlockHeight) / 2 + sheet.SafeFields.Bottom;
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

        public static TemplatePageContainer LooseBindingRotated(LooseBindingParameters parameters)
        {
            TemplatePageContainer templatePageContainer = new TemplatePageContainer();

            (double W, double H) printFieldFormat = GetPrintFieldFormat(parameters);

            TemplatePage masterPage = parameters.Sheet.MasterPage;

            double pageW = masterPage.W + masterPage.Margins.Left + masterPage.Margins.Right;
            double pageH = masterPage.H + masterPage.Margins.Bottom + masterPage.Margins.Top;

            var CntX = (int)(printFieldFormat.W / pageH);
            var CntY = (int)(printFieldFormat.H / pageW);

            if (CntX == 0 || CntY == 0) return templatePageContainer;

            var blockWidth = CntX * pageH;
            var blockHeight = CntY * pageW;

            double x, y;

            GetStartCoord(parameters, parameters.Sheet, blockWidth, blockHeight, out x, out y);

            PlacePages(templatePageContainer,masterPage,CntX,CntY,x,y,90);

            if (parameters.IsOneCut)
            {
                FixBleedsFront(templatePageContainer);
            }

            CropMarksService.FixCropMarksFront(templatePageContainer);

            return templatePageContainer;
        }

        public static TemplatePageContainer LooseBindingMaxNormal(LooseBindingParameters parameters)
        {
            TemplatePageContainer templatePageContainer = new TemplatePageContainer();

            bool isExtraRight = false;
            bool isExtraBottom = false;

            var sheet = parameters.Sheet;

            var printFieldFormat = GetPrintFieldFormat(parameters);

            TemplatePage masterPage = parameters.Sheet.MasterPage;

            var pageW = masterPage.W + masterPage.Margins.Left + masterPage.Margins.Right;
            var pageH = masterPage.H + masterPage.Margins.Bottom + masterPage.Margins.Top;

            var CntX = (int)(printFieldFormat.W / pageW);
            var CntY = (int)(printFieldFormat.H / pageH);

            if (CntX == 0 || CntY == 0) return templatePageContainer;

            var blockWidth = CntX * pageW;
            var blockHeight = CntY * pageH;

            //calc extra blocks right
            //1. отримати кількість вільного місця
            var extraRightW = printFieldFormat.W - blockWidth;
            var extraRightH = printFieldFormat.H;

            int extraCntRightX = (int)(extraRightW / pageH);
            int extraCntRightY = (int)(extraRightH / pageW);

            if (extraCntRightX > 0 && extraCntRightY > 0)
            {
                isExtraRight = true;
            }

            //calc extra blocks bottom
            var extraBottomW = printFieldFormat.W;
            var extraBottomH = printFieldFormat.H - blockHeight;

            var extraCntBottomX = (int)(extraBottomW / pageH);
            var extraCntBottomY = (int)(extraBottomH / pageW);

            if (extraCntBottomX > 0 && extraCntBottomY > 0)
            {
                isExtraBottom = true;
            }

            double x = sheet.SafeFields.Left + parameters.Xofs;
            double y = sheet.SafeFields.Bottom + parameters.Yofs;

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

            PlacePages(templatePageContainer,masterPage,CntX,CntY,x,y,0);

            if (isExtraRight)
            {
                PlacePages(templatePageContainer,masterPage, extraCntRightX, extraCntRightY, x + blockWidth,y,90);
            }

            if (isExtraBottom)
            {
                PlacePages(templatePageContainer,masterPage, extraCntBottomX, extraCntBottomY,x, y + pageH,90);
            }

            if (parameters.IsOneCut)
            {
                FixBleedsFront(templatePageContainer);
            }

            CropMarksService.FixCropMarksFront(templatePageContainer);

            return templatePageContainer;
        }

        public static TemplatePageContainer LooseBindingMaxRotated(LooseBindingParameters parameters)
        {
            TemplatePageContainer templatePageContainer = new TemplatePageContainer();


            bool isExtraRight = false;
            bool isExtraBottom = false;


            var sheet = parameters.Sheet;

            var printFieldFormat = GetPrintFieldFormat(parameters);

            TemplatePage masterPage = parameters.Sheet.MasterPage;

            var pageW = masterPage.H + masterPage.Margins.Bottom + masterPage.Margins.Top;
            var pageH = masterPage.W + masterPage.Margins.Left + masterPage.Margins.Right;

            var CntX = (int)(printFieldFormat.W / pageW);
            var CntY = (int)(printFieldFormat.H / pageH);

            if (CntX == 0 || CntY == 0) return templatePageContainer;

            var BlockWidth = CntX * pageW;
            var BlockHeight = CntY * pageH;

            //calc extra blocks right
            //1. отримати кількість вільного місця
            var extraRightW = printFieldFormat.W - BlockWidth;
            var extraRightH = printFieldFormat.H;

            int extraCntRightX = (int)(extraRightW / pageH);
            int extraCntRightY = (int)(extraRightH / pageW);

            if (extraCntRightX > 0 && extraCntRightY > 0)
            {
                isExtraRight = true;
            }

            //calc extra blocks bottom
            var extraBottomW = printFieldFormat.W;
            var extraBottomH = printFieldFormat.H - BlockHeight;

            var extraCntBottomX = (int)(extraBottomW / pageH);
            var extraCntBottomY = (int)(extraBottomH / pageW);

            if (extraCntBottomX > 0 && extraCntBottomY > 0)
            {
                isExtraBottom = true;
            }


            double x = sheet.SafeFields.Left + parameters.Xofs;
            double y = sheet.SafeFields.Bottom + parameters.Yofs;


            if (parameters.IsCenterHorizontal)
            {
                double extraX = isExtraRight ? extraCntRightX * pageH : 0;
                x = (sheet.W - sheet.SafeFields.Left - sheet.SafeFields.Right - BlockWidth - extraX) / 2 + sheet.SafeFields.Left;

            }


            if (parameters.IsCenterVertical)
            {
                double extraY = isExtraBottom ? extraCntBottomY * pageW : 0;
                y = (sheet.H - sheet.SafeFields.Top - sheet.SafeFields.Bottom - BlockHeight - extraY) / 2 + sheet.SafeFields.Bottom;
            }

            PlacePages(templatePageContainer,masterPage,CntX,CntY,x,y,90);

            if (isExtraRight)
            {
                PlacePages(templatePageContainer,masterPage,extraCntRightX,extraCntRightY, x + BlockWidth, y,0);
            }

            if (isExtraBottom)
            {
                PlacePages(templatePageContainer,masterPage, extraCntBottomX, extraCntBottomY,x, y + BlockHeight,0);
            }

            if (parameters.IsOneCut)
            {
                FixBleedsFront(templatePageContainer);
            }

            CropMarksService.FixCropMarksFront(templatePageContainer);

            return templatePageContainer;
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

                        List<RectangleD> rects = new List<RectangleD>(){
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