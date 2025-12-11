using JobSpace.Static.Pdf.Imposition.Drawers.PDF.Models;
using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Services
{
    public static class PdfFileService
    {
        public static PdfFilePage GetPage(List<PdfFile> files,ImposRunPage page)
        {
            var file = files.FirstOrDefault(f => f.Id == page.FileId);
            if (file == null)
                return null;
            return file.Pages[page.PageIdx-1];
        }

      
    }
}
