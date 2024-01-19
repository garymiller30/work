using Job.Static.Pdf.Imposition.Common;
using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Scheme
{
    public class PdfScheme
    {
       
        public int Rows { get; set; }
        public int Columns { get;set; }

        public PdfSchemePage[,] SchemePages { get;set; }

        public void CreateSchemePages(int rows,int columns)
        {
            Rows = rows;
            Columns = columns;

            SchemePages = new PdfSchemePage[rows, columns];

            for (int i = 0; i < SchemePages.GetLength(0); i++)
            {
                for (int j = 0; j < SchemePages.GetLength(1); j++)
                {
                    SchemePages[i,j] = new PdfSchemePage();
                }
            }
        }
    }
}
