using JobSpace.Static.Pdf.Imposition.Drawers.Services.Screen;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Services.Impos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services.Impos.Binding.Loose.Sheetwise
{
    public static class LooseBindingSheetwise
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

            TemplatePageContainer tc;

            switch (parameters.BindingPlace)
            {
                case Binding.BindingPlaceEnum.Normal:

                    tc = LooseBindingNormal(parameters);
                    break;

                case Binding.BindingPlaceEnum.Rotated:

                    tc = LooseBindingRotated(parameters);
                    break;

                case Binding.BindingPlaceEnum.MaxNormal:
                    tc = LooseBindingMaxNormal(parameters);
                    break;

                case Binding.BindingPlaceEnum.MaxRotated:
                    tc = LooseBindingMaxRotated(parameters);
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (tc == null) throw new NotImplementedException();
            return tc;

        }

        static TemplatePageContainer LooseBindingNormal(LooseBindingParameters parameters)
        {
            TemplatePageContainer tc = LooseBindingSingleSide.CreateTemplatePageContainer(parameters, 0);

            CalcBackCoord(parameters, tc);

            return tc;
        }

        static TemplatePageContainer LooseBindingRotated(LooseBindingParameters parameters)
        {
            TemplatePageContainer tc = LooseBindingSingleSide.CreateTemplatePageContainer(parameters, 90);
            CalcBackCoord(parameters, tc);
            return tc;
        }

        static TemplatePageContainer LooseBindingMaxNormal(LooseBindingParameters parameters)
        {
            TemplatePageContainer tc = LooseBindingSingleSide.LooseBindingMaxNormal(parameters);
            CalcBackCoord(parameters, tc);
            return tc;
        }

        static TemplatePageContainer LooseBindingMaxRotated(LooseBindingParameters parameters)
        {
            TemplatePageContainer tc = LooseBindingSingleSide.LooseBindingMaxRotated(parameters);
            CalcBackCoord(parameters, tc);
            return tc;
        }

        public static void CalcBackCoord(LooseBindingParameters parameters, TemplatePageContainer tc)
        {
            var sheet = parameters.Sheet;

            foreach (TemplatePage tp in tc.TemplatePages)
            {
                // BackX = SheetW - FrontX - FrontW

                tp.Back.X = sheet.W - tp.Front.X - tp.GetClippedWByRotate();
                tp.Back.Y = tp.Front.Y;

                tp.Back.MasterIdx = 2;

                tp.Back.Angle = GetBackAngle(tp.Front.Angle);
            }
        }

        public static double GetBackAngle(double frontAngle)
        {
            switch (frontAngle)
            {
                case 0:
                case 180:
                    return frontAngle;
                case 90:
                case 270:
                    return (frontAngle + 180) % 360;
                default:
                    throw new NotImplementedException();
            }
        }

        

        public static void FixBackPagePosition(TemplateSheet sheet, TemplatePage selectedPreviewPage)
        {
            selectedPreviewPage.Back.X = sheet.W - selectedPreviewPage.Front.X - selectedPreviewPage.GetClippedWByRotate();
            selectedPreviewPage.Back.Y = selectedPreviewPage.Front.Y;
        }
    }
}
