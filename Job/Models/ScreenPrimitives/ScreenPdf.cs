using Interfaces;
using JobSpace.Static.Pdf.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Models.ScreenPrimitives
{
    public class ScreenPdf : IScreenPrimitive, IDisposable
    {

        IFileSystemInfoExt _file;
        int _pageNo;
        Image image;
        PdfPageInfo info;


        public ScreenPdf(IFileSystemInfoExt file, int pageNo)
        {
            _file = file;
            _pageNo = pageNo;
            info = PdfHelper.GetPageInfo(_file.FullName);
            image = PdfHelper.RenderByTrimBox(_file.FullName, pageNo - 1);
        }

        public void Dispose()
        {
            image?.Dispose();
        }

        public void Draw(Graphics g)
        {
            float w = (float)info.Trimbox.wMM();
            float h = (float)info.Trimbox.hMM();

            // намалювати image по центру
            var x = (g.VisibleClipBounds.Width - w) / 2;
            var y = (g.VisibleClipBounds.Height - h) / 2;
            g.DrawImage(image, x, y, w, h);
        }
    }
}
