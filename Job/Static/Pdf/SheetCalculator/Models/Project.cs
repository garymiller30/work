using System;
using System.Collections.Generic;
using System.Linq;

namespace JobSpace.Static.Pdf.SheetCalculator.Models
{
    public class Project
    {
        public List<Sheet> Sheets { get; set; } = new List<Sheet>();
        public List<Product> Products { get; set; } = new List<Product>();
        public List<PlacedItemGroup> Groups { get; set; } = new List<PlacedItemGroup>();

        // Editor Settings
        public bool GridSnapEnabled { get; set; } = false;
        public double GridSize { get; set; } = 5; // in mm
        public bool StrictCollision { get; set; } = false;

        public Project Clone()
        {
            return new Project
            {
                Sheets = Sheets.Select(s => s.Clone()).ToList(),
                Products = Products.Select(p => p.Clone()).ToList(),
                Groups = Groups.Select(g => g.Clone()).ToList(),
                GridSnapEnabled = GridSnapEnabled,
                GridSize = GridSize,
                StrictCollision = StrictCollision
            };
        }
    }
}
