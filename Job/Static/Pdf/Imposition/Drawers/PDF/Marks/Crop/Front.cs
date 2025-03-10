using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Services;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Drawers.PDF.Marks.Crop
{
    public static partial class DrawCropMarks
    {
        public static void Front(PDFlib p, TemplatePage templatePage)
        {
            CropMarksController crops = templatePage.CropMarksController;

            p.save();

            int color = p.makespotcolor("All");
            p.setcolor("fillstroke", "spot", color, 1, 0, 0);
            p.setlinewidth(crops.Parameters.Height);

            foreach (var cropMark in crops.CropMarks.Where(x => x.IsFront))
            {
                PdfLineDrawer.DrawCropMark(p, cropMark);
            }

            p.restore();
        }

        internal static void Draw(PDFlib p, TemplatePage templatePage)
        {
            throw new NotImplementedException();
        }
    }
}
