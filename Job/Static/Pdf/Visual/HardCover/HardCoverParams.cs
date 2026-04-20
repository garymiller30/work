using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JobSpace.Static.Pdf.Visual.SoftCover.SoftCoverParams;

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
        public bool CreateSchema { get; set; }
        public bool SaveSchema { get; set; }
        public bool CreateFilePlusSchema { get; set; }
        public bool CreateBack { get; set; }
        public bool BackAnglesCut { get; set; }

        //public CreateCommand Command { get; set; } = CreateCommand.CreateSchema;

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

        public enum CreateCommand
        {
            CreateSchema,
            CreateCover,
            Back
        }
    }
}
