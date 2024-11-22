using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models
{
    public class MarkColor
    {
        public static MarkColor Registration = new MarkColor
        {
            IsSpot = true,
            Name = "All",
            C = 100,
            M = 100,
            Y = 100,
            K = 100,
        };

        public static MarkColor ProofColor = new MarkColor
        {
            IsSpot = true,
            Name = "ProofColor",
            IsOverprint = true,
            C = 79,
            M = 0,
            Y = 44,
            K = 21
        };

        public static MarkColor Cyan = new MarkColor
        {
            C = 100,
            M = 0,
            Y = 0,
            K = 0,
        };

        public static MarkColor Magenta  = new MarkColor
        {
            C = 0,
            M = 100,
            Y = 0,
            K = 0,
        };

        public static MarkColor Yellow = new MarkColor
        {
            C = 0,
            M = 0,
            Y = 100,
            K = 0,
        };

        public static MarkColor Black = new MarkColor
        {
            C = 0,
            M = 0,
            Y = 0,
            K = 100
        };

        public bool IsSpot { get; set; } = false;
        public string Name { get; set; } = "Spot";
        public double C { get; set; } = 0;
        public double M { get; set; } = 0;
        public double Y { get; set; } = 0;
        public double K { get; set; } = 100;

        public double Opasity { get; set; } = 100;
        public bool IsOverprint { get; set; } = false;


    }
}
