using System;

namespace Interfaces.PdfUtils
{
    [Flags]
    public enum ColorSpaces
    {
        Cmyk = 1,
        Gray = 2,
        Rgb = 4,
        Lab = 8,
        Spot = 16,
        Pattern = 32,
        Unknown = 64,
        ICCBased = 128,
        All = 256
    }
}
