﻿using Job.Static.Pdf.Imposition.Models.Marks;
using Job.Static.Pdf.Imposition.Models;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static.Pdf.Imposition.Drawers.PDF.Marks.Pdf
{
    public static partial class DrawPdfMarks
    {
        public static void Back(PDFlib p, MarksContainer marksContainer)
        {
            foreach (var mark in marksContainer.Pdf.Where(x => x.Parameters.IsBack))
            {
                using (PDFLIBDocument doc = new PDFLIBDocument(p, mark.File.FileName))
                {
                    int scaleX = 1;
                    int scaleY = 1;

                    if (mark.Parameters.IsBackMirrored)
                    {
                        if (mark.Angle == 90 || mark.Angle == 270)
                        {
                            scaleY = -1;
                        }
                        else
                        {
                            scaleX = -1;
                        }
                    }
                    doc.fit_pdi_page(1, mark.Back.X, mark.Back.Y, $"orientate={Commons.Orientate[mark.Angle]} scale={{{scaleX} {scaleY}}}");
                }
            }
        }


    }
}