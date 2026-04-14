using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace JobSpace.Models.PdfDrawer
{
    public class PdfDrawerSingle : IPdfDrawer
    {
        public async Task<Tuple<Image, double, double>> GetPreviewAsync(PdfPreviewParameters parameters, PdfDrawerPageCache pdfDrawerPageCache, int pageNo)
        {
            if (pdfDrawerPageCache == null || parameters == null) return new Tuple<Image, double, double>(null, 100, 100);

            Tuple<Image, double, double> res = await pdfDrawerPageCache.GetPreviewAsync(pageNo);

            if (res == null) return null;

            var with_primitives = PdfDrawerService.DrawImagePrimitives(parameters, pageNo, res.Item1, res.Item2, res.Item3);

            return with_primitives;

            
        }
    }
}
