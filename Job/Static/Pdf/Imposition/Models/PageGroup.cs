using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models
{
    public class PageGroup
    {
        public int Id { get; set; }

        public List<TemplatePage> Pages { get; set; } = new List<TemplatePage>();

        public RectangleD Rectangle { get; set; }
    }
}
