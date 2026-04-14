using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models
{
    /// <summary>
    /// кольори, які використовуються в спуску
    /// </summary>
    public class ImposColors
    {
        public List<ImposColor> Colors { get; set; } = new List<ImposColor>();

        public void Add(ImposColor c)
        {
            Colors.Add(c);
        }
    }
}
