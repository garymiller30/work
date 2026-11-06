using Interfaces;
using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Visual.TableCalendar;
using JobSpace.UserForms.PDF.Visual;
using PDFlib_dotnet;
using SharpCompress.Common;
using System;
using System.IO;
using System.Linq;

namespace JobSpace.Static.Pdf.Visual
{
    [PdfTool("Візуалізація","візуалізація настільного календаря",Description ="перевірка настільного календаря, як накладається календарна сітка, як згинається основа",Icon = "visual_table_calendar",Order = 20)]
    public class PDF_VisualTableCalendar : IPdfTool
    {
        IFileSystemInfoExt? file;

        const double COEF_DIMENSION = 0.3;

        public bool Configure(PdfJobContext context)
        {
            file = context.InputFiles.FirstOrDefault();
            if (file != null)
            {
                return true;
            }
            return false;
        }
        public void Execute(PdfJobContext context)
        {
            using (var form = new FormVisualTableCalendar(file))
            {
                if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // create pdf with the parameters from the form
                    CreatePdfSchema(file,form.TableCalendarParams);
                }
            }
        }

        private void CreatePdfSchema(IFileSystemInfoExt file, TableCalendarParams tableCalendarParams)
        {
            if (!File.Exists(file.FullName)) return;

            using var p = new PDFlib();

            try
            {
                string targetFile = Path.Combine(Path.GetDirectoryName(file.FullName) ?? "", Path.GetFileNameWithoutExtension(file.FullName) + "_+_schema.pdf");
                p.begin_document(targetFile, "optimize=true");

                int p_layer = p.define_layer(Common.Constants.PRINT_STRING, "");
                int v_layer = p.define_layer(Common.Constants.PROOF_STRING, "");

                int doc = p.open_pdi_document(file.FullName, "");
                int page_count = (int)p.pcos_get_number(doc, "length:pages");

                for (int i = 1; i <= page_count; i++)
                {
                    var page = p.open_pdi_page(doc, i, "cloneboxes");
                    p.begin_page_ext(0, 0, "");
                    Boxes boxes = PdfHelper.GetBoxes(p, doc, i - 1);
                    p.begin_layer(p_layer);
                    p.fit_pdi_page(page, 0, 0, "cloneboxes");
                    DrawProofColor(p, boxes, tableCalendarParams);
                    p.close_pdi_page(page);
                    p.end_page_ext("");
                }
                p.close_pdi_document(doc);
                p.end_document("");
            }
            catch (PDFlibException e)
            {
                PdfHelper.LogException(e, "TableCalendar");
            }
        }

        private void DrawProofColor(PDFlib p, Boxes boxes, TableCalendarParams tableCalendarParams)
        {
            var box = boxes.Trim;

            int spot = SetProofColorStroke(p);
            p.setlinewidth(1.0);
            p.rect(box.left + boxes.Media.left, box.bottom + boxes.Media.bottom, box.width, box.height);
            p.stroke();

            double yOfs = boxes.Trim.bottom + boxes.Media.bottom;

            double osnova = (double)tableCalendarParams.Osnova * PdfHelper.mn;
            double side = (box.height - osnova) / 2;


            double[] bigovki = new double[] {side,osnova,side };

            for (int i = 0; i < bigovki.Length; i++)
            {
                yOfs += bigovki[i];
                
                p.moveto(box.left + boxes.Media.left, yOfs);
                p.lineto(box.left + boxes.Media.left + box.width, yOfs);
                p.stroke();
            }

            DrawVerticalDimensions(p, spot, boxes, bigovki);
        }
        private void DrawVerticalDimensions(PDFlib p, int spot, Boxes boxes, double[] bigovki)
        {
            p.setlinewidth(0.1);
            p.set_graphics_option("dasharray=none");

            double x = (boxes.Media.left + boxes.Media.width) / PdfHelper.mn * COEF_DIMENSION;
            double y = (boxes.Trim.bottom + boxes.Media.bottom) / PdfHelper.mn;

            for (int i = 0; i < bigovki.Length; i++)
            {
                double height = bigovki[i]/PdfHelper.mn;
                PdfHelper.DrawDimensionsY(p, spot, x, y, Math.Round(height, 1));
                y += height;
            }
        }
        private int SetProofColorStroke(PDFlib p)
        {
            MarkColor markColor = MarkColor.ProofColor;
            int gstate = p.create_gstate("overprintmode=1 overprintfill=true overprintstroke=true");
            p.set_gstate(gstate);

            p.setcolor("fillstroke", "cmyk", markColor.C / 100, markColor.M / 100, markColor.Y / 100, markColor.K / 100);
            int spot = p.makespotcolor(markColor.Name);
            p.setcolor("stroke", "spot", spot, 1.0, 0.0, 0.0);

            return spot;
        }
    }
}
