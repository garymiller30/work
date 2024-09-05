using Job.Static.Pdf.Common;
using Job.Static.Pdf.Imposition.Models.Marks;
using Job.Static.Pdf.Imposition.Services;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Drawers.PDF.Marks.Text
{
    public static partial class DrawTextMarks
    {
        public static void Front(PDFlib p, MarksContainer marksContainer)
        {
            foreach (var mark in marksContainer.Text.Where(x => x.Parameters.IsFront))
            {

                string fillColor = mark.Color.IsSpot ? $"fillcolor={{spotname {{{mark.Color.Name}}} {mark.Color.Opasity / 100} {{cmyk {mark.Color.C / 100} {mark.Color.M} {mark.Color.Y} {mark.Color.K}}}}}" :
                    $"fillcolor={{cmyk {mark.Color.C / 100} {mark.Color.M} {mark.Color.Y} {mark.Color.K}}}";

                string txt = TextVariablesService.ReplaceToRealValues(mark.Text);

                p.fit_textline(txt, mark.Front.X * PdfHelper.mn, mark.Front.Y * PdfHelper.mn, $"fontname={mark.FontName} fontsize={mark.FontSize} {fillColor} orientate={Commons.Orientate[mark.Angle]}");
            }
        }
    }
}
