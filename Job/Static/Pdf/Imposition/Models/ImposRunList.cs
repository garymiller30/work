using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Models
{
    public class ImposRunList
    {
        public List<ImposRunPage> RunPages { get; set; } = new List<ImposRunPage>();
        public void AddPage(ImposRunPage page)
        {
            RunPages.Add(page);

        }

        public void AddFile(PdfFile file)
        {
            int idx = 0;
            foreach (var page in file.Pages)
            {
                RunPages.Add(new ImposRunPage(file, idx++));
            }
        }
    }
}
