using PDFiumSharp.Types;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static iText.IO.Image.Jpeg2000ImageData;

namespace JobSpace.Models.PdfDrawer
{
    public static class PdfDrawerService
    {
        public async static Task<Tuple<Image, double, double>> GetImageAsync(PdfPreviewParameters parameters, PdfDrawerPageCache pdfDrawerPageCache, int pageNo)
        {
            IPdfDrawer drawer = GetDrawer(parameters);

            var res = await drawer.GetPreviewAsync(parameters, pdfDrawerPageCache, pageNo);

            return res;
        }

        public static Tuple<Image, double, double> DrawImagePrimitives(PdfPreviewParameters parameters, int pageNo, Image img, double wMM, double hMM)
        {
            if (parameters.GetScreenPrimitives != null)
            {
                var primitives = parameters.GetScreenPrimitives(pageNo);

                Bitmap bmp = new Bitmap(img);

                using (var g = Graphics.FromImage(bmp))
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    // 🔥 правильний scale з мм → px
                    float scaleX = bmp.Width / (float)wMM;
                    float scaleY = bmp.Height / (float)hMM;

                    g.ScaleTransform(scaleX, scaleY);

                    primitives.ForEach(x => x.Draw(g));
                }

                return Tuple.Create<Image, double, double>(
                    bmp,
                    wMM,
                    hMM
                );
            }
            return new Tuple<Image, double, double>(img,wMM,hMM);
        }

        private static IPdfDrawer GetDrawer(PdfPreviewParameters parameters)
        {
            if (parameters.Display == PdfPreviewDisplay.Single)
            {
                return new PdfDrawerSingle();
            }
            else if (parameters.Display == PdfPreviewDisplay.Spread)
            {
                return new PdfDrawerSpread();
            }
            throw new Exception("GetDrawer: display param not supported");
        }
    }
}
