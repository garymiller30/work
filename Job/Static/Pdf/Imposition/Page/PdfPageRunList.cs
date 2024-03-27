using Job.Static.Pdf.Imposition.Product;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Page
{
    public class PdfPageRunList
    {
        PdfProductPart _part;

        public List<PdfPageRun> Pages { get;set;} = new List<PdfPageRun>();


        public PdfPageRunList(PdfProductPart part)
        {
            _part = part;
        }


        public void SetPagesCount(int pagesCount)
        {
            Pages.Clear();

            for (int i = 1; i <= pagesCount; i++)
            {
                Pages.Add(new PdfPageRun() { Idx = i});
            }

            
        }

        public void AssingPage(int runPageIdx, int contentPageIdx)
        {
            Pages[runPageIdx-1].ContentPageIdx = contentPageIdx;
        }
    }
}
