using System;
using System.Collections.Generic;
using System.Linq;
using JobSpace.Static.Pdf.SheetCalculator.Models;

namespace JobSpace.Static.Pdf.SheetCalculator.Services
{
    public class ProductCirculationResult
    {
        public Guid ProductId { get; set; }
        public int Required { get; set; }
        public int Actual { get; set; }
        public int Remaining => Required - Actual;
    }

    public static class CalculationService
    {
        public static void Recalculate(Project project)
        {
            if (project == null) return;

            // Reset sheet circulations
            foreach (var sheet in project.Sheets)
            {
                sheet.CalculatedCirculation = 0;
            }

            // Map products by Id for easy lookup
            var productMap = project.Products.ToDictionary(p => p.Id);

            // Step 1: For each sheet, count the quantity of each product type
            var sheetProductCounts = new Dictionary<Guid, Dictionary<Guid, int>>(); // SheetId -> ProductId -> Count
            
            foreach (var sheet in project.Sheets)
            {
                var counts = new Dictionary<Guid, int>();
                foreach (var item in sheet.PlacedItems)
                {
                    if (counts.ContainsKey(item.ProductId))
                        counts[item.ProductId]++;
                    else
                        counts[item.ProductId] = 1;
                }
                sheetProductCounts[sheet.Id] = counts;
            }

            // Step 2 & 3: For each sheet, find the maximum required circulation from its products
            foreach (var sheet in project.Sheets)
            {
                var counts = sheetProductCounts[sheet.Id];
                if (counts.Count == 0)
                {
                    sheet.CalculatedCirculation = 0;
                    continue;
                }

                int maxSheetRun = 0;
                foreach (var kvp in counts)
                {
                    Guid prodId = kvp.Key;
                    int countOnSheet = kvp.Value;

                    if (productMap.TryGetValue(prodId, out var product))
                    {
                        // ceil(Required / CountOnSheet)
                        int requiredSheetRun = (int)Math.Ceiling((double)product.RequiredCirculation / countOnSheet);
                        if (requiredSheetRun > maxSheetRun)
                        {
                            maxSheetRun = requiredSheetRun;
                        }
                    }
                }
                sheet.CalculatedCirculation = maxSheetRun;
            }

            // Step 4 & 5: Recalculate actual circulation for each product across all sheets
            var actualCounts = project.Products.ToDictionary(p => p.Id, p => 0);

            foreach (var sheet in project.Sheets)
            {
                var counts = sheetProductCounts[sheet.Id];
                int sheetRun = sheet.CalculatedCirculation;

                foreach (var kvp in counts)
                {
                    Guid prodId = kvp.Key;
                    int countOnSheet = kvp.Value;

                    if (actualCounts.ContainsKey(prodId))
                    {
                        actualCounts[prodId] += sheetRun * countOnSheet;
                    }
                }
            }

            // Update project items or return a summary if needed. 
            // In our architecture, the ViewModel will read actual counts from a compiled map or view model bindings.
            // Let's store actual circulations inside a results dictionary or expose it.
        }

        public static List<ProductCirculationResult> GetCirculationResults(Project project)
        {
            var results = new List<ProductCirculationResult>();
            if (project == null) return results;

            // First run recalculate to ensure data is up to date
            Recalculate(project);

            var productMap = project.Products.ToDictionary(p => p.Id);
            var actualCounts = project.Products.ToDictionary(p => p.Id, p => 0);

            foreach (var sheet in project.Sheets)
            {
                int sheetRun = sheet.CalculatedCirculation;
                foreach (var item in sheet.PlacedItems)
                {
                    if (actualCounts.ContainsKey(item.ProductId))
                    {
                        actualCounts[item.ProductId] += sheetRun;
                    }
                }
            }

            foreach (var prod in project.Products)
            {
                results.Add(new ProductCirculationResult
                {
                    ProductId = prod.Id,
                    Required = prod.RequiredCirculation,
                    Actual = actualCounts[prod.Id]
                });
            }

            return results;
        }
    }
}
