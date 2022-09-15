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
using Job.UserForms;
using PDFManipulate.Converters;
using PDFManipulate.Fasades;
//using ShowProgress;

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
            //string ret;
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



                //ret = $"Trim: {rect.Width / Mn:N1}x{rect.Height / Mn:N1}+{(media.Width-rect.Width)/2/Mn:N0}, {pages} pp.";


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
                    //sfi.Width = 0;
                    //sfi.Height = 0;
                    //sfi.Bleeds = 0;
                    //sfi.Pages = 0;
                }

                //ret = media != null ? $"Media: {media.Width / Mn:N1}x{media.Height / Mn:N1}, {pages} pp." : e.Message;
            }
            finally
            {
                pdfReader?.Dispose();
            }

            //return ret;

        }

        private static void GetTif(IFileSystemInfoExt sfi)
        {
            try
            {

                //string str;

                using (var stream = new FileStream(sfi.FileInfo.FullName, FileMode.Open, FileAccess.Read))
                {
                    using (var tif = Image.FromStream(stream, false, false))
                    {
                        var width = tif.PhysicalDimension.Width;
                        var height = tif.PhysicalDimension.Height;
                        var hresolution = tif.HorizontalResolution;
                        var vresolution = tif.VerticalResolution;

                        //var resolution = hresolution == vresolution
                        //    ? $"{hresolution} dpi"
                        //    : $"{hresolution}x{vresolution} dpi";

                        //str =                            $"{width / (hresolution / 25.4F):N2} x {height / (vresolution / 25.4F):N2} mm, {resolution}";

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
                                    foreach (FileSystemInfoExt ext in objects)
                                    {
                                        Pdf.SetTrimBoxByBleed(ext.FileInfo.FullName, result.Bleed);
                                    }
                                }
                                else if (result.ResultType == TrimBoxResultEnum.byTrimbox)
                                {
                                    foreach (FileSystemInfoExt ext in objects)
                                    {


                                        Pdf.SetTrimBox(
                                            ext.FileInfo.FullName,
                                            width: result.TrimBox.Width,
                                            height: result.TrimBox.Height);
                                    }
                                }
                                else if (result.ResultType == TrimBoxResultEnum.bySpread)
                                {
                                    foreach (FileSystemInfoExt ext in objects)
                                    {


                                        Pdf.SetTrimBoxBySpread(
                                            file: ext.FileInfo.FullName,
                                            inside: result.Spread.Inside,
                                            outside: result.Spread.Outside,
                                            top: result.Spread.Top,
                                            bottom: result.Spread.Bottom);
                                    }
                                }
                            }
                        });

                        //FormProgress.ShowProgress(() =>
                        //{

                        //    if (result.ResultType == TrimBoxResultEnum.byBleed)
                        //    {
                        //        foreach (FileSystemInfoExt ext in objects)
                        //        {
                        //            Pdf.SetTrimBoxByBleed(ext.FileInfo.FullName,result.Bleed);
                        //        }
                        //    }
                        //    else if (result.ResultType == TrimBoxResultEnum.byTrimbox)
                        //    {
                        //        foreach (FileSystemInfoExt ext in objects)
                        //        {


                        //            Pdf.SetTrimBox(
                        //                ext.FileInfo.FullName,
                        //                width: result.TrimBox.Width,
                        //                height: result.TrimBox.Height);
                        //        }
                        //    }
                        //    else if (result.ResultType == TrimBoxResultEnum.bySpread)
                        //    {
                        //        foreach (FileSystemInfoExt ext in objects)
                        //        {


                        //            Pdf.SetTrimBoxBySpread(
                        //                file: ext.FileInfo.FullName,
                        //                inside: result.Spread.Inside,
                        //                outside: result.Spread.Outside,
                        //                top: result.Spread.Top,
                        //                bottom: result.Spread.Bottom);
                        //        }
                        //    }
                        //});
                    }
                }
            }
        }

        internal static void SetTrimBox(IEnumerable objects, double trimBox)
        {
            if (objects != null)
            {
                BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("set trimbox by bleed", new Action(() =>
                {
                    foreach (FileSystemInfoExt ext in objects)
                    {
                        Pdf.SetTrimBoxByBleed($"{ext.FileInfo.FullName}.pdf", trimBox);
                    }
                })));
            }

            //FormProgress.ShowProgress(() =>
            //{
            //    foreach (FileSystemInfoExt ext in objects)
            //    {
            //        Pdf.SetTrimBoxByBleed($"{ext.FileInfo.FullName}.pdf", trimBox);
            //    }
            //});
            //}
        }

        public static void ConvertToPDF(IEnumerable<IFileSystemInfoExt> list)
        {
            var converter = Pdf.ConvertToPDF(list.Select(x => x.FileInfo.FullName));

            if (converter != null)
                //FormProgress.ShowProgress(() => { converter.Start(); });
                BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("convert to pdf", new Action(() => { converter.Start(); })));
        }

        public static void ConvertToPDF(IEnumerable<IFileSystemInfoExt> list, ConvertModeEnum mode)
        {
            var converter = Pdf.ConvertToPDF(list.Select(x => x.FileInfo.FullName), mode);
            if (converter != null)
                converter.Start();
            //FormProgress.ShowProgress();

        }

        public static void SplitPDF(IEnumerable<IFileSystemInfoExt> list)
        {
            var converter = Pdf.SplitPDF(list.Select(x => x.FileInfo.FullName));
            if (converter != null)
                BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("split pdf", new Action(() => { converter.SplitPdf(); })));
            //FormProgress.ShowProgress(() => { converter.SplitPdf(); });
        }

        public static void RepeatPages(IEnumerable<IFileSystemInfoExt> list)
        {
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("repeat pages pdf", new Action(() => { Pdf.RepeatPages(list.Select(x => x.FileInfo.FullName)); })));
            //FormProgress.ShowProgress(() => { Pdf.RepeatPages(list.Select(x => x.FileInfo.FullName)); });
        }

        public static void MergeFrontsAndBack(IEnumerable<IFileSystemInfoExt> list)
        {
            //FormProgress.ShowProgress(() => { Pdf.MergeFrontsAndBack(list.Select(x => x.FileInfo.FullName)); });
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("merge front and back pdf", new Action(
                () => { Pdf.MergeFrontsAndBack(list.Select(x => x.FileInfo.FullName)); }
                )));
        }

        public static void ReversePages(IEnumerable<IFileSystemInfoExt> list)
        {
            //FormProgress.ShowProgress(() => { Pdf.ReversePages(list.Select(x => x.FileInfo.FullName)); });
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("reverce pages pdf", new Action(
                () => { Pdf.ReversePages(list.Select(x => x.FileInfo.FullName)); }
                )));
        }

        public static void RepeatDocument(IEnumerable<IFileSystemInfoExt> toList)
        {
            //FormProgress.ShowProgress(() => { Pdf.RepeatDocument(toList.Select(x => x.FileInfo.FullName)); });
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("repeat document pdf", new Action(
                () => { Pdf.RepeatDocument(toList.Select(x => x.FileInfo.FullName)); }
                )));
        }

        public static void CreateRectangle(IEnumerable<IFileSystemInfoExt> toList)
        {
            //FormProgress.ShowProgress(() => { Pdf.CreateRectangle(toList.Select(x => x.FileInfo.FullName)); });
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("create rectangle pdf", new Action(
                () => { Pdf.CreateRectangle(toList.Select(x => x.FileInfo.FullName)); }
                )));
        }

        internal static void CreateEllipse(IEnumerable<IFileSystemInfoExt> toList)
        {
            //FormProgress.ShowProgress(() => { Pdf.CreateElipse(toList.Select(x => x.FileInfo.FullName)); });
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("create ellipse pdf", new Action(
                () => { Pdf.CreateElipse(toList.Select(x => x.FileInfo.FullName)); }
                )));
        }

        public static void ExtractPages(IEnumerable<IFileSystemInfoExt> toList)
        {

            var converter = Pdf.ExtractPages(toList.Select(x => x.FileInfo.FullName));
            if (converter != null)
            {
                //FormProgress.ShowProgress(() => { converter.ExtractPages(); });
                BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("extract pages pdf", new Action(
                () => { converter.ExtractPages(); }
                )));
            }
        }

        public static void SplitCoverAndBlock(IEnumerable<IFileSystemInfoExt> toList)
        {
            //FormProgress.ShowProgress(() => { Pdf.SplitCoverAndBlock(toList.Select(x => x.FileInfo.FullName)); });
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("split cover and block pdf", new Action(
               () => { Pdf.SplitCoverAndBlock(toList.Select(x => x.FileInfo.FullName)); }
                )));
        }

        public static void CreateEmptiesWithCount(string pathTo)
        {
            using (var form = new FormCreateEmptiesWithCount())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    var list = form.PdfTemplates;
                    if (list.Count > 0)
                    {
                        BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("create templates with count pdf", new Action(
               () => { Pdf.CreateEmptyPdfTemplateWithCount(pathTo, list); }
                )));

                    }
                }
            }
        }

        public static void RotatePagesMirror(IEnumerable<IFileSystemInfoExt> list)
        {
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("rotate mirror pages pdf", new Action(
               () => { Pdf.RotateMirrorFrontAndBack(list.Select(x => x.FileInfo.FullName)); }
                )));
        }

        public static void MergeOddAndEven(IEnumerable<IFileSystemInfoExt> list)
        {
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("merge odd and even paegs pdf", new Action(
                () => { Pdf.MergeOddAndEven(list.Select(x => x.FileInfo.FullName)); }
                )));
        }

        public static void SplitOddAndEven(IEnumerable<IFileSystemInfoExt> list)
        {
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("merge odd and even pages pdf", new Action(
                () => { Pdf.SplitOddAndEven(list.Select(x => x.FileInfo.FullName)); }
                )));
        }

        public static void PdfToJpg(IEnumerable<IFileSystemInfoExt> list, int dpi)
        {
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("create jpg from pdf", new Action(
                () =>
                {
                    foreach (var file in list)
                    {
                        PdfUtils.PdfToJpg(file.FileInfo.FullName,dpi);
                    }
                }
                )));
        }
    }
}
