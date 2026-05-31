using Interfaces;
using Interfaces.FileBrowser;
using Interfaces.Licensing;
using Interfaces.Plugins;
using JobSpace.Static.Pdf.Common;
using JobSpace.UserForms.PDF.Visual;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace JobSpace.Static.Pdf
{
    [PdfTool("Маніпуляції з файлом", "Макет розворотами на скобу", Icon = "saddle_stitch", Order = 5)]
    [RequiresFeature(LicenseFeature.ExportPdf)]
    public sealed class PdfSaddleStitchSpreads : IPdfTool
    {
        private readonly List<IFileSystemInfoExt> _files = new List<IFileSystemInfoExt>();
        private double _bleedMm = 3;

        public bool Configure(PdfJobContext context)
        {
            _files.Clear();

            var pdfFiles = context.InputFiles
                .Where(IsPdfFile)
                .ToList();

            if (pdfFiles.Count == 0)
            {
                MessageBox.Show(
                    "Виберіть хоча б один PDF файл.",
                    "Макет розворотами на скобу",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return false;
            }

            using (var form = new FormPdfSaddleStitchSpreads(pdfFiles.First()))
            {
                if (form.ShowDialog() != DialogResult.OK)
                    return false;

                _bleedMm = form.BleedMm;
            }

            _files.AddRange(pdfFiles);
            return true;
        }

        public void Execute(PdfJobContext context)
        {
            foreach (var file in _files)
            {
                try
                {
                    CreateSaddleStitchSpreads(file.FullName);
                }
                catch (Exception ex)
                {
                    Logger.Log.Error(this, "PdfSaddleStitchSpreads", $"Не вдалося створити макет для \"{file.FullName}\": {ex.Message}");
                    MessageBox.Show(
                        $"Не вдалося створити макет для \"{file.Name}\".\r\n{ex.Message}",
                        "Макет розворотами на скобу",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        public string CreateSaddleStitchSpreads(string filePath)
        {
            string outputPath = GetOutputPath(filePath);
            CreateSaddleStitchSpreads(filePath, outputPath, _bleedMm);
            return outputPath;
        }

        public static void CreateSaddleStitchSpreads(string filePath, string outputPath, double bleedMm)
        {
            using (var p = new PDFlib())
            {
                int indoc = -1;

                try
                {
                    indoc = p.open_pdi_document(filePath, "");
                    if (indoc == -1)
                        throw new InvalidOperationException(p.get_errmsg());

                    int pageCount = (int)p.pcos_get_number(indoc, "length:pages");
                    ValidatePageCount(pageCount);

                    Box firstTrimBox = PdfHelper.GetTrimbox(p, indoc, 0);
                    ValidateTrimBoxes(p, indoc, pageCount, firstTrimBox);

                    if (p.begin_document(outputPath, "optimize=true") == -1)
                        throw new InvalidOperationException(p.get_errmsg());

                    double bleed = Math.Max(0, bleedMm) * PdfHelper.mn;
                    double spreadWidth = bleed + firstTrimBox.width + firstTrimBox.width + bleed;
                    double spreadHeight = bleed + firstTrimBox.height + bleed;
                    double rootX = bleed + firstTrimBox.width;

                    foreach (var spread in GetSpreadOrder(pageCount))
                    {
                        p.begin_page_ext(
                            spreadWidth,
                            spreadHeight,
                            $"trimbox={{{bleed} {bleed} {bleed + firstTrimBox.width * 2} {bleed + firstTrimBox.height}}}");

                        PlaceTrimmedPage(
                            p,
                            indoc,
                            spread.LeftPage,
                            bleed,
                            bleed,
                            0,
                            0,
                            rootX,
                            spreadHeight);

                        PlaceTrimmedPage(
                            p,
                            indoc,
                            spread.RightPage,
                            rootX,
                            bleed,
                            rootX,
                            0,
                            firstTrimBox.width + bleed,
                            spreadHeight);

                        p.end_page_ext("");
                    }

                    p.end_document("");
                }
                catch (PDFlibException ex)
                {
                    PdfHelper.LogException(ex, "PdfSaddleStitchSpreads");
                    throw new InvalidOperationException(ex.get_errmsg(), ex);
                }
                finally
                {
                    if (indoc != -1)
                    {
                        p.close_pdi_document(indoc);
                    }
                }
            }
        }

        public static List<SaddleStitchSpread> GetSpreadOrder(int pageCount)
        {
            ValidatePageCount(pageCount);

            var spreads = new List<SaddleStitchSpread>();
            for (int i = 0; i < pageCount / 2; i++)
            {
                int low = i + 1;
                int high = pageCount - i;

                spreads.Add(i % 2 == 0
                    ? new SaddleStitchSpread(high, low)
                    : new SaddleStitchSpread(low, high));
            }

            return spreads;
        }

        private static void PlaceTrimmedPage(
            PDFlib p,
            int indoc,
            int pageNumber,
            double trimX,
            double trimY,
            double clipX,
            double clipY,
            double clipW,
            double clipH)
        {
            int page = p.open_pdi_page(indoc, pageNumber, "");
            try
            {
                Box trimBox = PdfHelper.GetTrimbox(p, indoc, pageNumber - 1);
                p.save();
                p.rect(clipX, clipY, clipW, clipH);
                p.clip();
                p.fit_pdi_page(page, trimX - trimBox.left, trimY - trimBox.bottom, "");
                p.restore();
            }
            finally
            {
                p.close_pdi_page(page);
            }
        }

        private static void ValidatePageCount(int pageCount)
        {
            if (pageCount <= 0)
                throw new InvalidOperationException("PDF не містить сторінок.");

            if (pageCount % 4 != 0)
                throw new InvalidOperationException("Кількість сторінок має бути кратна 4.");
        }

        private static void ValidateTrimBoxes(PDFlib p, int indoc, int pageCount, Box firstTrimBox)
        {
            var pageFormats = new List<PageFormatGroup>();

            for (int i = 1; i < pageCount; i++)
            {
                Box trimBox = PdfHelper.GetTrimbox(p, indoc, i);
                if (!SameSize(firstTrimBox, trimBox))
                {
                    pageFormats = GetPageFormats(p, indoc, pageCount);
                    break;
                }
            }

            if (pageFormats.Count > 1)
            {
                string message = "Усі сторінки PDF мають мати однаковий розмір TrimBox.\r\n\r\n" +
                                 string.Join("\r\n", pageFormats.Select(x => $"{FormatPageRanges(x.Pages)}: {FormatSize(x.WidthPt, x.HeightPt)}"));

                throw new InvalidOperationException(message);
            }
        }

        private static bool SameSize(Box a, Box b)
        {
            const double tolerance = 0.01;
            return Math.Abs(a.width - b.width) < tolerance &&
                   Math.Abs(a.height - b.height) < tolerance;
        }

        private static List<PageFormatGroup> GetPageFormats(PDFlib p, int indoc, int pageCount)
        {
            var groups = new List<PageFormatGroup>();

            for (int pageNumber = 1; pageNumber <= pageCount; pageNumber++)
            {
                Box trimBox = PdfHelper.GetTrimbox(p, indoc, pageNumber - 1);
                var group = groups.FirstOrDefault(x => SameSize(x.WidthPt, x.HeightPt, trimBox.width, trimBox.height));

                if (group == null)
                {
                    group = new PageFormatGroup(trimBox.width, trimBox.height);
                    groups.Add(group);
                }

                group.Pages.Add(pageNumber);
            }

            return groups;
        }

        private static bool SameSize(double widthA, double heightA, double widthB, double heightB)
        {
            const double tolerance = 0.01;
            return Math.Abs(widthA - widthB) < tolerance &&
                   Math.Abs(heightA - heightB) < tolerance;
        }

        private static string FormatPageRanges(List<int> pages)
        {
            var ranges = new List<string>();
            int start = pages[0];
            int end = pages[0];

            for (int i = 1; i < pages.Count; i++)
            {
                if (pages[i] == end + 1)
                {
                    end = pages[i];
                    continue;
                }

                ranges.Add(FormatRange(start, end));
                start = pages[i];
                end = pages[i];
            }

            ranges.Add(FormatRange(start, end));
            return string.Join(",", ranges);
        }

        private static string FormatRange(int start, int end)
        {
            return start == end ? start.ToString() : $"{start}-{end}";
        }

        private static string FormatSize(double widthPt, double heightPt)
        {
            return $"{widthPt / PdfHelper.mn:0.##}x{heightPt / PdfHelper.mn:0.##}";
        }

        private static bool IsPdfFile(IFileSystemInfoExt file)
        {
            return file?.FileInfo != null &&
                   !file.IsDir &&
                   string.Equals(file.FileInfo.Extension, ".pdf", StringComparison.InvariantCultureIgnoreCase);
        }

        private static string GetOutputPath(string filePath)
        {
            string directory = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string outputPath = Path.Combine(directory, $"{fileName}_saddle_spreads.pdf");

            if (!File.Exists(outputPath))
                return outputPath;

            int index = 1;
            do
            {
                outputPath = Path.Combine(directory, $"{fileName}_saddle_spreads_{index}.pdf");
                index++;
            }
            while (File.Exists(outputPath));

            return outputPath;
        }
    }

    public readonly struct SaddleStitchSpread
    {
        public SaddleStitchSpread(int leftPage, int rightPage)
        {
            LeftPage = leftPage;
            RightPage = rightPage;
        }

        public int LeftPage { get; }
        public int RightPage { get; }

        public override string ToString()
        {
            return $"{LeftPage}-{RightPage}";
        }
    }

    internal sealed class PageFormatGroup
    {
        public PageFormatGroup(double widthPt, double heightPt)
        {
            WidthPt = widthPt;
            HeightPt = heightPt;
        }

        public double WidthPt { get; }
        public double HeightPt { get; }
        public List<int> Pages { get; } = new List<int>();
    }
}
