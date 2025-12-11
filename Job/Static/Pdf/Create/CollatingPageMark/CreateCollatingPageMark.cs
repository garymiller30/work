using JobSpace.Static.Pdf.Common;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Create.CollatingPageMark
{
    public class CreateCollatingPageMark
    {
        CreateCollatingPageMarkParams _param;
        public CreateCollatingPageMark(CreateCollatingPageMarkParams param)
        {
            _param = param;
        }

        public void Run(string filePath)
        {
            PDFlib p = null;
            try
            {
                p = new PDFlib();

                string targetFile = Path.Combine(
                    Path.GetDirectoryName(filePath),
                    Path.GetFileNameWithoutExtension(filePath) +
                    "_colmark" +
                    Path.GetExtension(filePath));

                p.begin_document(targetFile, "");
                int doc = p.open_pdi_document(filePath, "");
                int page_count = (int)p.pcos_get_number(doc, "length:pages");


                double x = _param.X;
                double y = _param.Y;

                double step = _param.PathLen / page_count;

                double xOfs = 0;
                double yOfs = 0;

                if (_param.Position == PageCollatingMarkPositionEnum.LEFT)
                {
                    yOfs = step;
                }
                else if (_param.Position == PageCollatingMarkPositionEnum.RIGHT)
                {
                    //get width of first page
                    var width = p.pcos_get_number(doc, "pages[0]/width");
                    x = width / PdfHelper.mn - _param.X - _param.MarkWidth;
                    yOfs = step;
                }
                else if (_param.Position == PageCollatingMarkPositionEnum.TOP)
                {
                    //get height of first page
                    var height = p.pcos_get_number(doc, "pages[0]/height");
                    y = height / PdfHelper.mn - _param.Y - _param.MarkHeight;
                    xOfs = step;
                }
                else if (_param.Position == PageCollatingMarkPositionEnum.BOTTOM)
                {
                    xOfs = step;
                }

                for (int i = 1; i <= page_count; i++)
                {
                    var page = p.open_pdi_page(doc, i, "cloneboxes");

                    p.begin_page_ext(0, 0, "");
                    p.fit_pdi_page(page, 0, 0, "cloneboxes");

                    Boxes trimbox = PdfHelper.GetBoxes(p, doc, i - 1);
                    p.close_pdi_page(page);
                    // draw mark

                    p.setcolor("fill", "cmyk", 0, 0, 0, 1);
                    p.rect(x * PdfHelper.mn, y * PdfHelper.mn, _param.MarkWidth * PdfHelper.mn, _param.MarkHeight * PdfHelper.mn);
                    p.fill();
                    p.end_page_ext("");
                    
                    x += xOfs;
                    y += yOfs;
                }
                p.close_pdi_document(doc);
                p.end_document("");
            }
            catch (PDFlibException e)
            {
                Logger.Log.Error(null, "FormCreateCollatingPageMark", "PDFlib exception occurred in FormCreateCollatingPageMark.Run():\n" +
                    "[" + e.get_errnum() + "] " + e.get_apiname() + ": " + e.get_errmsg());
            }
            catch (Exception e)
            {
                Logger.Log.Error(null, "FormCreateCollatingPageMark", "Exception occurred in FormCreateCollatingPageMark.Run():\n" + e.Message);
            }
            finally
            {
                p?.Dispose();
            }
        }
    }
}
