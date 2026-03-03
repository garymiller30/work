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
        public static void Back(PDFlib p, MarksContainer marksContainer, bool foreground, GlobalImposParameters imposParameters)
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
                    MarkColor color = token.Color;

                    if (color.IsProofColor())
                    {
                        p.begin_layer(imposParameters.PdfDrawParameters.LayerProof);
                    }
                    else
                    {
                        p.begin_layer(imposParameters.PdfDrawParameters.LayerPrint);
                    }

                    p.save();
                    if (mark.Color.IsOverprint)
                    {
                        int gstate = p.create_gstate("overprintmode=1 overprintfill=true overprintstroke=true");
                        p.set_gstate(gstate);
                    }

                    
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

            marksContainer.Containers.ForEach(x => DrawTextMarks.Back(p, x, foreground, imposParameters));

            p.begin_layer(imposParameters.PdfDrawParameters.LayerPrint);
        }
    }
}
