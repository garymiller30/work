using PDFlib_dotnet;
using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Job.Static.Pdf.Common
{
    public static class PdfHelper
    {
        public static double mn = 2.83465;


        public static Boxes GetBoxes(PDFlib p, int doc, int page)
        {
            Boxes boxes = new Boxes();

            var trims = new double[] { 0, 0, 0, 0 };
            var media = new double[] { 0, 0, 0, 0 };

            for (int i = 0; i < 4; i++)
            {
                media[i] = p.pcos_get_number(doc, $"pages[{page}]/MediaBox[{i}]");
            }

            string trimtype = p.pcos_get_string(doc, $"type:pages[{page}]/TrimBox");

            if (string.Equals(trimtype, "array", System.StringComparison.OrdinalIgnoreCase))
            {
                for (int i = 0; i < 4; i++)
                {
                    trims[i] = p.pcos_get_number(doc, $"pages[{page}]/TrimBox[{i}]");
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    trims[i] = media[i];
                }
            }

            boxes.Media.left = media[0];
            boxes.Media.bottom = media[1];
            boxes.Media.width = media[2];
            boxes.Media.height = media[3];

            boxes.Trim.left = trims[0] - media[0];
            boxes.Trim.bottom = trims[1] - media[1];
            boxes.Trim.width = trims[2] - trims[0];
            boxes.Trim.height = trims[3] - trims[1];
            boxes.Trim.top = media[3] - trims[3];
            boxes.Trim.right = media[2] - trims[2];

            return boxes;
        }


        public static Box GetTrimbox(PDFlib p, int doc, int page)
        {
            var trims = new double[] { 0, 0, 0, 0 };
            var media = new double[] { 0, 0, 0, 0 };

            for (int i = 0; i < 4; i++)
            {
                media[i] = p.pcos_get_number(doc, $"pages[{page}]/MediaBox[{i}]");
            }

            string trimtype = p.pcos_get_string(doc, $"type:pages[{page}]/TrimBox");

            if (string.Equals(trimtype, "array", System.StringComparison.OrdinalIgnoreCase))
            {
                for (int i = 0; i < 4; i++)
                {
                    trims[i] = p.pcos_get_number(doc, $"pages[{page}]/TrimBox[{i}]");
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    trims[i] = media[i];
                }
            }

            return new Box()
            {
                left = trims[0] - media[0],
                bottom = trims[1] - media[1],
                width = trims[2] - trims[0],
                height = trims[3] - trims[1],
                top = media[3] - trims[3],
                right = media[2] - trims[2],
            };
        }


        public static List<PdfPageInfo> GetPagesInfo(string filePath)
        {
            List<PdfPageInfo> list = new List<PdfPageInfo>();
            
            PDFlib p = null;

            try
            {
                p = new PDFlib();
                p.begin_document("","");
                int indoc = p.open_pdi_document(filePath, "");
                int pageCnt = (int)p.pcos_get_number(indoc, "length:pages");

                for (int i = 0;i < pageCnt;i++)
                {
                    var info = new PdfPageInfo();

                    int page = p.open_pdi_page(indoc, i + 1, "");

                    string rotated = p.pcos_get_string(indoc, $"type:pages[{i}]/Rotate");

                    if (string.Equals(rotated, "number", System.StringComparison.OrdinalIgnoreCase))
                    {
                        info.Rotate = p.pcos_get_number(indoc, $"pages[{i}]/Rotate");
                    }

                    Boxes boxes = GetBoxes(p,indoc,page);
                    info.Mediabox = boxes.Media;
                    info.Trimbox = boxes.Trim;

                    list.Add(info);

                    p.close_pdi_page(page);

                }

                p.close_pdi_document(indoc);

            }
            catch (PDFlibException e)
            {

                LogException(e, "GetPagesInfo");
            }
            finally
            {
                p?.Dispose();
            }

            return list;
        }

        public static void LogException(PDFlibException e, string title)
        {
            Logger.Log.Error(null, title, $"[{e.get_errnum()}] {e.get_apiname()}: {e.get_errmsg()}");
        }
    }
}
