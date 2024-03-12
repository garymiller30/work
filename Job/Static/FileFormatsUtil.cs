﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BackgroundTaskServiceLib;
using ImageMagick;
using Interfaces;
using Interfaces.PdfUtils;
using iTextSharp.text.pdf;
using Job.Dlg;
using Job.Models;
using Job.Static.Pdf.Common;
using Job.Static.Pdf.Convert;
using Job.Static.Pdf.Create.Ellipse;
using Job.Static.Pdf.Create.EmptyPdfTemplateWithCount;
using Job.Static.Pdf.Create.Rectangle;
using Job.Static.Pdf.Divide;
using Job.Static.Pdf.ExtractPages;
using Job.Static.Pdf.Merge;
using Job.Static.Pdf.MergeFrontsAndBack;
using Job.Static.Pdf.MergeOddAndEven;
using Job.Static.Pdf.MergeTemporary;
using Job.Static.Pdf.Repeat.Document;
using Job.Static.Pdf.RepeatPages;
using Job.Static.Pdf.Reverse;
using Job.Static.Pdf.RotateMirrorFrontAndBack;
using Job.Static.Pdf.Scale;
using Job.Static.Pdf.SetTrimBox.ByBleed;
using Job.Static.Pdf.SetTrimBox.ByFormat;
using Job.Static.Pdf.SetTrimBox.BySpread;
using Job.Static.Pdf.SplitCoverAndBlock;
using Job.Static.Pdf.SplitOddAndEven;
using Job.Static.Pdf.SplitSpread;
using Job.Static.Pdf.SplitTemporary;
using Job.Static.Pdf.ToJpg;
using Job.UserForms;
using PDFManipulate.Forms;

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

        public static void SetTrimBox(IEnumerable objects)
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
                                    new PdfSetTrimBoxByFormatParams { Width = result.TrimBox.Width, Height = result.TrimBox.Height };

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

        public static void ConvertToPDF(IEnumerable<IFileSystemInfoExt> list,Action action)
        {
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("convert to pdf", new Action(() =>
            {
                foreach (IFileSystemInfoExt ext in list)
                {
                    var convert = new PdfConvert(new PdfConvertParams());
                    convert.Run(ext.FileInfo.FullName);
                }

                action?.Invoke();

            })));
        }

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
        }

        public static void RepeatPages(IEnumerable<IFileSystemInfoExt> list)
        {

            using (var form = new FormInputCountPages())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    PdfRepeatPagesParams param = new PdfRepeatPagesParams { Count = form.CountPages };
                    BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("repeat pages pdf",
                new Action(() =>
                {
                    foreach (var item in list)
                    {
                        new PdfRepeatPages(param).Run(item.FileInfo.FullName);
                    }

                })));
                }
            }
        }

        public static void MergeFrontsAndBack(IEnumerable<IFileSystemInfoExt> list)
        {

            IEnumerable<string> files = list.Select(x => x.FileInfo.FullName);

            using (var form = new FormSelectBackFile(files))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    PdfMergeFrontsAndBackParams param = new PdfMergeFrontsAndBackParams();
                    param.BackFile = form.Back;
                    param.FrontsFiles = files.Except(new[] { param.BackFile }, StringComparer.OrdinalIgnoreCase).ToArray();

                    BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("merge front and back pdf", new Action(
                () =>
                {
                    new PdfMergeFrontsAndBack(param).Run();
                }
                )));
                }
            }
        }

        public static void ReversePages(IEnumerable<IFileSystemInfoExt> list)
        {
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("reverce pages pdf", new Action(() =>
                {
                    foreach (var item in list)
                    {
                        new PdfReverse().Run(item.FileInfo.FullName);
                    }
                }
                )));
        }

        public static void RepeatDocument(IEnumerable<IFileSystemInfoExt> list)
        {
            using (var form = new FormInputCountPages())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    PdfRepeatDocumentParams param = new PdfRepeatDocumentParams { Count = form.CountPages };

                    BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("repeat document pdf", new Action(
                () =>
                {
                    foreach (var item in list)
                    {
                        new PdfRepeatDocument(param).Run(item.FileInfo.FullName);
                    }
                }
                )));
                }
            }
        }

        public static void CreateRectangle(IEnumerable<IFileSystemInfoExt> list)
        {
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("create rectangle pdf", new Action(() =>
                {
                    foreach (var item in list)
                    {
                        new PdfCreateRectangle().Run(item.FileInfo.FullName);
                    }
                }
                )));
        }

        internal static void CreateEllipse(IEnumerable<IFileSystemInfoExt> list)
        {
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("create ellipse pdf", new Action(() =>
                {
                    foreach (var item in list)
                    {
                        new PdfCreateEllipse().Run(item.FileInfo.FullName);
                    }
                }
                )));
        }

        public static void ExtractPages(IEnumerable<IFileSystemInfoExt> list)
        {
            using (var form = new FormSelectCountPages())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {

                    PdfExtractPagesParams param = new PdfExtractPagesParams();
                    param.Pages = form.Pages;

                    BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("extract pages pdf", new Action(() =>
                    {
                        foreach (var item in list)
                        {
                            new PdfExtractPages(param).Run(item.FileInfo.FullName);
                        }
                    }
                )));
                }
            }
        }

        public static void SplitCoverAndBlock(IEnumerable<IFileSystemInfoExt> list)
        {
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("split cover and block pdf", new Action(
               () =>
               {
                   foreach (var item in list)
                   {
                       new PdfSplitCoverAndBlock().Run(item.FileInfo.FullName);
                   }
               }
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
                        BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("create templates with count pdf", new Action(() =>
                        {
                            foreach (EmptyTemplate item in list)
                            {
                                new PdfCreateEmptyPdfTemplateWithCount().Run(pathTo, item);
                            }
                        }
                )));
                    }
                }
            }
        }

        public static void RotatePagesMirror(IEnumerable<IFileSystemInfoExt> list)
        {
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("rotate mirror pages pdf", new Action(
               () =>
               {
                   foreach (var item in list)
                   {
                       new PdfRotateMirrorFrontAndBack().Run(item.FileInfo.FullName);
                   }
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

        public static void MergePdf(string[] convertFiles, Action action)
        {
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("split pdf", new Action(
                () =>
                {
                    new PdfMerger(convertFiles).Run();
                    action?.Invoke();
                }
                )));
        }

        public static void PdfMergeTemporary(PdfMergeTemporaryParams param)
        {
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("PdfMergeTemporary", new Action(
               () =>
               {
                   new PdfMergeTemporary(param).Run();
               }
               )));
        }

        internal static void SplitTemporary(List<string> list)
        {
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("SplitTemporary", new Action(
               () =>
               {
                   foreach(var file in list)
                   {
                       new PdfSplitTemporary().Run(file);
                   }
               }
               )));
        }
    }
}
