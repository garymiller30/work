using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Create;
using JobSpace.UserForms.PDF;
using PDFlib_dotnet;
using System.IO;

namespace JobSpace.Static.Pdf.Create
{
    [PdfTool("Створити","Заливка прямокутника",Icon = "create_rectangle_fill",Order = 10)]
    public sealed class PdfCreateFillRectangle : IPdfTool
    {

        PdfCreateFillRectangleParams _param;

        public bool Configure(PdfJobContext context)
        {
            using (var form = new FormCreateFillRectangle())
            {
                
                if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    _param = form.FillRectangleParams;
                    return true;
                }
                return false;
            }
        }

        public void Execute(PdfJobContext context)
        {
            var fileName = Path.Combine(context.FileManager.Settings.CurFolder,_param.ToString());
            CreateFillRectangle(fileName);
        }

        public void CreateFillRectangle(string filePath)
        {
            PDFlib p = null;

            try
            {
                p = new PDFlib();

                p.begin_document(filePath, "optimize=true");

                double w = (_param.Width + _param.Bleeds*2) * PdfHelper.mn;
                double h = (_param.Height + _param.Bleeds*2) * PdfHelper.mn;

                p.begin_page_ext( w,h, "");

                if (_param.Color.IsSpot)
                {
                    string[]labstr = _param.Lab.Split(' ');

                    double l = double.Parse(labstr[0], System.Globalization.CultureInfo.InvariantCulture);
                    double a = double.Parse(labstr[1], System.Globalization.CultureInfo.InvariantCulture);
                    double b = double.Parse(labstr[2], System.Globalization.CultureInfo.InvariantCulture);

                    p.setcolor("fill", "lab",l/100, a/100, b/100, 0);
                    int spot = p.makespotcolor(_param.Color.Name);
                    p.setcolor("fill", "spot", spot, 1.0, 0.0, 0.0);
                }
                else
                {
                    p.setcolor("fill", "cmyk", 
                        (double)_param.Color.C/100, 
                        (double)_param.Color.M/100, 
                        (double)_param.Color.Y/100, 
                        (double)_param.Color.K/100);
                }

                p.rect(0,0,w,h);
                p.fill();

                double trim_x = _param.Bleeds * PdfHelper.mn;
                double trim_y = _param.Bleeds * PdfHelper.mn;
                double trim_w = (_param.Bleeds + _param.Width) * PdfHelper.mn;
                double trim_h = (_param.Bleeds + _param.Height) * PdfHelper.mn;
                p.end_page_ext($"trimbox {{{trim_x} {trim_y} {trim_w} {trim_h}}}");
                
                p.end_document("");


            }
            catch (PDFlibException e)
            {
                Logger.Log.Error(null, "PdfCreateFillRectangle", $"[{e.get_errnum()}] {e.get_apiname()}: {e.get_errmsg()}");
            }
            finally
            {
                p?.Dispose();
            }
        }
    }
}
