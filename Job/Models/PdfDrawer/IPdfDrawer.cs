using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Models.PdfDrawer
{
    public interface IPdfDrawer
    {
        Task<Tuple<Image, double, double>> GetPreviewAsync(PdfPreviewParameters parameters, PdfDrawerPageCache pdfDrawerPageCache, int pageNo);
    }
}
