using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition.Models;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Visual.SoftCover
{
    public class SoftCover
    {
        SoftCoverParams _coverParams;
        public SoftCover(SoftCoverParams param)
        {
            _coverParams = param;
        }
        public void Run(string file = null)
        {
            string output_file = null;
            if (string.IsNullOrEmpty(file))
            {
                output_file = System.IO.Path.Combine(_coverParams.FolderOutput, "SoftCover_schema.pdf");
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
                    p.fit_pdi_page(page_handle, (_coverParams.TotalWidth / 2) * PdfHelper.mn, (_coverParams.TotalHeight / 2) * PdfHelper.mn, "position={center center}");
                    p.close_pdi_page(page_handle);
                    p.end_layer();
                }

                p.begin_layer(v_layer);
                // Draw cover schema here
                int gstate = p.create_gstate("overprintmode=1 overprintfill=true overprintstroke=true");
                p.set_gstate(gstate);

                MarkColor c = MarkColor.ProofColor;
                p.setcolor("fillstroke", "cmyk", c.C / 100, c.M / 100, c.Y / 100, c.K / 100);
                int spot = p.makespotcolor(c.Name);
                // p.setdash(4, 2);
                p.set_graphics_option("dasharray={4 2}");
                p.setlinewidth(0.2);
                p.setcolor("fillstroke", "spot", spot, 1.0, 0, 0);

                // обрізний прямокутник
                double x = _coverParams.Bleed;
                double y = _coverParams.Bleed;
                double w = _coverParams.TotalWidth - 2 * _coverParams.Bleed;
                double h = _coverParams.TotalHeight - 2 * _coverParams.Bleed;
                p.rect(x * PdfHelper.mn, y * PdfHelper.mn, w * PdfHelper.mn, h * PdfHelper.mn);

                if (_coverParams.LeftKlapan > 0)
                {
                    x += _coverParams.LeftKlapan;
                    p.moveto(x * PdfHelper.mn, y * PdfHelper.mn);
                    p.lineto(x * PdfHelper.mn, (y + h) * PdfHelper.mn);
                }

                x += _coverParams.Width;
                p.moveto(x * PdfHelper.mn, y * PdfHelper.mn);
                p.lineto(x * PdfHelper.mn, (y + h) * PdfHelper.mn);

                x += _coverParams.Root;
                p.moveto(x * PdfHelper.mn, y * PdfHelper.mn);
                p.lineto(x * PdfHelper.mn, (y + h) * PdfHelper.mn);

                x += _coverParams.Width;
                p.moveto(x * PdfHelper.mn, y * PdfHelper.mn);
                p.lineto(x * PdfHelper.mn, (y + h) * PdfHelper.mn);
                if (_coverParams.RightKlapan > 0)
                {
                    x += _coverParams.RightKlapan;
                    p.moveto(x * PdfHelper.mn, y * PdfHelper.mn);
                    p.lineto(x * PdfHelper.mn, (y + h) * PdfHelper.mn);
                }

                p.stroke();

                DrawDimensions(p, spot);

                x = _coverParams.Bleed;
                y = _coverParams.Bleed;
                w = _coverParams.TotalWidth - x;
                h = _coverParams.TotalHeight - y;

                p.end_page_ext($"trimbox {{{x * PdfHelper.mn} {y * PdfHelper.mn} {w * PdfHelper.mn} {h * PdfHelper.mn}}}");
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

        private void DrawDimensions(PDFlib p, int spot)
        {
            p.setlinewidth(0.1);
            p.set_graphics_option("dasharray=none");

            double[] widths = new double[7] { _coverParams.Bleed,_coverParams.LeftKlapan,  _coverParams.Width, _coverParams.Root,  _coverParams.Width,_coverParams.RightKlapan, _coverParams.Bleed };
            double[] heigts = new double[3] { _coverParams.Bleed, _coverParams.Height, _coverParams.Bleed };

            double x = 0;
            double y = _coverParams.Bleed + _coverParams.Height * 0.3;

            for (int i = 0; i < widths.Count(); i++)
            {
                if (widths[i] == 0) continue;
                PdfHelper.DrawDimensionsX(p, spot, x, y, widths[i]);
                x += widths[i];
            }

            x = _coverParams.Bleed + _coverParams.Width * 0.3;
            y = 0;

            for (int i = 0; i < heigts.Count(); i++)
            {
                PdfHelper.DrawDimensionsY(p, spot, x, y, heigts[i]);
                y += heigts[i];
            }
        }
    }
}
