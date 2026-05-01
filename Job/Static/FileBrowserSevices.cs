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
using Newtonsoft.Json;
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
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.Static
{
    public static class FileBrowserSevices
    {
        private const string PreviewCacheFolderName = ".preview";
        private const string PreviewCacheIndexFileName = "preview-cache.json";
        private static readonly object PreviewCacheLock = new object();

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
                FileInfo sourceFile = new FileInfo(f.FileInfo.FullName);
                Image cachedPreview = TryGetCachedPreview(sourceFile, pageIdx);
                if (cachedPreview != null)
                    return cachedPreview;

                Exception lastException = null;

                for (int attempt = 1; attempt <= 2; attempt++)
                {
                    try
                    {
                        using (Bitmap preview = PdfHelper.RenderByTrimBox(f.FileInfo.FullName, pageIdx))
                        {
                            Image savedPreview = TrySaveCachedPreview(sourceFile, pageIdx, preview);
                            if (savedPreview != null)
                                return savedPreview;

                            return new Bitmap(preview);
                        }
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

        private static Image TryGetCachedPreview(FileInfo sourceFile, int pageIdx)
        {
            try
            {
                if (sourceFile == null || pageIdx < 0 || !sourceFile.Exists)
                    return null;

                lock (PreviewCacheLock)
                {
                    PreviewCacheIndex index = LoadPreviewCacheIndex(sourceFile.DirectoryName);
                    PreviewCacheEntry entry = index?.Files?.FirstOrDefault(x =>
                        string.Equals(x.FileName, sourceFile.Name, StringComparison.InvariantCultureIgnoreCase) &&
                        x.PageIndex == pageIdx);

                    if (entry == null ||
                        entry.Length != sourceFile.Length ||
                        entry.LastWriteTimeUtcTicks != sourceFile.LastWriteTimeUtc.Ticks ||
                        string.IsNullOrWhiteSpace(entry.PreviewFileName))
                    {
                        return null;
                    }

                    string previewPath = Path.Combine(GetPreviewCacheDirectory(sourceFile.DirectoryName, false), entry.PreviewFileName);
                    if (!File.Exists(previewPath))
                        return null;

                    return LoadBitmapWithoutFileLock(previewPath);
                }
            }
            catch (Exception e)
            {
                Log.Error(null, "TryGetCachedPreview", $"Cannot read cached preview for {sourceFile?.FullName}, page {pageIdx + 1}: {e.Message}");
                return null;
            }
        }

        private static Image TrySaveCachedPreview(FileInfo sourceFile, int pageIdx, Bitmap preview)
        {
            try
            {
                if (sourceFile == null || pageIdx < 0 || preview == null || !sourceFile.Exists)
                    return null;

                lock (PreviewCacheLock)
                {
                    string previewDir = GetPreviewCacheDirectory(sourceFile.DirectoryName, true);
                    PreviewCacheIndex index = LoadPreviewCacheIndex(sourceFile.DirectoryName) ?? new PreviewCacheIndex();
                    if (index.Files == null)
                        index.Files = new List<PreviewCacheEntry>();

                    PreviewCacheEntry entry = index.Files.FirstOrDefault(x =>
                        string.Equals(x.FileName, sourceFile.Name, StringComparison.InvariantCultureIgnoreCase) &&
                        x.PageIndex == pageIdx);

                    string previewFileName = BuildPreviewFileName(sourceFile, pageIdx);
                    string previewPath = Path.Combine(previewDir, previewFileName);
                    string tempPreviewPath = Path.Combine(previewDir, $"{Guid.NewGuid():N}.tmp");

                    preview.Save(tempPreviewPath, System.Drawing.Imaging.ImageFormat.Png);
                    if (File.Exists(previewPath))
                        File.Delete(previewPath);
                    File.Move(tempPreviewPath, previewPath);

                    if (entry == null)
                    {
                        entry = new PreviewCacheEntry();
                        index.Files.Add(entry);
                    }

                    entry.FileName = sourceFile.Name;
                    entry.FullName = sourceFile.FullName;
                    entry.PageIndex = pageIdx;
                    entry.Length = sourceFile.Length;
                    entry.LastWriteTimeUtcTicks = sourceFile.LastWriteTimeUtc.Ticks;
                    entry.PreviewFileName = previewFileName;
                    entry.PreviewCreatedUtcTicks = DateTime.UtcNow.Ticks;

                    SavePreviewCacheIndex(sourceFile.DirectoryName, index);

                    return LoadBitmapWithoutFileLock(previewPath);
                }
            }
            catch (Exception e)
            {
                Log.Error(null, "TrySaveCachedPreview", $"Cannot save cached preview for {sourceFile?.FullName}, page {pageIdx + 1}: {e.Message}");
                return null;
            }
        }

        private static string GetPreviewCacheDirectory(string sourceDirectory, bool create)
        {
            if (string.IsNullOrWhiteSpace(sourceDirectory))
                return null;

            string previewDir = Path.Combine(sourceDirectory, PreviewCacheFolderName);
            if (create && !Directory.Exists(previewDir))
            {
                System.IO.Directory.CreateDirectory(previewDir);
                try
                {
                    File.SetAttributes(previewDir, File.GetAttributes(previewDir) | FileAttributes.Hidden);
                }
                catch
                {
                }
            }

            return previewDir;
        }

        private static PreviewCacheIndex LoadPreviewCacheIndex(string sourceDirectory)
        {
            string previewDir = GetPreviewCacheDirectory(sourceDirectory, false);
            if (string.IsNullOrWhiteSpace(previewDir))
                return new PreviewCacheIndex();

            string indexPath = Path.Combine(previewDir, PreviewCacheIndexFileName);
            if (!File.Exists(indexPath))
                return new PreviewCacheIndex();

            string json = File.ReadAllText(indexPath, Encoding.UTF8);
            return JsonConvert.DeserializeObject<PreviewCacheIndex>(json) ?? new PreviewCacheIndex();
        }

        private static void SavePreviewCacheIndex(string sourceDirectory, PreviewCacheIndex index)
        {
            string previewDir = GetPreviewCacheDirectory(sourceDirectory, true);
            string indexPath = Path.Combine(previewDir, PreviewCacheIndexFileName);
            string tempIndexPath = Path.Combine(previewDir, $"{Guid.NewGuid():N}.json.tmp");
            string json = JsonConvert.SerializeObject(index, Formatting.Indented);

            File.WriteAllText(tempIndexPath, json, Encoding.UTF8);
            if (File.Exists(indexPath))
                File.Delete(indexPath);
            File.Move(tempIndexPath, indexPath);
        }

        private static Bitmap LoadBitmapWithoutFileLock(string imagePath)
        {
            using (var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var image = Image.FromStream(stream))
            {
                return new Bitmap(image);
            }
        }

        private static string BuildPreviewFileName(FileInfo sourceFile, int pageIdx)
        {
            string safeName = SanitizePreviewFileName(Path.GetFileNameWithoutExtension(sourceFile.Name));
            if (safeName.Length > 80)
                safeName = safeName.Substring(0, 80);

            return $"{safeName}_p{pageIdx + 1}_{GetStablePathHash(sourceFile.FullName)}.png";
        }

        private static string SanitizePreviewFileName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "preview";

            char[] invalidChars = Path.GetInvalidFileNameChars();
            var builder = new StringBuilder(value.Length);
            foreach (char c in value)
            {
                builder.Append(invalidChars.Contains(c) ? '_' : c);
            }

            string result = builder.ToString().Trim();
            return string.IsNullOrWhiteSpace(result) ? "preview" : result;
        }

        private static string GetStablePathHash(string value)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] bytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(value ?? string.Empty));
                return BitConverter.ToString(bytes, 0, 8).Replace("-", string.Empty).ToLowerInvariant();
            }
        }

        private class PreviewCacheIndex
        {
            public List<PreviewCacheEntry> Files { get; set; } = new List<PreviewCacheEntry>();
        }

        private class PreviewCacheEntry
        {
            public string FileName { get; set; }
            public string FullName { get; set; }
            public int PageIndex { get; set; }
            public long Length { get; set; }
            public long LastWriteTimeUtcTicks { get; set; }
            public string PreviewFileName { get; set; }
            public long PreviewCreatedUtcTicks { get; set; }
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
