using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BackgroundTaskServiceLib;
using ImageMagick;
using Interfaces;
using Interfaces.PdfUtils;
using iTextSharp.text.pdf;
using Job.Models;
using Job.Static.Pdf.Convert;
using Job.Static.Pdf.Divide;
using Job.Static.Pdf.Merge;
using Job.Static.Pdf.MergeOddAndEven;
using Job.Static.Pdf.Scale;
using Job.Static.Pdf.SetTrimBox.ByBleed;
using Job.Static.Pdf.SetTrimBox.ByFormat;
using Job.Static.Pdf.SetTrimBox.BySpread;
using Job.Static.Pdf.SplitOddAndEven;
using Job.Static.Pdf.SplitSpread;
using Job.Static.Pdf.ToJpg;
using Job.UserForms;

namespace Job.Static
{


    public static class FileFormatsUtil
    {
        public const decimal Mn = (decimal)2.83465;


        private static readonly string[] Extension = { ".tif", ".tiff", ".pdf" };

        public static bool IsExistExtension(string ext)
        {
            var lowEx = ext.ToLower(CultureInfo.InvariantCulture);
            return Extension.FirstOrDefault(x => x.Equals(lowEx)) != null;
        }


        public static void GetFormat(IFileSystemInfoExt sfi)
        {
            var ext = sfi.FileInfo.Extension.ToLower(CultureInfo.InvariantCulture);

            switch (ext)
            {
                case ".psd":
                    GetPsd(sfi);
                    break;
                case ".jpg":
                case ".tif":
                case ".tiff":
                    GetTif(sfi);
                    break;
                case ".ai":
                case ".pdf":
                    GetPdf(sfi);
                    break;
            }

        }

        private static void GetPsd(IFileSystemInfoExt sfi)
        {
            try
            {
                MagickImageInfo info = new MagickImageInfo(sfi.FileInfo.FullName);

                sfi.Format = new FileFormat
                {
                    Width = info.Width * 25.4M / (decimal)info.Density.X,
                    Height = info.Height * 25.4M / (decimal)info.Density.Y,
                    Bleeds = (decimal)(info.Density.X + info.Density.Y) / 2
                };
            }
            catch
            {
            }
        }

        private static void GetPdf(IFileSystemInfoExt sfi)
        {
            iTextSharp.text.Rectangle media = null;

            PdfReader pdfReader = null;
            int pages = 0;

            try
            {
                pdfReader = new PdfReader(sfi.FileInfo.FullName);
                pages = pdfReader.NumberOfPages;
                media = pdfReader.GetBoxSize(1, "media");
                var rect = pdfReader.GetBoxSize(1, "trim");
                pdfReader.Dispose();

                if (rect == null)
                {
                    sfi.Format = new FileFormat
                    {
                        Width = (decimal)media.Width / Mn,
                        Height = (decimal)media.Height / Mn,
                        Bleeds = 0,
                        cntPages = pages
                    };
                }
                else
                {
                    sfi.Format = new FileFormat
                    {
                        Width = (decimal)rect.Width / Mn,
                        Height = (decimal)rect.Height / Mn,
                        Bleeds = (decimal)(media.Width - rect.Width) / 2 / Mn,
                        cntPages = pages
                    };
                }
            }
            catch
            {
                if (media != null)
                {
                    sfi.Format = new FileFormat()
                    {
                        Width = (decimal)media.Width / Mn,
                        Height = (decimal)media.Height / Mn,
                        Bleeds = 0,
                        cntPages = pages
                    };
                }
                else
                {
                    sfi.Format = new FileFormat();
                }
            }
            finally
            {
                pdfReader?.Dispose();
            }
        }

        private static void GetTif(IFileSystemInfoExt sfi)
        {
            try
            {
                using (var stream = new FileStream(sfi.FileInfo.FullName, FileMode.Open, FileAccess.Read))
                {
                    using (var tif = System.Drawing.Image.FromStream(stream, false, false))
                    {
                        var width = tif.PhysicalDimension.Width;
                        var height = tif.PhysicalDimension.Height;
                        var hresolution = tif.HorizontalResolution;
                        var vresolution = tif.VerticalResolution;

                        sfi.Format = new FileFormat
                        {
                            Width = (decimal)(width / (hresolution / 25.4F)),
                            Height = (decimal)(height / (vresolution / 25.4F)),
                            Bleeds = (decimal)hresolution
                        };
                    }
                }
            }
            catch
            {
            }
        }

        internal static void SetTrimBox(IEnumerable objects)
        {
            if (objects != null)
            {
                var firstFile = objects.Cast<FileSystemInfoExt>().First();
                using (var formGetTrimBox = new FormGetTrimBox(firstFile))
                {
                    if (formGetTrimBox.ShowDialog() == DialogResult.OK)
                    {
                        var result = formGetTrimBox.Result;
                        BackgroundTaskService.AddTask(new BackgroundTaskItem()
                        {
                            Name = "set trimbox...",
                            BackgroundAction = () =>
                            {
                                if (result.ResultType == TrimBoxResultEnum.byBleed)
                                {
                                    PdfSetTrimBoxByBleedParams param = new PdfSetTrimBoxByBleedParams { Bleed = result.Bleed };

                                    foreach (FileSystemInfoExt ext in objects)
                                    {
                                        new PdfSetTrimBoxByBleed(param).Run(ext.FileInfo.FullName);
                                        
                                    }
                                }
                                else if (result.ResultType == TrimBoxResultEnum.byTrimbox)
                                {
                                    PdfSetTrimBoxByFormatParams param = 
                                    new PdfSetTrimBoxByFormatParams { Width = result.TrimBox.Width,Height = result.TrimBox.Height };

                                    foreach (FileSystemInfoExt ext in objects)
                                    {
                                        new PdfSetTrimBoxByFormat(param).Run(ext.FileInfo.FullName);
                                    }

                                }
                                else if (result.ResultType == TrimBoxResultEnum.bySpread)
                                {

                                    PdfSetTrimBoxBySpreadParams param = new PdfSetTrimBoxBySpreadParams
                                    {
                                        Top = result.Spread.Top,
                                        Bottom = result.Spread.Bottom,
                                        Inside = result.Spread.Inside,
                                        Outside = result.Spread.Outside,
                                    };

                                    foreach (FileSystemInfoExt ext in objects)
                                    {
                                        new PdfSetTrimBoxBySpread(param).Run(ext.FileInfo.FullName);
                                    }
                                }
                            }
                        });
                    }
                }
            }
        }

        //internal static void SetTrimBox(IEnumerable objects, double trimBox)
        //{
        //    if (objects != null)
        //    {
        //        BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("set trimbox by bleed", new Action(() =>
        //        {
        //            foreach (FileSystemInfoExt ext in objects)
        //            {
        //                // TODO: need to refactor
        //                //PDFManipulate.Fasades.Pdf.SetTrimBoxByBleed($"{ext.FileInfo.FullName}.pdf", trimBox);
        //            }
        //        })));
        //    }
        //}

        public static void ConvertToPDF(IEnumerable<IFileSystemInfoExt> list)
        {
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("convert to pdf", new Action(() =>
            {
                foreach (IFileSystemInfoExt ext in list)
                {
                    var convert = new PdfConvert(new PdfConvertParams());
                    convert.Run(ext.FileInfo.FullName);
                }

            })));

            //var converter = PDFManipulate.Fasades.Pdf.ConvertToPDF(list.Select(x => x.FileInfo.FullName));

            //if (converter != null)
            //    BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("convert to pdf", new Action(() => { converter.Start(); })));
        }

        // TODO: need to refactor
        //public static void ConvertToPDF(IEnumerable<IFileSystemInfoExt> list, ConvertModeEnum mode)
        //{
        //    var converter = PDFManipulate.Fasades.Pdf.ConvertToPDF(list.Select(x => x.FileInfo.FullName), mode);
        //    if (converter != null)
        //        converter.Start();
        //}

        public static void SplitPDF(IEnumerable<IFileSystemInfoExt> list, PdfDividerParams param)
        {

            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("split pdf", new Action(
                () =>
                {
                    foreach (var file in list)
                    {
                        new PdfDivider(param).Run(file.FileInfo.FullName);
                    }
                }
                )));

            //var converter = PDFManipulate.Fasades.Pdf.SplitPDF(list.Select(x => x.FileInfo.FullName));
            //if (converter != null)
            //    BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("split pdf", new Action(() => { converter.SplitPdf(); })));
        }

        public static void RepeatPages(IEnumerable<IFileSystemInfoExt> list)
        {
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("repeat pages pdf", new Action(() => {
                // TODO: need to refactor
                //PDFManipulate.Fasades.Pdf.RepeatPages(list.Select(x => x.FileInfo.FullName));
                })));
        }

        public static void MergeFrontsAndBack(IEnumerable<IFileSystemInfoExt> list)
        {
            //FormProgress.ShowProgress(() => { Pdf.MergeFrontsAndBack(list.Select(x => x.FileInfo.FullName)); });
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("merge front and back pdf", new Action(
                () => {
                    // TODO: need to refactor
                    //PDFManipulate.Fasades.Pdf.MergeFrontsAndBack(list.Select(x => x.FileInfo.FullName));
                    }
                )));
        }

        public static void ReversePages(IEnumerable<IFileSystemInfoExt> list)
        {
            //FormProgress.ShowProgress(() => { Pdf.ReversePages(list.Select(x => x.FileInfo.FullName)); });
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("reverce pages pdf", new Action(
                () => {
                    // TODO: need to refactor
                    //PDFManipulate.Fasades.Pdf.ReversePages(list.Select(x => x.FileInfo.FullName)); 
                    }
                )));
        }

        public static void RepeatDocument(IEnumerable<IFileSystemInfoExt> toList)
        {
            //FormProgress.ShowProgress(() => { Pdf.RepeatDocument(toList.Select(x => x.FileInfo.FullName)); });
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("repeat document pdf", new Action(
                () => {
                    // TODO: need to refactor
                    //PDFManipulate.Fasades.Pdf.RepeatDocument(toList.Select(x => x.FileInfo.FullName)); 
                    }
                )));
        }

        public static void CreateRectangle(IEnumerable<IFileSystemInfoExt> toList)
        {
            //FormProgress.ShowProgress(() => { Pdf.CreateRectangle(toList.Select(x => x.FileInfo.FullName)); });
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("create rectangle pdf", new Action(
                () => {
                    // TODO: need to refactor
                    //PDFManipulate.Fasades.Pdf.CreateRectangle(toList.Select(x => x.FileInfo.FullName)); 
                    }
                )));
        }

        internal static void CreateEllipse(IEnumerable<IFileSystemInfoExt> toList)
        {
            //FormProgress.ShowProgress(() => { Pdf.CreateElipse(toList.Select(x => x.FileInfo.FullName)); });
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("create ellipse pdf", new Action(
                () => {
                    // TODO: need to refactor
                    //PDFManipulate.Fasades.Pdf.CreateElipse(toList.Select(x => x.FileInfo.FullName));
                    }
                )));
        }

        public static void ExtractPages(IEnumerable<IFileSystemInfoExt> toList)
        {
            // TODO: need to refactor
            //var converter = PDFManipulate.Fasades.Pdf.ExtractPages(toList.Select(x => x.FileInfo.FullName));
            //if (converter != null)
            //{
            //    //FormProgress.ShowProgress(() => { converter.ExtractPages(); });
            //    BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("extract pages pdf", new Action(
            //    () => { converter.ExtractPages(); }
            //    )));
            //}
        }

        public static void SplitCoverAndBlock(IEnumerable<IFileSystemInfoExt> toList)
        {
            //FormProgress.ShowProgress(() => { Pdf.SplitCoverAndBlock(toList.Select(x => x.FileInfo.FullName)); });
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("split cover and block pdf", new Action(
               () => {

                   // TODO: need to refactor
                   //PDFManipulate.Fasades.Pdf.SplitCoverAndBlock(toList.Select(x => x.FileInfo.FullName));
                   }
                )));
        }

        public static void CreateEmptiesWithCount(string pathTo)
        {
            using (var form = new FormCreateEmptiesWithCount())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // TODO: need to refactor

               //     var list = form.PdfTemplates;
               //     if (list.Count > 0)
               //     {
               //         BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("create templates with count pdf", new Action(
               //() => {
               //    // TODO: need to refactor
               //    //PDFManipulate.Fasades.Pdf.CreateEmptyPdfTemplateWithCount(pathTo, list); 
               //    }
               // )));

               //     }
                }
            }
        }

        public static void RotatePagesMirror(IEnumerable<IFileSystemInfoExt> list)
        {
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("rotate mirror pages pdf", new Action(
               () => {
                   // TODO: need to refactor
                   //PDFManipulate.Fasades.Pdf.RotateMirrorFrontAndBack(list.Select(x => x.FileInfo.FullName));
                   }
                )));
        }

        public static void MergeOddAndEven(PdfMergeOddAndEvenParams param)
        {
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("merge odd and even pages pdf", new Action(
                () =>
                {
                    new PdfMergeOddAndEven(param).Run();
                }
                
                )));
        }

        public static void SplitOddAndEven(IEnumerable<IFileSystemInfoExt> list)
        {
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("merge odd and even pages pdf", new Action(
                () =>
                {
                    foreach (IFileSystemInfoExt file in list)
                    {
                        new PdfSplitOddAndEven().Run(file.FileInfo.FullName);
                    }
                }
                )));
        }

        public static void PdfToJpg(IEnumerable<IFileSystemInfoExt> list, PdfToJpgParams param)
        {
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("create jpg from pdf", new Action(
                () =>
                {
                    foreach (var file in list)
                    {
                        new PdfToJpg(param).Run(file.FileInfo.FullName);
                    }
                }
                )));
        }

        public static void ScalePdf(IEnumerable<IFileSystemInfoExt> list, PdfScaleParams param)
        {
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("scale pdf", new Action(
                () =>
                {
                    foreach (var file in list)
                    {
                        new PdfScaler(param).Run(file.FileInfo.FullName);
                    }
                }
                )));
        }

        public static void SplitPdf(IEnumerable<IFileSystemInfoExt> list, PdfSplitterParams param)
        {
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("split pdf", new Action(
                () =>
                {
                    foreach (var file in list)
                    {
                        new PdfSpliter(param).Run(file.FileInfo.FullName);
                    }
                }
                )));
        }

        internal static void MergePdf(string[] convertFiles)
        {
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("split pdf", new Action(
                () =>
                {
                    new PdfMerger(convertFiles).Run();
                }
                )));
        }
    }
}
