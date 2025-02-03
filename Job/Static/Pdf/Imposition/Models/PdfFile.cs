using JobSpace.Static.Pdf.Common;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.Static.Pdf.Imposition.Models
{
    public class PdfFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        /// <summary>
        /// тираж
        /// </summary>
        public int Count { get;set; }
        public string ShortName { get; set; }
        public bool IsMediaboxCentered { get; set; } = false;

        public PdfFilePage[] Pages { get; private set; }
        public PdfFile(string fileName)
        {
            FileName = fileName;
            GetCount();
            GetPagesInfo();

        }

        private void GetCount()
        {
            var reg = new Regex(@"#(\d+)\.");
            var match = reg.Match(FileName);
            
            if (match.Success)
            {
                try
                {
                    Count = int.Parse(match.Groups[1].Value);
                }
                catch (Exception)
                {
                }
            }
        }

        private void GetPagesInfo()
        {
            ShortName = Path.GetFileName(FileName);

            PDFlib p = new PDFlib();

            try
            {

                Console.WriteLine($"File : {Path.GetFileName(FileName)}");

                p.begin_document("", "");

                int doc = p.open_pdi_document(FileName, "");

                var pageCnt = (int)p.pcos_get_number(doc, "length:pages");

                Pages = new PdfFilePage[pageCnt];

                for (int i = 1; i <= pageCnt; i++)
                {

                    Console.WriteLine($"Page {i}:");

                    int page = p.open_pdi_page(doc, i, "");

                    PdfFilePage filePage = new PdfFilePage();
                    filePage.Idx = i;

                    var trims = new double[] { 0, 0, 0, 0 };
                    var media = new double[] { 0, 0, 0, 0 };

                    // get media box
                    for (int j = 0; j < 4; j++)
                    {
                        media[j] = p.pcos_get_number(doc, $"pages[{page}]/MediaBox[{j}]");
                    }
                    string trimtype = p.pcos_get_string(doc, $"type:pages[{page}]/TrimBox");

                    if (trimtype == "array")
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            trims[j] = p.pcos_get_number(doc, $"pages[{page}]/TrimBox[{j}]");
                        }
                    }
                    else
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            trims[j] = media[j];
                        }
                    }

                    filePage.Media.X1 = Math.Round(media[0] / PdfHelper.mn, 2);
                    filePage.Media.Y1 = Math.Round(media[1] / PdfHelper.mn, 2);
                    filePage.Media.X2 = Math.Round(media[2] / PdfHelper.mn, 2);
                    filePage.Media.Y2 = Math.Round(media[3] / PdfHelper.mn, 2);

                    filePage.Trim.X1 = Math.Round(trims[0] / PdfHelper.mn, 2);
                    filePage.Trim.Y1 = Math.Round(trims[1] / PdfHelper.mn, 2);
                    filePage.Trim.X2 = Math.Round(trims[2] / PdfHelper.mn, 2);
                    filePage.Trim.Y2 = Math.Round(trims[3] / PdfHelper.mn, 2);
                    //filePage.Angle = p.pcos_get_number(doc, $"length:pages[{i - 1}]/Rotate");

                    Pages[i - 1] = filePage;

                    Console.WriteLine($"MediaBox: X1={filePage.Media.X1:N2}, Y1={filePage.Media.Y1:N2}, X2={filePage.Media.X2:N2}, Y2={filePage.Media.Y2:N2}, W={filePage.Media.W:N2}, H={filePage.Media.H:N2}");
                    Console.WriteLine($"TrimBox : X1={filePage.Trim.X1:N2}, Y1={filePage.Trim.Y1:N2}, X2={filePage.Trim.X2:N2}, Y2={filePage.Trim.Y2:N2}, W={filePage.Trim.W:N2}, H={filePage.Trim.H:N2}");
                    // Console.WriteLine($"Angle: {filePage.Angle}");
                    Console.WriteLine();


                    p.close_pdi_page(page);
                }
                p.close_pdi_document(doc);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                p?.Dispose();
            }
        }

        public static int GetParentId(List<PdfFile> files, PdfFilePage page)
        {
            foreach (PdfFile file in files)
            {
                if (file.Pages.Contains(page))
                {
                    return file.Id;

                }
            }

            return -1;
        }
    }
}
