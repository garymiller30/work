using System;
using System.Globalization;
using System.Linq;

namespace PluginWorkProcessPlates
{
    [Serializable]
    public sealed class Format
    {
        public decimal Width { get; set; }
        public decimal Height { get; set; }

        public override string ToString()
        {
            return $"{Width:N1} x {Height:N1}";
        }

        public override int GetHashCode()
        {
            return ($"{Width.ToString(CultureInfo.InvariantCulture)}{Height.ToString(CultureInfo.InvariantCulture)}").GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (obj is Format format)
            {
                return format.Width == Width && format.Height == Height;
            }

            return false;
        }
    }
}
