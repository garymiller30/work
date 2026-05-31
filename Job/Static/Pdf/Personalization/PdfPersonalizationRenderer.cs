using JobSpace.Static.Pdf.Common;
using iText.Barcodes;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using PageSize = iText.Kernel.Geom.PageSize;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JobSpace.Static.Pdf.Personalization
{
    public sealed class PdfPersonalizationRenderer
    {
        private const double MinScale = 0.01;
        private static bool _systemFontsRegistered;

        public void Render(PdfPersonalizationSettings settings)
        {
            PdfPersonalizationData data = PdfPersonalizationData.Load(settings.DataFilePath);
            if (data.Rows.Count == 0)
                throw new InvalidOperationException("Файл персоналізації не містить рядків даних.");

            Directory.CreateDirectory(settings.OutputFolder);
            var rowIndexes = ParseExportRows(settings.ExportRows, data.Rows.Count);

            if (settings.ExportMode == PersonalizationExportMode.SingleFile)
            {
                string output = Path.Combine(
                    settings.OutputFolder,
                    $"{Path.GetFileNameWithoutExtension(settings.BasePdfPath)}_personalized.pdf");

                RenderRowsToSingleFile(settings, data, rowIndexes, output);
                return;
            }

            foreach (int rowIndex in rowIndexes)
            {
                string output = Path.Combine(
                    settings.OutputFolder,
                    $"{Path.GetFileNameWithoutExtension(settings.BasePdfPath)}_{rowIndex + 1:D4}.pdf");

                RenderRow(settings, data, data.Rows[rowIndex], output);
            }
        }

        public void RenderPreview(PdfPersonalizationSettings settings, int rowIndex, string outputFile)
        {
            PdfPersonalizationData data = PdfPersonalizationData.Load(settings.DataFilePath);
            RenderPreview(settings, data, rowIndex, outputFile);
        }

        public void RenderPreview(PdfPersonalizationSettings settings, PdfPersonalizationData data, int rowIndex, string outputFile)
        {
            if (data.Rows.Count == 0)
                throw new InvalidOperationException("Файл персоналізації не містить рядків даних.");

            rowIndex = Math.Max(0, Math.Min(rowIndex, data.Rows.Count - 1));
            RenderRow(settings, data, data.Rows[rowIndex], outputFile);
        }

        private void RenderRowsToSingleFile(
            PdfPersonalizationSettings settings,
            PdfPersonalizationData data,
            IReadOnlyList<int> rowIndexes,
            string outputFile)
        {
            using (var p = new PDFlib())
            {
                var openedDocuments = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                var temporaryFiles = new List<string>();

                try
                {
                    p.begin_document(outputFile, "optimize=true");

                    int baseDoc = OpenDocument(p, openedDocuments, settings.BasePdfPath);
                    double pageWidth = p.pcos_get_number(baseDoc, "pages[0]/width");
                    double pageHeight = p.pcos_get_number(baseDoc, "pages[0]/height");

                    foreach (int rowIndex in rowIndexes)
                    {
                        p.begin_page_ext(pageWidth, pageHeight, "");

                        foreach (PdfPersonalizationLayer layer in settings.Layers.Where(x => x.Enabled))
                        {
                            DrawLayer(p, openedDocuments, temporaryFiles, settings, data, data.Rows[rowIndex], layer, pageWidth, pageHeight);
                        }

                        p.end_page_ext("");
                    }

                    p.end_document("");
                }
                catch (PDFlibException e)
                {
                    PdfHelper.LogException(e, "PdfPersonalization");
                    throw;
                }
                finally
                {
                    foreach (int doc in openedDocuments.Values)
                        p.close_pdi_document(doc);

                    foreach (string file in temporaryFiles)
                        TryDelete(file);
                }
            }
        }

        private void RenderRow(
            PdfPersonalizationSettings settings,
            PdfPersonalizationData data,
            IReadOnlyDictionary<string, string> row,
            string outputFile)
        {
            using (var p = new PDFlib())
            {
                var openedDocuments = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                var temporaryFiles = new List<string>();

                try
                {
                    p.begin_document(outputFile, "optimize=true");

                    int baseDoc = OpenDocument(p, openedDocuments, settings.BasePdfPath);
                    double pageWidth = p.pcos_get_number(baseDoc, "pages[0]/width");
                    double pageHeight = p.pcos_get_number(baseDoc, "pages[0]/height");

                    p.begin_page_ext(pageWidth, pageHeight, "");

                    foreach (PdfPersonalizationLayer layer in settings.Layers.Where(x => x.Enabled))
                    {
                        DrawLayer(p, openedDocuments, temporaryFiles, settings, data, row, layer, pageWidth, pageHeight);
                    }

                    p.end_page_ext("");
                    p.end_document("");
                }
                catch (PDFlibException e)
                {
                    PdfHelper.LogException(e, "PdfPersonalization");
                    throw;
                }
                finally
                {
                    foreach (int doc in openedDocuments.Values)
                        p.close_pdi_document(doc);

                    foreach (string file in temporaryFiles)
                        TryDelete(file);
                }
            }
        }

        private void DrawLayer(
            PDFlib p,
            Dictionary<string, int> openedDocuments,
            List<string> temporaryFiles,
            PdfPersonalizationSettings settings,
            PdfPersonalizationData data,
            IReadOnlyDictionary<string, string> row,
            PdfPersonalizationLayer layer,
            double pageWidth,
            double pageHeight)
        {
            if (layer.Type == PersonalizationLayerType.BasePdf)
            {
                string path = string.IsNullOrWhiteSpace(layer.Source)
                    ? settings.BasePdfPath
                    : data.ResolveFile(row, layer.Source);

                DrawPdf(p, openedDocuments, path, layer, pageWidth, pageHeight, true);
                return;
            }

            if (layer.Type == PersonalizationLayerType.Pdf)
            {
                string path = data.ResolveFile(row, layer.Source);
                DrawPdf(p, openedDocuments, path, layer, pageWidth, pageHeight, false);
                return;
            }

            if (layer.Type == PersonalizationLayerType.Text)
            {
                string text = data.GetValue(row, layer.Source);
                DrawText(p, text, layer, pageWidth, pageHeight);
                return;
            }

            if (layer.Type == PersonalizationLayerType.Code)
            {
                string code = data.GetValue(row, layer.Source);
                DrawCode(p, openedDocuments, temporaryFiles, code, layer, pageWidth, pageHeight);
            }
        }

        private void DrawPdf(
            PDFlib p,
            Dictionary<string, int> openedDocuments,
            string path,
            PdfPersonalizationLayer layer,
            double pageWidth,
            double pageHeight,
            bool fullPage)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                return;

            int doc = OpenDocument(p, openedDocuments, path);
            int page = p.open_pdi_page(doc, 1, fullPage ? "cloneboxes" : "");

            try
            {
                if (fullPage)
                {
                    p.fit_pdi_page(page, 0, 0, "cloneboxes");
                    return;
                }

                double scale = Math.Max(MinScale, layer.ScalePercent / 100.0);
                double width = p.pcos_get_number(doc, "pages[0]/width") * scale;
                double height = p.pcos_get_number(doc, "pages[0]/height") * scale;
                var point = GetPlacementPoint(layer, width, height, pageWidth, pageHeight);

                p.save();
                p.translate(point.x, point.y);
                p.rotate(layer.Rotation);
                p.scale(scale, scale);
                p.fit_pdi_page(page, 0, 0, "");
                p.restore();
            }
            finally
            {
                p.close_pdi_page(page);
            }
        }

        private void DrawText(PDFlib p, string text, PdfPersonalizationLayer layer, double pageWidth, double pageHeight)
        {
            if (string.IsNullOrEmpty(text))
                return;

            int font = p.load_font(layer.FontName, "auto", "");
            p.setfont(font, layer.FontSize);

            double width = p.stringwidth(text, font, layer.FontSize);
            double height = layer.FontSize;
            var point = GetPlacementPoint(layer, width, height, pageWidth, pageHeight);

            string fillColor = string.Format(
                System.Globalization.CultureInfo.InvariantCulture,
                "fillcolor={{cmyk {0} {1} {2} {3}}}",
                ClampColor(layer.C) / 100.0,
                ClampColor(layer.M) / 100.0,
                ClampColor(layer.Y) / 100.0,
                ClampColor(layer.K) / 100.0);

            p.save();
            p.translate(point.x, point.y);
            p.rotate(layer.Rotation);
            p.fit_textline(text, 0, 0, fillColor);
            p.restore();
        }

        private void DrawCode(
            PDFlib p,
            Dictionary<string, int> openedDocuments,
            List<string> temporaryFiles,
            string code,
            PdfPersonalizationLayer layer,
            double pageWidth,
            double pageHeight)
        {
            if (string.IsNullOrWhiteSpace(code))
                return;

            string codePdf = Path.Combine(Path.GetTempPath(), $"pdf_personalization_code_{Guid.NewGuid():N}.pdf");
            temporaryFiles.Add(codePdf);

            CreateCodePdf(codePdf, code, layer);
            DrawGeneratedPdf(p, openedDocuments, codePdf, layer, pageWidth, pageHeight);
        }

        private void DrawGeneratedPdf(
            PDFlib p,
            Dictionary<string, int> openedDocuments,
            string path,
            PdfPersonalizationLayer layer,
            double pageWidth,
            double pageHeight)
        {
            int doc = OpenDocument(p, openedDocuments, path);
            int page = p.open_pdi_page(doc, 1, "");

            try
            {
                double originalWidth = p.pcos_get_number(doc, "pages[0]/width");
                double originalHeight = p.pcos_get_number(doc, "pages[0]/height");
                double scaleX = Math.Max(MinScale, layer.ScalePercent / 100.0);
                double scaleY = scaleX;

                if (layer.TargetWidthMm > 0)
                    scaleX = layer.TargetWidthMm * PdfHelper.mn / originalWidth;
                if (layer.TargetHeightMm > 0)
                    scaleY = layer.TargetHeightMm * PdfHelper.mn / originalHeight;

                double width = originalWidth * scaleX;
                double height = originalHeight * scaleY;
                var point = GetPlacementPoint(layer, width, height, pageWidth, pageHeight);

                p.save();
                p.translate(point.x, point.y);
                p.rotate(layer.Rotation);
                p.scale(scaleX, scaleY);
                p.fit_pdi_page(page, 0, 0, "");
                p.restore();
            }
            finally
            {
                p.close_pdi_page(page);
            }
        }

        internal static void CreateCodePdf(string filePath, string code, PdfPersonalizationLayer layer)
        {
            using (var writer = new PdfWriter(filePath))
            using (var pdf = new iText.Kernel.Pdf.PdfDocument(writer))
            {
                iText.Kernel.Pdf.Xobject.PdfFormXObject form;
                var color = new DeviceCmyk(
                    (float)(ClampColor(layer.C) / 100.0),
                    (float)(ClampColor(layer.M) / 100.0),
                    (float)(ClampColor(layer.Y) / 100.0),
                    (float)(ClampColor(layer.K) / 100.0));

                if (layer.CodeType == PersonalizationCodeType.Qr)
                {
                    var qr = new BarcodeQRCode(code);
                    form = qr.CreateFormXObject(color, pdf);
                }
                else if (layer.CodeType == PersonalizationCodeType.Ean13 || layer.CodeType == PersonalizationCodeType.Ean8)
                {
                    var barcode = new BarcodeEAN(pdf);
                    barcode.SetCode(code);
                    barcode.SetCodeType(layer.CodeType == PersonalizationCodeType.Ean13 ? BarcodeEAN.EAN13 : BarcodeEAN.EAN8);
                    if (!layer.ShowHumanReadableText)
                    {
                        barcode.SetFont(null);
                    }
                    else
                    {
                        barcode.SetFont(CreateBarcodeFont(layer.FontName));
                        barcode.SetSize((float)Math.Max(1, layer.FontSize));
                    }
                    if (layer.TargetHeightMm > 0)
                        barcode.SetBarHeight((float)(layer.TargetHeightMm * PdfHelper.mn));
                    form = barcode.CreateFormXObject(color, color, pdf);
                }
                else
                {
                    var barcode = new Barcode128(pdf);
                    barcode.SetCode(code);
                    barcode.SetCodeType(Barcode128.CODE128);
                    if (!layer.ShowHumanReadableText)
                    {
                        barcode.SetFont(null);
                    }
                    else
                    {
                        barcode.SetFont(CreateBarcodeFont(layer.FontName));
                        barcode.SetSize((float)Math.Max(1, layer.FontSize));
                    }
                    if (layer.TargetHeightMm > 0)
                        barcode.SetBarHeight((float)(layer.TargetHeightMm * PdfHelper.mn));
                    form = barcode.CreateFormXObject(color, color, pdf);
                }

                float width = Math.Max(1, form.GetWidth());
                float height = Math.Max(1, form.GetHeight());
                var page = pdf.AddNewPage(new PageSize(width, height));
                var canvas = new PdfCanvas(page);
                canvas.AddXObjectAt(form, 0, 0);
            }
        }

        private static PdfFont CreateBarcodeFont(string fontName)
        {
            EnsureSystemFontsRegistered();

            if (!string.IsNullOrWhiteSpace(fontName))
            {
                try
                {
                    return PdfFontFactory.CreateRegisteredFont(fontName);
                }
                catch
                {
                }
            }

            return PdfFontFactory.CreateFont();
        }

        private static void EnsureSystemFontsRegistered()
        {
            if (_systemFontsRegistered)
                return;

            PdfFontFactory.RegisterSystemDirectories();
            _systemFontsRegistered = true;
        }

        private static int OpenDocument(PDFlib p, Dictionary<string, int> openedDocuments, string path)
        {
            string fullPath = Path.GetFullPath(path);
            if (!openedDocuments.TryGetValue(fullPath, out int doc))
            {
                doc = p.open_pdi_document(fullPath, "");
                if (doc == -1)
                    throw new InvalidOperationException($"Не вдалося відкрити PDF: {fullPath}");

                openedDocuments.Add(fullPath, doc);
            }

            return doc;
        }

        private static (double x, double y) GetPlacementPoint(
            PdfPersonalizationLayer layer,
            double width,
            double height,
            double pageWidth,
            double pageHeight)
        {
            var basePoint = GetAnchorPoint(layer.BaseAnchor, pageWidth, pageHeight);
            var objectPoint = GetAnchorPoint(layer.Anchor, width, height);

            double x = basePoint.x + layer.Xmm * PdfHelper.mn - objectPoint.x;
            double y = basePoint.y + layer.Ymm * PdfHelper.mn - objectPoint.y;

            return (x, y);
        }

        private static (double x, double y) GetAnchorPoint(PersonalizationAnchorPoint anchor, double width, double height)
        {
            switch (anchor)
            {
                case PersonalizationAnchorPoint.BottomCenter:
                    return (width / 2, 0);
                case PersonalizationAnchorPoint.BottomRight:
                    return (width, 0);
                case PersonalizationAnchorPoint.LeftCenter:
                    return (0, height / 2);
                case PersonalizationAnchorPoint.Center:
                    return (width / 2, height / 2);
                case PersonalizationAnchorPoint.RightCenter:
                    return (width, height / 2);
                case PersonalizationAnchorPoint.TopLeft:
                    return (0, height);
                case PersonalizationAnchorPoint.TopCenter:
                    return (width / 2, height);
                case PersonalizationAnchorPoint.TopRight:
                    return (width, height);
                default:
                    return (0, 0);
            }
        }

        private static double ClampColor(double value)
        {
            if (value < 0)
                return 0;
            if (value > 100)
                return 100;
            return value;
        }

        private static IReadOnlyList<int> ParseExportRows(string exportRows, int rowCount)
        {
            if (rowCount <= 0)
                return Array.Empty<int>();

            if (string.IsNullOrWhiteSpace(exportRows))
                return Enumerable.Range(0, rowCount).ToList();

            var result = new SortedSet<int>();
            string[] parts = exportRows.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string rawPart in parts)
            {
                string part = rawPart.Trim();
                int dashIndex = part.IndexOf('-');

                if (dashIndex > 0)
                {
                    int start = ParseRowNumber(part.Substring(0, dashIndex), rowCount);
                    int end = ParseRowNumber(part.Substring(dashIndex + 1), rowCount);

                    if (start > end)
                    {
                        int tmp = start;
                        start = end;
                        end = tmp;
                    }

                    for (int i = start; i <= end; i++)
                        result.Add(i - 1);
                }
                else
                {
                    result.Add(ParseRowNumber(part, rowCount) - 1);
                }
            }

            if (result.Count == 0)
                throw new InvalidOperationException("Діапазон експорту не містить жодного рядка.");

            return result.ToList();
        }

        private static int ParseRowNumber(string value, int rowCount)
        {
            if (!int.TryParse(value, out int rowNumber) || rowNumber < 1 || rowNumber > rowCount)
                throw new InvalidOperationException($"Невірний номер рядка експорту: {value}. Доступний діапазон: 1-{rowCount}.");

            return rowNumber;
        }

        private static void TryDelete(string file)
        {
            try
            {
                if (File.Exists(file))
                    File.Delete(file);
            }
            catch
            {
            }
        }
    }
}
