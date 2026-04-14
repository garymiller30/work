using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models
{
    public class ImposRunList
    {
        public List<ImposRunPage> RunPages { get; set; } = new List<ImposRunPage>();
        public void AddPage(ImposRunPage page)
        {
            RunPages.Add(page);
        }

        public void AddPages(IEnumerable<ImposRunPage> pages)
        {
            RunPages.AddRange(pages);
        }

        public List<ImposRunPage> AddFile(PdfFile file)
        {
            var pages = CreatePagesFromFile(file);
            AddPages(pages);
            return pages;
        }

        public static List<ImposRunPage> CreatePagesFromFile(PdfFile file)
        {
            List<ImposRunPage> pages = new List<ImposRunPage>();

            int idx = 1;
            foreach (var page in file.Pages)
            {
                pages.Add(new ImposRunPage(file, idx++));
            }
            
            return pages;
        }
    }
}
