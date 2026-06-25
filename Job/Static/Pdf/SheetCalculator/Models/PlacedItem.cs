using System;

namespace JobSpace.Static.Pdf.SheetCalculator.Models
{
    public class PlacedItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ProductId { get; set; }
        public double X { get; set; } // position in mm
        public double Y { get; set; } // position in mm
        public int Angle { get; set; } // 0, 90, 180, 270
        public bool IsLocked { get; set; }
        public Guid? GroupId { get; set; }

        public PlacedItem Clone()
        {
            return new PlacedItem
            {
                Id = Id,
                ProductId = ProductId,
                X = X,
                Y = Y,
                Angle = Angle,
                IsLocked = IsLocked,
                GroupId = GroupId
            };
        }
    }
}
