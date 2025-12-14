using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition.Models;
using PDFlib_dotnet;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace JobSpace.Static.Pdf.Visual.HardCover
{
    public class HardCover
    {
        HardCoverParams _coverParams;
        public HardCover(HardCoverParams coverParams)
        {
            _coverParams = coverParams;
        }

        public void Run(string file = null)
        {
            string output_file = null;

            if (string.IsNullOrEmpty(file))
            {
                output_file = System.IO.Path.Combine(_coverParams.FolderOutput, "HardCover_schema.pdf");
            }
            else
            {
                var fi = new System.IO.FileInfo(file);
                output_file = System.IO.Path.Combine(_coverParams.FolderOutput, $"{fi.Name}_+_schema.pdf");
            }

            var p = new PDFlib();

            try
            {
                p.begin_document(output_file, "optimize=true");

                p.begin_page_ext(_coverParams.TotalWidth * PdfHelper.mn, _coverParams.TotalHeight * PdfHelper.mn, "");

                int l_print = p.define_layer("print", "");
                int v_layer = p.define_layer("visual", "");

                if (!string.IsNullOrEmpty(file))
                {
                    p.begin_layer(l_print);
                    var doc = p.open_pdi_document(file, "");
                    var page_handle = p.open_pdi_page(doc, 1, "");
                    p.fit_pdi_page(page_handle, (_coverParams.TotalWidth /2) * PdfHelper.mn, (_coverParams.TotalHeight /2) * PdfHelper.mn, "position={center center}");
                    p.close_pdi_page(page_handle);
                    p.end_layer();
                }
                p.begin_layer(v_layer);
                // Draw cover schema here
                int gstate = p.create_gstate("overprintmode=1 overprintfill=true overprintstroke=true");
                p.set_gstate(gstate);

                MarkColor c = MarkColor.ProofColor;
                p.setcolor("fillstroke", "cmyk", c.C/100, c.M/100, c.Y/100, c.K/100);
                int spot = p.makespotcolor(c.Name);
                // p.setdash(4, 2);
                p.set_graphics_option("dasharray={4 2}");
                p.setlinewidth(0.2);
                p.setcolor("fillstroke", "spot", spot, 1.0, 0, 0);

                // розмір документу
                p.rect(0, 0, _coverParams.TotalWidth * PdfHelper.mn, _coverParams.TotalHeight * PdfHelper.mn);
                // ліва сторінка
                p.rect(_coverParams.Zagyn * PdfHelper.mn, _coverParams.Zagyn * PdfHelper.mn, _coverParams.Width * PdfHelper.mn, _coverParams.Height * PdfHelper.mn);
                // корінець
                p.rect((_coverParams.Zagyn + _coverParams.Width + _coverParams.Rastav) * PdfHelper.mn, 
                    _coverParams.Zagyn * PdfHelper.mn,
                    _coverParams.Root * PdfHelper.mn,
                    _coverParams.Height * PdfHelper.mn);
                // права сторінка
                p.rect((_coverParams.Zagyn + _coverParams.Width + _coverParams.Rastav * 2 + _coverParams.Root) * PdfHelper.mn,
                    _coverParams.Zagyn * PdfHelper.mn,
                    _coverParams.Width * PdfHelper.mn,
                    _coverParams.Height * PdfHelper.mn);
                p.stroke();

                DrawDimensions(p,spot);

                p.end_page_ext("");
                p.end_document("");
            }
            catch (PDFlibException e)
            {
                Logger.Log.Error(null, "PdfSpliter", $"[{e.get_errnum()}] {e.get_apiname()}: {e.get_errmsg()}");
            }
            finally
            {
                p?.Dispose();
            }
        }

        private void DrawDimensions(PDFlib p,int spot)
        {
            p.setlinewidth(0.1);
            p.set_graphics_option("dasharray=none");

            double[] widths = new double[7] { _coverParams.Zagyn, _coverParams.Width, _coverParams.Rastav, _coverParams.Root, _coverParams.Rastav, _coverParams.Width, _coverParams.Zagyn };
            double[] heigts = new double[3] { _coverParams.Zagyn, _coverParams.Height, _coverParams.Zagyn };

            double x = 0;
            double y = _coverParams.Zagyn + _coverParams.Height * 0.3;

            for (int i = 0; i < widths.Count(); i++)
            {
                DimensionsX(p,spot,x, y, widths[i]);
                x += widths[i];
            }

            x = _coverParams.Zagyn + _coverParams.Width * 0.3;
            y = 0;

            for (int i = 0; i < heigts.Count(); i++)
            {
                DimensionsY(p,spot, x, y, heigts[i]);
                y += heigts[i];
            }
        }

        private void DimensionsY(PDFlib p,int spot, double x, double y, double value)
        {
            if (value == 0) return;
            p.moveto(x * PdfHelper.mn, y * PdfHelper.mn);        // переместить курсор
            p.lineto(x * PdfHelper.mn, (y + value) * PdfHelper.mn); // нарисовать
            p.stroke();

            String txt = value.ToString();

            String cr_optlist = "fontname=Calibri fontsize=12 fillcolor={spot " + spot + " 1} encoding=unicode " +
            "leading=100% alignment=center";

            int tf = p.create_textflow(txt, cr_optlist);
            p.fit_textflow(tf, x * PdfHelper.mn, y * PdfHelper.mn, (x - 5) * PdfHelper.mn, (y + value ) * PdfHelper.mn, "orientate=west");
            p.delete_textflow(tf);
        }

        private void DimensionsX(PDFlib p,int spot, double x, double y, double value)
        {
            if (value == 0) return;
            p.moveto(x * PdfHelper.mn, y * PdfHelper.mn);        
            p.lineto((x + value) * PdfHelper.mn, y * PdfHelper.mn); 
            p.stroke();

            String txt = value.ToString();
            String cr_optlist = "fontname=Calibri fontsize=12 fillcolor={spot " + spot + " 1} encoding=unicode " +
            "leading=100% alignment=center";
            int tf = p.create_textflow(txt, cr_optlist);
            p.fit_textflow(tf, x * PdfHelper.mn, y * PdfHelper.mn, (x + value) * PdfHelper.mn, (y + 5) * PdfHelper.mn, "");
            p.delete_textflow(tf);
        }
    }
}
