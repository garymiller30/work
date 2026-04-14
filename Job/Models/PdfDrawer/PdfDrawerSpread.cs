using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Models.PdfDrawer
{
    public class PdfDrawerSpread : IPdfDrawer
    {
        public async Task<Tuple<Image, double, double>> GetPreviewAsync(PdfPreviewParameters parameters, PdfDrawerPageCache pdfDrawerPageCache, int pageNo)
        {
            int leftPageNo, rightPageNo;
            if (pageNo % 2 == 0)
            {
                leftPageNo = pageNo;
                rightPageNo = pageNo + 1;
            }
            else
            {
                leftPageNo = pageNo - 1;
                rightPageNo = pageNo;
            }

            var leftTask = pdfDrawerPageCache.GetPreviewAsync(leftPageNo);
            var rightTask = pdfDrawerPageCache.GetPreviewAsync(rightPageNo);

            await Task.WhenAll(leftTask, rightTask);

            var pageLeft = leftTask.Result;
            var pageRight = rightTask.Result;

            if (pageLeft == null && pageRight != null)
            {
                return PdfDrawerService.DrawImagePrimitives(parameters, rightPageNo, pageRight.Item1, pageRight.Item2, pageRight.Item3);
            }
            else if (pageRight == null && pageLeft != null)
                return PdfDrawerService.DrawImagePrimitives(parameters, leftPageNo, pageLeft.Item1, pageLeft.Item2, pageLeft.Item3);

            else if (pageLeft != null && pageRight != null)
            {
                var pageLeftWithPrimitives = PdfDrawerService.DrawImagePrimitives(parameters, leftPageNo, pageLeft.Item1, pageLeft.Item2, pageLeft.Item3);
                var pageRightWithPrimitives = PdfDrawerService.DrawImagePrimitives(parameters, rightPageNo, pageRight.Item1, pageRight.Item2, pageRight.Item3);

                var image = CombineSpread(pageLeftWithPrimitives.Item1, pageRightWithPrimitives.Item1);

                return new Tuple<Image, double, double>(image, pageLeft.Item2 + pageRight.Item2, Math.Max(pageLeft.Item3, pageRight.Item3));

            }
            return null;
        }

        public Image CombineSpread(Image left, Image right)
        {
            int width = left.Width + right.Width;
            int height = Math.Max(left.Height, right.Height);

            Bitmap result = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(result))
            {
                g.Clear(System.Drawing.Color.White);

                // ліва сторінка
                g.DrawImage(left, 0, 0, left.Width, left.Height);

                // права сторінка
                g.DrawImage(right, left.Width, 0, right.Width, right.Height);

                using (var pen = new Pen(System.Drawing.Color.Blue,1))
                {
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    g.DrawLine(pen,left.Width,0, left.Width,left.Height);
                }
                
            }

            return result;
        }

    }
}
