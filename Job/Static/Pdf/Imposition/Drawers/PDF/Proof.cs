﻿using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition.Drawers.Services.Screen;
using JobSpace.Static.Pdf.Imposition.Models;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition.Drawers.PDF
{
    public static class Proof
    {

        public static void DrawPage(PDFlib p, TemplatePage templatePage,PageSide side, ProofParameters proof)
        {

            if (!proof.Enable) return;
            (double x,double y, double w,double h) = ScreenDrawCommons.GetPageDraw(templatePage, side);

            DrawStrokeRect(p, MarkColor.ProofColor,
                new RectangleD
                {
                    X1 = x,
                    Y1 = y,
                    X2 = x + w,
                    Y2 = y + h
                });


        }
        public static void DrawPageBack(PDFlib p, PrintSheet sheet, TemplatePage templatePage, PageSide side, ProofParameters proof)
        {
            if (!proof.Enable) return;
            (double x, double y, double w, double h) = ScreenDrawCommons.GetPageDrawBack(sheet,templatePage, side);
            DrawStrokeRect(p, MarkColor.ProofColor,
               new RectangleD
               {
                   X1 = x,
                   Y1 = y,
                   X2 = x + w,
                   Y2 = y + h
               });
        }

        static void DrawStrokeRect(PDFlib p, MarkColor color, RectangleD rect)
        {
            p.save();
            int gstate = p.create_gstate("overprintmode=1 overprintfill=true overprintstroke=true");
            p.set_gstate(gstate);
            p.setcolor("fillstroke", "cmyk", color.C / 100, color.M / 100, color.Y / 100, color.K / 100);
            int spot = p.makespotcolor("ProofColor");
            p.setlinewidth(1.0);
            p.setcolor("stroke", "spot", spot, 1.0, 0.0, 0.0);
            p.rect(rect.X1 * PdfHelper.mn, rect.Y1 * PdfHelper.mn, rect.W * PdfHelper.mn, rect.H * PdfHelper.mn);
            p.stroke();

            p.restore();
        }

        public static void DrawSheet(PDFlib p, TemplateSheet sheet, ProofParameters proof)
        {

            if (!proof.Enable) return;

            DrawStrokeRect(p, MarkColor.ProofColor,
               new RectangleD
               {
                   X1 = 0,
                   Y1 = 0,
                   X2 = sheet.W,
                   Y2 = sheet.H
               });
        }

       
    }
}
