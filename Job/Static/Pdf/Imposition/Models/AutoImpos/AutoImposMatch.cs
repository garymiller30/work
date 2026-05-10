using JobSpace.Static.Pdf.Imposition.Models;
using System.Collections.Generic;

namespace JobSpace.Static.Pdf.Imposition.Models.AutoImpos
{
    public class AutoImposMatch
    {
        public AutoImposRule Rule { get; set; }
        public List<PrintSheet> PrintSheets { get; set; } = new List<PrintSheet>();
        public int Score { get; set; }
        public List<string> Reasons { get; set; } = new List<string>();

        public bool HasTemplate => Rule != null && PrintSheets.Count > 0;
    }
}
