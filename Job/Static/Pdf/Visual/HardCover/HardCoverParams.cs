using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JobSpace.Static.Pdf.Visual.SoftCover.SoftCoverParams;

namespace JobSpace.Static.Pdf.Visual.HardCover
{
    /// <summary>
    /// Параметри для створення макету твердої обкладинки. Враховує розміри картонки, загин, відстань від корінця до початку картонки та ширину корінця. Також містить параметри для збереження та створення схеми обкладинки.
    /// </summary>
    public class HardCoverParams
    {
        /// <summary>
        /// Ширина картонки
        /// </summary>
        public double Width { get; set; }
        /// <summary>
        /// Висота картонки
        /// </summary>
        public double Height { get; set; }
        /// <summary>
        /// скільки паперу загинається на картонку з кожного боку (загин)
        /// </summary>
        public double Zagyn { get; set; }
        /// <summary>
        ///  відстань від корінця до початку картонки (растав)
        /// </summary>
        public double Rastav { get; set; }
        /// <summary>
        /// ширина корінця (root)
        /// </summary>
        public double Root { get; set; }
        public string FolderOutput { get; set; }
        public bool CreateSchema { get; set; }
        public bool SaveSchema { get; set; }
        public bool CreateFilePlusSchema { get; set; }
        public bool CreateBack { get; set; }
        public double DistanceAngleCut { get; set; } = 5;
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
