using JobSpace.Static.Pdf.Imposition.Models;
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
            
            if (parameters.IsOneCut)
            {
                parameters.Sheet.MasterPage.Margins.Set(0d);
            }
            else
            {
                parameters.Sheet.MasterPage.SetMarginsLikeBleed();
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

            LooseBindingSingleSide.PlacePages(templatePageContainer, masterPage, CntX / 2, CntY, x, y, 0, 1, 0);

            x += blockWidth / 2;
            LooseBindingSingleSide.PlacePages(templatePageContainer, masterPage, CntX / 2, CntY, x, y, 0, 2, 0);

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

            LooseBindingSingleSide.PlacePages(templatePageContainer, masterPage, CntX / 2, CntY, x, y, 270, 1, 0);

            x += blockWidth / 2;
            LooseBindingSingleSide.PlacePages(templatePageContainer, masterPage, CntX / 2, CntY, x, y, 90, 2, 0);

            LooseBindingSingleSide.ApplyFixes(parameters, templatePageContainer);

            return templatePageContainer;
        }
        public static TemplatePageContainer LooseBindingMaxNormal(LooseBindingParameters parameters)
        {
            //TODO: реалізувати логіку розкидання на лист зі своїм зворотом 
            TemplatePageContainer templatePageContainer = new TemplatePageContainer();
            return templatePageContainer;
        }
        public static TemplatePageContainer LooseBindingMaxRotated(LooseBindingParameters parameters)
        {
            //TODO: реалізувати логіку розкидання на лист зі своїм зворотом 
            TemplatePageContainer templatePageContainer = new TemplatePageContainer();
            return templatePageContainer;
        }

    }
}
