﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Models
{
    public class ImposRunPage
    {
        public int FileId { get; set; }
        public int PageIdx { get; set; }
        public bool IsAssumed { get;set; }
        public bool IsValidFormat { get; set; }
        public ImposRunPage()
        {

        }

        public ImposRunPage(PdfFile pdfFile, int pageIdx)
        {
            FileId = pdfFile.Id;
            PageIdx = pageIdx;
        }

        public ImposRunPage(int fileId, int pageIdx)
        {
            FileId = fileId;
            PageIdx = pageIdx;
        }
    }
}
