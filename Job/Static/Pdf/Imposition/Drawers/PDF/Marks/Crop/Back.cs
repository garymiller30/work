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
        public static void Back(PDFlib p, TemplatePage templatePage)
        {
            CropMarksController crops = templatePage.CropMarksController;

            p.save();

            int color = p.makespotcolor("All");
            p.setcolor("fillstroke", "spot", color, 1, 0, 0);
            p.setlinewidth(crops.Parameters.Height);

            foreach (var cropMark in crops.CropMarks.Where(x => x.IsBack))
            {
                PdfLineDrawer.DrawCropMark(p, cropMark);
            }

            p.restore();
        }
    }
}
