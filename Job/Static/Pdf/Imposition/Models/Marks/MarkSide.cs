using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models.Marks
{
    public class MarkSide
    {
        public bool SingleSide { get; set; } = false;
        public bool Sheetwise { get; set; } = false;
        public bool WorkAndTurn { get; set; } = false;
        public bool Perfecting { get;set; } = false;
    }
}
