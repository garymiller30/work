using BackgroundTaskServiceLib;
using ImageMagick;
using Interfaces;
using Interfaces.Profile;
using JobSpace.Dlg;
using JobSpace.Profiles;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Imposition;
using JobSpace.Static.Pdf.MergeOddAndEven;
using JobSpace.Static.Pdf.MergeTemporary;
using JobSpace.UC;
using JobSpace.UserForms;
using JobSpace.UserForms.PDF;
using Logger;
using Microsoft.VisualBasic.FileIO;
using PDFManipulate.Forms;
using PythonEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
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
       
        public static void PDF_ShowImposDialog(IList files, ImposInputParam param)
        {
            if (files.Count == 0) return;

            param.Files = files.Cast<IFileSystemInfoExt>().Select(x => x.FileInfo.FullName).ToList();

            var form = new FormPdfImposition(param);
            form.Show();

        }

        #endregion

        #region FILE
        
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
                ), fileList.Select(x => x?.FileInfo?.FullName)));
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
                FileManager.CutFromClipboard = false;
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
        public static void Clipboard_PasteLikeCopyFiles(IFileManager fileManager)
        {
            if (Clipboard.ContainsFileDropList())
            {
                // вставити файли як копії тих, що в буфері обміну
                var filePaths = Clipboard.GetFileDropList();
                fileManager.PasteFromClipboardLikeCopy(filePaths.Cast<string>().ToArray());
            }
        }

        public static void Clipboard_PasteWithSpecificName(IFileManager fileManager, IFileSystemInfoExt Targetfile)
        {
            if (Clipboard.ContainsFileDropList())
            {
                var filePaths = Clipboard.GetFileDropList();

                fileManager.PasteFromClipboardWithSpecificName(filePaths[0], Targetfile);
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

                FileManager.CutFromClipboard = true;
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
                Files = filelist
                    .Where(x => x != null && !x.IsDir)
                    .Select(x => x.FileInfo.FullName)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct(StringComparer.InvariantCultureIgnoreCase)
                    .ToList(),
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

        public static Image File_GetPreview(IFileSystemInfoExt f, int pageIdx = 0)
        {
            string ext = f.FileInfo.Extension.ToLowerInvariant();
            if (ext == ".pdf" || ext == ".ai")
            {
                Exception lastException = null;

                for (int attempt = 1; attempt <= 2; attempt++)
                {
                    try
                    {
                        return PdfHelper.RenderByTrimBox(f.FileInfo.FullName, pageIdx);
                    }
                    catch (Exception e)
                    {
                        lastException = e;
                        if (attempt == 1)
                            System.Threading.Thread.Sleep(100);
                    }
                }

                Log.Error(null, "File_GetPreview", $"Cannot render preview for {f.FileInfo.FullName}, page {pageIdx + 1}: {lastException?.Message}");
                return null;
            }
            else if (ext == ".tif" || ext == ".tiff" || ext == ".png" || ext == ".bmp" || ext == ".jpg" || ext == ".jpeg")
            {
                return Image.FromFile(f.FileInfo.FullName);
            }
            else if (ext == ".psd" || ext == ".eps")
            {
                using (var psd = new MagickImage(f.FileInfo.FullName))
                {
                    return psd.ToBitmap();
                }
            }
          
            return null;
        }

        public static void File_FindReplaceTirag(IList selectedObjects)
        {
            using (var form = new FormRegexRenameFiles(selectedObjects.Cast<IFileSystemInfoExt>().ToList()))
            {
                form.ShowDialog();
            }
        }

        /// <summary>
        /// отримати розмір зображення в міліметрах. 
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static Size GetImageSize(string fullName)
        {

            string ext = Path.GetExtension(fullName).ToLowerInvariant();

            if (ext == ".tif" || ext == ".tiff" || ext == ".png" || ext == ".bmp" || ext == ".jpg" || ext == ".jpeg")
            {
                using (var img = Image.FromFile(fullName))
                {
                    // розмір в міліметрах
                    return new Size((int)(img.Width * 25.4 / img.HorizontalResolution), (int)(img.Height * 25.4 / img.VerticalResolution));
                }
            }
            else if (ext == ".psd" || ext == ".eps")
            {
                using (var psd = new MagickImage(fullName))
                {
                    return new Size((int)(psd.Width * 25.4 / psd.Density.X), (int)(psd.Height * 25.4 / psd.Density.Y));
                }
            }

            return new Size(100, 100);
        }

        #endregion
    }
}
