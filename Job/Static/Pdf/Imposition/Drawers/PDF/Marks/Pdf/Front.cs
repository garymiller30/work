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
        public static void Front(PDFlib p, MarksContainer marksContainer)
        {
            foreach (var mark in marksContainer.Pdf.Where(x => x.Parameters.IsFront && x.Enable == true))
            {
                using (PDFLIBDocument doc = new PDFLIBDocument(p, mark.File.FileName))
                {
                    doc.fit_pdi_page(1, mark.Front.X, mark.Front.Y, $"orientate={Commons.Orientate[mark.Angle]}");
                }
            }

            marksContainer.Containers.ForEach(x=> DrawPdfMarks.Front(p,x));

        }
    }
}
