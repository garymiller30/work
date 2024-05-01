using Job.Static.Pdf.Common;
using Job.Static.Pdf.Create.Rectangle;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Create.FillRectangle
{
    public sealed class PdfCreateFillRectangle
    {

        PdfCreateFillRectangleParams _param;

        public PdfCreateFillRectangle(PdfCreateFillRectangleParams param)
        {
            _param = param;
        }
        
        public void Run(string filePath)
        {
            PDFlib p = null;

            try
            {
                p = new PDFlib();

                p.begin_document(filePath, "");

                double w = _param.Width * PdfHelper.mn;
                double h = _param.Height * PdfHelper.mn;

                p.begin_page_ext( w,h, "");

                if (_param.isSpot)
                {
                    string[]labstr = _param.Lab.Split(' ');

                    double l = double.Parse(labstr[0], System.Globalization.CultureInfo.InvariantCulture);
                    double a = double.Parse(labstr[1], System.Globalization.CultureInfo.InvariantCulture);
                    double b = double.Parse(labstr[2], System.Globalization.CultureInfo.InvariantCulture);

                    p.setcolor("fill", "lab",l/100, a/100, b/100, 0);
                    int spot = p.makespotcolor(_param.Name);
                    p.setcolor("fill", "spot", spot, 1.0, 0.0, 0.0);
                }
                else
                {
                    p.setcolor("fill", "cmyk", 
                        (double)_param.C/100, 
                        (double)_param.M/100, 
                        (double)_param.Y/100, 
                        (double)_param.K/100);
                }

                p.rect(0,0,w,h);
                p.fill();

                p.end_page_ext("");
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
