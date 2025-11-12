using BackgroundTaskServiceLib;
using BrightIdeasSoftware;
using ExtensionMethods;
using Interfaces;
using JobSpace.Dlg;
using JobSpace.Menus;
using JobSpace.Profiles;
using JobSpace.Static.Pdf.Imposition;
using JobSpace.Static.Pdf.Imposition.Services;
using JobSpace.Static.Pdf.MergeOddAndEven;
using JobSpace.Static.Pdf.MergeTemporary;
using JobSpace.UC;
using JobSpace.UserForms;
using JobSpace.UserForms.PDF;
using JobSpace.UserForms.PDF.Visual;
using Krypton.Toolkit;
using Logger;
using Microsoft.VisualBasic.FileIO;
using PDFManipulate.Forms;
using PythonEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.Static
{
    public static class FileBrowserSevices
    {
        #region PDF
        public static void PDF_VisualFalc(object selectedObject)
        {
            if (selectedObject != null && selectedObject is IFileSystemInfoExt fsi)
            {
                using (var form = new FormVisualFalc(fsi))
                {

                    form.ShowDialog();

                }

            }

        }
        public static void PDF_ConvertToPdf(IList files, Action action)
        {
            if (files.Count == 0) return;
            FileFormatsUtil.ConvertToPDF(files.Cast<IFileSystemInfoExt>().ToList(), action);
        }
        public static void PDF_CreateCollatingPageMark(IList files)
        {
            if (files.Count == 0) return;

            using (var form = new FormCreateCollatingPageMark())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    FileFormatsUtil.CreateCollatingPageMark(files.Cast<IFileSystemInfoExt>().Select(x => x.FileInfo.FullName), form.CreatePageCollationMarksParam);
                }
            }
        }
        public static void PDF_RepeatDocument(IList files)
        {
            if (files.Count == 0) return;
            FileFormatsUtil.RepeatDocument(files.Cast<IFileSystemInfoExt>().ToList());
        }
        public static void PDF_MergeFrontsAndBacks(IList files)
        {
            if (files.Count < 2) return;
            FileFormatsUtil.MergeFrontsAndBack(files.Cast<IFileSystemInfoExt>().ToList());
        }
        public static void PDF_RepeatPages(IList files)
        {
            if (files.Count == 0) return;
            FileFormatsUtil.RepeatPages(files.Cast<IFileSystemInfoExt>().ToList());
        }
        public static void PDF_ReversePages(IList files)
        {
            if (files.Count == 0) return;
            FileFormatsUtil.ReversePages(files.Cast<IFileSystemInfoExt>().ToList());
        }
        public static void PDF_CreateRectangle(IList files)
        {
            if (files.Count == 0) return;
            FileFormatsUtil.CreateRectangle(files.Cast<IFileSystemInfoExt>().ToList());
        }
        public static void PDF_CreateEllipse(IList files)
        {
            if (files.Count == 0) return;
            FileFormatsUtil.CreateEllipse(files.Cast<IFileSystemInfoExt>().ToList());
        }
        public static void PDF_SplitSpread(IList files)
        {
            if (files.Count == 0) return;
            using (var form = new FormPdfSplitterParams())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    FileFormatsUtil.SplitPdf(files.Cast<IFileSystemInfoExt>().ToList(), form.Params);
                }
            }
        }
        public static void PDF_SplitFile(IList files)
        {
            if (files.Count == 0) return;
            using (var form = new FormDividerParams())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    FileFormatsUtil.SplitPDF(files.Cast<IFileSystemInfoExt>().ToList(), form.Params);
                }
            }
        }
        public static void PDF_AddCutRectangle(IList files)
        {
            if (files.Count == 0) return;
            FileFormatsUtil.AddCutRectangle(files.Cast<IFileSystemInfoExt>().ToList());
        }
        public static void PDF_MergeFiles(IList files, Action action)
        {
            if (files.Count == 0) return;

            var _files = files.Cast<IFileSystemInfoExt>().ToList();

            using (var form = new UserForms.PDF.FormList(_files.Select(x => x.FileInfo.FullName).ToArray()))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    FileFormatsUtil.MergePdf(form.ConvertFiles, action);
                }
            }
        }
        public static void PDF_AddCutCircle(IList files)
        {
            if (files.Count == 0) return;
            FileFormatsUtil.AddCutCircle(files.Cast<IFileSystemInfoExt>().ToList());
        }
        public static void PDF_MergeToTemporaryFile(IList files, Action action)
        {
            if (files.Count == 0) return;
            FileFormatsUtil.PdfMergeTemporary(
                new PdfMergeTemporaryParams
                {
                    Files = files.Cast<IFileSystemInfoExt>().Select(x => x.FileInfo.FullName).ToList()
                }, action);
        }
        public static void PDF_SplitTemporaryFile(IList files)
        {
            if (files.Count == 0) return;
            FileFormatsUtil.SplitTemporary(files.Cast<IFileSystemInfoExt>().Select(x => x.FileInfo.FullName).ToList());
        }
        public static void PDF_AddFormatToFileName(IList files)
        {
            if (files.Count == 0) return;
            FileFormatsUtil.AddFormatToFileName(files.Cast<IFileSystemInfoExt>().ToList());
        }
        public static void PDF_CreateBigovkaMarks(IList files)
        {
            if (files.Count == 0) return;
            else if (files.Count == 1)
            {
                using (var form = new FormCreateBigovkaMarks((IFileSystemInfoExt)files[0]))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        FileFormatsUtil.CreateBigovkaMarks(files.Cast<IFileSystemInfoExt>().Select(x => x.FileInfo.FullName), form.BigovkaMarksParams);
                    }
                }
            }
            else
            {
                using (var form = new FormCreateBigovkaMarks())
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        FileFormatsUtil.CreateBigovkaMarks(files.Cast<IFileSystemInfoExt>().Select(x => x.FileInfo.FullName), form.BigovkaMarksParams);
                    }
                }
            }
        }
        public static void PDF_ExtractPages(IList files)
        {
            if (files.Count == 0) return;
            FileFormatsUtil.ExtractPages(files.Cast<IFileSystemInfoExt>().ToList());
        }
        public static void PDF_CreateFillRectangle(string targetDir)
        {
            using (var form = new FormCreateFillRectangle())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    var color = form.PdfColorResult;

                    FileFormatsUtil.CreateFillRectangle(new Static.Pdf.Create.Rectangle.PdfCreateFillRectangleParams
                    {
                        Width = (double)form.PdfWidth,
                        Height = (double)form.PdfHeight,
                        Lab = color.Lab,
                        Color = new Pdf.Imposition.Models.MarkColor
                        {
                            IsSpot = color.IsSpot,
                            C = (double)color.C,
                            M = (double)color.M,
                            Y = (double)color.Y,
                            K = (double)color.K,
                            Name = color.Name,
                        },
                        
                        
                    }, targetDir);

                }
            }
        }
        public static void PDF_ScaleFiles(IList files)
        {
            if (files.Count == 0) return;
            using (var form = new FormSelectPdfNewSize((IFileSystemInfoExt)files[0]))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    FileFormatsUtil.ScalePdf(files.Cast<IFileSystemInfoExt>().ToList(), form.Params);
                }
            }
        }
        public static void PDF_SaveToJpeg(IList files)
        {
            if (files.Count == 0) return;

            using (var form = new FormSelectDpi())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    FileFormatsUtil.PdfToJpg(
                        files.Cast<IFileSystemInfoExt>().ToList(),
                        new Static.Pdf.ToJpg.PdfToJpgParams { Dpi = form.Dpi, Quality = form.Quality });
                }
            }
        }
        public static void PDF_SplitToOddAndEven(IList files)
        {
            if (files.Count == 0) return;
            FileFormatsUtil.SplitOddAndEven(files.Cast<IFileSystemInfoExt>().ToList());
        }
        public static void PDF_MergeOddAndEven(IList files)
        {
            if (files.Count == 2)
            {
                PdfMergeOddAndEvenParams param = new PdfMergeOddAndEvenParams();

                var _files = files.Cast<IFileSystemInfoExt>().ToList();
                if (_files[0].FileInfo.Name.ToLower(CultureInfo.InvariantCulture).Contains("_even"))
                {
                    param.EvenFile = _files[0].FileInfo.FullName;
                    param.OddFile = _files[1].FileInfo.FullName;
                }
                else
                {
                    param.EvenFile = _files[1].FileInfo.FullName;
                    param.OddFile = _files[0].FileInfo.FullName;
                }
                FileFormatsUtil.MergeOddAndEven(param);
            }
            else
            {
                MessageBox.Show("Файлів має бути два! В одному непарні сторінки, а в іншому - парні", "Альо!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public static void PDF_Rotate90MirrorPages(IList files)
        {
            if (files.Count == 0) return;
            FileFormatsUtil.RotatePagesMirror(files.Cast<IFileSystemInfoExt>().ToList());
        }
        public static void PDF_SplitCoverAndBlock(IList files)
        {
            if (files.Count == 0) return;
            FileFormatsUtil.SplitCoverAndBlock(files.Cast<IFileSystemInfoExt>().ToList());
        }
        public static void PDF_VisualBlocknoteSpiral(IList files)
        {
            if (files.Count == 0) return;
            FileFormatsUtil.VisualBlocknoteSpiral(files.Cast<IFileSystemInfoExt>().ToList());
        }
        public static void PDF_ShowImposDialog(IList files, ImposInputParam param)
        {
            if (files.Count == 0) return;

            param.Files = files.Cast<IFileSystemInfoExt>().Select(x => x.FileInfo.FullName).ToList();

            var form = new FormPdfImposition(param);
            form.Show();

        }

        public static void PDF_MergeBlockBy3Months(object file)
        {
            if (file is IFileSystemInfoExt fsi)
            {
                FileFormatsUtil.MergeBlockBy3Months(fsi.FileInfo.FullName);
            }
        }

        #endregion

        #region FILE
        public static void File_NumericFiles(IList files)
        {
            if (files.Count == 0) return;
            FileFormatsUtil.NumericFiles(files.Cast<IFileSystemInfoExt>().Select(x => x.FileInfo.FullName));
        }
        public static void File_MoveFolderContentsToHere(IList files, IFileManager fileManager)
        {
            if (files.Count == 0) return;

            foreach (IFileSystemInfoExt infoExt in files)
            {
                if (infoExt.IsDir)
                {
                    fileManager.MoveFolderContentsToHere(infoExt, true);
                }
            }
        }
        public static void File_SendMail(IList files, IUserProfile profile)
        {
            profile.MailNotifier.SetCurJob(profile.Jobs.CurrentJob);

            if (files.Count != 0)
            {
                var attach = files.Cast<IFileSystemInfoExt>()
                    .Where(y => !y.IsDir)
                    .Select(x => x.FileInfo.FullName);

                if (attach.Any())
                {
                    profile.MailNotifier.SetAttachmentsList(attach);
                    return;
                }
            }

            profile.MailNotifier.SetAttachmentsList(new List<string>());

        }
        public static void File_DeleteFilesAndDirectories(IList files, IFileManager fileManager)
        {
            if (files.Count == 0) return;

            if (MessageBox.Show("Видалити?", "Видалити файл чи папку?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                DialogResult.Yes)
            {
                fileManager.DeleteFilesAndDirectories(files.Cast<IFileSystemInfoExt>());
            }
        }
        public static Tuple<int, int, long> File_GetSelectedFileSize(IList files)
        {
            if (files.Count == 0) return new Tuple<int, int, long>(0, 0, 0);

            long len = 0;
            int pages = 0;

            var selectedFiles = files.Cast<IFileSystemInfoExt>().ToArray();

            len = selectedFiles.Sum(x => x.IsDir ? 0 : x.FileInfo.Length);
            pages = selectedFiles.Sum(x => x.IsDir ? 0 : x.Format.cntPages);

            return new Tuple<int, int, long>(files.Count, pages, len);
        }
        public static IList File_SelectByExt(IList files, IEnumerable allFiles)
        {
            if (files.Count == 0) return files;
            if (allFiles == null) return files;
            var exts = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (IFileSystemInfoExt fsi in files)
            {
                if (!fsi.IsDir)
                {
                    exts.Add(fsi.FileInfo.Extension);
                }
            }
            if (exts.Count == 0) return files;

            var selectedFiles = allFiles.Cast<IFileSystemInfoExt>()
                .Where(x => !x.IsDir && exts.Contains(x.FileInfo.Extension))
                .ToArray();
            return selectedFiles;

        }
        public static async Task File_CopyToAsync(IUserProfile profile, IMenuSendTo menu, IList files)
        {
            if (files == null || files.Count == 0) return;
            if (menu == null) throw new ArgumentNullException(nameof(menu));

            var curJob = profile?.Jobs?.CurrentJob;
            var fileList = files.Cast<IFileSystemInfoExt>().ToArray();

            // If menu.FileName expects job-dependent placeholders but no current job -> do nothing
            if (!string.IsNullOrEmpty(menu.FileName) &&
                (menu.FileName.Contains("{3}") || menu.FileName.Contains("{4}") || menu.FileName.Contains("{5}")) &&
                curJob == null)
            {
                return;
            }

            foreach (var info in fileList)
            {
                string fileName;
                if (menu.FileName == null)
                {
                    fileName = info.FileInfo.Name;
                }
                else
                {
                    fileName = string.Format(menu.FileName, info.FileInfo.Name, curJob?.Number, curJob?.Customer);
                }


                var fn = Path.Combine(menu.Path, fileName);
                var info1 = info;

                if ((info.FileInfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    try
                    {
                        await Task.Run(() => FileSystem.CopyDirectory(info1.FileInfo.FullName, fn, UIOption.AllDialogs)).ConfigureAwait(false);
                    }
                    catch
                    {
                    }

                }
                else
                {
                    try
                    {
                        await Task.Run(() => FileSystem.CopyFile(info1.FileInfo.FullName, fn, UIOption.AllDialogs)).ConfigureAwait(false);
                    }
                    catch
                    {
                    }
                }
            }
        }
        public static void File_AddTirag(IFileManager manager, IList files)
        {
            if (files.Count == 0) return;
            if (files.Count > 1)
            {
                var form = new FormEnterTirag(manager, files.Cast<IFileSystemInfoExt>(), RenameFileByTirag);
                form.Show();
            }
            else
            {
                using (var form = new FormTirag())
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        foreach (IFileSystemInfoExt file in files)
                        {
                            RenameFileByTirag(manager, form.Tirag, file);
                        }
                    }
                }
            }
        }
        private static void RenameFileByTirag(IFileManager manager, int tirag, IFileSystemInfoExt file)
        {
            var reg = new Regex(@"#(\d+)\.");
            var match = reg.Match(file.FileInfo.Name);
            string targetFile;
            if (match.Success)
            {
                targetFile =
                    $"{Path.GetFileNameWithoutExtension(file.FileInfo.Name).Substring(0, match.Index)}#{tirag}{file.FileInfo.Extension}";
            }
            else
            {
                targetFile = $"{Path.GetFileNameWithoutExtension(file.FileInfo.Name)}#{tirag}{file.FileInfo.Extension}";
            }

            manager.MoveFileOrDirectoryToCurrentFolder(file, targetFile);
        }
        #endregion

        #region PROCESS
        public static void Process_StartFromMenuSendTo(IMenuSendTo menu)
        {
            try
            {
                var pi = new ProcessStartInfo
                {
                    WorkingDirectory = Path.GetDirectoryName(menu.Path) ?? throw new InvalidOperationException(),
                    FileName = Path.GetDirectoryName(menu.Path),
                };
                Process.Start(pi);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public static void Process_AppOrScript(IUserProfile profile, IMenuSendTo menu, IFileManager fileManager, IList files)
        {
            if (string.IsNullOrEmpty(menu.CommandLine)) { OpenFileForEdit(profile, menu); return; }

            if (profile.ScriptEngine.IsScriptFile(menu.Path))
            {
                ProcessScript(profile, fileManager, menu, files);
            }
            else
            {
                ProcessAppFiles(profile, fileManager, menu, files);
            }

        }
        private static void ProcessAppFiles(IUserProfile profile, IFileManager fileManager, IMenuSendTo menu, IList files)
        {
            if (menu.CommandLine.Contains("{1}") && files.Count == 0) // папка
            {
                ProcessAppFolder(profile, fileManager, menu);

            }
            else if (files.Count > 0)
            {
                var fileList = files.Cast<IFileSystemInfoExt>().ToArray();

                foreach (var info in fileList)
                {
                    ProcessAppFile(profile, info, menu);
                }
            }
        }
        private static void ProcessAppFile(IUserProfile profile, IFileSystemInfoExt info, IMenuSendTo menu)
        {
            var curJob = profile.Jobs?.CurrentJob;

            var args = string.Format(menu.CommandLine,
                        info.FileInfo.FullName,
                        Path.GetDirectoryName(info.FileInfo.FullName),
                        Path.GetFileNameWithoutExtension(info.FileInfo.FullName),
                        curJob?.Number,
                        curJob?.Customer,
                        curJob?.Description);
            var pii = new ProcessStartInfo
            {
                WorkingDirectory = Path.GetDirectoryName(menu.Path),
                FileName = menu.Path,
                Arguments = args,
            };

            var p = Process.Start(pii);
            Log.Info(profile, "Utils", $"process: {menu.Path} cmd: {pii.Arguments}");

            if (menu.EventOnFinish)
            {
                p?.WaitForExit();
                if (menu.StatusCode != 0)
                {
                    var number = info.FileInfo.Name.Split('_').First();
                    profile.Jobs.ChangeStatusCode(number, menu.StatusCode);
                }
            }
        }
        private static void ProcessAppFolder(IUserProfile profile, IFileManager manager, IMenuSendTo menu)
        {
            var curJob = profile.Jobs?.CurrentJob;

            var args = string.Format(menu.CommandLine,
                                                string.Empty,
                                                manager.Settings.CurFolder,
                                                string.Empty,
                                                curJob?.Number,
                                                curJob?.Customer,
                                                curJob?.Description);

            var processStartInfo = new ProcessStartInfo
            {
                WorkingDirectory = Path.GetDirectoryName(menu.Path),
                FileName = menu.Path,
                Arguments = args,

            };
            var p = Process.Start(processStartInfo);
            Log.Info(profile, "Utils", $"process: {menu.Path} cmd: {processStartInfo.Arguments}");

            if (!menu.EventOnFinish) return;
            p?.WaitForExit();
        }
        private static void ProcessScript(IUserProfile profile, IFileManager fileManager, IMenuSendTo menu, IList files)
        {
            if (menu.CommandLine.Contains("{1}")) // папка
            {
                ProcessScriptFolder(profile, fileManager, menu);

            }
            else if (files.Count > 0)
            {
                var fileList = files.Cast<IFileSystemInfoExt>().ToArray();

                BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask($"run script {menu.Name}", new Action(
                () =>
                {
                    foreach (var info in fileList)
                    {
                        ProcessScriptFile(profile, info, menu);
                    }
                }
                )));
            }
        }
        private static void ProcessScriptFile(IUserProfile profile, IFileSystemInfoExt info, IMenuSendTo menu)
        {
            var curJob = profile.Jobs?.CurrentJob;
            var args = string.Format(menu.CommandLine,
                        info.FileInfo.FullName,
                        Path.GetDirectoryName(info.FileInfo.FullName),
                        Path.GetFileNameWithoutExtension(info.FileInfo.FullName),
                        curJob?.Number,
                        curJob?.Customer,
                        curJob?.Description);

            var param = CreateScriptRunParameters(args, profile, menu);

            profile.ScriptEngine.FileBrowser.RunScript(param);

            if (menu.StatusCode != 0)
            {
                var number = info.FileInfo.Name.Split('_').First();
                profile.Jobs.ChangeStatusCode(number, menu.StatusCode);
            }
        }
        private static void ProcessScriptFolder(IUserProfile profile, IFileManager manager, IMenuSendTo menu)
        {
            var curJob = profile.Jobs?.CurrentJob;
            var args = string.Format(menu.CommandLine,
                                                string.Empty,
                                                manager.Settings.CurFolder,
                                                string.Empty,
                                                curJob?.Number,
                                                curJob?.Customer,
                                                curJob?.Description);


            var param = CreateScriptRunParameters(args, profile, menu);// new ScriptRunParameters();
                                                                       //UserProfile.ScriptEngine.FileBrowser.RunScript(param);
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask($"run script '{menu.Name}'", new Action(
               () => { profile.ScriptEngine.FileBrowser.RunScript(param); }
               )));
        }
        private static IScriptRunParameters CreateScriptRunParameters(string args, IUserProfile profile, IMenuSendTo menu)
        {
            var curJob = profile.Jobs?.CurrentJob;

            var param = new ScriptRunParameters();
            param.Values.Order = curJob;
            param.Values.OrderNumber = curJob?.Number;
            param.Values.Customer = curJob?.Customer;
            param.Values.Description = curJob?.Description;
            param.Values.Profile = profile;
            param.ScriptPath = menu.Path;
            param.Values.FileName = args;
            param.Values.ImposFactory = new ImpositionFactory((Profile)profile);
            return param;
        }
        private static void OpenFileForEdit(IUserProfile profile, IMenuSendTo menuSendTo)
        {
            var pi = new ProcessStartInfo
            {
                WorkingDirectory = Path.GetDirectoryName(menuSendTo.Path) ?? throw new InvalidOperationException(),
                FileName = menuSendTo.Path,
            };

            if (profile.ScriptEngine.IsScriptFile(menuSendTo.Path))
            {
                pi.Verb = "edit";
            }

            Process.Start(pi);
        }
        #endregion

        #region CLIPBOARD
        public static void Clipboard_CopyFileNames(IList files)
        {
            if (files.Count == 0) return;
            var sb = new StringBuilder();
            foreach (var file in files.Cast<IFileSystemInfoExt>())
            {
                sb.AppendLine(file.FileInfo.Name);
            }
            try
            {
                Clipboard.SetText(sb.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error copying to clipboard: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static void Clipboard_CopyFileNamesWithoutExtension(IList files)
        {
            if (files.Count == 0) return;
            var sb = new StringBuilder();
            foreach (var file in files.Cast<IFileSystemInfoExt>())
            {
                sb.AppendLine(Path.GetFileNameWithoutExtension(file.FileInfo.FullName));
            }

            try
            {
                Clipboard.SetText(sb.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error copying to clipboard: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
        public static void Clipboard_CopyFileNamesWithFormat(IList files)
        {
            if (files.Count == 0) return;
            var filePath = new StringBuilder();
            foreach (IFileSystemInfoExt fsi in files)
            {
                filePath.AppendLine($"{Path.GetFileNameWithoutExtension(fsi.FileInfo.FullName)}\t{fsi.Format.Width:0.#}x{fsi.Format.Height:0.#}");
            }

            try
            {
                Clipboard.SetText(filePath.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error copying to clipboard: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        public static void Clipboard_CopyFiles(IList files)
        {
            if (files.Count == 0) return;
            var filePaths = new System.Collections.Specialized.StringCollection();
            foreach (IFileSystemInfoExt fsi in files)
            {
                filePaths.Add(fsi.FileInfo.FullName);
            }
            try
            {
                Clipboard.SetFileDropList(filePaths);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error copying to clipboard: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static void Clipboard_PasteFiles(IFileManager fileManager)
        {
            if (Clipboard.ContainsFileDropList())
            {
                var filePaths = Clipboard.GetFileDropList();
                fileManager.PasteFromClipboard(filePaths.Cast<string>().ToArray());
            }
        }
        public static void Clipboard_CutFiles(IList files)
        {
            if (files.Count == 0) return;

            byte[] moveEffect = { 2, 0, 0, 0 };
            var dropEffect = new MemoryStream();
            dropEffect.Write(moveEffect, 0, moveEffect.Length);

            var data = new DataObject();

            var filePath = new StringCollection();
            foreach (IFileSystemInfoExt fsi in files)
            {
                filePath.Add(fsi.FileInfo.FullName);
            }

            try
            {
                data.SetFileDropList(filePath);
                data.SetData("Preferred DropEffect", DragDropEffects.Move);
                Clipboard.Clear();
                Clipboard.SetDataObject(data, true);

                FileManager.CopyPaste = true;
            }
            catch
            {
            }
            finally
            {
                dropEffect.Dispose();
            }

        }
        #endregion

        #region MAIL
        public static void Mail_SendFiles(IUserProfile profile, IList files, string mailAddress)
        {
            if (files.Count == 0) return;


            var filelist = files.Cast<IFileSystemInfoExt>();

            BackgroundTaskService.AddTask(new BackgroundTaskItem()
            {
                Name = $"send mail to {mailAddress}",
                BackgroundAction = () =>
                {
                    foreach (var file in filelist)
                    {
                        if (!file.IsDir)
                        {
                            profile.MailNotifier.SendFile(mailAddress, file.FileInfo.FullName);
                        }
                    }
                },
            });

        }

        public static void PDF_RemoveICCProfiles(IList files)
        {
            if (files.Count == 0) return;
            FileFormatsUtil.RemoveICCProfiles(files.Cast<IFileSystemInfoExt>().ToList());
        }



        #endregion
    }
}
