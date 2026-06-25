using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using JobSpace.Static.Pdf.SheetCalculator.Models;
using JobSpace.Static.Pdf.SheetCalculator.Utilities;

namespace JobSpace.Static.Pdf.SheetCalculator.Rendering
{
    public static class GdiRenderer
    {
        public const int RulerSize = 25;

        // Custom Modern Color Palette
        public static readonly Color ColorBg = Color.FromArgb(30, 30, 30);       // Dark gray canvas background
        public static readonly Color ColorGrid = Color.FromArgb(45, 45, 48);     // Subtle grid line
        public static readonly Color ColorSheetBg = Color.FromArgb(240, 240, 240); // Light gray sheet background
        public static readonly Color ColorSheetBorder = Color.FromArgb(80, 80, 80);
        public static readonly Color ColorUnprintableBg = Color.FromArgb(220, 220, 220); // Margins shading
        public static readonly Color ColorPrintAreaBorder = Color.FromArgb(0, 122, 204); // Cyan/blue print area outline

        // Products Palette
        public static readonly Color ColorProdTextNormal = Color.FromArgb(240, 240, 240);

        private static readonly Color[] ProductBgColors = new Color[]
        {
            Color.FromArgb(28, 80, 110),   // Dark Teal
            Color.FromArgb(38, 76, 112),   // Dark Blue
            Color.FromArgb(85, 48, 110),   // Dark Purple
            Color.FromArgb(105, 63, 115),  // Dark Lavender
            Color.FromArgb(128, 51, 92),   // Dark Pink
            Color.FromArgb(128, 59, 73),   // Dark Rose
            Color.FromArgb(138, 54, 30),   // Dark Red/Coral
            Color.FromArgb(133, 72, 23),   // Dark Orange
            Color.FromArgb(120, 85, 10),   // Dark Amber
            Color.FromArgb(87, 102, 12),   // Dark Yellow-Green
            Color.FromArgb(25, 105, 50),   // Dark Green
            Color.FromArgb(10, 81, 61),    // Dark Forest
            Color.FromArgb(9, 92, 102)     // Dark Cyan
        };

        public static void GetProductColors(Guid productId, out Color bg, out Color border)
        {
            int index = Math.Abs(productId.GetHashCode()) % ProductBgColors.Length;
            bg = ProductBgColors[index];
            
            // Generate a lighter border color dynamically
            border = Color.FromArgb(
                Math.Min(255, bg.R + 40),
                Math.Min(255, bg.G + 40),
                Math.Min(255, bg.B + 40)
            );
        }

        public static readonly Color ColorCollisionOverlay = Color.FromArgb(80, 220, 50, 50); // Red highlight for overlaps
        public static readonly Color ColorTechBorder = Color.FromArgb(120, 180, 120); // Soft green for technological margins

        public static readonly Color ColorRulerBg = Color.FromArgb(45, 45, 48);
        public static readonly Color ColorRulerText = Color.FromArgb(200, 200, 200);
        public static readonly Color ColorRulerTick = Color.FromArgb(100, 100, 100);

        public static void Render(
            Graphics g,
            int clientWidth,
            int clientHeight,
            Sheet sheet,
            Dictionary<Guid, Product> productMap,
            HashSet<Guid> selectedItemIds,
            RectangleF? marqueeRect, // In screen coordinates
            float zoom,
            PointF pan,
            PointF mousePosScreen,
            bool gridEnabled,
            double gridSizeMm,
            List<double> activeSnapX,
            List<double> activeSnapY)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // 1. Clear background
            g.Clear(ColorBg);

            // 2. Draw canvas elements (clipped to avoid drawing over rulers)
            var oldClip = g.Clip;
            g.SetClip(new Rectangle(RulerSize, RulerSize, clientWidth - RulerSize, clientHeight - RulerSize));

            // Draw grid in world coordinates
            if (gridEnabled && gridSizeMm > 0)
            {
                DrawGrid(g, clientWidth, clientHeight, zoom, pan, gridSizeMm);
            }

            if (sheet != null)
            {
                DrawSheet(g, sheet, zoom, pan);
                DrawPlacedItems(g, sheet, productMap, selectedItemIds, zoom, pan);
                DrawActiveSnapLines(g, clientWidth, clientHeight, activeSnapX, activeSnapY, zoom, pan);
            }

            // Draw selection box (in screen space, but clipped inside canvas)
            if (marqueeRect.HasValue)
            {
                using (var fillBrush = new SolidBrush(Color.FromArgb(40, 0, 122, 204)))
                using (var borderPen = new Pen(Color.FromArgb(0, 122, 204), 1) { DashStyle = DashStyle.Dash })
                {
                    g.FillRectangle(fillBrush, marqueeRect.Value);
                    g.DrawRectangle(borderPen, marqueeRect.Value.X, marqueeRect.Value.Y, marqueeRect.Value.Width, marqueeRect.Value.Height);
                }
            }

            g.Clip = oldClip;

            // 3. Draw Rulers and tracker lines (in screen space, outside clipping)
            DrawRulers(g, clientWidth, clientHeight, zoom, pan, mousePosScreen);
        }

        private static void DrawGrid(Graphics g, int clientWidth, int clientHeight, float zoom, PointF pan, double gridSizeMm)
        {
            // Convert viewport corners to world space to know what range of grid lines to draw
            PointF topLeft = GeometryHelper.ScreenToWorld(new PointF(RulerSize, RulerSize), zoom, pan);
            PointF bottomRight = GeometryHelper.ScreenToWorld(new PointF(clientWidth, clientHeight), zoom, pan);

            double gridStep = gridSizeMm;
            
            // Calculate start and end grid lines
            double startX = Math.Floor(topLeft.X / gridStep) * gridStep;
            double endX = Math.Ceiling(bottomRight.X / gridStep) * gridStep;
            double startY = Math.Floor(topLeft.Y / gridStep) * gridStep;
            double endY = Math.Ceiling(bottomRight.Y / gridStep) * gridStep;

            using (var pen = new Pen(ColorGrid, 1))
            {
                // Vertical grid lines
                for (double x = startX; x <= endX; x += gridStep)
                {
                    PointF screenPt = GeometryHelper.WorldToScreen(new PointF((float)x, 0), zoom, pan);
                    g.DrawLine(pen, screenPt.X, RulerSize, screenPt.X, clientHeight);
                }

                // Horizontal grid lines
                for (double y = startY; y <= endY; y += gridStep)
                {
                    PointF screenPt = GeometryHelper.WorldToScreen(new PointF(0, (float)y), zoom, pan);
                    g.DrawLine(pen, RulerSize, screenPt.Y, clientWidth, screenPt.Y);
                }
            }
        }

        private static void DrawSheet(Graphics g, Sheet sheet, float zoom, PointF pan)
        {
            // World coordinates of sheet: [0, 0, sheet.Width, sheet.Height]
            RectangleF sheetWorld = new RectangleF(0, 0, (float)sheet.Width, (float)sheet.Height);
            RectangleF sheetScreen = GeometryHelper.WorldToScreen(sheetWorld, zoom, pan);

            // Draw Drop Shadow
            RectangleF shadowScreen = new RectangleF(sheetScreen.X + 4, sheetScreen.Y + 4, sheetScreen.Width, sheetScreen.Height);
            using (var shadowBrush = new SolidBrush(Color.FromArgb(60, 0, 0, 0)))
            {
                g.FillRectangle(shadowBrush, shadowScreen);
            }

            // Fill Sheet background
            using (var sheetBrush = new SolidBrush(ColorSheetBg))
            {
                g.FillRectangle(sheetBrush, sheetScreen);
            }

            // Hatch unprintable margin area
            RectangleF printableAreaWorld = GeometryHelper.GetPrintableArea(sheet);
            RectangleF printableAreaScreen = GeometryHelper.WorldToScreen(printableAreaWorld, zoom, pan);

            using (var marginBrush = new HatchBrush(HatchStyle.Percent10, Color.Silver, ColorSheetBg))
            {
                // Left margin
                g.FillRectangle(marginBrush, sheetScreen.X, sheetScreen.Y, printableAreaScreen.X - sheetScreen.X, sheetScreen.Height);
                // Right margin
                g.FillRectangle(marginBrush, printableAreaScreen.Right, sheetScreen.Y, sheetScreen.Right - printableAreaScreen.Right, sheetScreen.Height);
                // Top margin
                g.FillRectangle(marginBrush, printableAreaScreen.X, sheetScreen.Y, printableAreaScreen.Width, printableAreaScreen.Y - sheetScreen.Y);
                // Bottom margin
                g.FillRectangle(marginBrush, printableAreaScreen.X, printableAreaScreen.Bottom, printableAreaScreen.Width, sheetScreen.Bottom - printableAreaScreen.Bottom);
            }

            // Draw physical sheet border
            using (var borderPen = new Pen(ColorSheetBorder, 1))
            {
                g.DrawRectangle(borderPen, sheetScreen.X, sheetScreen.Y, sheetScreen.Width, sheetScreen.Height);
            }

            // Draw printable area border
            using (var printPen = new Pen(ColorPrintAreaBorder, 1) { DashStyle = DashStyle.Dash })
            {
                g.DrawRectangle(printPen, printableAreaScreen.X, printableAreaScreen.Y, printableAreaScreen.Width, printableAreaScreen.Height);
            }
        }

        private static void DrawPlacedItems(
            Graphics g,
            Sheet sheet,
            Dictionary<Guid, Product> productMap,
            HashSet<Guid> selectedItemIds,
            float zoom,
            PointF pan)
        {
            // For collision checking
            var products = sheet.PlacedItems
                .Select(item => productMap.TryGetValue(item.ProductId, out var prod) ? (item, prod) : (item, null))
                .Where(x => x.prod != null)
                .ToList();

            foreach (var (item, prod) in products)
            {
                bool isSelected = selectedItemIds.Contains(item.Id);
                
                // Get rects in world coordinates
                RectangleF prodWorld = GeometryHelper.GetProductRect(item, prod);
                RectangleF techWorld = GeometryHelper.GetTechRect(item, prod);

                // Convert to screen
                RectangleF prodScreen = GeometryHelper.WorldToScreen(prodWorld, zoom, pan);
                RectangleF techScreen = GeometryHelper.WorldToScreen(techWorld, zoom, pan);

                // Collision checks
                bool hasCollision = false;
                // Check collision with other items
                foreach (var (otherItem, otherProd) in products)
                {
                    if (otherItem.Id != item.Id && GeometryHelper.CheckCollision(item, prod, otherItem, otherProd))
                    {
                        hasCollision = true;
                        break;
                    }
                }
                // Check if it is outside printable area
                if (GeometryHelper.IsOutsidePrintableArea(item, prod, sheet))
                {
                    hasCollision = true;
                }

                // Get the unique per-product base color
                GetProductColors(prod.Id, out Color productBg, out Color productBorder);

                // 3. Choose styling based on state
                Color fillCol;
                Color borderCol;
                Color textCol = ColorProdTextNormal;
                int borderThickness = 1;

                if (item.IsLocked)
                {
                    // Locked: dark overlay preserving a muted tint of the product color
                    fillCol = Color.FromArgb(
                        (productBg.R / 4),
                        (productBg.G / 4),
                        (productBg.B / 4));
                    borderCol = Color.FromArgb(60, 60, 60);
                    textCol = Color.FromArgb(110, 110, 110);
                }
                else
                {
                    fillCol = productBg;
                    borderCol = productBorder;
                }

                // 4. Draw Product Card with product-unique color
                using (var fillBrush = new SolidBrush(fillCol))
                using (var borderPen = new Pen(borderCol, borderThickness))
                {
                    g.FillRectangle(fillBrush, prodScreen);
                    g.DrawRectangle(borderPen, prodScreen.X, prodScreen.Y, prodScreen.Width, prodScreen.Height);
                }

                // 5. Selection Glow Overlay (drawn on top of the product color)
                if (isSelected)
                {
                    // Semi-transparent blue fill
                    using (var selBrush = new SolidBrush(Color.FromArgb(90, 0, 140, 255)))
                    using (var selPen = new Pen(Color.FromArgb(0, 162, 232), 2))
                    {
                        g.FillRectangle(selBrush, prodScreen);
                        g.DrawRectangle(selPen, prodScreen.X, prodScreen.Y, prodScreen.Width, prodScreen.Height);
                    }
                }

                // 6. Collision Overlay (drawn last so it always shows through)
                if (hasCollision)
                {
                    using (var collBrush = new SolidBrush(ColorCollisionOverlay))
                    using (var collPen = new Pen(Color.Red, 1.5f) { DashStyle = DashStyle.Dash })
                    {
                        g.FillRectangle(collBrush, prodScreen);
                        g.DrawRectangle(collPen, prodScreen.X, prodScreen.Y, prodScreen.Width, prodScreen.Height);
                    }
                }

                // 7. Tech margin border tinted to match product color
                using (var techPen = new Pen(Color.FromArgb(160,
                    Math.Min(255, productBg.R + 60),
                    Math.Min(255, productBg.G + 60),
                    Math.Min(255, productBg.B + 60)), 1) { DashStyle = DashStyle.Dot })
                {
                    g.DrawRectangle(techPen, techScreen.X, techScreen.Y, techScreen.Width, techScreen.Height);
                }

                // 5. Draw Info Texts (Name, Size, Angle, Lock status)
                // Use a smaller font if zoomed out too much
                float fontSize = Math.Max(7f, Math.Min(10f, 8f * zoom));
                
                // Only render text if the card is big enough on screen
                if (prodScreen.Width > 35 && prodScreen.Height > 20)
                {
                    using (var fontName = new Font("Segoe UI", fontSize, FontStyle.Bold))
                    using (var fontDesc = new Font("Segoe UI", fontSize - 1.5f, FontStyle.Regular))
                    using (var textBrush = new SolidBrush(textCol))
                    {
                        var format = new StringFormat
                        {
                            Alignment = StringAlignment.Center,
                            LineAlignment = StringAlignment.Center
                        };

                        // Group indicator/text if part of a group
                        string displayTitle = prod.Name;
                        if (item.GroupId.HasValue)
                        {
                            displayTitle = $"[G] {prod.Name}";
                        }

                        if (item.IsLocked)
                        {
                            displayTitle = $"🔒 {displayTitle}";
                        }

                        // Dimensions: e.g. "90 x 50"
                        string sizeStr = $"{prod.Width}x{prod.Height} mm";
                        if (item.Angle != 0)
                        {
                            sizeStr += $" ⟳ {item.Angle}°";
                        }

                        // Draw Title in upper half, description in lower half
                        var nameRect = new RectangleF(prodScreen.X, prodScreen.Y + 2, prodScreen.Width, prodScreen.Height * 0.5f);
                        var descRect = new RectangleF(prodScreen.X, prodScreen.Y + prodScreen.Height * 0.5f - 2, prodScreen.Width, prodScreen.Height * 0.5f);

                        g.DrawString(displayTitle, fontName, textBrush, nameRect, format);
                        g.DrawString(sizeStr, fontDesc, textBrush, descRect, format);
                    }
                }
            }
        }

        private static void DrawActiveSnapLines(
            Graphics g,
            int clientWidth,
            int clientHeight,
            List<double> activeSnapX,
            List<double> activeSnapY,
            float zoom,
            PointF pan)
        {
            if (activeSnapX == null || activeSnapY == null) return;

            using (var snapPen = new Pen(Color.FromArgb(255, 85, 0), 1) { DashStyle = DashStyle.Dash })
            {
                foreach (double x in activeSnapX)
                {
                    PointF pt = GeometryHelper.WorldToScreen(new PointF((float)x, 0), zoom, pan);
                    g.DrawLine(snapPen, pt.X, RulerSize, pt.X, clientHeight);
                }

                foreach (double y in activeSnapY)
                {
                    PointF pt = GeometryHelper.WorldToScreen(new PointF(0, (float)y), zoom, pan);
                    g.DrawLine(snapPen, RulerSize, pt.Y, clientWidth, pt.Y);
                }
            }
        }

        private static void DrawRulers(Graphics g, int clientWidth, int clientHeight, float zoom, PointF pan, PointF mousePosScreen)
        {
            using (var bgBrush = new SolidBrush(ColorRulerBg))
            using (var textBrush = new SolidBrush(ColorRulerText))
            using (var tickPen = new Pen(ColorRulerTick, 1))
            using (var trackerPen = new Pen(Color.FromArgb(0, 162, 232), 1))
            using (var font = new Font("Segoe UI", 8, FontStyle.Regular))
            {
                // Top Ruler Background
                g.FillRectangle(bgBrush, 0, 0, clientWidth, RulerSize);
                // Left Ruler Background
                g.FillRectangle(bgBrush, 0, 0, RulerSize, clientHeight);

                // Top Ruler Ticks & Numbers
                // Convert screen bounds to world coordinates
                PointF worldLeft = GeometryHelper.ScreenToWorld(new PointF(RulerSize, 0), zoom, pan);
                PointF worldRight = GeometryHelper.ScreenToWorld(new PointF(clientWidth, 0), zoom, pan);

                // Grid divisions on ruler (typically tick every 5mm, labels every 50mm, or adjusted by zoom)
                double step = 10.0; // 10mm
                if (zoom < 0.25f) step = 100.0;
                else if (zoom < 0.8f) step = 50.0;
                else if (zoom > 3.0f) step = 2.0;

                double firstValX = Math.Ceiling(worldLeft.X / step) * step;
                double lastValX = Math.Floor(worldRight.X / step) * step;

                for (double val = firstValX; val <= lastValX; val += step)
                {
                    PointF pt = GeometryHelper.WorldToScreen(new PointF((float)val, 0), zoom, pan);
                    
                    // Tick line
                    bool isMajor = Math.Abs(val % (step * 5)) < 0.001;
                    int tickLen = isMajor ? 12 : 6;
                    g.DrawLine(tickPen, pt.X, RulerSize - tickLen, pt.X, RulerSize);

                    if (isMajor || zoom > 1.5f)
                    {
                        g.DrawString(val.ToString("0"), font, textBrush, pt.X + 2, 2);
                    }
                }

                // Left Ruler Ticks & Numbers
                PointF worldTop = GeometryHelper.ScreenToWorld(new PointF(0, RulerSize), zoom, pan);
                PointF worldBottom = GeometryHelper.ScreenToWorld(new PointF(0, clientHeight), zoom, pan);

                double firstValY = Math.Ceiling(worldTop.Y / step) * step;
                double lastValY = Math.Floor(worldBottom.Y / step) * step;

                for (double val = firstValY; val <= lastValY; val += step)
                {
                    PointF pt = GeometryHelper.WorldToScreen(new PointF(0, (float)val), zoom, pan);

                    bool isMajor = Math.Abs(val % (step * 5)) < 0.001;
                    int tickLen = isMajor ? 12 : 6;
                    g.DrawLine(tickPen, RulerSize - tickLen, pt.Y, RulerSize, pt.Y);

                    if (isMajor || zoom > 1.5f)
                    {
                        // Draw vertically or rotated for nice look, but plain horizontal text in ruler width is fine
                        g.DrawString(val.ToString("0"), font, textBrush, 2, pt.Y + 2);
                    }
                }

                // Ruler Borders
                using (var linePen = new Pen(Color.FromArgb(70, 70, 70), 1))
                {
                    g.DrawLine(linePen, RulerSize, 0, RulerSize, clientHeight);
                    g.DrawLine(linePen, 0, RulerSize, clientWidth, RulerSize);
                }

                // Top-Left corner box
                g.FillRectangle(bgBrush, 0, 0, RulerSize, RulerSize);
                g.DrawRectangle(tickPen, 0, 0, RulerSize, RulerSize);
                using (var boldFont = new Font("Segoe UI", 7, FontStyle.Bold))
                {
                    g.DrawString("mm", boldFont, textBrush, 3, 5);
                }

                // Draw Cursor Tracker lines if mouse is inside canvas
                if (mousePosScreen.X >= RulerSize && mousePosScreen.Y >= RulerSize)
                {
                    g.DrawLine(trackerPen, mousePosScreen.X, 0, mousePosScreen.X, RulerSize);
                    g.DrawLine(trackerPen, 0, mousePosScreen.Y, RulerSize, mousePosScreen.Y);
                }
            }
        }
    }
}
