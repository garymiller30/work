using JobSpace.Static.Pdf.Common;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace JobSpace.Static.Pdf.Create.BigovkaMarks
{
    public class CreateBigovkaMarks
    {
        CreateBigovkaMarksParams _param;
        int curPage = 1;
        public CreateBigovkaMarks(CreateBigovkaMarksParams param)
        {
            _param = param;
        }

        public void Run(string filePath)
        {
            PDFlib p = null;
            try
            {
                p = new PDFlib();

                string targetFile =
                    Path.Combine(
                        Path.GetDirectoryName(filePath),
                        Path.GetFileNameWithoutExtension(filePath) +
                        "_big_" + CreateBigovkaName() +
                        Path.GetExtension(filePath));

                p.begin_document(targetFile, "");

                int doc = p.open_pdi_document(filePath, "");
                int page_count = (int)p.pcos_get_number(doc, "length:pages");

                for (int i = 1; i <= page_count; i++)
                {
                    curPage = i;
                    var page = p.open_pdi_page(doc, i, "cloneboxes");
                    p.begin_page_ext(0, 0, "");
                    p.fit_pdi_page(page, 0, 0, "cloneboxes");

                    Boxes trimbox = PdfHelper.GetBoxes(p, doc, i - 1);
                    p.close_pdi_page(page);

                    CreateBigovki(p, trimbox);

                    p.end_page_ext("");
                }

                p.close_pdi_document(doc);
                p.end_document("");
            }
            catch (PDFlibException e)
            {
                Logger.Log.Error(null, "CreateBigovkaMarks", $"[{e.get_errnum()}] {e.get_apiname()}: {e.get_errmsg()}");
            }
            finally
            {
                p?.Dispose();
            }

        }

        private void CreateBigovki(PDFlib p, Boxes boxes)
        {
            var box = boxes.Trim;

            p.setcolor("fillstroke", "cmyk", _param.Color.C / 100, _param.Color.M / 100, _param.Color.Y / 100, _param.Color.K / 100);
            p.setlinewidth(2.0);

            double x = box.left;
            double y = box.bottom;

            if (_param.Direction == DirectionEnum.Horizontal)
            {

                y -= (_param.DistanceFromTrim + _param.Length) * PdfHelper.mn;
                
                if (_param.MirrorEven && curPage % 2 == 0)
                {
                    x = boxes.Media.width - box.right;
                    double xOfs = x + boxes.Media.left;

                    for (int i = 0; i < _param.Bigovki.Length; i++)
                    {
                        xOfs -= _param.Bigovki[i] * PdfHelper.mn;

                        DrawHorLines(p, boxes, box, y, xOfs);
                    }
                }
                else
                {
                    double xOfs = x + boxes.Media.left;
                    for (int i = 0; i < _param.Bigovki.Length; i++)
                    {
                        xOfs += _param.Bigovki[i] * PdfHelper.mn;

                        DrawHorLines(p, boxes, box, y, xOfs);
                    }
                }
            }
            else
            {

                double ofsY = y + boxes.Media.bottom;

                for (int i = 0; i < _param.Bigovki.Length; i++)
                {
                    ofsY += _param.Bigovki[i] * PdfHelper.mn;

                    p.moveto(box.left + boxes.Media.left - (_param.DistanceFromTrim + _param.Length) * PdfHelper.mn, ofsY);
                    p.lineto(box.left + boxes.Media.left - (_param.DistanceFromTrim) * PdfHelper.mn, ofsY);
                    p.stroke();

                    p.moveto(box.left + boxes.Media.left + box.width + _param.DistanceFromTrim * PdfHelper.mn, ofsY);
                    p.lineto(box.left + boxes.Media.left + box.width + (_param.DistanceFromTrim + _param.Length) * PdfHelper.mn, ofsY);
                    p.stroke();
                }

            }
        }

        private void DrawHorLines(PDFlib p, Boxes boxes, Box box, double y, double xOfs)
        {
            p.moveto(xOfs, y + boxes.Media.bottom);
            p.lineto(xOfs, y + boxes.Media.bottom + _param.Length * PdfHelper.mn);
            p.stroke();

            p.moveto(xOfs, box.bottom + boxes.Media.bottom + box.height + _param.DistanceFromTrim * PdfHelper.mn);
            p.lineto(xOfs, box.bottom + boxes.Media.bottom + box.height + (_param.DistanceFromTrim + _param.Length) * PdfHelper.mn);
            p.stroke();
        }

      

        string CreateBigovkaName()
        {
            if (_param.Bigovki.Length > 1)
            {
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < _param.Bigovki.Length; i++)
                {
                    sb.Append(_param.Bigovki[i]);
                    if (i < _param.Bigovki.Length - 1)
                    {
                        sb.Append("+");
                    }
                }

                return sb.ToString();
            }

            return _param.Bigovki[0].ToString();

        }
    }
}
