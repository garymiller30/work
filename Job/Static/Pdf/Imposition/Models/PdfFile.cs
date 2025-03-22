using JobSpace.Static.Pdf.Common;
using Microsoft.Web.WebView2.Core;
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
                    var crops = new double[] {0, 0, 0, 0 };

                    int pageIdx = i -1;

                    // get media box
                    for (int j = 0; j < 4; j++)
                    {
                        media[j] = p.pcos_get_number(doc, $"pages[{pageIdx}]/MediaBox[{j}]");
                    }

                    // get trim box
                    string trimtype = p.pcos_get_string(doc, $"type:pages[{pageIdx}]/TrimBox");

                    if (trimtype == "array")
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            trims[j] = p.pcos_get_number(doc, $"pages[{pageIdx}]/TrimBox[{j}]");
                        }
                    }
                    else
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            trims[j] = media[j];
                        }
                    }
                    // get cropbox
                    string croptype = p.pcos_get_string(doc, $"type:pages[{pageIdx}]/CropBox");
                    if (croptype == "array")
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            crops[j] = p.pcos_get_number(doc, $"pages[{pageIdx}]/CropBox[{j}]");
                        }
                    }
                    else
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            crops[j] = media[j];
                        }
                    }

                    filePage.Media.X1 = Math.Round(media[0] / PdfHelper.mn, 1);
                    filePage.Media.Y1 = Math.Round(media[1] / PdfHelper.mn, 1);
                    filePage.Media.X2 = Math.Round(media[2] / PdfHelper.mn, 1);
                    filePage.Media.Y2 = Math.Round(media[3] / PdfHelper.mn, 1);

                    filePage.Trim.X1 = Math.Round(trims[0] / PdfHelper.mn, 1);
                    filePage.Trim.Y1 = Math.Round(trims[1] / PdfHelper.mn, 1);
                    filePage.Trim.X2 = Math.Round(trims[2] / PdfHelper.mn, 1);
                    filePage.Trim.Y2 = Math.Round(trims[3] / PdfHelper.mn, 1);

                    filePage.Crop.X1 = Math.Round(crops[0] / PdfHelper.mn, 1);
                    filePage.Crop.Y1 = Math.Round(crops[1] / PdfHelper.mn, 1);
                    filePage.Crop.X2 = Math.Round(crops[2] / PdfHelper.mn, 1);
                    filePage.Crop.Y2 = Math.Round(crops[3] / PdfHelper.mn, 1);

                    Pages[i - 1] = filePage;

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
