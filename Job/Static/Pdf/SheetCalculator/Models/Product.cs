using System;

namespace JobSpace.Static.Pdf.SheetCalculator.Models
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "Новий виріб";
        public double Width { get; set; } = 90; // in mm
        public double Height { get; set; } = 50; // in mm
        public int RequiredCirculation { get; set; } = 1000;
        public double TechMargin { get; set; } = 2; // uniform bleed/offset in mm

        public Product Clone()
        {
            return new Product
            {
                Id = Id,
                Name = Name,
                Width = Width,
                Height = Height,
                RequiredCirculation = RequiredCirculation,
                TechMargin = TechMargin
            };
        }
    }
}
