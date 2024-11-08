using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition.Drawers.PDF.Marks.Pdf;
using JobSpace.Static.Pdf.Imposition.Models.Marks;
using JobSpace.Static.Pdf.Imposition.Services;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Drawers.PDF.Marks.Text
{
    public static partial class DrawTextMarks
    {
        public static void Back(PDFlib p, MarksContainer marksContainer)
        {
            foreach (var mark in marksContainer.Text.Where(x => x.Parameters.IsBack && x.Enable))
            {
                p.save();

                if (mark.Color.IsOverprint)
                {
                    int gstate = p.create_gstate("overprintmode=1 overprintfill=true overprintstroke=true");
                    p.set_gstate(gstate);
                }

                string fillColor = mark.Color.IsSpot ? $"fillcolor={{spotname {{{mark.Color.Name}}} {mark.Color.Opasity / 100} {{cmyk {mark.Color.C / 100} {mark.Color.M / 100} {mark.Color.Y / 100} {mark.Color.K / 100}}}}}" :
                    $"fillcolor={{cmyk {mark.Color.C / 100} {mark.Color.M / 100} {mark.Color.Y / 100} {mark.Color.K / 100}}}";

                string txt = TextVariablesService.ReplaceToRealValues(mark.Text);

                double x = mark.Back.X;
                if (mark.Parameters.IsBackMirrored)
                {
                    //TODO: хз, що тут робити. Поки не знадобилося
                }

                p.fit_textline(txt, x * PdfHelper.mn, mark.Back.Y * PdfHelper.mn, $"fontname={mark.FontName} fontsize={mark.FontSize} {fillColor} orientate={Commons.Orientate[mark.Angle]}");
                p.restore();
            }

            marksContainer.Containers.ForEach(x => DrawTextMarks.Back(p, x));
        }
    }
}
