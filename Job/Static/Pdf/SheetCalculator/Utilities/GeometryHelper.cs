using System;
using System.Collections.Generic;
using System.Drawing;
using JobSpace.Static.Pdf.SheetCalculator.Models;

namespace JobSpace.Static.Pdf.SheetCalculator.Utilities
{
    public static class GeometryHelper
    {
        public static void GetItemDimensions(Product product, int angle, out double width, out double height)
        {
            if (angle == 90 || angle == 270)
            {
                width = product.Height;
                height = product.Width;
            }
            else
            {
                width = product.Width;
                height = product.Height;
            }
        }

        public static RectangleF GetProductRect(PlacedItem item, Product product)
        {
            GetItemDimensions(product, item.Angle, out double w, out double h);
            return new RectangleF((float)item.X, (float)item.Y, (float)w, (float)h);
        }

        public static RectangleF GetTechRect(PlacedItem item, Product product)
        {
            GetItemDimensions(product, item.Angle, out double w, out double h);
            double m = product.TechMargin;
            return new RectangleF((float)(item.X - m), (float)(item.Y - m), (float)(w + 2 * m), (float)(h + 2 * m));
        }

        public static RectangleF GetPrintableArea(Sheet sheet)
        {
            float x = (float)sheet.MarginLeft;
            float y = (float)sheet.MarginTop;
            float w = (float)(sheet.Width - sheet.MarginLeft - sheet.MarginRight);
            float h = (float)(sheet.Height - sheet.MarginTop - sheet.MarginBottom);
            return new RectangleF(x, y, w, h);
        }

        public static PointF WorldToScreen(PointF worldPoint, float zoom, PointF pan)
        {
            return new PointF(
                worldPoint.X * zoom + pan.X,
                worldPoint.Y * zoom + pan.Y
            );
        }

        public static PointF ScreenToWorld(PointF screenPoint, float zoom, PointF pan)
        {
            return new PointF(
                (screenPoint.X - pan.X) / zoom,
                (screenPoint.Y - pan.Y) / zoom
            );
        }

        public static RectangleF WorldToScreen(RectangleF worldRect, float zoom, PointF pan)
        {
            return new RectangleF(
                worldRect.X * zoom + pan.X,
                worldRect.Y * zoom + pan.Y,
                worldRect.Width * zoom,
                worldRect.Height * zoom
            );
        }

        public static RectangleF ScreenToWorld(RectangleF screenRect, float zoom, PointF pan)
        {
            return new RectangleF(
                (screenRect.X - pan.X) / zoom,
                (screenRect.Y - pan.Y) / zoom,
                screenRect.Width / zoom,
                screenRect.Height / zoom
            );
        }

        public static bool CheckCollision(PlacedItem item1, Product prod1, PlacedItem item2, Product prod2)
        {
            RectangleF r1 = GetTechRect(item1, prod1);
            RectangleF r2 = GetTechRect(item2, prod2);
            return r1.IntersectsWith(r2);
        }

        public static bool IsOutsidePrintableArea(PlacedItem item, Product product, Sheet sheet)
        {
            RectangleF itemProductRect = GetProductRect(item, product);
            RectangleF printableArea = GetPrintableArea(sheet);

            // product must be fully within printable area
            // We use small epsilon for rounding errors
            float eps = 0.001f;
            return itemProductRect.Left < printableArea.Left - eps ||
                   itemProductRect.Right > printableArea.Right + eps ||
                   itemProductRect.Top < printableArea.Top - eps ||
                   itemProductRect.Bottom > printableArea.Bottom + eps;
        }

        public static bool IsOutsideSheet(PlacedItem item, Product product, Sheet sheet)
        {
            RectangleF itemTechRect = GetTechRect(item, product);
            float eps = 0.001f;
            return itemTechRect.Left < 0 - eps ||
                   itemTechRect.Right > (float)sheet.Width + eps ||
                   itemTechRect.Top < 0 - eps ||
                   itemTechRect.Bottom > (float)sheet.Height + eps;
        }

        public class SnapResult
        {
            public double SnappedX { get; set; }
            public double SnappedY { get; set; }
            public List<double> VerticalSnapLines { get; set; } = new List<double>();
            public List<double> HorizontalSnapLines { get; set; } = new List<double>();
        }

        public static SnapResult CalculateSnap(
            PlacedItem draggedItem,
            Product draggedProduct,
            List<PlacedItem> otherItems,
            Dictionary<Guid, Product> productMap,
            Sheet sheet,
            bool gridEnabled,
            double gridVal,
            double thresholdPixels, // Threshold in screen pixels
            float zoom,
            PointF pan)
        {
            SnapResult result = new SnapResult
            {
                SnappedX = draggedItem.X,
                SnappedY = draggedItem.Y
            };

            double thresholdWorld = thresholdPixels / zoom;

            GetItemDimensions(draggedProduct, draggedItem.Angle, out double w, out double h);
            double m = draggedProduct.TechMargin;

            // Gather candidate X snap lines in World coordinates
            List<double> candidateX = new List<double>
            {
                0, // sheet left
                sheet.Width, // sheet right
                sheet.MarginLeft, // print area left
                sheet.Width - sheet.MarginRight // print area right
            };

            // Gather candidate Y snap lines in World coordinates
            List<double> candidateY = new List<double>
            {
                0, // sheet top
                sheet.Height, // sheet bottom
                sheet.MarginTop, // print area top
                sheet.Height - sheet.MarginBottom // print area bottom
            };

            foreach (var other in otherItems)
            {
                if (productMap.TryGetValue(other.ProductId, out var otherProduct))
                {
                    GetItemDimensions(otherProduct, other.Angle, out double ow, out double oh);
                    double om = otherProduct.TechMargin;

                    // product borders
                    candidateX.Add(other.X);
                    candidateX.Add(other.X + ow);
                    // tech margin borders
                    candidateX.Add(other.X - om);
                    candidateX.Add(other.X + ow + om);

                    // product borders
                    candidateY.Add(other.Y);
                    candidateY.Add(other.Y + oh);
                    // tech margin borders
                    candidateY.Add(other.Y - om);
                    candidateY.Add(other.Y + oh + om);
                }
            }

            // Snap X
            // Points on our dragged item that can snap:
            // 1. Product left: X
            // 2. Product right: X + w
            // 3. Tech left: X - m
            // 4. Tech right: X + w + m
            double bestOffsetX = double.MaxValue;
            double snapLineX = double.NaN;

            double[] myPointsX = { result.SnappedX, result.SnappedX + w, result.SnappedX - m, result.SnappedX + w + m };

            foreach (double cx in candidateX)
            {
                foreach (double px in myPointsX)
                {
                    double diff = cx - px;
                    if (Math.Abs(diff) < thresholdWorld && Math.Abs(diff) < Math.Abs(bestOffsetX))
                    {
                        bestOffsetX = diff;
                        snapLineX = cx;
                    }
                }
            }

            if (bestOffsetX != double.MaxValue)
            {
                result.SnappedX += bestOffsetX;
                result.VerticalSnapLines.Add(snapLineX);
            }
            else if (gridEnabled && gridVal > 0)
            {
                // Snap to grid
                double gridSnapX = Math.Round(result.SnappedX / gridVal) * gridVal;
                if (Math.Abs(gridSnapX - result.SnappedX) < thresholdWorld)
                {
                    result.SnappedX = gridSnapX;
                }
            }

            // Snap Y
            // Points on our dragged item that can snap:
            // 1. Product top: Y
            // 2. Product bottom: Y + h
            // 3. Tech top: Y - m
            // 4. Tech bottom: Y + h + m
            double bestOffsetY = double.MaxValue;
            double snapLineY = double.NaN;

            double[] myPointsY = { result.SnappedY, result.SnappedY + h, result.SnappedY - m, result.SnappedY + h + m };

            foreach (double cy in candidateY)
            {
                foreach (double py in myPointsY)
                {
                    double diff = cy - py;
                    if (Math.Abs(diff) < thresholdWorld && Math.Abs(diff) < Math.Abs(bestOffsetY))
                    {
                        bestOffsetY = diff;
                        snapLineY = cy;
                    }
                }
            }

            if (bestOffsetY != double.MaxValue)
            {
                result.SnappedY += bestOffsetY;
                result.HorizontalSnapLines.Add(snapLineY);
            }
            else if (gridEnabled && gridVal > 0)
            {
                double gridSnapY = Math.Round(result.SnappedY / gridVal) * gridVal;
                if (Math.Abs(gridSnapY - result.SnappedY) < thresholdWorld)
                {
                    result.SnappedY = gridSnapY;
                }
            }

            return result;
        }
    }
}
