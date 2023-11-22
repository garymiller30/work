using Job.Static.Pdf.Common;
using Job.Static.PdfScale;
using PDFlib_dotnet;
using System;
using System.IO;

namespace Job.Static.Pdf.Scale
{
    public class PdfScaler
    {
        public static readonly double mn = 2.83465;
        private PdfScaleParams _params;

        public PdfScaler()
        {
            _params = new PdfScaleParams();
        }

        public PdfScaler(PdfScaleParams param)
        {
            _params = param ?? new PdfScaleParams();
        }

        public void Run(string filePath)
        {
            PDFlib p = null;

            try
            {
                string targetFile = Path.Combine(Path.GetDirectoryName(filePath), $"{Path.GetFileNameWithoutExtension(filePath)}_{_params.TargetSize.Width}x{_params.TargetSize.Height}.pdf");


                p = new PDFlib();

                p.begin_document(targetFile, "");

                var indoc = p.open_pdi_document(filePath, "");
                var endpage = (int)p.pcos_get_number(indoc, "length:pages");

                for (var pageno = 1; pageno <= endpage; pageno++)
                {
                    var page = p.open_pdi_page(indoc, pageno, "");

                    if (page == -1)
                        throw new Exception("Error: " + p.get_errmsg());

                    if (_params.ScaleBy == ScaleByEnum.TrimBox)
                    {
                        Box box = GetTrimbox(p, indoc, pageno - 1);

                        double xScale = _params.TargetSize.Width / box.wMM();
                        double yScale = _params.TargetSize.Height / box.hMM();

                        if (_params.ScaleVariant == ScaleVariantEnum.Proportial)
                        {
                            if (xScale > yScale) { xScale = yScale; }
                            else { yScale = xScale; }
                        }

                        double trim_left = _params.TargetSize.BleedInch();
                        double trim_right = _params.TargetSize.WidthInch() + trim_left;
                        double trim_bottom = _params.TargetSize.BleedInch();
                        double trim_top = _params.TargetSize.HeigthInch() + trim_bottom;

                        double deltaX = (_params.TargetSize.WidthInch() - box.w(xScale)) / 2;
                        double deltaY = (_params.TargetSize.HeigthInch() - box.h(yScale)) / 2;

                        p.begin_page_ext(
                            _params.TargetSize.WidthWithBleedInch(),
                            _params.TargetSize.HeightWithBleedInch(),
                            $"trimbox={{{trim_left} {trim_bottom} {trim_right} {trim_top}}}");

                        double pageX = trim_left - box.x * xScale + deltaX;
                        double pageY = trim_bottom - box.y * yScale + deltaY;

                        p.fit_pdi_page(page, pageX, pageY, $"scale={{{xScale} {yScale}}}");
                        p.end_page_ext("");

                    }
                    else if (_params.ScaleBy == ScaleByEnum.Mediabox)
                    {
                        Box box = new Box();

                        //get page width
                        box.width = p.pcos_get_number(indoc, "pages[" + page + "]/width");
                        //get page heigth
                        box.height = p.pcos_get_number(indoc, "pages[" + page + "]/height");

                        double xScale = _params.TargetSize.WidthWithBleedInch() / box.width;
                        double yScale = _params.TargetSize.HeightWithBleedInch() / box.height;

                        if (_params.ScaleVariant == ScaleVariantEnum.Proportial)
                        {
                            if (xScale > yScale) { xScale = yScale; }
                            else { yScale = xScale; }
                        }

                        double trim_left = _params.TargetSize.BleedInch();
                        double trim_right = _params.TargetSize.WidthInch() + trim_left;
                        double trim_bottom = _params.TargetSize.BleedInch();
                        double trim_top = _params.TargetSize.HeigthInch() + trim_bottom;

                        double deltaX = (_params.TargetSize.WidthWithBleedInch() - box.w(xScale)) / 2;
                        double deltaY = (_params.TargetSize.HeightWithBleedInch() - box.h(yScale)) / 2;

                        p.begin_page_ext(
                           _params.TargetSize.WidthWithBleedInch(),
                           _params.TargetSize.HeightWithBleedInch(),
                           $"trimbox={{{trim_left} {trim_bottom} {trim_right} {trim_top}}}");

                        double pageX = deltaX;
                        double pageY = deltaY;

                        p.fit_pdi_page(page, pageX, pageY, $"scale={{{xScale} {yScale}}}");
                        p.end_page_ext("");

                    }

                    p.close_pdi_page(page);

                }

                p.end_document("");

            }
            catch (PDFlibException e)
            {
                Logger.Log.Error(null, "ScalePdf", $"[{e.get_errnum()}] {e.get_apiname()}: {e.get_errmsg()}");
            }
            finally
            {
                p?.Dispose();
            }
        }

        Box GetTrimbox(PDFlib p, int doc, int page)
        {
            var trims = new double[] { 0, 0, 0, 0 };
            var media = new double[] { 0, 0, 0, 0 };

            for (int i = 0; i < 4; i++)
            {
                media[i] = p.pcos_get_number(doc, $"pages[{page}]/MediaBox[{i}]");

            }

            string trimtype = p.pcos_get_string(doc, $"type:pages[{page}]/TrimBox");

            if (string.Equals(trimtype, "array", StringComparison.OrdinalIgnoreCase))
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

    }
}
