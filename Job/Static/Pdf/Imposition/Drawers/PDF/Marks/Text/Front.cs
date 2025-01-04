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
        public static void Front(PDFlib p, MarksContainer marksContainer, bool foreground)
        {
            foreach (var mark in marksContainer.Text.Where(x => x.Parameters.IsFront && x.Enable == true && x.IsForeground == foreground))
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
                    p.fit_textline(token.Text, x, y, $"{fillColor} orientate={Commons.Orientate[mark.Angle]}");

                    double string_w = p.stringwidth(token.Text, font, mark.FontSize);

                    switch (mark.Angle)
                    {
                        case 0:
                            x += string_w;
                            break;
                        case 90:
                            y += string_w;
                            break;
                        case 180:
                            x -= string_w;
                            break;
                        case 270:
                            y -= string_w;
                            break;
                    }
                   
                    p.restore();
                }
            }

            marksContainer.Containers.ForEach(x => DrawTextMarks.Front(p, x,foreground));
        }
    }
}
