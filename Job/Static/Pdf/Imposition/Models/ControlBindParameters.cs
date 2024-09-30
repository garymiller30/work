using BrightIdeasSoftware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models
{
    public class ControlBindParameters
    {
        public TreeListView PdfFileList { get;set; }
        public List<PdfFile> PdfFiles { get;set; }

        public TemplatePage MasterPage { get;set; } = new TemplatePage();

    }
}
