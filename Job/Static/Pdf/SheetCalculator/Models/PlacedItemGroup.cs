using System;
using System.Collections.Generic;

namespace JobSpace.Static.Pdf.SheetCalculator.Models
{
    public class PlacedItemGroup
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public List<Guid> PlacedItemIds { get; set; } = new List<Guid>();

        public PlacedItemGroup Clone()
        {
            return new PlacedItemGroup
            {
                Id = Id,
                PlacedItemIds = new List<Guid>(PlacedItemIds)
            };
        }
    }
}
