using System;
using System.Collections.Generic;
using JobSpace.Static.Pdf.SheetCalculator.Models;
using JobSpace.Static.Pdf.SheetCalculator.Utilities;

namespace JobSpace.Static.Pdf.SheetCalculator.Services
{
    public static class NestingService
    {
        public static List<PlacedItem> AutoLayout(Product product, Sheet sheet)
        {
            var placedItems = new List<PlacedItem>();
            if (product == null || sheet == null) return placedItems;

            // Try Orientation 1: Normal (Angle = 0)
            int countNormal = GetGridCapacity(product.Width, product.Height, product.TechMargin, sheet, out int colsNormal, out int rowsNormal);

            // Try Orientation 2: Rotated 90 degrees (Angle = 90)
            int countRotated = GetGridCapacity(product.Height, product.Width, product.TechMargin, sheet, out int colsRotated, out int rowsRotated);

            int bestAngle = 0;
            int bestCols = colsNormal;
            int bestRows = rowsNormal;
            double w = product.Width;
            double h = product.Height;

            if (countRotated > countNormal)
            {
                bestAngle = 90;
                bestCols = colsRotated;
                bestRows = rowsRotated;
                w = product.Height;
                h = product.Width;
            }

            if (bestCols <= 0 || bestRows <= 0) return placedItems;

            double m = product.TechMargin;
            double startX = Math.Max(sheet.MarginLeft, m);
            double startY = Math.Max(sheet.MarginTop, m);

            // Generate items
            for (int r = 0; r < bestRows; r++)
            {
                for (int c = 0; c < bestCols; c++)
                {
                    double x = startX + c * (w + 2 * m);
                    double y = startY + r * (h + 2 * m);

                    placedItems.Add(new PlacedItem
                    {
                        Id = Guid.NewGuid(),
                        ProductId = product.Id,
                        X = x,
                        Y = y,
                        Angle = bestAngle,
                        IsLocked = false
                    });
                }
            }

            return placedItems;
        }

        private static int GetGridCapacity(
            double prodW, 
            double prodH, 
            double techMargin, 
            Sheet sheet, 
            out int cols, 
            out int rows)
        {
            cols = 0;
            rows = 0;

            double startX = Math.Max(sheet.MarginLeft, techMargin);
            double limitX = Math.Min(sheet.Width - sheet.MarginRight, sheet.Width - techMargin);

            if (limitX - startX < prodW) return 0;
            cols = 1 + (int)Math.Floor((limitX - prodW - startX) / (prodW + 2 * techMargin));
            if (cols < 0) cols = 0;

            double startY = Math.Max(sheet.MarginTop, techMargin);
            double limitY = Math.Min(sheet.Height - sheet.MarginBottom, sheet.Height - techMargin);

            if (limitY - startY < prodH) return 0;
            rows = 1 + (int)Math.Floor((limitY - prodH - startY) / (prodH + 2 * techMargin));
            if (rows < 0) rows = 0;

            return cols * rows;
        }
    }
}
