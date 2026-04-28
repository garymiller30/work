using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Convert;
using JobSpace.Static.Pdf.Scale;
using PDFlib_dotnet;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.Static.Pdf.SetTrimBox.ByBleed
{
    public sealed class PdfSetTrimBoxByBleed : SetTrimBoxBase
    {
        PdfSetTrimBoxByBleedParams _params;

        public PdfSetTrimBoxByBleed(PdfSetTrimBoxByBleedParams param)
        {
            _params = param;
        }

        public void Run(string filePath)
        {
            string fileExt = Path.GetExtension(filePath);

            if (string.Equals(fileExt, ".pdf", StringComparison.OrdinalIgnoreCase)){
                var tmpFile = Path.GetTempFileName();

                using (PDFlib p = new PDFlib())
                {
                    try
                    {
                        p.begin_document(tmpFile, "");

                        if (string.Equals(fileExt, ".pdf", StringComparison.OrdinalIgnoreCase))
                        {
                            SetTrimToPdf(p, filePath);
                        }
                        else
                        {

                        }
                        p.end_document("");

                        RewriteFile(tmpFile, filePath);
                    }
                    catch (PDFlibException e)
                    {
                        PdfHelper.LogException(e, "PdfSetTrimBoxByBleed");
                    }
                    finally
                    {
                        File.Delete(tmpFile);
                    }
                }
            }
            else
            {
                double bleed = _params.Bleed * PdfHelper.mn;

                var param = new PdfConvertParams
                {
                    TrimBox = {
                            bottom = bleed,
                            top = bleed,
                            left = bleed,
                            right = bleed
                        }
                };

                var convert = new PdfConvert();
                convert.Configure(param);
                convert.Convert(filePath);
            }
        }

        void SetTrimToPdf(PDFlib p,string filePath)
        {
            var indoc = p.open_pdi_document(filePath, "");
            var endpage = (int)p.pcos_get_number(indoc, "length:pages");

            for (var pageno = 1; pageno <= endpage; pageno++)
            {
                var page = p.open_pdi_page(indoc, pageno, "");

                Box media = new Box();
                media.GetMediabox(p, indoc, pageno-1);

                double bleed = _params.Bleed * PdfHelper.mn;

                double x = bleed;
                double y = bleed;
                double w = media.width - bleed;
                double h = media.height - bleed;

                if (page == -1) throw new Exception("Error: " + p.get_errmsg());

                p.begin_page_ext(0, 0, "");
                p.fit_pdi_page(page, 0, 0, "adjustpage");
                p.end_page_ext($"trimbox {{{x} {y} {w} {h}}}");

                p.close_pdi_page(page);
            }
            p.close_pdi_document(indoc);
        }
    }
}
