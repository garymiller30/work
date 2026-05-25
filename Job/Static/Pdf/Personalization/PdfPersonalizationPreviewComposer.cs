using JobSpace.Static.Pdf.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace JobSpace.Static.Pdf.Personalization
{
    public sealed class PdfPersonalizationPreviewComposer : IDisposable
    {
        private const double MinScale = 0.01;
        private readonly Dictionary<string, CachedBitmap> _cache = new Dictionary<string, CachedBitmap>(StringComparer.OrdinalIgnoreCase);

        public Bitmap Compose(PdfPersonalizationSettings settings, PdfPersonalizationData data, int rowIndex, int dpi)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (data.Rows.Count == 0)
                throw new InvalidOperationException("Файл персоналізації не містить рядків даних.");

            rowIndex = Math.Max(0, Math.Min(rowIndex, data.Rows.Count - 1));
            IReadOnlyDictionary<string, string> row = data.Rows[rowIndex];

            string basePath = ResolveBasePath(settings, data, row);
            PdfPageInfo baseInfo = PdfHelper.GetPageInfo(basePath, 0);
            Box pageBox = baseInfo.Mediabox ?? baseInfo.Trimbox ?? new Box { left = 0, bottom = 0, width = 1, height = 1 };
            Box viewBox = baseInfo.Trimbox ?? pageBox;
            double pageWidth = Math.Max(1, pageBox.width);
            double pageHeight = Math.Max(1, pageBox.height);
            double pixelsPerPoint = dpi / 72.0;
            int bitmapWidth = Math.Max(1, (int)Math.Round(viewBox.width * pixelsPerPoint));
            int bitmapHeight = Math.Max(1, (int)Math.Round(viewBox.height * pixelsPerPoint));

            var result = new Bitmap(bitmapWidth, bitmapHeight, PixelFormat.Format32bppArgb);
            result.SetResolution(dpi, dpi);

            using (Graphics graphics = Graphics.FromImage(result))
            {
                graphics.Clear(Color.White);
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

                foreach (PdfPersonalizationLayer layer in settings.Layers.Where(x => x.Enabled))
                {
                    DrawLayer(graphics, settings, data, row, layer, pageWidth, pageHeight, viewBox, pixelsPerPoint, dpi);
                }
            }

            return result;
        }

        private void DrawLayer(
            Graphics graphics,
            PdfPersonalizationSettings settings,
            PdfPersonalizationData data,
            IReadOnlyDictionary<string, string> row,
            PdfPersonalizationLayer layer,
            double pageWidth,
            double pageHeight,
            Box viewBox,
            double pixelsPerPoint,
            int dpi)
        {
            if (layer.Type == PersonalizationLayerType.BasePdf)
            {
                string path = string.IsNullOrWhiteSpace(layer.Source)
                    ? settings.BasePdfPath
                    : data.ResolveFile(row, layer.Source);

                DrawFullPagePdf(graphics, path, viewBox, pixelsPerPoint, dpi);
                return;
            }

            if (layer.Type == PersonalizationLayerType.Pdf)
            {
                DrawPlacedPdf(graphics, data.ResolveFile(row, layer.Source), layer, pageWidth, pageHeight, viewBox, pixelsPerPoint, dpi);
                return;
            }

            if (layer.Type == PersonalizationLayerType.Text)
            {
                DrawText(graphics, data.GetValue(row, layer.Source), layer, pageWidth, pageHeight, viewBox, pixelsPerPoint, dpi);
                return;
            }

            if (layer.Type == PersonalizationLayerType.Code)
            {
                DrawCode(graphics, data.GetValue(row, layer.Source), layer, pageWidth, pageHeight, viewBox, pixelsPerPoint, dpi);
            }
        }

        private void DrawFullPagePdf(Graphics graphics, string path, Box viewBox, double pixelsPerPoint, int dpi)
        {
            CachedBitmap cached = GetPdfBitmap(path, dpi);
            if (cached == null)
                return;

            graphics.DrawImage(
                cached.Bitmap,
                new RectangleF(0, 0, (float)(viewBox.width * pixelsPerPoint), (float)(viewBox.height * pixelsPerPoint)));
        }

        private void DrawPlacedPdf(
            Graphics graphics,
            string path,
            PdfPersonalizationLayer layer,
            double pageWidth,
            double pageHeight,
            Box viewBox,
            double pixelsPerPoint,
            int dpi)
        {
            CachedBitmap cached = GetPdfBitmap(path, dpi);
            if (cached == null)
                return;

            double scale = Math.Max(MinScale, layer.ScalePercent / 100.0);
            DrawBitmapLayer(
                graphics,
                cached.Bitmap,
                cached.PageWidthPt * scale,
                cached.PageHeightPt * scale,
                cached.TrimLeftPt * scale,
                cached.TrimBottomPt * scale,
                cached.TrimWidthPt * scale,
                cached.TrimHeightPt * scale,
                layer,
                pageWidth,
                pageHeight,
                viewBox,
                pixelsPerPoint);
        }

        private void DrawCode(
            Graphics graphics,
            string code,
            PdfPersonalizationLayer layer,
            double pageWidth,
            double pageHeight,
            Box viewBox,
            double pixelsPerPoint,
            int dpi)
        {
            if (string.IsNullOrWhiteSpace(code))
                return;

            CachedBitmap cached = GetCodeBitmap(code, layer, dpi);
            if (cached == null)
                return;

            double scaleX = Math.Max(MinScale, layer.ScalePercent / 100.0);
            double scaleY = scaleX;
            if (layer.TargetWidthMm > 0)
                scaleX = layer.TargetWidthMm * PdfHelper.mn / cached.PageWidthPt;
            if (layer.TargetHeightMm > 0)
                scaleY = layer.TargetHeightMm * PdfHelper.mn / cached.PageHeightPt;

            DrawBitmapLayer(
                graphics,
                cached.Bitmap,
                cached.PageWidthPt * scaleX,
                cached.PageHeightPt * scaleY,
                cached.TrimLeftPt * scaleX,
                cached.TrimBottomPt * scaleY,
                cached.TrimWidthPt * scaleX,
                cached.TrimHeightPt * scaleY,
                layer,
                pageWidth,
                pageHeight,
                viewBox,
                pixelsPerPoint);
        }

        private void DrawBitmapLayer(
            Graphics graphics,
            Bitmap bitmap,
            double widthPt,
            double heightPt,
            double drawOffsetXPt,
            double drawOffsetYPt,
            double drawWidthPt,
            double drawHeightPt,
            PdfPersonalizationLayer layer,
            double pageWidth,
            double pageHeight,
            Box viewBox,
            double pixelsPerPoint)
        {
            var point = GetPlacementPoint(layer, widthPt, heightPt, pageWidth, pageHeight);
            GraphicsState state = graphics.Save();
            try
            {
                double viewTop = viewBox.bottom + viewBox.height;
                graphics.TranslateTransform((float)((point.x - viewBox.left) * pixelsPerPoint), (float)((viewTop - point.y) * pixelsPerPoint));
                graphics.RotateTransform((float)-layer.Rotation);
                graphics.DrawImage(
                    bitmap,
                    new RectangleF(
                        (float)(drawOffsetXPt * pixelsPerPoint),
                        (float)(-(drawOffsetYPt + drawHeightPt) * pixelsPerPoint),
                        (float)(drawWidthPt * pixelsPerPoint),
                        (float)(drawHeightPt * pixelsPerPoint)));
            }
            finally
            {
                graphics.Restore(state);
            }
        }

        private void DrawText(
            Graphics graphics,
            string text,
            PdfPersonalizationLayer layer,
            double pageWidth,
            double pageHeight,
            Box viewBox,
            double pixelsPerPoint,
            int dpi)
        {
            if (string.IsNullOrEmpty(text))
                return;

            float fontSizePx = (float)(Math.Max(1, layer.FontSize) * dpi / 72.0);
            using (var font = new Font(string.IsNullOrWhiteSpace(layer.FontName) ? "Arial" : layer.FontName, fontSizePx, GraphicsUnit.Pixel))
            using (var brush = new SolidBrush(CmykToRgb(layer.C, layer.M, layer.Y, layer.K)))
            using (var format = new StringFormat(StringFormat.GenericTypographic))
            {
                SizeF size = graphics.MeasureString(text, font, PointF.Empty, format);
                double widthPt = size.Width / pixelsPerPoint;
                double heightPt = Math.Max(1, layer.FontSize);
                var point = GetPlacementPoint(layer, widthPt, heightPt, pageWidth, pageHeight);
                float baselinePx = GetBaselineOffset(font);

                GraphicsState state = graphics.Save();
                try
                {
                    double viewTop = viewBox.bottom + viewBox.height;
                    graphics.TranslateTransform((float)((point.x - viewBox.left) * pixelsPerPoint), (float)((viewTop - point.y) * pixelsPerPoint));
                    graphics.RotateTransform((float)-layer.Rotation);
                    graphics.DrawString(text, font, brush, new PointF(0, -baselinePx), format);
                }
                finally
                {
                    graphics.Restore(state);
                }
            }
        }

        private static float GetBaselineOffset(Font font)
        {
            FontFamily family = font.FontFamily;
            FontStyle style = font.Style;
            int emHeight = family.GetEmHeight(style);
            if (emHeight <= 0)
                return font.Size;

            return font.Size * family.GetCellAscent(style) / emHeight;
        }

        private CachedBitmap GetPdfBitmap(string path, int dpi)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                return null;

            string fullPath = Path.GetFullPath(path);
            DateTime writeTimeUtc = File.GetLastWriteTimeUtc(fullPath);
            string key = $"pdf|{fullPath}|{writeTimeUtc.Ticks}|{dpi}";

            if (_cache.TryGetValue(key, out CachedBitmap cached))
                return cached;

            PdfPageInfo info = PdfHelper.GetPageInfo(fullPath, 0);
            Bitmap bitmap = PdfHelper.RenderByTrimBox(fullPath, 0, dpi);
            Box pageBox = info.Mediabox ?? info.Trimbox ?? new Box { left = 0, bottom = 0, width = bitmap.Width * 72.0 / dpi, height = bitmap.Height * 72.0 / dpi };
            Box trimBox = info.Trimbox ?? pageBox;
            cached = new CachedBitmap(bitmap, pageBox, trimBox);
            _cache[key] = cached;
            TrimCache();
            return cached;
        }

        private CachedBitmap GetCodeBitmap(string code, PdfPersonalizationLayer layer, int dpi)
        {
            string key = $"code|{dpi}|{GetLayerContentKey(code, layer)}";
            if (_cache.TryGetValue(key, out CachedBitmap cached))
                return cached;

            string tempFile = Path.Combine(Path.GetTempPath(), $"pdf_personalization_code_preview_{Guid.NewGuid():N}.pdf");
            try
            {
                PdfPersonalizationRenderer.CreateCodePdf(tempFile, code, layer);
                PdfPageInfo info = PdfHelper.GetPageInfo(tempFile, 0);
                Bitmap bitmap = PdfHelper.RenderByTrimBox(tempFile, 0, dpi);
                Box pageBox = info.Mediabox ?? info.Trimbox ?? new Box { left = 0, bottom = 0, width = bitmap.Width * 72.0 / dpi, height = bitmap.Height * 72.0 / dpi };
                Box trimBox = info.Trimbox ?? pageBox;
                cached = new CachedBitmap(bitmap, pageBox, trimBox);
                _cache[key] = cached;
                TrimCache();
                return cached;
            }
            finally
            {
                TryDelete(tempFile);
            }
        }

        private static string ResolveBasePath(PdfPersonalizationSettings settings, PdfPersonalizationData data, IReadOnlyDictionary<string, string> row)
        {
            PdfPersonalizationLayer baseLayer = settings.Layers.FirstOrDefault(x => x.Enabled && x.Type == PersonalizationLayerType.BasePdf);
            if (baseLayer == null || string.IsNullOrWhiteSpace(baseLayer.Source))
                return settings.BasePdfPath;

            string path = data.ResolveFile(row, baseLayer.Source);
            return string.IsNullOrWhiteSpace(path) ? settings.BasePdfPath : path;
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

        private static Color CmykToRgb(double c, double m, double y, double k)
        {
            double cc = ClampColor(c) / 100.0;
            double mm = ClampColor(m) / 100.0;
            double yy = ClampColor(y) / 100.0;
            double kk = ClampColor(k) / 100.0;

            int r = (int)Math.Round(255 * (1 - cc) * (1 - kk));
            int g = (int)Math.Round(255 * (1 - mm) * (1 - kk));
            int b = (int)Math.Round(255 * (1 - yy) * (1 - kk));
            return Color.FromArgb(ClampByte(r), ClampByte(g), ClampByte(b));
        }

        private static double ClampColor(double value)
        {
            if (value < 0)
                return 0;
            if (value > 100)
                return 100;
            return value;
        }

        private static int ClampByte(int value)
        {
            if (value < 0)
                return 0;
            if (value > 255)
                return 255;
            return value;
        }

        private static string GetLayerContentKey(string code, PdfPersonalizationLayer layer)
        {
            string raw = string.Join("|",
                code,
                layer.CodeType,
                layer.ShowHumanReadableText,
                layer.FontName,
                layer.FontSize.ToString(System.Globalization.CultureInfo.InvariantCulture),
                layer.C.ToString(System.Globalization.CultureInfo.InvariantCulture),
                layer.M.ToString(System.Globalization.CultureInfo.InvariantCulture),
                layer.Y.ToString(System.Globalization.CultureInfo.InvariantCulture),
                layer.K.ToString(System.Globalization.CultureInfo.InvariantCulture));

            using (SHA256 sha = SHA256.Create())
            {
                return System.Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(raw)));
            }
        }

        private void TrimCache()
        {
            const int maxItems = 64;
            if (_cache.Count <= maxItems)
                return;

            foreach (string key in _cache.Keys.Take(_cache.Count - maxItems).ToList())
            {
                _cache[key].Dispose();
                _cache.Remove(key);
            }
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

        public void Dispose()
        {
            foreach (CachedBitmap cached in _cache.Values)
                cached.Dispose();
            _cache.Clear();
        }

        private sealed class CachedBitmap : IDisposable
        {
            public CachedBitmap(Bitmap bitmap, Box pageBox, Box trimBox)
            {
                Bitmap = bitmap;
                PageWidthPt = Math.Max(1, pageBox.width);
                PageHeightPt = Math.Max(1, pageBox.height);
                TrimLeftPt = trimBox.left;
                TrimBottomPt = trimBox.bottom;
                TrimWidthPt = Math.Max(1, trimBox.width);
                TrimHeightPt = Math.Max(1, trimBox.height);
            }

            public Bitmap Bitmap { get; }
            public double PageWidthPt { get; }
            public double PageHeightPt { get; }
            public double TrimLeftPt { get; }
            public double TrimBottomPt { get; }
            public double TrimWidthPt { get; }
            public double TrimHeightPt { get; }

            public void Dispose()
            {
                Bitmap.Dispose();
            }
        }
    }
}
