using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.Static.Pdf.Common;
using JobSpace.UserForms;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace JobSpace.Static.Pdf.Create
{
    [PdfTool("Створити","порожній PDF шаблон з кількістю",Icon = "create_empty_pdf_template",Order =4)]
    public sealed class PdfCreateEmptyPdfTemplateWithCount : IPdfTool
    {
        List<EmptyTemplate> _templates;


        public bool Configure(PdfJobContext context)
        {
            using (var form = new FormCreateEmptiesWithCount())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _templates = form.PdfTemplates;
                    return true;
                }
            }
            return false;
        }

        public void Execute(PdfJobContext context)
        {
            foreach (var template in _templates)
            {
                CreateEmptyPdfTemplateWithCount(context.FileManager.Settings.CurFolder, template);
            }
        }

        public void CreateEmptyPdfTemplateWithCount(string pathTo,EmptyTemplate template)
        {
            using ( PDFlib p = new PDFlib())
            {
                try
                {
                    var filename = $"{template.Width}x{template.Height}";

                    for (int i = 1; i <= template.Multiplier; i++)
                    {
                        int fileCount = i;

                        string outfile;
                        // на початку імені файлу буде порядковий номер, який буде виглядати так: 001, 002, 003 і так далі, щоб уникнути перезапису файлів при повторному запуску програми

                        string fileIdx = template.Idx.ToString("D3"); // Форматування числа з провідними нулями (D3 - 3 цифри) 
                        do
                        {
                            outfile = Path.Combine(pathTo, $"{fileIdx}_{filename}_{fileCount}#{template.Count}.pdf");
                            fileCount++;
                        } while (File.Exists(outfile));

                        if (p.begin_document(outfile, "optimize=true") == -1)
                            throw new Exception("Error: " + p.get_errmsg());

                        Box trimbox = new Box();
                        trimbox.CreateCustomBox(template.Width, template.Height, 3);

                        var (width, height) = trimbox.GetMediaBox();

                        p.begin_page_ext(width, height, "");

                        int gstate = p.create_gstate("overprintmode=1 overprintfill=true overprintstroke=true");

                        p.set_gstate(gstate);
                        p.setcolor("fillstroke", "cmyk", 0.79, 0, 0.44, 0.21);

                        int spot = p.makespotcolor("ProofColor");

                        p.setlinewidth(1.0);

                        /* Red rectangle */
                        p.setcolor("stroke", "spot", spot, 1.0, 0.0, 0.0);
                        p.rect(trimbox.left, trimbox.bottom, trimbox.width, trimbox.height);
                        p.stroke();

                        DrawTemplateInfo(p, template, width, height);

                        p.end_page_ext($"trimbox {{{trimbox.left} {trimbox.bottom} {trimbox.left + trimbox.width} {trimbox.height + trimbox.bottom}}}");
                        p.end_document("");
                    }

                }
                catch (PDFlibException e)
                {
                    PdfHelper.LogException(e, "PdfCreateEmptyPdfTemplateWithCount");
                }
            }
        }

        private static void DrawTemplateInfo(PDFlib p, EmptyTemplate template, double pageWidth, double pageHeight)
        {
            const double fontSize = 24;
            const double leading = 32;

            int font = p.load_font("Arial", "auto", "");
            p.setfont(font, fontSize);
            p.setcolor("fill", "cmyk", 0, 0, 0, 1);

            string formatText = $"\u0424\u043e\u0440\u043c\u0430\u0442: {template.Width:0.##} x {template.Height:0.##}";
            string countText = $"\u0422\u0438\u0440\u0430\u0436: {template.Count}";

            double centerX = pageWidth / 2;
            double centerY = pageHeight / 2;

            DrawCenteredTextLine(p, font, fontSize, formatText, centerX, centerY + leading / 2);
            DrawCenteredTextLine(p, font, fontSize, countText, centerX, centerY - leading / 2);
        }

        private static void DrawCenteredTextLine(PDFlib p, int font, double fontSize, string text, double centerX, double baselineY)
        {
            double textWidth = p.stringwidth(text, font, fontSize);
            p.fit_textline(text, centerX - textWidth / 2, baselineY, "");
        }
    }
}
