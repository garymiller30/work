﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Models
{
    public class PdfFilePage
    {
        public PdfBox Media { get; set; } = new PdfBox();
        public PdfBox Trim { get; set; } = new PdfBox();
        // public double Angle { get; internal set; }
    }
}