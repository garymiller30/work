using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Visual.SoftCover
{
    public class SoftCoverParams
    {
        public double Bleed { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double LeftKlapan { get; set; }
        public double RightKlapan { get; set; }
        public double Root { get; set; }
        public string FolderOutput { get; set; }
        public double TotalWidth
        {
            get
            {
                return LeftKlapan + RightKlapan + Root + (Width + Bleed) * 2;
            }
        }
        public double TotalHeight
        {
            get
            {
                return Height + (Bleed * 2);
            }
        }
    }
}
