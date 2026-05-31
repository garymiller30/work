using Interfaces.FileBrowser;
using Interfaces.Licensing;
using Interfaces.Plugins;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.UserForms.PDF;
using PDFlib_dotnet;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace JobSpace.Static.Pdf.Create.BigovkaMarks
{
    [PdfTool("Візуалізація", "Біговка",Description ="Перевірити лінії біговок",Icon ="visual_bigovka",Order = 1)]
    public class CreateBigovkaMarks : IPdfTool
    {
        const double COEF_DIMENSION = 0.3;

        CreateBigovkaMarksParams _param;
        int curPage = 1;

        public void Execute(PdfJobContext context)
        {
            foreach (var file in context.InputFiles)
            {
                CreateBigovkaMark(file.FullName);
            }
        }

        public bool Configure(PdfJobContext context)
        {
            using (var form = new FormCreateBigovkaMarks(context.InputFiles))
            {
                if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    _param = form.BigovkaMarksParams;
                    return true;
                }
            }

            return false;
        }
        public void CreateBigovkaMark(string filePath)
        {
            using ( PDFlib p = new PDFlib())
            {
                try
                {
                    var filename = Path.GetFileName(filePath);

                    var reg = new Regex(@"#(\d+)\.");
                    var match = reg.Match(filename);
                    string targetFile;
                    if (match.Success)
                    {
                        int len = match.Groups[1].Value.Length + 1;
                        var filenameWithoutExt = Path.GetFileNameWithoutExtension(filename);
                        filenameWithoutExt = filenameWithoutExt.Substring(0, filenameWithoutExt.Length - len);

                        targetFile = Path.Combine(
                            Path.GetDirectoryName(filePath), filenameWithoutExt + "_big_" + CreateBigovkaName() + "_#" + match.Groups[1].Value + Path.GetExtension(filePath));
                    }
                    else
                    {
                        targetFile =
                        Path.Combine(
                            Path.GetDirectoryName(filePath),
                            Path.GetFileNameWithoutExtension(filePath) +
                            "_big_" + CreateBigovkaName() +
                            Path.GetExtension(filePath));
                    }

                    p.begin_document(targetFile, "optimize=true");

                    int doc = p.open_pdi_document(filePath, "");
                    int page_count = (int)p.pcos_get_number(doc, "length:pages");

                    for (int i = 1; i <= page_count; i++)
                    {
                        curPage = i;
                        var page = p.open_pdi_page(doc, i, "cloneboxes");
                        p.begin_page_ext(0, 0, "");

                        int p_layer = p.define_layer("print", "");
                        int v_layer = p.define_layer("visual", "");

                        Boxes trimbox = PdfHelper.GetBoxes(p, doc, i - 1);
                        p.begin_layer(p_layer);
                        p.fit_pdi_page(page, 0, 0, "cloneboxes");
                        DrawPrintBigovkaMarks(p, trimbox);
                        p.end_layer();
                        p.close_pdi_page(page);
                        p.begin_layer(v_layer);
                        DrawBigovkaSchema(p, trimbox);
                        p.end_layer();
                        p.end_page_ext("");
                    }

                    p.close_pdi_document(doc);
                    p.end_document("");
                }
                catch (PDFlibException e)
                {
                    PdfHelper.LogException(e, "CreateBigovkaMarks");
                }
            }
        }

        private void DrawPrintBigovkaMarks(PDFlib p, Boxes boxes)
        {
            var box = boxes.Trim;
            double markLength = _param.Length * PdfHelper.mn;
            double distanceFromTrim = _param.DistanceFromTrim * PdfHelper.mn;
            double outsideOffset = distanceFromTrim + markLength;

            p.setcolor("fillstroke", "cmyk", _param.Color.C / 100, _param.Color.M / 100, _param.Color.Y / 100, _param.Color.K / 100);
            p.setlinewidth(2.0);

            if (_param.Direction == DirectionEnum.Horizontal)
            {
                double y = box.bottom - outsideOffset;
                double xOfs = GetHorizontalStartX(boxes);
                int direction = IsMirroredHorizontalPage() ? -1 : 1;
                double[] bigovki = GetBigovkiForPage();

                for (int i = 0; i < bigovki.Length; i++)
                {
                    xOfs += direction * bigovki[i] * PdfHelper.mn;
                    DrawVerticalMarks(p, boxes, box, y, xOfs, markLength, distanceFromTrim);
                }

                return;
            }

            double yOfs = box.bottom + boxes.Media.bottom;

            for (int i = 0; i < _param.Bigovki.Length; i++)
            {
                yOfs += _param.Bigovki[i] * PdfHelper.mn;
                DrawHorizontalMarks(p, boxes, box, yOfs, markLength, distanceFromTrim);
            }
        }

        private void DrawBigovkaSchema(PDFlib p, Boxes boxes)
        {
            int spot = SetProofColorStroke(p);

            p.setlinewidth(1.0);
            p.rect(boxes.Trim.left + boxes.Media.left, boxes.Trim.bottom + boxes.Media.bottom, boxes.Trim.width, boxes.Trim.height);
            p.stroke();

            if (_param.Direction == DirectionEnum.Horizontal)
            {
                DrawVerticalCreaseLines(p, boxes);
                DrawHorizontalDimensions(p, spot, boxes);
            }
            else
            {
                DrawHorizontalCreaseLines(p, boxes);
                DrawVerticalDimensions(p, spot, boxes);
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

        private void DrawVerticalCreaseLines(PDFlib p, Boxes boxes)
        {
            double xOfs = GetHorizontalStartX(boxes);
            int direction = IsMirroredHorizontalPage() ? -1 : 1;
            double[] bigovki = GetBigovkiForPage();

            for (int i = 0; i < bigovki.Length; i++)
            {
                xOfs += direction * bigovki[i] * PdfHelper.mn;
                DrawLine(
                    p,
                    xOfs,
                    boxes.Trim.bottom + boxes.Media.bottom,
                    xOfs,
                    boxes.Trim.bottom + boxes.Media.bottom + boxes.Trim.height);
            }
        }

        private void DrawHorizontalCreaseLines(PDFlib p, Boxes boxes)
        {
            double yOfs = boxes.Trim.bottom + boxes.Media.bottom;

            for (int i = 0; i < _param.Bigovki.Length; i++)
            {
                yOfs += _param.Bigovki[i] * PdfHelper.mn;
                DrawLine(
                    p,
                    boxes.Trim.left + boxes.Media.left,
                    yOfs,
                    boxes.Trim.left + boxes.Media.left + boxes.Trim.width,
                    yOfs);
            }
        }

        private void DrawHorizontalDimensions(PDFlib p, int spot, Boxes boxes)
        {
            p.setlinewidth(0.1);
            p.set_graphics_option("dasharray=none");

            double x = (boxes.Trim.left + boxes.Media.left) / PdfHelper.mn;
            double y = boxes.Media.height / PdfHelper.mn * COEF_DIMENSION;

            foreach (double width in GetHorizontalDimensionParts(boxes))
            {
                PdfHelper.DrawDimensionsX(p, spot, x, y, Math.Round( width,1));
                x += width;
            }
        }

        private void DrawVerticalDimensions(PDFlib p, int spot, Boxes boxes)
        {
            p.setlinewidth(0.1);
            p.set_graphics_option("dasharray=none");

            double x = (boxes.Media.left + boxes.Media.width) / PdfHelper.mn * COEF_DIMENSION;
            double y = (boxes.Trim.bottom + boxes.Media.bottom) / PdfHelper.mn;

            foreach (double height in GetVerticalDimensionParts(boxes))
            {
                PdfHelper.DrawDimensionsY(p, spot, x, y, Math.Round( height,1));
                y += height;
            }
        }

        private void DrawVerticalMarks(PDFlib p, Boxes boxes, Box box, double y, double xOfs, double markLength, double distanceFromTrim)
        {
            DrawLine(p, xOfs, y + boxes.Media.bottom, xOfs, y + boxes.Media.bottom + markLength);
            DrawLine(
                p,
                xOfs,
                box.bottom + boxes.Media.bottom + box.height + distanceFromTrim,
                xOfs,
                box.bottom + boxes.Media.bottom + box.height + distanceFromTrim + markLength);
        }

        private void DrawHorizontalMarks(PDFlib p, Boxes boxes, Box box, double yOfs, double markLength, double distanceFromTrim)
        {
            double left = box.left + boxes.Media.left;
            double right = left + box.width;

            DrawLine(p, left - distanceFromTrim - markLength, yOfs, left - distanceFromTrim, yOfs);
            DrawLine(p, right + distanceFromTrim, yOfs, right + distanceFromTrim + markLength, yOfs);
        }

        private void DrawLine(PDFlib p, double x1, double y1, double x2, double y2)
        {
            p.moveto(x1, y1);
            p.lineto(x2, y2);
            p.stroke();
        }

        private bool IsMirroredHorizontalPage()
        {
            return _param.MirrorEven && curPage % 2 == 0;
        }

        private double GetHorizontalStartX(Boxes boxes)
        {
            return IsMirroredHorizontalPage()
                ? boxes.Media.width - boxes.Trim.right
                : boxes.Trim.left + boxes.Media.left;
        }

        private double[] GetBigovkiForPage()
        {
            return _param.Bigovki;
        }

        private double[] GetHorizontalDimensionParts(Boxes boxes)
        {
            double remaining = boxes.Trim.width / PdfHelper.mn - _param.Bigovki.Sum();

            if (remaining <= 0)
            {
                return _param.Bigovki;
            }

            return _param.Bigovki.Concat(new[] { remaining }).ToArray();
        }

        private double[] GetVerticalDimensionParts(Boxes boxes)
        {
            double remaining = boxes.Trim.height / PdfHelper.mn - _param.Bigovki.Sum();

            if (remaining <= 0)
            {
                return _param.Bigovki;
            }

            return _param.Bigovki.Concat(new[] { remaining }).ToArray();
        }

        string CreateBigovkaName()
        {
            return string.Join("+", _param.Bigovki);
        }


    }
}
