using System;
using System.Collections.Generic;
using System.Linq;

namespace JobSpace.Static.Pdf.SheetCalculator.Models
{
    public class Sheet
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "Новий лист";
        public double Width { get; set; } = 700; // in mm
        public double Height { get; set; } = 500; // in mm

        // Unprintable margins (left, right, top, bottom)
        public double MarginLeft { get; set; } = 10;
        public double MarginRight { get; set; } = 10;
        public double MarginTop { get; set; } = 10;
        public double MarginBottom { get; set; } = 10;

        public List<PlacedItem> PlacedItems { get; set; } = new List<PlacedItem>();

        // Calculated field: circulation of sheets
        public int CalculatedCirculation { get; set; }

        public Sheet Clone()
        {
            return new Sheet
            {
                Id = Id,
                Name = Name,
                Width = Width,
                Height = Height,
                MarginLeft = MarginLeft,
                MarginRight = MarginRight,
                MarginTop = MarginTop,
                MarginBottom = MarginBottom,
                CalculatedCirculation = CalculatedCirculation,
                PlacedItems = PlacedItems.Select(x => x.Clone()).ToList()
            };
        }
    }
}
