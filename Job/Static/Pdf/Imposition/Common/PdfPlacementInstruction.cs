using Job.Static.Pdf.Imposition.Sheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Common
{
    public class PdfPlacementInstruction
    {
        PdfFormat _format;
        public int Columns { get; set; }
        public int Rows { get; set; }

        public double Rotate { get; set; }

        public PdfPlacementInstruction(PdfFormat format)
        {
            _format = format;
        }

        public int GetCount(PdfSheetRun sheet)
        {
            if (Rotate == 90)
            {
                Columns = (int)(sheet.Height / _format.Width);
                Rows = (int)(sheet.Width / _format.Height);

            }
            else
            {
                Columns = (int)(sheet.Width / _format.Width);
                Rows = (int)(sheet.Height / _format.Height);
            }

            return Columns * Rows;  
        }
    }
}

