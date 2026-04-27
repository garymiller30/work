using ImageMagick;
using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.Static.Pdf.Common;
using PDFlib_dotnet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ghostscript.NET.Processor;

namespace JobSpace.Static.Pdf.Convert
{
    [PdfTool("", "Конвертувати в PDF", Order = 0, Icon = "convert", Description = "Конвертувати в PDF файли з розширенням jpg, png, jpeg, tif, tiff, svg, psd, ai", SeparatorAfter = true, IsBackgroundTask = true)]
    public sealed class PdfConvert : IPdfTool
    {
        PdfConvertParams _params = new PdfConvertParams();

        public bool Configure(PdfJobContext context)
        {
            return true;
        }

        public void Configure(PdfConvertParams param)
        {
            _params = param;
        }

        public void Execute(PdfJobContext context)
        {
            foreach (var file in context.InputFiles)
            {
                Convert(file.FullName);
            }
        }



        public void Convert(string filePath)
        {
            string ext = Path.GetExtension(filePath).ToLower(System.Globalization.CultureInfo.InvariantCulture);

            switch (ext)
            {
                case ".jpg":
                case ".png":
                case ".jpeg":
                case ".tif":
                case ".tiff":
                    ConvertImage(filePath);
                    break;
                case ".svg":
                    ConvertSvg(filePath);
                    break;
                case ".psd":
                    ConvertPsd(filePath);
                    break;
                case ".pdf":
                    ConvertPdf(filePath);
                    break;
                case ".ai":
                    ConvertAi(filePath);
                    break;
                case ".eps":
                    ConvertEps(filePath);
                    break;
                default:
                    break;
            }
        }

        private void ConvertEps(string filePath)
        {
            string targetfile = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath) + ".pdf");

            using (GhostscriptProcessor processor = new GhostscriptProcessor())
            {
                List<string> switches = new List<string>();
                switches.Add("-empty");
                switches.Add("-dQUIET");
                switches.Add("-dSAFER");
                switches.Add("-dBATCH");
                switches.Add("-dNOPAUSE");
                switches.Add("-dNOPROMPT");
                switches.Add("-sDEVICE=pdfwrite");
                switches.Add("-dEPSCrop"); // Важливо для збереження розмірів EPS
                switches.Add($"-sOutputFile={targetfile}");
                switches.Add("-f");
                switches.Add(filePath);

                processor.Process(switches.ToArray());
            }
        }

        public void ConvertEpsToPdf(string epsPath, string pdfPath, string gsPath)
        {
            // Аргументи для Ghostscript для конвертації EPS у PDF
            string args = $"-q -dNOPAUSE -dBATCH -sDEVICE=pdfwrite -dEPSCrop -sOutputFile=\"{pdfPath}\" \"{epsPath}\"";

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = gsPath, // Шлях до gswin64c.exe
                Arguments = args,
                CreateNoWindow = true,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            using (Process process = Process.Start(startInfo))
            {
                process.WaitForExit();
            }
        }


        private void ConvertSvg(string filePath)
        {
            using (PDFlib p = new PDFlib())
            {
                string targetFile = CreateTargetFileName(filePath);
                try
                {
                    p.begin_document(targetFile, "optimize=true");
                    p.begin_page_ext(0, 0, "");
                    int graphics = p.load_graphics("auto", filePath, "");
                    p.fit_graphics(graphics, 0, 0, "adjustpage");
                    p.close_graphics(graphics);
                    p.end_page_ext("");
                    p.end_document("");
                }
                catch (PDFlibException e)
                {
                    PdfHelper.LogException(e, "CreatePdf");
                }
            }

        }

        private void ConvertAi(string filePath)
        {
            string targetFile = CreateTargetFileName(filePath);
            try
            {
                File.Copy(filePath, targetFile);
            }
            catch (Exception e)
            {
                Logger.Log.Error(null, "CreatePdf", $"[{e.Message}]");
            }

        }

        void ConvertImage(string filePath, string tmpFile = null)
        {
            using (PDFlib p = new PDFlib())
            {
                string targetFile = CreateTargetFileName(filePath);

                string sourceFile = tmpFile ?? filePath;

                try
                {
                    p.begin_document(targetFile, "optimize=true");

                    p.begin_page_ext(0, 0, "");

                    int image = p.load_image("auto", sourceFile, "honoriccprofile=false ignoremask=true");
                    p.fit_image(image, 0, 0, "adjustpage");

                    double width = p.info_image(image, "width", "");
                    double height = p.info_image(image, "height", "");

                    p.close_image(image);

                    if (_params.TrimBox.IsEmpty())
                    {
                        p.end_page_ext($"trimbox {{{_params.TrimBox.left} {_params.TrimBox.bottom} {width - _params.TrimBox.right} {height - _params.TrimBox.top}}}");
                    }
                    else
                    {
                        double l = (width - _params.TrimBox.width) / 2;
                        double b = (height - _params.TrimBox.height) / 2;
                        p.end_page_ext($"trimbox {{{l} {b} {width - l} {height - b}}}");
                    }
                    p.end_document("");

                }
                catch (PDFlibException e)
                {
                    PdfHelper.LogException(e, "CreatePdf");
                }
            }
        }

        void ConvertPsd(string filePath)
        {
            string tempFile = Path.GetTempFileName() + ".jpg";

            var magickSettings = new MagickReadSettings
            {
                Compression = CompressionMethod.LosslessJPEG,
            };

            using (var reader = new MagickImage(filePath, magickSettings))
            {
                reader.Write(tempFile);
            }

            ConvertImage(filePath, tempFile);

            File.Delete(tempFile);

        }
        void ConvertPdf(string filePath)
        {

        }
        string CreateTargetFileName(string filePath)
        {
            string dir = Path.GetDirectoryName(filePath);
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(filePath);

            string target = Path.Combine(dir, fileNameWithoutExt + ".pdf");
            var cnt = 1;
            while (File.Exists(target))
            {
                target = Path.Combine(dir, $"{fileNameWithoutExt}_{cnt++}.pdf");
            }
            return target;
        }


    }
}
