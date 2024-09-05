using Job.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Services.Impos
{
    public static class LooseBindingService
    {

        public static TemplatePageContainer CalcOptimal2PageProduct(LooseBindingParameters parameters)
        {
            TemplateSheet sheet = parameters.Sheet;

            TemplatePageContainer templatePageContainer = new TemplatePageContainer();
            TemplatePage tPage = new TemplatePage(parameters.PageW, parameters.PageH);
            tPage.Clip.Set(parameters.Bleed);

            AbstractPlaceVariant nonRotated = new PlaceVariantNonRotated(parameters);
            AbstractPlaceVariant rotated = new PlaceVariantRotated(parameters);

            AbstractPlaceVariant selVariant;

            if (nonRotated.Total > rotated.Total)
            {
                selVariant = nonRotated;
            }
            else if (nonRotated.Total < rotated.Total)
            {
                selVariant = rotated;
            }
            else if (nonRotated.FreeSpace >= rotated.FreeSpace)
            {
                selVariant = nonRotated;
            }
            else
            {
                selVariant = rotated;
            }

            tPage.Angle = selVariant.IsRotated ? 90 : 0;

            double x = sheet.SafeFields.Left + parameters.Xofs;
            double y = sheet.SafeFields.Bottom + parameters.Yofs;

            if (parameters.IsCenterHorizontal)
                x = (sheet.W - sheet.SafeFields.Left - sheet.SafeFields.Right - selVariant.BlockWidth) / 2;

            if (parameters.IsCenterVertical)
                y = (sheet.H - sheet.SafeFields.Top - sheet.SafeFields.Top - selVariant.BlockHeight) / 2;

            double xOfs = x;

            int backIdx = parameters.IsSingleSide ? 0 : 2;

            for (int cy = 0; cy < selVariant.CntY; cy++)
            {
                TemplatePage templatePage = new TemplatePage();
                for (int cx = 0; cx < selVariant.CntX; cx++)
                {
                    templatePage = new TemplatePage(xOfs, y, tPage.W, tPage.H, tPage.Angle);
                    templatePage.Clip.Set(parameters.Bleed);
                    templatePage.FrontIdx = 1;
                    templatePage.BackIdx = backIdx;


                    templatePageContainer.AddPage(templatePage);
                    xOfs += templatePage.GetClippedWByRotate();
                }
                xOfs = x;
                y += templatePage.GetClippedHByRotate();
            }


            return templatePageContainer;
        }



    }

    class PlaceVariantNonRotated : AbstractPlaceVariant
    {
        public PlaceVariantNonRotated(LooseBindingParameters bindingParameters) : base(bindingParameters)
        {
            Calc(Parameters.PageW + Parameters.Bleed * 2, Parameters.PageH + Parameters.Bleed * 2);
        }
    }


    class PlaceVariantRotated : AbstractPlaceVariant
    {
        public PlaceVariantRotated(LooseBindingParameters bindingParameters) : base(bindingParameters)
        {
            IsRotated = true;
            Calc(Parameters.PageH + Parameters.Bleed * 2, Parameters.PageW + Parameters.Bleed * 2);
        }
    }


    abstract class AbstractPlaceVariant
    {
        public int CntX { get; set; }
        public int CntY { get; set; }

        public bool IsRotated { get; set; }

        public int Total => CntX * CntY;

        protected LooseBindingParameters Parameters { get; set; }

        public double BlockWidth { get; set; }
        public double BlockHeight { get; set; }

        public double FreeSpace => (Parameters.Sheet.W - BlockWidth) * (Parameters.Sheet.H - BlockHeight);

        public AbstractPlaceVariant(LooseBindingParameters bindingParameters)
        {
            Parameters = bindingParameters;
        }

        protected void Calc(double w, double h)
        {
            var _sheet = Parameters.Sheet;

            double sheetW = _sheet.W - _sheet.SafeFields.Left - _sheet.SafeFields.Right;
            double sheetH = _sheet.H - _sheet.SafeFields.Top - _sheet.SafeFields.Bottom;

            if (!Parameters.IsCenterHorizontal) sheetW += Parameters.Xofs;

            if (!Parameters.IsCenterVertical) sheetH += Parameters.Yofs;

            CntX = (int)(sheetW / w);
            CntY = (int)(sheetH / h);

            BlockWidth = CntX * w;
            BlockHeight = CntY * h;
        }

    }
}
