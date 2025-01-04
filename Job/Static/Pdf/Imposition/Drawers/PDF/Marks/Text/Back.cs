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

namespace JobSpace.Static.Pdf.Imposition.Drawers.PDF.Marks.Text
{
    public static partial class DrawTextMarks
    {
        public static void Back(PDFlib p, MarksContainer marksContainer, bool foreground)
        {
            foreach (var mark in marksContainer.Text.Where(x => x.Parameters.IsBack && x.Enable && x.IsForeground == foreground))
            {
                StringToken stringToken = new StringToken(mark);
                int font = p.load_font(mark.FontName, "auto", "");
                p.setfont(font, mark.FontSize);

                double x = mark.Back.X * PdfHelper.mn;
                double y = mark.Back.Y * PdfHelper.mn;

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

                //p.save();

                //if (mark.Color.IsOverprint)
                //{
                //    int gstate = p.create_gstate("overprintmode=1 overprintfill=true overprintstroke=true");
                //    p.set_gstate(gstate);
                //}

                //string fillColor = mark.Color.IsSpot ? $"fillcolor={{spotname {{{mark.Color.Name}}} {mark.Color.Opasity / 100} {{cmyk {mark.Color.C / 100} {mark.Color.M / 100} {mark.Color.Y / 100} {mark.Color.K / 100}}}}}" :
                //    $"fillcolor={{cmyk {mark.Color.C / 100} {mark.Color.M / 100} {mark.Color.Y / 100} {mark.Color.K / 100}}}";

                //string txt = TextVariablesService.ReplaceToRealValues(mark.Text);

                //double x = mark.Back.X;
                //if (mark.Parameters.IsBackMirrored)
                //{
                //    //TODO: хз, що тут робити. Поки не знадобилося
                //}

                //p.fit_textline(txt, x * PdfHelper.mn, mark.Back.Y * PdfHelper.mn, $"fontname={mark.FontName} fontsize={mark.FontSize} {fillColor} orientate={Commons.Orientate[mark.Angle]}");
                //p.restore();
            }

            marksContainer.Containers.ForEach(x => DrawTextMarks.Back(p, x, foreground));
        }
    }
}
