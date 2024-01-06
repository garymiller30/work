using PDFlib_dotnet;

namespace Job.Static.Pdf.Common
{
    public static class PdfHelper
    {
        public static double mn = 2.83465;

        public static Box GetTrimbox(PDFlib p, int doc, int page)
        {
            var trims = new double[] { 0, 0, 0, 0 };
            var media = new double[] { 0, 0, 0, 0 };

            for (int i = 0; i < 4; i++)
            {
                media[i] = p.pcos_get_number(doc, $"pages[{page}]/MediaBox[{i}]");

            }

            string trimtype = p.pcos_get_string(doc, $"type:pages[{page}]/TrimBox");

            if (trimtype == "array")
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
                x = trims[0] - media[0],
                y = trims[1] - media[1],
                width = trims[2] - trims[0],
                height = trims[3] - trims[1]
            };
        }

        public static void LogException(PDFlibException e, string title)
        {
            Logger.Log.Error(null, title, $"[{e.get_errnum()}] {e.get_apiname()}: {e.get_errmsg()}");
        }
    }
}
