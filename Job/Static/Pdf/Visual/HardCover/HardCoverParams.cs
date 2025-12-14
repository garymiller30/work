using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Visual.HardCover
{
    public class HardCoverParams
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public double Zagyn { get; set; }
        public double Rastav { get; set; }
        public double Root { get; set; }
        public string FolderOutput { get; set; }

        public double TotalWidth
        {
            get
            {
                return Root + (Width + Rastav + Zagyn) * 2;
            }
        }
        public double TotalHeight
        {
            get
            {
                return Height + (Zagyn * 2);
            }
        }
    }
}
