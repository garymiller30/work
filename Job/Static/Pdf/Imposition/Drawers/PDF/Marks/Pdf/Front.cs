using JobSpace.Static.Pdf.Imposition.Models.Marks;
using JobSpace.Static.Pdf.Imposition.Models;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Drawers.PDF.Marks.Pdf
{
    public static partial class DrawPdfMarks
    {
        public static void Front(PDFlib p, TemplateSheet sheet, MarksContainer marksContainer,bool foreground)
        {
            foreach (var mark in marksContainer.Pdf.Where(x => x.GetMarkSideFront(sheet.SheetPlaceType) && x.Enable == true && x.IsForeground == foreground))
            {
                using (PDFLIBDocument doc = new PDFLIBDocument(p, mark.File.FileName))
                {
                    doc.fit_pdi_page(mark);
                    //doc.fit_pdi_page(1, mark.Front.X, mark.Front.Y, $"orientate={Commons.Orientate[mark.Angle]}");
                }
            }

            marksContainer.Containers.ForEach(x=> DrawPdfMarks.Front(p, sheet, x,foreground));

        }
    }
}
