using JobSpace.Static.Pdf.Imposition.Models.Marks;
using JobSpace.Static.Pdf.Imposition.Models;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobSpace.Static.Pdf.Common;

namespace JobSpace.Static.Pdf.Imposition.Drawers.PDF.Marks.Pdf
{
    public static partial class DrawPdfMarks
    {
        public static void Back(PDFlib p, TemplateSheet sheet, MarksContainer marksContainer, bool foreground)
        {
            foreach (var mark in marksContainer.Pdf.Where(x => x.GetMarkSideBack(sheet.SheetPlaceType) && x.Enable && x.IsForeground == foreground))
            {
                using (PDFLIBDocument doc = new PDFLIBDocument(p, mark.File.FileName))
                {
                    int scaleX = 1;
                    int scaleY = 1;

                    if (mark.Parameters.IsBackMirrored)
                    {
                        if (mark.Angle == 90 || mark.Angle == 270)
                        {
                            scaleY = -1;
                        }
                        else
                        {
                            scaleX = -1;
                        }
                    }

                    var clipBox = mark.ClipBoxBack;

                    string clipping_optlist = $"matchbox={{clipping={{{clipBox.Left * PdfHelper.mn} {clipBox.Bottom * PdfHelper.mn} {clipBox.Right * PdfHelper.mn} {clipBox.Top * PdfHelper.mn}}}}} orientate={JobSpace.Static.Pdf.Imposition.Drawers.PDF.Commons.Orientate[mark.Angle]} scale={{{scaleX} {scaleY}}}";

                    doc.fit_pdi_page(1, mark.Back.X, mark.Back.Y, clipping_optlist);// $"orientate={Commons.Orientate[mark.Angle]} scale={{{scaleX} {scaleY}}}");
                }
            }

            marksContainer.Containers.ForEach(x => DrawPdfMarks.Back(p,sheet, x,foreground));

        }


    }
}
