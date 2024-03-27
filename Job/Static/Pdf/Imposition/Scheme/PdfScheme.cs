using Job.Static.Pdf.Imposition.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Scheme
{
    public class PdfScheme
    {
        public PdfMasterScheme MasterScheme { get; }
        public PdfMasterPage MasterPage { get;}

        public int RowIdx { get;set;}
        public int ColumnIdx { get; set; }
        public double Rotate { get; set; }

        public PdfScheme(PdfMasterScheme masterScheme, PdfMasterPage masterPage)
        {
            MasterScheme = masterScheme;
            MasterPage = masterPage;
        }

        /// <summary>
        /// отримати ширину в залежності від поворота
        /// </summary>
        /// <returns></returns>
        public double GetRotatedWidth()
        {
            if (Rotate == 0 || Rotate == 180) return MasterPage.Width;
            return MasterPage.Height;
        }
        /// <summary>
        /// отримати висоту в залежності від поворота
        /// </summary>
        /// <returns></returns>
        public double GetRotatedHeight()
        {
            if (Rotate == 0 || Rotate == 180) return MasterPage.Height;
            return MasterPage.Width;
        }
    }
}
