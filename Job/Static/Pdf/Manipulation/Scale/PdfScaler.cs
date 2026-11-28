using Interfaces;
using Interfaces.FileBrowser;
using Interfaces.Licensing;
using Interfaces.Plugins;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.PdfScale;
using JobSpace.UserForms;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace JobSpace.Static.Pdf.Manipulation.Scale
{
    [PdfTool("Маніпуляції з файлом", "Масштабувати PDF", Description = "змінити/масштабувати розмір/формат документа", Icon = "scale", Order = 5)]
    [RequiresFeature(LicenseFeature.ExportPdf)]
    public class PdfScaler : IPdfTool
    {
        private PdfScaleParams? _params;
        private List<IFileSystemInfoExt>? _inputFiles;

        public bool Configure(PdfJobContext context)
        {
            if (!context.InputFiles.Any())
                return false;

            // отримати файли з розширенням pdf
            _inputFiles = context.InputFiles.Where(f => f.FileInfo.Extension.ToLower() == ".pdf").ToList();

            if (_inputFiles.Count == 0)
            {
                MessageBox.Show("Виберіть файли з розширенням .pdf");
                return false;
            }

            var file = _inputFiles[0];
            if (file != null)
            {
                using (var form = new FormSelectPdfNewSize(file))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        _params = form.Params;
                        return true;
                    }
                }
            }
            return false;
        }

        public void Execute(PdfJobContext context)
        {
            if (_params == null || _inputFiles == null) return;

            foreach (var file in _inputFiles)
            {
                Scaler(file.FullName);
            }
        }

        public void Scaler(string filePath)
        {
            using (PDFlib p = new PDFlib())
            {
                try
                {
                    string suffix = _params.ScaleMode == ScaleModeEnum.ScaleByPercent
                        ? $"{_params.ScalePercent.ToString("N01")}%"
                        : $"{_params.TargetSize.Width.ToString("N01")}x{_params.TargetSize.Height.ToString("N01")}";

                    string targetFile = Path.Combine(
                        Path.GetDirectoryName(filePath) ?? string.Empty,
                        $"{Path.GetFileNameWithoutExtension(filePath)}_{suffix}.pdf");

                    p.begin_document(targetFile, "optimize=true");

                    var indoc = p.open_pdi_document(filePath, "");
                    var endpage = (int)p.pcos_get_number(indoc, "length:pages");

                    for (var pageno = 1; pageno <= endpage; pageno++)
                    {
                        var page = p.open_pdi_page(indoc, pageno, "");

                        if (page == -1)
                            throw new Exception("Error: " + p.get_errmsg());

                        ScalePage(p, indoc, page, pageno);

                        p.close_pdi_page(page);
                    }
                    p.end_document("");
                }
                catch (PDFlibException e)
                {
                    PdfHelper.LogException(e, "ScalePdf");
                }
            }
        }

        private void ScalePage(PDFlib p, int indoc, int page, int pageno)
        {
            int pageIdx = pageno - 1;
            Boxes boxes = PdfHelper.GetBoxes(p, indoc, pageIdx);

            Box sourceBox = _params.ScaleBy == ScaleByEnum.TrimBox ? boxes.Trim : boxes.Media;
            double sourceW = sourceBox.wMM();
            double sourceH = sourceBox.hMM();

            if (sourceW <= 0 || sourceH <= 0)
                return;

            double bleed = _params.TargetSize.Bleed;
            double targetTrimW;
            double targetTrimH;
            double targetMediaW;
            double targetMediaH;
            double xScale;
            double yScale;

            if (_params.ScaleMode == ScaleModeEnum.ScaleByPercent)
            {
                double factor = _params.ScalePercent / 100.0;
                xScale = factor;
                yScale = factor;

                if (_params.ScaleBy == ScaleByEnum.TrimBox)
                {
                    targetTrimW = sourceW * xScale;
                    targetTrimH = sourceH * yScale;
                    targetMediaW = targetTrimW + bleed * 2;
                    targetMediaH = targetTrimH + bleed * 2;
                }
                else
                {
                    targetMediaW = sourceW * xScale;
                    targetMediaH = sourceH * yScale;
                    targetTrimW = Math.Max(0, targetMediaW - bleed * 2);
                    targetTrimH = Math.Max(0, targetMediaH - bleed * 2);
                }
            }
            else
            {
                bool isSourceLandscape = sourceW > sourceH;
                bool isTargetLandscape = _params.TargetSize.Width > _params.TargetSize.Height;

                if (_params.AutoMatchOrientation && (isSourceLandscape != isTargetLandscape))
                {
                    targetTrimW = _params.TargetSize.Height;
                    targetTrimH = _params.TargetSize.Width;
                }
                else
                {
                    targetTrimW = _params.TargetSize.Width;
                    targetTrimH = _params.TargetSize.Height;
                }

                targetMediaW = targetTrimW + bleed * 2;
                targetMediaH = targetTrimH + bleed * 2;

                if (_params.ScaleBy == ScaleByEnum.TrimBox)
                {
                    xScale = targetTrimW / sourceW;
                    yScale = targetTrimH / sourceH;
                }
                else // Mediabox: масштабування по Mediabox у формат з вильотами (наприклад, 148x210 + 2мм bleed -> 152x214)
                {
                    xScale = targetMediaW / sourceW;
                    yScale = targetMediaH / sourceH;
                }

                if (_params.ScaleVariant == ScaleVariantEnum.Proportial)
                {
                    if (xScale > yScale) { xScale = yScale; }
                    else { yScale = xScale; }
                }
            }

            double pageWidthPt = targetMediaW * PdfHelper.mn;
            double pageHeightPt = targetMediaH * PdfHelper.mn;

            double trim_left = bleed * PdfHelper.mn;
            double trim_bottom = bleed * PdfHelper.mn;
            double trim_right = (targetMediaW - bleed) * PdfHelper.mn;
            double trim_top = (targetMediaH - bleed) * PdfHelper.mn;

            double pageX;
            double pageY;

            if (_params.ScaleBy == ScaleByEnum.TrimBox)
            {
                double targetTrimWPt = targetTrimW * PdfHelper.mn;
                double targetTrimHPt = targetTrimH * PdfHelper.mn;

                double scaledTrimWidthPt = sourceBox.width * xScale;
                double scaledTrimHeightPt = sourceBox.height * yScale;

                double deltaX = (targetTrimWPt - scaledTrimWidthPt) / 2.0;
                double deltaY = (targetTrimHPt - scaledTrimHeightPt) / 2.0;

                pageX = trim_left - (sourceBox.left * xScale) + deltaX;
                pageY = trim_bottom - (sourceBox.bottom * yScale) + deltaY;
            }
            else // Mediabox
            {
                double scaledMediaWidthPt = sourceBox.width * xScale;
                double scaledMediaHeightPt = sourceBox.height * yScale;

                double deltaX = (pageWidthPt - scaledMediaWidthPt) / 2.0;
                double deltaY = (pageHeightPt - scaledMediaHeightPt) / 2.0;

                pageX = deltaX;
                pageY = deltaY;
            }

            p.begin_page_ext(
                pageWidthPt,
                pageHeightPt,
                $"trimbox={{{trim_left} {trim_bottom} {trim_right} {trim_top}}}");

            p.fit_pdi_page(page, pageX, pageY, $"scale={{{xScale} {yScale}}}");
            p.end_page_ext("");
        }
    }
}
