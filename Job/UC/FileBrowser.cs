using BackgroundTaskServiceLib;
using BrightIdeasSoftware;
using ExtensionMethods;
using FtpClient;
using Interfaces;
using Interfaces.PdfUtils;
using JobSpace.Dlg;
using JobSpace.Ext;
using JobSpace.Menus;
using JobSpace.Models;
using JobSpace.Profiles;
using JobSpace.Static;
using JobSpace.Static.Pdf.Imposition;
using JobSpace.Static.Pdf.Imposition.Services;
using JobSpace.Static.Pdf.Imposition.Services.TextVariables;
using JobSpace.Static.Pdf.MergeOddAndEven;
using JobSpace.Static.Pdf.MergeTemporary;
using JobSpace.UserForms;
using JobSpace.UserForms.PDF;
using Logger;
using Microsoft.VisualBasic.FileIO;
using PDFManipulate.Forms;
using PythonEngine;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using static JobSpace.Static.NaturalSorting;

namespace JobSpace.UC
{
    public sealed partial class FileBrowser : UserControl, IFileBrowser
    {
        private const string Loading = "завантаження";
        private const string ConfirmDelete = "Видалити?";
        private const string Delete = "Видалити";

        private IUserProfile UserProfile { get; set; }

        //PythonEngine.PythonEngine _pythonEngine;
        private IFileManager _fileManager;

        private string[] _customButtonPath;


        public string DefaultSettingsFolder { get; set; }

        #region [ EVENTS ]

        public event EventHandler OnCustomButtonClick = delegate { };
        public event EventHandler<string[]> OnFtpUpload = delegate { };
        public event EventHandler<string> OnDropHttpLink = delegate { };
        public event EventHandler<string> OnChangeJobDescription = delegate { };
        public event EventHandler<string> OnMoveFileToArchive = delegate { };
        public event EventHandler<IJob> OnChangeStatus = delegate { };
        public event EventHandler<string> OnCreateOrderFromFile = delegate { };

        #endregion

        public FileBrowser(IUserProfile profile)
        {
            UserProfile = profile;

            InitializeComponent();
            InitFileManager();
            InitListView();
           
            ApplySettings();


            ClonePdfMenu();
        }

        

        private void ClonePdfMenu()
        {
            foreach (var item in утилітиДляPDFToolStripMenuItem.DropDownItems)
            {
                if (item is ToolStripMenuItem ttmi)
                {
                    toolStripDropDownButtonSplitPdfMenu.DropDownItems.Add(ttmi.Clone());
                }
                else if (item is ToolStripSeparator)
                {
                    toolStripDropDownButtonSplitPdfMenu.DropDownItems.Add(new ToolStripSeparator());
                }
            }
        }

        private void InitListView()
        {
            var rbd = new RowBorderDecoration
            {
                BorderPen = new Pen(System.Drawing.Color.FromArgb(255, System.Drawing.Color.DarkGreen), 1),
                BoundsPadding = new Size(0, -1),
                CornerRounding = 3.0F,
            };

            objectListView1.SelectedRowDecoration = rbd;

            flagRenderer1.Add(ColorSpaces.Cmyk, "CMYK");
            flagRenderer1.Add(ColorSpaces.Gray, "gray");
            flagRenderer1.Add(ColorSpaces.Rgb, "RGB");
            flagRenderer1.Add(ColorSpaces.Lab, "LAB");
            flagRenderer1.Add(ColorSpaces.Spot, "spot");
            flagRenderer1.Add(ColorSpaces.Pattern, "pattern");
            flagRenderer1.Add(ColorSpaces.Unknown, "unknown");
            flagRenderer1.Add(ColorSpaces.ICCBased, "icc");
            flagRenderer1.Add(ColorSpaces.All, "all");


            olvColumn_FileName.AspectGetter +=
                r => ((IFileSystemInfoExt)r).IsDir ? ((IFileSystemInfoExt)r).FileInfo.Name : Path.GetFileNameWithoutExtension(((IFileSystemInfoExt)r).FileInfo.Name);
            olvColumnType.AspectGetter += r => ((IFileSystemInfoExt)r).IsDir ? string.Empty : Path.GetExtension(((IFileSystemInfoExt)r).FileInfo.Name);


            olvColumn_Size.AspectGetter += r =>
            {

                if (((IFileSystemInfoExt)r).IsDir)
                {
                    return "<DIR>";
                }

                return ((IFileSystemInfoExt)r).FileInfo.Length.GetFileSizeInString();

            };

            var helper = new SysImageListHelper(objectListView1);
            olvColumn_FileName.ImageGetter = x => helper.GetImageIndex(((IFileSystemInfoExt)x).FileInfo.FullName);

            objectListView1.CustomSorter = delegate (OLVColumn column, SortOrder order)
            {
                if (column == olvColumn_FileName) objectListView1.ListViewItemSorter = new FileNameNaturalComparer(order);
                else if (column == olvColumnWidth) objectListView1.ListViewItemSorter = new FileWidthComparer(order);
                else if (column == olvColumnHeight) objectListView1.ListViewItemSorter = new FileHeightComparer(order);
                else if (column == olvColumnPages) objectListView1.ListViewItemSorter = new FilePagesComparer(order);
                else if (column == olvColumnBleeds) objectListView1.ListViewItemSorter = new FileBleedComparer(order);
                else if (column == olvColumn_DateTime) objectListView1.ListViewItemSorter = new FileDateComparer(order);
            };
        }

        #region [FILE MANAGER]

        private void InitFileManager()
        {

            _fileManager = new FileManager();

            _fileManager.OnRefreshDirectory += FileManager_OnRefreshDirectory;
            _fileManager.OnSelectFileName += FileManagerOnOnSelectFileName;
            _fileManager.OnChangeRootDirectory += FileManager_OnChangeRootDirectory;
            _fileManager.OnAddFile += FileManager_OnAddFile;
            _fileManager.OnDeleteFile += FileManager_OnDeleteFile;
            _fileManager.OnChangeFile += FileManager_OnChangeFile;
            _fileManager.OnChangeDirectory += FileManager_OnChangeDirectory;
            _fileManager.OnError += FileManager_OnError;
            _fileManager.OnSelectParent += FileManagerOnOnSelectParent;
        }

        private void FileManagerOnOnSelectFileName(object sender, string e)
        {
            IFileSystemInfoExt file = objectListView1.Objects.Cast<IFileSystemInfoExt>()
                .FirstOrDefault(x => x.FileInfo.Name.Equals(e, StringComparison.InvariantCultureIgnoreCase));
            if (file != null) objectListView1.SelectObject(file);

        }

        private void FileManagerOnOnSelectParent(object sender, IFileSystemInfoExt e)
        {
            objectListView1.SelectObject(e, true);
        }

        private void FileManager_OnChangeDirectory(object sender, IFileSystemInfoExt e)
        {
            objectListView1.ClearObjects();
            objectListView1.EmptyListMsg = Loading;
        }

        private void FileManager_OnError(object sender, string e)
        {
            objectListView1.EmptyListMsg = e;
            UpdateStatusControl();
        }

        private void FileManager_OnChangeFile(object sender, IFileSystemInfoExt e)
        {
            if (_fileManager.Settings.ScanFiles)
                e.GetExtendedFileInfoFormat();

            objectListView1.RefreshObject(e);
            UpdateStatusControl();
        }

        private void FileManager_OnDeleteFile(object sender, IFileSystemInfoExt e)
        {
            objectListView1.RemoveObject(e);
            UpdateStatusControl();
        }

        private void FileManager_OnAddFile(object sender, IFileSystemInfoExt e)
        {
            objectListView1.AddObject(e);
            UpdateStatusControl();
        }

        private void FileManager_OnChangeRootDirectory(object sender, EventArgs e)
        {
            objectListView1.ClearObjects();
            objectListView1.EmptyListMsg = Loading;
        }

        private void FileManager_OnRefreshDirectory(object sender, List<IFileSystemInfoExt> e)
        {
            StopTaskGetExtendedInfo();

            objectListView1.EmptyListMsg = null;
            objectListView1.AddObjects(e);

            StartTaskGetExtendedInfo(e);

            UpdateStatusControl();

        }

        #endregion

        private CancellationTokenSource _cts;
        private Task _taskGetExtendedFileInfo;

        void StartTaskGetExtendedInfo(List<IFileSystemInfoExt> e)
        {
            if (!_fileManager.Settings.ScanFiles) return;

            _cts = new CancellationTokenSource();
            _taskGetExtendedFileInfo = Task.Run(() => { ProcessTaskGetExtendedFileInfo(e, _cts.Token); }, _cts.Token);
        }


        void ProcessTaskGetExtendedFileInfo(List<IFileSystemInfoExt> e, CancellationToken token)
        {
            foreach (var ext in e)
            {
                ext.GetExtendedFileInfoFormat();
                if (token.IsCancellationRequested) break;
            }
        }

        void StopTaskGetExtendedInfo()
        {
            if (_cts == null) return;
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
            if (_taskGetExtendedFileInfo == null) return;
            if (!_taskGetExtendedFileInfo.IsCompleted) return;
            _taskGetExtendedFileInfo.Wait();

        }

        private void UpdateStatusControl()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateStatusControl));
                return;
            }

            toolStripStatusLabelCountFiles.Text = _fileManager.GetCountFiles().ToString(CultureInfo.InvariantCulture);
            toolStripStatusLabelSelected.Text = GetSelectedFilesSize();

            if (IsHandleCreated)
                kryptonLabelPath.Text = _fileManager.Settings.CurFolder ?? "\\";
        }

        private string GetSelectedFilesSize()
        {
            var cnt = 0;
            long len = 0;
            int pages = 0;
            IFileSystemInfoExt file = null;
            
            cnt = objectListView1.SelectedObjects.Count;
            if (cnt > 1)
            {
                var selectedFiles = objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().ToArray();

                len = selectedFiles.Sum(x => x.IsDir ? 0 : x.FileInfo.Length);
                pages = selectedFiles.Sum(x => x.IsDir ? 0 : x.Format.cntPages);
            }

            if (cnt == 1)
            {
                file = objectListView1.SelectedObject as IFileSystemInfoExt;
                cnt = 1;
            }

            if (file == null || file.IsDir) return $"{cnt} ({len.GetFileSizeInString()}, {pages} pp.)";

            len = file.FileInfo.Length;
            return $"{cnt} ({len.GetFileSizeInString()}, {file.Format.cntPages} pp., {file.Format.Width:N1} x {file.Format.Height:N1} mm)";
        }

        public void InitToolStripUtils(int idx)
        {
            if (idx != -1)
            {
                toolStripLeft.Items.Clear();
                var menus = UserProfile.MenuManagers.Utils.GetToolStripButtons(idx, TtmClick);

                toolStripScriptPanel.Items.Clear();

                foreach (var item in menus)
                {
                    if (item.Tag != null)
                    {
                        AddRightClick(item);

                        if (UserProfile.ScriptEngine.IsScriptFile(((MenuSendTo)item.Tag).Path))
                        {

                            toolStripScriptPanel.Items.Add(item);

                        }
                        else
                        {
                            toolStripLeft.Items.Add(item);
                        }
                    }
                }
            }
            toolStripLeft.Visible = toolStripLeft.Items.Count > 0;
            toolStripScriptPanel.Visible = toolStripScriptPanel.Items.Count > 0;
        }

        /// <summary>
        /// по кліку правою кнопкою миші, відкривати папку з утилітою
        /// </summary>
        /// <param name="obj"></param>
        private static void AddRightClick(ToolStripItem obj)
        {
            obj.MouseDown += delegate (object sender, MouseEventArgs args)
            {
                if (args.Button == MouseButtons.Right)
                {
                    var menuSendTo = (MenuSendTo)((ToolStripItem)sender).Tag;
                    try
                    {
                        var pi = new ProcessStartInfo
                        {
                            WorkingDirectory =  Path.GetDirectoryName(menuSendTo.Path) ?? throw new InvalidOperationException(),
                            FileName = Path.GetDirectoryName(menuSendTo.Path),
                        };
                        Process.Start(pi);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }

                }
            };
        }

        private void TtmClick(object sender, EventArgs eventArgs)
        {
            var menuSendTo = (MenuSendTo)((ToolStripItem)sender).Tag;

            if (string.IsNullOrEmpty(menuSendTo.CommandLine)) { OpenFileForEdit(menuSendTo); return; }

            if (UserProfile.ScriptEngine.IsScriptFile(menuSendTo.Path))
            {
                ProcessScript(menuSendTo);
            }
            else
            {
                ProcessAppFiles(menuSendTo);
            }
        }

        private void ProcessAppFiles(MenuSendTo menuSendTo)
        {
            if (menuSendTo.CommandLine.Contains("{1}") && objectListView1.SelectedObjects.Count == 0) // папка
            {
                ProcessAppFolder(menuSendTo);

            }
            else if (objectListView1.SelectedObjects.Count > 0)
            {
                var fileList = objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().ToArray();

                foreach (var info in fileList)
                {
                    ProcessAppFile(info, menuSendTo);
                }
            }
        }

        private void ProcessAppFolder(MenuSendTo menuSendTo)
        {
            var curJob = UserProfile.Jobs?.CurrentJob;

            var args = string.Format(menuSendTo.CommandLine,
                                                string.Empty,
                                                _fileManager.Settings.CurFolder,
                                                string.Empty,
                                                curJob?.Number,
                                                curJob?.Customer,
                                                curJob?.Description);

            var processStartInfo = new ProcessStartInfo
            {
                WorkingDirectory = Path.GetDirectoryName(menuSendTo.Path),
                FileName = menuSendTo.Path,
                Arguments = args,
                
            };
            var p = Process.Start(processStartInfo);
            Log.Info(UserProfile, "Utils", $"process: {menuSendTo.Path} cmd: {processStartInfo.Arguments}");

            if (!menuSendTo.EventOnFinish) return;
            p?.WaitForExit();
        }

        private void ProcessAppFile(IFileSystemInfoExt info, MenuSendTo menuSendTo)
        {
            var curJob = UserProfile.Jobs?.CurrentJob;

            var args = string.Format(menuSendTo.CommandLine,
                        info.FileInfo.FullName,
                        Path.GetDirectoryName(info.FileInfo.FullName),
                        Path.GetFileNameWithoutExtension(info.FileInfo.FullName),
                        curJob?.Number,
                        curJob?.Customer,
                        curJob?.Description);
            var pii = new ProcessStartInfo
            {
                WorkingDirectory = Path.GetDirectoryName(menuSendTo.Path),
                FileName = menuSendTo.Path,
                Arguments = args,
            };

            var p = Process.Start(pii);
            Log.Info(UserProfile, "Utils", $"process: {menuSendTo.Path} cmd: {pii.Arguments}");

            if (menuSendTo.EventOnFinish)
            {
                p?.WaitForExit();
                if (menuSendTo.StatusCode != 0)
                {
                    var number = info.FileInfo.Name.Split('_').First();
                    UserProfile.Jobs.ChangeStatusCode(number, menuSendTo.StatusCode);
                }
            }
        }

        private void ProcessScript(MenuSendTo menuSendTo)
        {
            if (menuSendTo.CommandLine.Contains("{1}")) // папка
            {
                ProcessScriptFolder(menuSendTo);

            }
            else if (objectListView1.SelectedObjects.Count > 0)
            {
                var fileList = objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().ToArray();

                BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask($"run script {menuSendTo.Name}", new Action(
                () =>
                {
                    foreach (var info in fileList)
                    {
                        ProcessScriptFile(info, menuSendTo);
                    }
                }
                )));
            }
        }

        private void ProcessScriptFile(IFileSystemInfoExt info, MenuSendTo menuSendTo)
        {
            var curJob = UserProfile.Jobs?.CurrentJob;
            var args = string.Format(menuSendTo.CommandLine,
                        info.FileInfo.FullName,
                        Path.GetDirectoryName(info.FileInfo.FullName),
                        Path.GetFileNameWithoutExtension(info.FileInfo.FullName),
                        curJob?.Number,
                        curJob?.Customer,
                        curJob?.Description);

            var param = CreateScriptRunParameters(args, menuSendTo);

            UserProfile.ScriptEngine.FileBrowser.RunScript(param);

            if (menuSendTo.StatusCode != 0)
            {
                var number = info.FileInfo.Name.Split('_').First();
                UserProfile.Jobs.ChangeStatusCode(number, menuSendTo.StatusCode);
            }
        }

        private void ProcessScriptFolder(MenuSendTo menuSendTo)
        {
            var curJob = UserProfile.Jobs?.CurrentJob;
            var args = string.Format(menuSendTo.CommandLine,
                                                string.Empty,
                                                _fileManager.Settings.CurFolder,
                                                string.Empty,
                                                curJob?.Number,
                                                curJob?.Customer,
                                                curJob?.Description);


            var param = CreateScriptRunParameters(args, menuSendTo);// new ScriptRunParameters();
                                                                    //UserProfile.ScriptEngine.FileBrowser.RunScript(param);
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask($"run script '{menuSendTo.Name}'", new Action(
               () => { UserProfile.ScriptEngine.FileBrowser.RunScript(param); }
               )));
        }

        private ScriptRunParameters CreateScriptRunParameters(string args, MenuSendTo menuSendTo)
        {
            var curJob = UserProfile.Jobs?.CurrentJob;

            var param = new ScriptRunParameters();
            param.Values.Order = curJob;
            param.Values.OrderNumber = curJob?.Number;
            param.Values.Customer = curJob?.Customer;
            param.Values.Description = curJob?.Description;
            param.Values.Profile = UserProfile;
            param.ScriptPath = menuSendTo.Path;
            param.Values.FileName = args;
            param.Values.ImposFactory = new ImpositionFactory((Profile)UserProfile);
            return param;
        }

        private void OpenFileForEdit(MenuSendTo menuSendTo)
        {
            var pi = new ProcessStartInfo
            {
                WorkingDirectory = Path.GetDirectoryName(menuSendTo.Path) ?? throw new InvalidOperationException(),
                FileName = menuSendTo.Path,
            };

            if (UserProfile.ScriptEngine.IsScriptFile(menuSendTo.Path))
            {
                pi.Verb = "edit";
            }

            Process.Start(pi);
        }

        private void ObjectListView1_DoubleClick(object sender, EventArgs e)
        {

            if (objectListView1.SelectedObject != null)
            {
                _fileManager.OpenFileOrFolder((IFileSystemInfoExt)objectListView1.SelectedObject);
            }
        }

        private void ObjectListView1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data is OLVDataObject olvobj)
            {
                var downloadParam = UserProfile.Ftp.FtpExplorer.GetDownloadFileParam(olvobj.ModelObjects);
                if (downloadParam != null)
                {
                    StartDownloadFilesFromFtp(downloadParam);
                }
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null)
                    foreach (var file in files)
                    {
                        var dest = $"{_fileManager.Settings.CurFolder}\\{Path.GetFileName(file)}";

                        if (_fileManager.Settings.CurFolder.Equals(Path.GetDirectoryName(file))) // копіюємо у ту саму папку
                        {
                            continue;
                        }

                        var isFolder = Directory.Exists(file);
                        var isFile = File.Exists(file);
                        if (!isFolder && !isFile)
                            // Ignore if it doesn't exist
                            continue;

                        try
                        {
                            switch (e.Effect)
                            {
                                case DragDropEffects.Copy:

                                    if (isFile)
                                    {
                                        FileSystem.CopyFile(file, dest, UIOption.AllDialogs);
                                    }
                                    else
                                    {
                                        FileSystem.CopyDirectory(file, dest, UIOption.AllDialogs);
                                    }
                                    break;
                            }
                        }
                        catch (IOException ex)
                        {
                            MessageBox.Show(this, $@"Failed to perform the specified operation:\n\n{ex.Message}", @"File operation failed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }
            }
            else if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                var link = e.Data.GetData(DataFormats.StringFormat);
                if (link.ToString().ToLower().StartsWith("http"))
                {
                    OnDropHttpLink(this, link.ToString());
                }
            }
        }

        private void StartDownloadFilesFromFtp(IDownloadFileParam list)
        {

            var downloader = new Downloader();

            var ticket = new DownloadTicket();
            ticket.DownloadFileParam = list;
            ticket.TargetDir = _fileManager.Settings.CurFolder;

            downloader.AddFile(ticket);

            DownloadService.AddToQuery(downloader);
        }

        private void ObjectListView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            //Debug.WriteLine("objectListView1_ItemDrag");
            var filePath = new StringCollection();
            foreach (IFileSystemInfoExt fsi in objectListView1.SelectedObjects)
            {
                //Debug.WriteLine("Drag "+fsi.FullName);
                filePath.Add(fsi.FileInfo.FullName);
            }
            var dataObject = new DataObject();

            dataObject.SetFileDropList(filePath);

            objectListView1.DoDragDrop(dataObject, DragDropEffects.Copy);
        }

        private void ObjectListView1_DragOver(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {

                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }


        private void УдалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteFilesAndDirectories();
        }

        private void DeleteFilesAndDirectories()
        {
            if (objectListView1.SelectedObjects != null)
            {
                if (MessageBox.Show(ConfirmDelete, Delete, MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                    DialogResult.Yes)
                {
                    _fileManager.DeleteFilesAndDirectories(objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>());
                }
            }
        }

        private void КопирвоатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyToClipboard();
        }

        void CopyToClipboard()
        {
            var filePath = new StringCollection();
            foreach (IFileSystemInfoExt fsi in objectListView1.SelectedObjects)
            {
                filePath.Add(fsi.FileInfo.FullName);
            }
            try
            {
                Clipboard.SetFileDropList(filePath);
                FileManager.CopyPaste = false;
            }
            catch { }
        }

        void CopyFileNamesToClipboard()
        {
            var filePath = new StringBuilder();
            foreach (IFileSystemInfoExt fsi in objectListView1.SelectedObjects)
            {
                filePath.AppendLine(fsi.FileInfo.Name);
            }
            try
            {
                Clipboard.SetText(filePath.ToString());

            }
            catch { }
        }


        private void ВставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PasteFromClipboard();
        }

        private void PasteFromClipboard()
        {
            var data = Clipboard.GetDataObject();
            if (data != null)
            {
                if (data.GetDataPresent(DataFormats.FileDrop))
                {
                    var files = (string[])data.GetData(DataFormats.FileDrop);

                    _fileManager.PasteFromClipboard(files);
                }
            }
        }

        private void СоздатьПапкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateDirectory();
        }

        private void CreateDirectory()
        {
            using (var ff = new FormEditFolder(UserProfile, _fileManager.CreateDirectoryInCurrentFolder))
            {
                if (ff.ShowDialog() == DialogResult.OK)
                {
                    _fileManager.CreateDirectoryInCurrentFolder(ff.textBox_Name.Text);
                }
            }
        }



        private void ПереименоватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObject == null) return;

            var old = (IFileSystemInfoExt)objectListView1.SelectedObject;

            using (var ff = new FormEditFolder(old.FileInfo.Name))
            {
                if (ff.ShowDialog() == DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(ff.textBox_Name.Text))
                    {
                        _fileManager.MoveFileOrDirectoryToCurrentFolder(old, ff.textBox_Name.Text);

                    }
                }
            }

        }




        private void OpenCurrentFolderInExplorer()
        {
            try
            {
                Process.Start(_fileManager.Settings.CurFolder);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void СкопироватьВБуферПутьToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (objectListView1.SelectedObjects.Count > 1)
            {
                string str = "";

                foreach (IFileSystemInfoExt infoExt in objectListView1.SelectedObjects)
                {
                    str += $"{infoExt.FileInfo.FullName}\r\n";
                }
                Clipboard.SetText(str);
            }
            else if (objectListView1.SelectedObjects.Count == 1)
            {
                Clipboard.SetText(((IFileSystemInfoExt)objectListView1.SelectedObject).FileInfo.FullName);
            }
            else
            {
                Clipboard.SetText(_fileManager.Settings.CurFolder);
            }

        }

        private void ОбновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            objectListView1.ClearObjects();
            objectListView1.EmptyListMsg = "Loading...";
            _fileManager.RefreshAsync();
        }

        private void ContextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            предварительныйПросмотрToolStripMenuItem.Visible = UserProfile.Settings.GetFileBrowser().UseViewer;

            отправитьВToolStripMenuItem.DropDownItems.Clear();
            отправитьВToolStripMenuItem.DropDownItems.AddRange(UserProfile.MenuManagers.SendTo.Get(SendMenuItem_ClickAsync).ToArray());

            SendEmailToolStripMenuItem.DropDownItems.Clear();
            SendEmailToolStripMenuItem.DropDownItems.AddRange(UserProfile.MailNotifier.GetMenu(ToolStripSendMenu_Click).ToArray());

            var nonPdfFiles = objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().Where(x =>
                !x.FileInfo.Extension.Equals(".pdf", StringComparison.InvariantCultureIgnoreCase));

            bool visible = !nonPdfFiles.Any();
            утилітиДляPDFToolStripMenuItem.Visible = visible;
            setTrimBoxToolStripMenuItem.Visible = visible;

            SetToMoveFolders();
        }

        private void SetToMoveFolders()
        {
            переміститиДоToolStripMenuItem.Visible = false;

            var folders = UserProfile.Settings.GetFileBrowser().FolderNamesForCreate;



            //// перевірити, чи це коренева папка

            //if (!_fileManager.Settings.IsRoot) return;

            // перевірити, чи вибрано файли

            if (objectListView1.SelectedObjects.Count == 0) return;

            // перевірити, чи нема у вибраних файлів назви папок

            var files = objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>();

            var filteredFiles = files.Where(x =>
            {
                return !folders.Contains(x.FileInfo.Name, StringComparer.OrdinalIgnoreCase);
            });

            if (!filteredFiles.Any()) return;

            // створити меню
            переміститиДоToolStripMenuItem.Visible = true;
            переміститиДоToolStripMenuItem.DropDownItems.Clear();



            foreach (var folder in folders)
            {
                переміститиДоToolStripMenuItem.DropDownItems.Add(folder, null, (sender, e) =>
                {

                    var targetDir = Path.Combine(_fileManager.Settings.RootFolder, folder);
                    if (!Directory.Exists(targetDir)) { Directory.CreateDirectory(targetDir); }

                    foreach (var file in filteredFiles)
                    {
                        var targetFile = Path.Combine(targetDir, file.FileInfo.Name);

                        _fileManager.MoveTo(file, targetFile);
                    }

                });
            }

            var localFolders = _fileManager.GetDirs();

            if (!localFolders.Any()) return;

            var lf = localFolders.Where(x =>
            {
                foreach (var folder in folders)
                {
                    if (x.FileInfo.Name.Equals(folder, StringComparison.InvariantCultureIgnoreCase)) return false;
                }
                return true;
            });


            if (!lf.Any()) return;

            переміститиДоToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());

            foreach (var localFolder in lf)
            {
                переміститиДоToolStripMenuItem.DropDownItems.Add(localFolder.FileInfo.Name, null, (sender, e) =>
                {

                    var targetDir = Path.Combine(_fileManager.Settings.RootFolder, localFolder.FileInfo.Name);
                    if (!Directory.Exists(targetDir)) { Directory.CreateDirectory(targetDir); }

                    foreach (var file in filteredFiles)
                    {
                        var targetFile = Path.Combine(targetDir, file.FileInfo.Name);

                        _fileManager.MoveTo(file, targetFile);
                    }

                });
            }





        }

        private void ToolStripSendMenu_Click(object sender, EventArgs eevnArgs)
        {
            if (objectListView1.SelectedObjects != null)
            {
                var files = objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>();

                BackgroundTaskService.AddTask(new BackgroundTaskItem()
                {
                    Name = $"send mail to {((ToolStripMenuItem)sender).Text}",
                    BackgroundAction = () =>
                    {
                        foreach (var file in files)
                        {
                            if (!file.IsDir)
                            {
                                UserProfile.MailNotifier.SendFile(((ToolStripMenuItem)sender).Text, file.FileInfo.FullName);
                            }
                        }
                    },
                });

                //ShowProgress.FormProgress.ShowProgress(() =>
                //    {
                //        //string s = string.Empty;
                //        foreach (var file in files)
                //        {
                //            if (!file.IsDir)
                //            {
                //                UserProfile.MailNotifier.SendFile(((ToolStripMenuItem)sender).Text, file.FileInfo.FullName);
                //            }
                //        }
                //    }
                //);
            }
        }

        private void ToolsStripMenuItem_Click(object sender, EventArgs eventArgs)
        {
            var path = ((MenuSendTo)((ToolStripMenuItem)sender).Tag).Path;

            var comLine = ((MenuSendTo)((ToolStripMenuItem)sender).Tag).CommandLine;

            if (objectListView1.SelectedObjects != null)
            {
                if (!string.IsNullOrEmpty(comLine))
                {
                    var fileList = objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().ToArray();
                    var curJob = UserProfile.Jobs?.CurrentJob;
                    foreach (IFileSystemInfoExt info in fileList)
                    {
                        var pii = new ProcessStartInfo
                        {
                            WorkingDirectory = Path.GetDirectoryName(path),
                            FileName = path,
                            Arguments = string.Format(comLine,
                            info.FileInfo.FullName,
                            Path.GetDirectoryName(info.FileInfo.FullName),
                            Path.GetFileNameWithoutExtension(info.FileInfo.FullName),
                            curJob?.Number,
                            curJob?.Customer,
                            curJob?.Description),
                        };

                        var p = Process.Start(pii);
                        p?.WaitForExit();
                    }
                    return;
                }
            }

            var pi = new ProcessStartInfo
            {
                WorkingDirectory = Path.GetDirectoryName(path),
                FileName = path,
            };
            Process.Start(pi);
        }

        async void SendMenuItem_ClickAsync(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects != null)
            {
                var curJob = UserProfile.Jobs?.CurrentJob;
                var fileList = objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().ToArray();

                var sendToMenu = (MenuSendTo)((ToolStripMenuItem)sender).Tag;

                if ((sendToMenu.FileName.Contains("{3}") ||
                    sendToMenu.FileName.Contains("{4}") ||
                    sendToMenu.FileName.Contains("{5}")) && curJob == null
                    )
                    return;

                foreach (var info in fileList)
                {
                    string fileName;
                    if (sendToMenu.FileName == null)
                    {
                        fileName = info.FileInfo.Name;
                    }
                    else
                    {
                        fileName = string.Format(sendToMenu.FileName, info.FileInfo.Name, curJob?.Number, curJob?.Customer);
                    }


                    var fn = Path.Combine(sendToMenu.Path, fileName);
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
        }


        private void ToolStripButton_Up_Click(object sender, EventArgs e)
        {
            objectListView1.ClearObjects();
            objectListView1.EmptyListMsg = Loading;

            _fileManager.DirectoryUp();

        }

        private void ПредварительныйПросмотрToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (objectListView1.SelectedObject is IFileSystemInfoExt f)
            {
                ShowPreview(f);
            }
        }

        private void ShowPreview(IFileSystemInfoExt f)
        {
            if (!string.IsNullOrEmpty(UserProfile.Settings.GetFileBrowser().Viewer))
            {
                if (File.Exists(UserProfile.Settings.GetFileBrowser().Viewer))
                {
                    var commandLine = string.Format(UserProfile.Settings.GetFileBrowser().ViewerCommandLine, f.FileInfo.FullName);
                    var pi = new ProcessStartInfo(UserProfile.Settings.GetFileBrowser().Viewer, commandLine);
                    Process.Start(pi);
                }
            }
        }

        /*
                private void ShowPdf(string fileName)
                {
                    _pdfPreview.ShowPdf(fileName);
                }
        */

        private void ToolStripButton1_Click(object sender, EventArgs e)
        {
            var menu = new ContextMenuStrip();

            foreach (var s in _customButtonPath)
            {
                var m = menu.Items.Add(s);

                m.Click += (o, args) =>
                {
                    _fileManager.SetRootDirectory(((ToolStripItem)o).Text);
                    UserProfile.Jobs.SetCurrentJob(null);
                    OnCustomButtonClick(this, null);

                    menu.Dispose();
                };
            }

            menu.Show(Cursor.Position);
        }

        private void ToolStripTextBox_Filter_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(toolStripTextBox_Filter.Text))
            {
                ClearFilter();
            }
            else
            {
                objectListView1.ModelFilter = TextMatchFilter.Regex(objectListView1, toolStripTextBox_Filter.Text);
            }
        }

        public void SetFilter(string filterString)
        {
            toolStripTextBox_Filter.Text = filterString;
        }


        public void SetCustomButtonPath(string[] customPath)
        {
            _customButtonPath = customPath;
            if (customPath != null && customPath.Any())
            {
                var inti = 1;
                toolStripButton_Custom.ToolTipText = customPath.Aggregate((c, n) => $"{inti++}. {c}\n{inti++}. {n}");
                toolStripButton_Custom.Enabled = true;
            }
            else
            {
                toolStripButton_Custom.Enabled = false;
            }
        }

        //private void СоздатьJDFToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    CreateJdf();
        //}

        //private void CreateJdf()
        //{
        //    var sel = objectListView1.SelectedObjects;
        //    if (sel != null)
        //    {
        //        _fileManager.CreateJdf(objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>());
        //    }

        //}

        private void ВырезатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CutToClipboard();
        }

        void CutToClipboard()
        {
            byte[] moveEffect = { 2, 0, 0, 0 };
            var dropEffect = new MemoryStream();
            dropEffect.Write(moveEffect, 0, moveEffect.Length);

            var data = new DataObject();

            var filePath = new StringCollection();
            foreach (IFileSystemInfoExt fsi in objectListView1.SelectedObjects)
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

        private void ObjectListView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                CopyToClipboard();
            }
            else
            if (e.Control && e.KeyCode == Keys.V)
            {
                PasteFromClipboard();
            }
            else if (e.Control && e.KeyCode == Keys.X)
            {
                CutToClipboard();
            }
            else if (e.KeyCode == Keys.Delete)
            {
                DeleteFilesAndDirectories();
            }
            else if (e.KeyCode == Keys.Add)
            {
                SelectFilesByExtention();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                if (objectListView1.SelectedObject != null)
                {
                    _fileManager.OpenFileOrFolder((IFileSystemInfoExt)objectListView1.SelectedObject);
                }
            }
        }

        private void SelectFilesByExtention()
        {
            if (objectListView1.SelectedObject == null) return;

            var file = (IFileSystemInfoExt)objectListView1.SelectedObject;

            var list = objectListView1.Objects.Cast<IFileSystemInfoExt>().Where(info => info.FileInfo.Extension.Equals(file.FileInfo.Extension, StringComparison.InvariantCultureIgnoreCase)).ToList();

            objectListView1.SelectObjects(list);
        }

        private void ОткрытьВПрограммеПоУмолчаниюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSelectedFiles();
        }

        private void OpenSelectedFiles()
        {
            if (objectListView1.SelectedObjects != null)
            {
                foreach (IFileSystemInfoExt selectedObject in objectListView1.SelectedObjects)
                {
                    Process.Start(selectedObject.FileInfo.FullName);
                    Thread.Sleep(700);
                }
            }
        }

        private void ToolStripButton_ClearFilter_Click(object sender, EventArgs e)
        {
            ClearFilter();
        }

        private void ClearFilter()
        {
            toolStripTextBox_Filter.Text = string.Empty;
            objectListView1.ModelFilter = null;
        }

        private void ИмяФайлаОписаниеЗаказаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var file = (IFileSystemInfoExt)objectListView1.SelectedObject;
            if (file != null)
            {
                OnChangeJobDescription(this, Path.GetFileNameWithoutExtension(file.FileInfo.Name));
            }
        }

        private void ObjectListView1_CellEditFinishing(object sender, CellEditEventArgs e)
        {
            if (e.Column == olvColumn_FileName)
            {
                var name = e.NewValue as string;
                if (e.RowObject is IFileSystemInfoExt file)
                {
                    if (!Path.GetFileNameWithoutExtension(file.FileInfo.Name).Equals(name))
                    {
                        _fileManager.MoveFileOrDirectoryToCurrentFolder(file, $"{name}{file.FileInfo.Extension}");
                    }
                }
            }
            else if (e.Column == olvColumnWidth)
            {
                //if (e.RowObject is IFileSystemInfoExt file)
                //{
                //    PdfUtils.SetTrimBoxWidth(file.FileInfo.FullName, e.NewValue);
                //}
            }
            else if (e.Column == olvColumnHeight)
            {
                //if (e.RowObject is IFileSystemInfoExt file)
                //{
                //    PdfUtils.SetTrimBoxHeight(file.FileInfo.FullName, e.NewValue);
                //}
            }


            e.Cancel = true;
        }

        private void ObjectListView1_ModelCanDrop(object sender, ModelDropEventArgs e)
        {
            if (e.TargetModel == null)
                e.Effect = DragDropEffects.Copy;
            else if (!(e.TargetModel is IFileSystemInfoExt file))
                e.Effect = DragDropEffects.None;
            else
            {
                if (file.IsDir)
                {
                    Debug.WriteLine($"{file.FileInfo.FullName} - dir");
                    e.Effect = DragDropEffects.Move;
                    e.InfoMessage = "папка";
                }
                else

                {
                    Debug.WriteLine($"{file.FileInfo.FullName} - file");
                    e.Effect = DragDropEffects.Move;
                    e.InfoMessage = "file";
                }
            }


        }

        private void ObjectListView1_CanDrop_1(object sender, OlvDropEventArgs e)
        {
            if (e.DropTargetItem == null) return;

            if (e.DropTargetItem.RowObject is IFileSystemInfoExt fi)
            {
                if (fi.IsDir)
                {
                    Debug.WriteLine($"{fi.FileInfo.FullName} - dir");
                    e.Effect = DragDropEffects.Move;
                    e.InfoMessage = "папка";
                }
                else

                {
                    Debug.WriteLine($"{fi.FileInfo.FullName} - file");
                    e.Effect = DragDropEffects.None;
                }
            }
            else
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        /// <summary>
        /// завантажити налаштування
        /// </summary>
        public void LoadSettings()
        {
            if (DefaultSettingsFolder != null && Directory.Exists(DefaultSettingsFolder))
            {
                var statePath = Path.Combine(DefaultSettingsFolder, "state.olv");

                if (File.Exists(statePath))
                {
                    var state = File.ReadAllBytes(statePath);
                    objectListView1.RestoreState(state);
                }

                var setPath = Path.Combine(DefaultSettingsFolder, "settings");
                _fileManager.LoadSettings(setPath);
            }
        }

        public IFileBrowserControlSettings GetSettings()
        {
            return _fileManager.Settings;
        }

        public void SetRootFolder(string directory)
        {
            _fileManager.SetRootDirectory(directory);
        }

        public void LockUI(bool enabled)
        {
            objectListView1.ClearObjects();
            Enabled = enabled;

        }
        /// <summary>
        /// зберегти налаштування
        /// </summary>
        public void SaveSettings()
        {
            if (!Directory.Exists(DefaultSettingsFolder)) Directory.CreateDirectory(DefaultSettingsFolder);

            if (DefaultSettingsFolder == null || !Directory.Exists(DefaultSettingsFolder)) return;

            var statePath = Path.Combine(DefaultSettingsFolder, "state.olv");
            var state = objectListView1.SaveState();
            File.WriteAllBytes(statePath, state);

            var setPath = Path.Combine(DefaultSettingsFolder, "settings");

            _fileManager.SaveSettings(setPath);
        }

        private void ToolStripSeparator9_Click(object sender, EventArgs e)
        {


        }

        private void ОтриматиДодатковуІнформаціюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetFilesInfo();
        }

        private void GetFilesInfo()
        {
            IFileSystemInfoExt[] fileSystemInfoExts;

            if (objectListView1.SelectedObjects.Count > 0)
            {
                var files = objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>();

                fileSystemInfoExts = files as IFileSystemInfoExt[] ?? files.ToArray();
            }
            else
            {
                var files = objectListView1.Objects?.Cast<IFileSystemInfoExt>();

                fileSystemInfoExts = files as IFileSystemInfoExt[] ?? files?.ToArray();
            }
            //todo: показати прогрес


            BackgroundTaskService.AddTask(new BackgroundTaskItem()
            {
                Name = "get info from files...",
                BackgroundAction = () =>
                {
                    foreach (IFileSystemInfoExt infoExt in fileSystemInfoExts)
                    {
                        infoExt.GetExtendedFileInfo();
                    }

                    objectListView1.RefreshObjects(fileSystemInfoExts.ToArray());
                },
            });
        }

        private void ПереместитьВTEMPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoveToTrash(objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().ToArray());
        }

        private void MoveToTrash(IFileSystemInfoExt[] files = null)
        {
            if (files == null || files.Length == 0) return;

            //var files = objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().ToArray();
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("move files to trash", new Action(
                () => { _fileManager.MoveFilesToTrash(files); }
                )));

            //Task.Run(() => _fileManager.MoveFilesToTrash(files));
        }

        public List<IFileSystemInfoExt> GetFilesFromDirectory(string path)
        {
            return _fileManager.GetFiles(path);
        }

        private void DeleteToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            DeleteFilesAndDirectories();
        }

        private void SetTrimBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileFormatsUtil.SetTrimBox(objectListView1.SelectedObjects);
            //objectListView1.RefreshObjects(objectListView1.SelectedObjects);
        }

        private void ObjectListView1_SelectionChanged(object sender, EventArgs e)
        {
            toolStripStatusLabelSelected.Text = GetSelectedFilesSize();
        }

        private void ОтправитьEmailToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            UserProfile.MailNotifier.SetCurJob(UserProfile.Jobs.CurrentJob);

            if (objectListView1.SelectedObjects != null)
            {
                var attach = objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>()
                    .Where(y => !y.IsDir)
                    .Select(x => x.FileInfo.FullName);

                if (attach.Any())
                {
                    UserProfile.MailNotifier.SetAttachmentsList(attach);
                    return;
                }
            }

            UserProfile.MailNotifier.SetAttachmentsList(new List<string>());

        }

        private void ObjectListView1_ModelDropped(object sender, ModelDropEventArgs e)
        {
            MessageBox.Show($"dropped: {e.SourceModels} to {e.TargetModel}");
        }

        private void ObjectListView1_FormatRow(object sender, FormatRowEventArgs e)
        {
            // виклик плагіну
            UserProfile?.Plugins?.FileBrowserFormatRow(this, e);
        }

        private void ConvertToPDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects.Count == 0) return;

            List<IFileSystemInfoExt> files = objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().ToList();

            FileFormatsUtil.ConvertToPDF(files, CreateMoveToTrashAction(files));

        }

        private Action CreateMoveToTrashAction(List<IFileSystemInfoExt> files)
        {
            Action moveToTrash = null;

            if (UserProfile.Settings.GetPdfConverterSettings().MoveOriginalsToTrash)
                moveToTrash = () => MoveToTrash(files.ToArray());

            return moveToTrash;
        }

        private void SplitPDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects.Count == 0) return;

            using (var form = new FormDividerParams())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    FileFormatsUtil.SplitPDF(objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().ToList(), form.Params);
                }
            }
        }

        private void RepeatPagesPDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects.Count > 0)
            {
                FileFormatsUtil.RepeatPages(objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().ToList());
            }
        }

        private void CombineFrontsBackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects.Count > 1)
            {
                FileFormatsUtil.MergeFrontsAndBack(objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().ToList());
            }
        }

        private void ReversePagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects.Count > 0)
            {
                FileFormatsUtil.ReversePages(objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().ToList());
            }
        }

        private void RepeatDocumentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects.Count > 0)
            {
                FileFormatsUtil.RepeatDocument(objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().ToList());
            }
        }

        private void СтворитиПрямокутникToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects.Count > 0)
            {
                FileFormatsUtil.CreateRectangle(objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().ToList());
            }
        }

        private void СтворитиЕліпсToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects.Count > 0)
            {
                FileFormatsUtil.CreateEllipse(objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().ToList());
            }
        }

        private void CreateNewOrderFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (IFileSystemInfoExt ext in objectListView1.SelectedObjects)
            {
                OnCreateOrderFromFile(this, ext.FileInfo.FullName);
            }
        }

        private void toolStripSplitButtonTrash_ButtonClick(object sender, EventArgs e)
        {
            MoveToTrash(objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().ToArray());
        }

        private void openTrashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // open trash folder
            _fileManager.GetTempFolder();
        }

        private void kryptonLabelPath_LinkClicked(object sender, EventArgs e)
        {
            OpenCurrentFolderInExplorer();
        }

        private void перенестиВмістПапкиСюдиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (IFileSystemInfoExt infoExt in objectListView1.SelectedObjects)
            {
                if (infoExt.IsDir)
                {
                    _fileManager.MoveFolderContentsToHere(infoExt,false);
                }
            }
        }

        private void objectListView1_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObject is IFileSystemInfoExt file)
                UserProfile.Plugins.FileBrowserSelectObject(file);
        }

        private void витягтиСторінкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects.Count > 0)
            {
                FileFormatsUtil.ExtractPages(objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().ToList());
            }
        }

        private void toolStripButtonSettings_Click(object sender, EventArgs e)
        {
            using (var form = new FormFileBrowserSettings(_fileManager.Settings))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    SaveSettings();
                    ApplySettings();
                }
            }
        }

        private void ApplySettings()
        {
            objectListView1.ShowGroups = _fileManager.Settings.ShowGroups;
            olvColumn_DateTime.GroupKeyGetter = r =>
            {

                var file = r as FileSystemInfoExt;
                if (file.IsDir)
                {
                    return new { Title = "" };
                }

                var date = ((FileSystemInfoExt)r).FileInfo.LastWriteTime;

                return new { Title = $"{date.Year}.{date.Month:00}.{date.Day:00}" };
            };



            olvColumn_DateTime.GroupKeyToTitleConverter = key =>
            {
                return ((dynamic)key).Title;
            };

        }

        private void toolStripButtonFileInfo_Click(object sender, EventArgs e) => GetFilesInfo();
        private void toolStripButtonCopyToClipboard_Click(object sender, EventArgs e) => CopyToClipboard();
        private void toolStripButtonCut_Click(object sender, EventArgs e) => CutToClipboard();
        private void toolStripButtonPaste_Click(object sender, EventArgs e) => PasteFromClipboard();
        private void додатиТираж000ToolStripMenuItem_Click(object sender, EventArgs e) => AddTirag();

        private void AddTirag()
        {
            if (objectListView1.SelectedObjects.Count > 0)
            {

                if (objectListView1.SelectedObjects.Count > 1)
                {
                    var form = new FormEnterTirag(_fileManager, objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>(), RenameFileByTirag);
                    form.Show();

                }
                else
                {
                    using (var form = new FormTirag())
                    {
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            foreach (IFileSystemInfoExt file in objectListView1.SelectedObjects)
                            {
                                RenameFileByTirag(form.Tirag, file);
                            }
                        }
                    }

                }

            }
        }

        private void RenameFileByTirag(int tirag, IFileSystemInfoExt file)
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
            _fileManager.MoveFileOrDirectoryToCurrentFolder(file, targetFile);
        }

        private void розділитиОбкладинкуІБлокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects.Count > 0)
                FileFormatsUtil.SplitCoverAndBlock(objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().ToList());
        }

        private void створитипустишкиЗТиражамиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileFormatsUtil.CreateEmptiesWithCount(_fileManager.Settings.CurFolder);
        }

        private void розвернутиСторінкиНа90ДзеркальноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects.Count > 0)
                FileFormatsUtil.RotatePagesMirror(objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().ToList());
        }

        private void зєднатиПарніІНепарніСторінкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects.Count == 2)
            {
                PdfMergeOddAndEvenParams param = new PdfMergeOddAndEvenParams();

                var files = objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().ToList();
                if (files[0].FileInfo.Name.ToLower(CultureInfo.InvariantCulture).Contains("_even"))
                {
                    param.EvenFile = files[0].FileInfo.FullName;
                    param.OddFile = files[1].FileInfo.FullName;
                }
                else
                {
                    param.EvenFile = files[1].FileInfo.FullName;
                    param.OddFile = files[0].FileInfo.FullName;
                }
                FileFormatsUtil.MergeOddAndEven(param);
            }
            else
            {
                MessageBox.Show("Файлів має бути два! В одному непарні сторінки, а в іншому - парні", "Альо!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void розділитиНаПарніІНепарніСторінкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects.Count > 0)
            {
                FileFormatsUtil.SplitOddAndEven(objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().ToList());
            }
        }

        private void зберегтиЯкJpgToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects.Count > 0)
            {
                using (var form = new FormSelectDpi())
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        FileFormatsUtil.PdfToJpg(
                            objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().ToList(),
                            new Static.Pdf.ToJpg.PdfToJpgParams { Dpi = form.Dpi, Quality = form.Quality });
                    }
                }
            }
        }

        private void toolStripButton_NewFolder_DropDownOpening(object sender, EventArgs e)
        {
            // get from settings folder's names
            var folders = UserProfile.Settings.GetFileBrowser().FolderNamesForCreate;

            if (folders == null) return;

            toolStripButton_NewFolder.DropDownItems.Clear();

            foreach (var folder in folders)
            {
                string f = folder;
                var item = new ToolStripMenuItem(f);
                item.Click += (object s, EventArgs ea) =>
                {
                    _fileManager.CreateDirectoryInCurrentFolder(f);
                };
                toolStripButton_NewFolder.DropDownItems.Add(item);
            }

        }

        private void показатиВсіФайлибезПапокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _fileManager.Settings.ShowAllFilesWithoutDir = показатиВсіФайлибезПапокToolStripMenuItem.Checked;

            objectListView1.ClearObjects();
            objectListView1.EmptyListMsg = "Loading...";
            _fileManager.RefreshAsync();

        }

        private void копіюватиІмяФайлуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyFileNamesToClipboard();
        }

        private void змінитиРозмірToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects.Count == 0) return;

            using (var form = new FormSelectPdfNewSize())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    FileFormatsUtil.ScalePdf(objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().ToList(), form.Params);
                }
            }
        }

        private void розділитиРозворотиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects.Count == 0) return;

            using (var form = new FormPdfSplitterParams())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    FileFormatsUtil.SplitPdf(objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().ToList(), form.Params);
                }
            }
        }

        private void toolStripButtonCreatePdf_Click(object sender, EventArgs e)
        {
            ConvertToPDFToolStripMenuItem_Click(null, null);
        }

        private void зєднатиФайлиВОдинToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects.Count == 0) return;

            var files = objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().ToList();

            using (var form = new UserForms.PDF.FormList(files.Select(x => x.FileInfo.FullName).ToArray()))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    FileFormatsUtil.MergePdf(form.ConvertFiles, CreateMoveToTrashAction(files));
                }
            }
        }

        private void зєднатиФайлиВОдинтимчасовоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects.Count == 0) return;

            FileFormatsUtil.PdfMergeTemporary(
                new PdfMergeTemporaryParams
                {
                    Files = objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().Select(x => x.FileInfo.FullName).ToList()
                }, CreateMoveToTrashAction(objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().ToList()));
        }

        private void розділитиТимчасовоЗібранийФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects.Count == 0) return;

            FileFormatsUtil.SplitTemporary(objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().Select(x => x.FileInfo.FullName).ToList());

        }

        private void створитиМіткиДляБіговкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects.Count == 0) return;

            using (var form = new FormCreateBigovkaMarks())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    FileFormatsUtil.CreateBigovkaMarks(objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().Select(x => x.FileInfo.FullName), form.BigovkaMarksParams);
                }
            }

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            AddTirag();
        }

        private void створитиПлашкуToolStripMenuItem_Click(object sender, EventArgs e)
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
                        isSpot = color.IsSpot,
                        C = color.C,
                        M = color.M,
                        Y = color.Y,
                        K = color.K,
                        Name = color.Name,
                        Lab = color.Lab
                    }, _fileManager.Settings.CurFolder);

                }
            }
        }

        private void спускПолосToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowImposDialog();
        }

        void ShowImposDialog()
        {

            if (objectListView1.SelectedObjects.Count == 0) return;
            var curJob = UserProfile.Jobs?.CurrentJob;
            if (curJob != null)
            {
                TextVariablesService.SetValue(ValueList.OrderNo, curJob.Number);
                TextVariablesService.SetValue(ValueList.Customer, curJob.Customer);
                TextVariablesService.SetValue(ValueList.OrderDesc, curJob.Description);
            }

            var selectedFiles = objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().Select(x => x.FileInfo.FullName).ToList();

            var param = new ImposInputParam
            {
                Files = selectedFiles,
               JobFolder = _fileManager.Settings.CurFolder,
               UserProfile = UserProfile,
            };


            var form = new FormPdfImposition((Profile)UserProfile, param);
            form.Show();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            ShowImposDialog();
        }


        private void toolStripButtonNumericFiles_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects.Count == 0) return;
            FileFormatsUtil.NumericFiles(objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().Select(x => x.FileInfo.FullName));
        }

        private void tsb_ExtractPages_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects.Count == 0) return;
            FileFormatsUtil.ExtractPages(objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().ToList());
        }

        private void коміюватиІмяФайлуБезРозширенняToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyFileNamesWithoutExt();
        }

        private void CopyFileNamesWithoutExt()
        {
            var filePath = new StringBuilder();
            foreach (IFileSystemInfoExt fsi in objectListView1.SelectedObjects)
            {
                filePath.AppendLine(Path.GetFileNameWithoutExtension(fsi.FileInfo.FullName));
            }

            try
            {
                Clipboard.SetText(filePath.ToString());
            }
            catch { }
        }

        private void додатиФорматСторінкиДоІменіФайлуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects.Count == 0) return;
            FileFormatsUtil.AddFormatToFileName(objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().ToList());
        }

        private void додатиКонтурВисічкиКолоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects.Count == 0) return;
            FileFormatsUtil.AddCutCircle(objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().ToList());
        }

        private void контурВисічкиПрямокутникToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects.Count == 0) return;
            FileFormatsUtil.AddCutRectangle(objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().ToList());
        }

        private void пружинаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects.Count == 0) return;

            FileFormatsUtil.VisualBlocknoteSpiral(objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().ToList());
        }

        private void перенестиВмістПапкиСюдипапкафайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (IFileSystemInfoExt infoExt in objectListView1.SelectedObjects)
            {
                if (infoExt.IsDir)
                {
                    _fileManager.MoveFolderContentsToHere(infoExt,true);
                }
            }
        }

        private void копіюватиІмяФайлуРозмірToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var filePath = new StringBuilder();
            foreach (IFileSystemInfoExt fsi in objectListView1.SelectedObjects)
            {
                filePath.AppendLine($"{Path.GetFileNameWithoutExtension(fsi.FileInfo.FullName)}\t{fsi.Format.Width:0.#}x{fsi.Format.Height:0.#}");
            }

            try
            {
                Clipboard.SetText(filePath.ToString());
            }
            catch { }
        }

        private void створитиМіткиДляПідборуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects.Count == 0) return;

            using (var form = new FormCreateCollatingPageMark())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    FileFormatsUtil.CreateCollatingPageMark(objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().Select(x => x.FileInfo.FullName),form.CreatePageCollationMarksParam);
                }
            }
        }
    }
}
