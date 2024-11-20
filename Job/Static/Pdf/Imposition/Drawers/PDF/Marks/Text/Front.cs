using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition.Drawers.PDF.Marks.Pdf;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Models.Marks;
using JobSpace.Static.Pdf.Imposition.Services;
using JobSpace.Static.Pdf.Imposition.Services.TextVariables;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.AxHost;

namespace JobSpace.Static.Pdf.Imposition.Drawers.PDF.Marks.Text
{
    public static partial class DrawTextMarks
    {
        public static void Front(PDFlib p, MarksContainer marksContainer)
        {
            foreach (var mark in marksContainer.Text.Where(x => x.Parameters.IsFront && x.Enable == true))
            {
                

                StringToken stringToken = new StringToken(mark);

                int font = p.load_font(mark.FontName, "auto", "");
                p.setfont(font, mark.FontSize);

                double x = mark.Front.X * PdfHelper.mn;
                double y = mark.Front.Y * PdfHelper.mn;

                foreach (var token in stringToken.Tokens)
                {
                    p.save();
                    if (mark.Color.IsOverprint)
                    {
                        int gstate = p.create_gstate("overprintmode=1 overprintfill=true overprintstroke=true");
                        p.set_gstate(gstate);
                    }

                    MarkColor color = token.Color;
                    string fillColor = color.IsSpot ? $"fillcolor={{spotname {{{color.Name}}} {color.Opasity / 100} {{cmyk {color.C / 100} {color.M / 100} {color.Y / 100} {color.K / 100}}}}}" :
                                                      $"fillcolor={{cmyk {color.C / 100} {color.M / 100} {color.Y / 100} {color.K / 100}}}";
                    if (mark.Angle == 0)
                    {
                        p.fit_textline(token.Text,x,y, $"{fillColor} orientate={Commons.Orientate[mark.Angle]}");
                        x += p.stringwidth(token.Text,font,mark.FontSize);
                    }
                    p.restore();
                }
                

                //if (mark.Color.IsOverprint)
                //{
                //    int gstate = p.create_gstate("overprintmode=1 overprintfill=true overprintstroke=true");
                //    p.set_gstate(gstate);
                //}

                //int font = p.load_font(mark.FontName,"auto","");
                //p.setfont(font,mark.FontSize);

                //string fillColor = mark.Color.IsSpot ? $"fillcolor={{spotname {{{mark.Color.Name}}} {mark.Color.Opasity / 100} {{cmyk {mark.Color.C / 100} {mark.Color.M/100} {mark.Color.Y/100} {mark.Color.K / 100}}}}}" :
                //    $"fillcolor={{cmyk {mark.Color.C / 100} {mark.Color.M / 100} {mark.Color.Y / 100} {mark.Color.K / 100}}}";

                //string txt = TextVariablesService.ReplaceToRealValues(mark.Text);

                //p.fit_textline(txt, mark.Front.X * PdfHelper.mn, mark.Front.Y * PdfHelper.mn, $"{fillColor} orientate={Commons.Orientate[mark.Angle]}");

                //p.restore();
            }

            marksContainer.Containers.ForEach(x => DrawTextMarks.Front(p, x));
        }
    }
}
