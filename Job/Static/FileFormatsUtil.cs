using System;
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
using iTextSharp.text;
using iTextSharp.text.pdf;
using JobSpace.Dlg;
using JobSpace.Models;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Convert;
using JobSpace.Static.Pdf.Create.BigovkaMarks;
using JobSpace.Static.Pdf.Create.CollatingPageMark;
using JobSpace.Static.Pdf.Create.CutEllipse;
using JobSpace.Static.Pdf.Create.Ellipse;
using JobSpace.Static.Pdf.Create.EmptyPdfTemplateWithCount;
using JobSpace.Static.Pdf.Create.FillRectangle;
using JobSpace.Static.Pdf.Create.Rectangle;
using JobSpace.Static.Pdf.Divide;
using JobSpace.Static.Pdf.ExtractPages;
using JobSpace.Static.Pdf.Merge;
using JobSpace.Static.Pdf.MergeFrontsAndBack;
using JobSpace.Static.Pdf.MergeOddAndEven;
using JobSpace.Static.Pdf.MergeTemporary;
using JobSpace.Static.Pdf.Rearange;
using JobSpace.Static.Pdf.Remove;
using JobSpace.Static.Pdf.Repeat.Document;
using JobSpace.Static.Pdf.RepeatPages;
using JobSpace.Static.Pdf.Reverse;
using JobSpace.Static.Pdf.RotateMirrorFrontAndBack;
using JobSpace.Static.Pdf.Scale;
using JobSpace.Static.Pdf.SetTrimBox.ByBleed;
using JobSpace.Static.Pdf.SetTrimBox.ByFormat;
using JobSpace.Static.Pdf.SetTrimBox.BySpread;
using JobSpace.Static.Pdf.SplitCoverAndBlock;
using JobSpace.Static.Pdf.SplitOddAndEven;
using JobSpace.Static.Pdf.SplitSpread;
using JobSpace.Static.Pdf.SplitTemporary;
using JobSpace.Static.Pdf.ToJpg;
using JobSpace.Static.Pdf.Visual.BlocknoteSpiral;
using JobSpace.UserForms;
using JobSpace.UserForms.PDF.Visual;
using PDFManipulate.Forms;

namespace JobSpace.Static
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
                case ".png":
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
                    Bleeds = (decimal)(info.Density.X + info.Density.Y) / 2,
                };
            }
            catch
            {
            }
        }

        private static void GetPdf(IFileSystemInfoExt sfi)
        {

            var boxes = PdfHelper.GetPagesInfo(sfi.FileInfo.FullName);
            var box = boxes[0];
            sfi.Format = new FileFormat
            {
                Width = (decimal)box.Trimbox.wMM(),
                Height = (decimal)box.Trimbox.hMM(),
                Bleeds = (decimal)((box.Mediabox.wMM() - box.Trimbox.wMM()) / 2),
                cntPages = boxes.Count(),
            };
            #region [USING ITEXT]
            //Rectangle media = null;

            //PdfReader pdfReader = null;
            //int pages = 0;

            //try
            //{
            //    pdfReader = new PdfReader(sfi.FileInfo.FullName);
            //    pages = pdfReader.NumberOfPages;
            //    media = pdfReader.GetBoxSize(1, "media");
            //    var rect = pdfReader.GetBoxSize(1, "trim");
            //    pdfReader.Dispose();

            //    if (rect == null)
            //    {
            //        sfi.Format = new FileFormat
            //        {
            //            Width = (decimal)media.Width / Mn,
            //            Height = (decimal)media.Height / Mn,
            //            Bleeds = 0,
            //            cntPages = pages,
            //        };
            //    }
            //    else
            //    {
            //        sfi.Format = new FileFormat
            //        {
            //            Width = (decimal)rect.Width / Mn,
            //            Height = (decimal)rect.Height / Mn,
            //            Bleeds = (decimal)(media.Width - rect.Width) / 2 / Mn,
            //            cntPages = pages,
            //        };
            //    }
            //}
            //catch
            //{
            //    if (media != null)
            //    {
            //        sfi.Format = new FileFormat()
            //        {
            //            Width = (decimal)media.Width / Mn,
            //            Height = (decimal)media.Height / Mn,
            //            Bleeds = 0,
            //            cntPages = pages,
            //        };
            //    }
            //    else
            //    {
            //        sfi.Format = new FileFormat();
            //    }
            //}
            //finally
            //{
            //    pdfReader?.Dispose();
            //}
            #endregion
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
                            Bleeds = (decimal)hresolution,
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
            if (objects == null) return;

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
                        },
                    });
                }
            }

        }

        public static void ConvertToPDF(IEnumerable<IFileSystemInfoExt> list, Action action)
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
                    PdfMergeFrontsAndBackParams param = new PdfMergeFrontsAndBackParams
                    {
                        BackFile = form.Back,
                    };
                    param.FrontsFiles = files.Except(new[] { param.BackFile, }, StringComparer.OrdinalIgnoreCase).ToArray();

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

                    PdfExtractPagesParams param = new PdfExtractPagesParams
                    {
                        Pages = form.Pages,
                    };

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
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("split odd and even pages pdf", new Action(
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

        public static void PdfMergeTemporary(PdfMergeTemporaryParams param, Action action)
        {
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("PdfMergeTemporary", new Action(
               () =>
               {
                   bool result = new PdfMergeTemporary(param).Run();
                   if (result)
                   {
                       action?.Invoke();
                   }
               }
               )));
        }

        public static void SplitTemporary(IEnumerable<string> list)
        {
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("SplitTemporary", new Action(
               () =>
               {
                   foreach (var file in list)
                   {
                       new PdfSplitTemporary().Run(file);
                   }
               }
               )));
        }

        public static void CreateBigovkaMarks(IEnumerable<string> files, CreateBigovkaMarksParams param)
        {
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("CreateBigovkaMarks", new Action(
               () =>
               {
                   foreach (var file in files)
                   {
                       new CreateBigovkaMarks(param).Run(file);
                   }
               }
               )));
        }

        internal static void CreateFillRectangle(PdfCreateFillRectangleParams param, string pathTo)
        {
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("CreateFillRectangle", new Action(
               () =>
               {
                   new PdfCreateFillRectangle(param).Run(Path.Combine(pathTo, $"{param.Width}x{param.Height}.pdf"));
               }
               )));

        }

        internal static void NumericFiles(IEnumerable<string> files)
        {
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("Numeric files", new Action(
            () =>
            {
                var arr = files.ToArray();
                int count = files.Count();
                int numCnt = $"{count}".Length;

                for (int i = 1; i <= count; i++)
                {

                    File.Move(arr[i - 1],
                        Path.Combine(
                            Path.GetDirectoryName(arr[i - 1]), $"{i.ToString($"D0{numCnt}", CultureInfo.InvariantCulture)}.{Path.GetFileName(arr[i - 1])}"
                            )
                        );
                }
            }
               )));
        }

        public static void AddFormatToFileName(List<IFileSystemInfoExt> fileSystemInfoExts)
        {
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("Додати формат документа до імені файлу", new Action(
            () =>
            {
                foreach (var file in fileSystemInfoExts)
                {
                    var info = PdfHelper.GetPageInfo(file.FileInfo.FullName);

                    string newFileName = $"{Path.GetFileNameWithoutExtension(file.FileInfo.FullName)}_{info.Trimbox.wMM():N0}x{info.Trimbox.hMM():N0}{Path.GetExtension(file.FileInfo.FullName)}";
                    File.Move(file.FileInfo.FullName, Path.Combine(Path.GetDirectoryName(file.FileInfo.FullName), newFileName));

                }
            })));
        }

        public static void AddCutCircle(List<IFileSystemInfoExt> fileSystemInfoExts)
        {
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("Додати контур висічки 'коло' до файлу", new Action(
            () =>
            {
                foreach (var file in fileSystemInfoExts)
                {
                    new PdfCreateCutEllipse().Run(file.FileInfo.FullName);

                }
            })));
        }

        public static void AddCutRectangle(List<IFileSystemInfoExt> fileSystemInfoExts)
        {
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("Додати контур висічки 'прямокутник' до файлу", new Action(
            () =>
            {
                foreach (var file in fileSystemInfoExts)
                {
                    new PdfCreateCutRectangle().Run(file.FileInfo.FullName);

                }
            })));
        }

        public static void VisualBlocknoteSpiral(List<IFileSystemInfoExt> files)
        {
            if (files.Count == 1)
            {
                using (var form = new FormVisualBlocknoteSpiral(files[0]))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("Додати контур спіралі до файлу", new Action(
                        () =>
                        {
                            foreach (var file in files)
                            {
                                new VisualBlocknoteSpiral(form.SpiralSettings).Run(file.FileInfo.FullName);
                            }
                        })));
                    }
                }
            }
            else
            {
                using (var form = new FormVisualBlocknoteSpiral(files))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("Додати контур спіралі до файлу", new Action(
                        () =>
                        {
                            foreach (var file in files)
                            {
                                new VisualBlocknoteSpiral(form.SpiralSettings).Run(file.FileInfo.FullName);
                            }
                        })));
                    }
                }
            }
        }

        public static void CreateCollatingPageMark(IEnumerable<string> enumerable, CreateCollatingPageMarkParams param)
        {
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("Додати мітки підбору до файлу", new Action(
            () =>
            {
                foreach (var file in enumerable)
                {
                    new CreateCollatingPageMark(param).Run(file);
                }
            })));
        }

        public static void MergeBlockBy3Months(string file)
        {
            new MergeBlockBy3Months().Run(file);
        }

        public static void RemoveICCProfiles(List<IFileSystemInfoExt> fileSystemInfoExts)
        {
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("Remove ICC Profiles from PDF", new Action(
            () =>
            {
                foreach (var file in fileSystemInfoExts)
                {
                    new PdfRemoveICCProfiles().Run(file.FileInfo.FullName);
                }
            })));
        }

        public static void RearangePagesForQuartalCalendar(List<IFileSystemInfoExt> files, int cntMonthInBlock = 3)
        {
            foreach (var file in files)
            {
                new RearangePagesForQuartalCalendar(cntMonthInBlock).Run(file.FileInfo.FullName);
            }


        }
    }
}
