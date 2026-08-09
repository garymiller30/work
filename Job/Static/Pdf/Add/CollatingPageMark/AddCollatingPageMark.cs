using ExtensionMethods;
using Interfaces;
using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.Static.Pdf.Common;
using JobSpace.UserForms;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.Static.Pdf.Add.CollatingPageMark
{
    [PdfTool("Додати","мітки для підбору",Icon = "create_page_mark",Order =2,Description ="додати мітки підбору до файлу")]
    public class AddCollatingPageMark : IPdfTool
    {
        private AddCollatingPageMarkParams? _param;

       

        public bool Configure(PdfJobContext context)
        {
            using (var form = new FormCreateCollatingPageMark())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _param = form.CreatePageCollationMarksParam;
                    return true;
                }
            }
            return false;
        }

        public void Execute(PdfJobContext context)
        {
            if (_param == null)
                throw new InvalidOperationException("Tool is not configured. Call Configure() before Execute().");

            foreach (var file in context.InputFiles)
            {
                CollatingPageMark(file.FileInfo.FullName, _param);
            }
        }

        public void CollatingPageMark(string filePath, AddCollatingPageMarkParams param)
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

                p.begin_document(targetFile, "optimize=true");
                int doc = p.open_pdi_document(filePath, "");
                int page_count = (int)p.pcos_get_number(doc, "length:pages");


                double x = param.X;
                double y = param.Y;

                double step = param.PathLen / page_count;

                double xOfs = 0;
                double yOfs = 0;

                if (param.Position == PageCollatingMarkPositionEnum.LEFT)
                {
                    yOfs = step;
                }
                else if (param.Position == PageCollatingMarkPositionEnum.RIGHT)
                {
                    //get width of first page
                    var width = p.pcos_get_number(doc, "pages[0]/width");
                    x = width / PdfHelper.mn - param.X - param.MarkWidth;
                    yOfs = step;
                }
                else if (param.Position == PageCollatingMarkPositionEnum.TOP)
                {
                    //get height of first page
                    var height = p.pcos_get_number(doc, "pages[0]/height");
                    y = height / PdfHelper.mn - param.Y - param.MarkHeight;
                    xOfs = step;
                }
                else if (param.Position == PageCollatingMarkPositionEnum.BOTTOM)
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
                    p.rect(x * PdfHelper.mn, y * PdfHelper.mn, param.MarkWidth * PdfHelper.mn, param.MarkHeight * PdfHelper.mn);
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
                Logger.Log.Error(null, "FormCreateCollatingPageMark", "PDFlib exception occurred in FormCreateCollatingPageMark.CreateSoftCover():\n" +
                    "[" + e.get_errnum() + "] " + e.get_apiname() + ": " + e.get_errmsg());
            }
            catch (Exception e)
            {
                Logger.Log.Error(null, "FormCreateCollatingPageMark", "Exception occurred in FormCreateCollatingPageMark.CreateSoftCover():\n" + e.Message);
            }
            finally
            {
                p?.Dispose();
            }
        }
    }
}
