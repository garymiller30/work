using BackgroundTaskServiceLib;
using BrightIdeasSoftware;
using ExtensionMethods;
using FtpClient;
using Interfaces;
using Interfaces.FileBrowser;
using Interfaces.Plugins;
using Interfaces.Profile;
using JobSpace.Menus;
using JobSpace.Models;
using JobSpace.Static;
using JobSpace.Static.Pdf.Imposition;
using JobSpace.UserForms;
using JobSpace.UserForms.PDF;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using static JobSpace.Static.NaturalSorting;

namespace JobSpace.UC
{
    public sealed partial class UCFileBrowser : UserControl, IFileBrowser
    {
        private const string LOADING = "завантаження...";
        private const string PdfToolUsageMenuItemName = "pdfToolUsageStatsToolStripMenuItem";
        private const string ObjectListViewDragSourceFormat = "JobSpace.UC.UCFileBrowser.ObjectListViewDragSource";
        private static readonly object PdfToolUsageSync = new object();
        Dictionary<string, ToolStripMenuItem> menuCache = new Dictionary<string, ToolStripMenuItem>(StringComparer.InvariantCultureIgnoreCase);
        private IUserProfile UserProfile { get; set; }

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

        public UCFileBrowser(IUserProfile profile)
        {
            UserProfile = profile;

            InitializeComponent();
            InitFileManager();
            InitListView();

            ApplySettings();
        }

        #region [PDFTool menu]

        
        ContextMenuStrip toolbarMenu = new ContextMenuStrip();
        private void AddRightContextMenuToPdfTools()
        {
            toolStripPDF.ContextMenuStrip = toolbarMenu;
            toolStripPDF.MouseUp += toolStripPDF_MouseUp;

            toolStripPDF.Parent.ContextMenuStrip = toolbarMenu;
            toolStripPDF.Parent.MouseUp += toolStripPDF_MouseUp;
        }

        private void toolStripPDF_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                BuildToolbarMenu();
                toolbarMenu.Show(toolStripPDF, e.Location);
            }
        }

        private void BuildToolbarMenu()
        {
            toolbarMenu.Items.Clear();

            var settings = _fileManager.LoadToolbarSettings();
            var allTools = _fileManager.LoadPdfTools();

            int idx = 1;

            foreach (var tool in allTools.OrderBy(o=>o.Meta.Order).ThenBy(t => t.ToolType.Name))
            {
                var item = new ToolStripMenuItem($"{idx++}. {tool.Meta.MenuPath} {tool.Meta.Name}");

                item.Checked = settings.Tools.Contains(tool.ToolType.Name);
                if (item.Checked) item.Font = new Font(item.Font, FontStyle.Bold);
                if (!string.IsNullOrEmpty(tool.Meta.Icon))
                    item.Image = GetToolIcon(tool);
                item.Tag = tool;

                item.Click += ToolbarMenu_Click;

                toolbarMenu.Items.Add(item);
            }

            if (toolbarMenu.Items.Count > 0)
                toolbarMenu.Items.Add(new ToolStripSeparator());

            var usageItem = new ToolStripMenuItem("Статистика використання PDF-утиліт...");
            usageItem.Name = PdfToolUsageMenuItemName;
            usageItem.Click += PdfToolUsageMenuItem_Click;
            toolbarMenu.Items.Add(usageItem);
        }
        
        void BuildToolbar(List<ToolInfo> tools, IToolbarSettings settings)
        {
            var panel = toolStripPDF.Parent;
            toolStripPDF.SuspendLayout();
            panel.SuspendLayout();

            toolStripPDF.Items.Clear();

            foreach (var toolName in settings.Tools)
            {
                var tool = tools.FirstOrDefault(t => t.ToolType.Name == toolName);

                if (tool == null)
                    continue;

                var button = new ToolStripButton();

                button.Tag = tool;
                button.Image = GetToolIcon(tool);
                button.Text = tool.Meta.Name;
                button.DisplayStyle = ToolStripItemDisplayStyle.Image;
                button.ToolTipText = tool.Meta.Description;

                button.Click += Tool_Click;

                toolStripPDF.Items.Add(button);
            }

            toolStripPDF.ResumeLayout(true);
            panel.ResumeLayout(true);
        }


        void ToolbarMenu_Click(object sender, EventArgs e)
        {
            var item = (ToolStripMenuItem)sender;
            var tool = (ToolInfo)item.Tag;

            var settings = _fileManager.LoadToolbarSettings();

            if (item.Checked)
                settings.Tools.Remove(tool.ToolType.Name);
            else
                settings.Tools.Add(tool.ToolType.Name);

            _fileManager.SaveToolbarSettings(settings);

            BuildToolbar(_fileManager.LoadPdfTools(), settings);
        }
        ToolStripMenuItem GetOrCreateMenu(string path)
        {
            if (string.IsNullOrEmpty(path))
                return утилітиДляPDFToolStripMenuItem;

            if (menuCache.ContainsKey(path))
                return menuCache[path];

            var parts = path.Split('/');
            ToolStripMenuItem parent = null;
            string currentPath = "";

            foreach (var part in parts)
            {
                if (currentPath == "")
                    currentPath = part;
                else
                    currentPath += "/" + part;

                if (!menuCache.TryGetValue(currentPath, out var item))
                {
                    item = new ToolStripMenuItem(part);
                    menuCache[currentPath] = item;

                    if (parent == null)
                        утилітиДляPDFToolStripMenuItem.DropDownItems.Add(item);
                    else
                        parent.DropDownItems.Add(item);
                }

                parent = item;
            }

            return parent;
        }

        void BuildUI(List<ToolInfo> tools)
        {
            foreach (var tool in tools)
            {
                var parent = GetOrCreateMenu(tool.Meta.MenuPath);

                if (tool.Meta.SeparatorBefore)
                    parent.DropDownItems.Add(new ToolStripSeparator());

                var item = new ToolStripMenuItem(tool.Meta.Name);
                item.Tag = tool;
                if (!string.IsNullOrEmpty(tool.Meta.Icon))
                {
                    item.Image = GetToolIcon(tool);
                }
                item.Click += Tool_Click;

                parent.DropDownItems.Add(item);

                if (tool.Meta.SeparatorAfter)
                    parent.DropDownItems.Add(new ToolStripSeparator());

            }
            EnsurePdfToolUsageMenuItem();
            BuildToolbar(tools, _fileManager.LoadToolbarSettings());
        }

        private void EnsurePdfToolUsageMenuItem()
        {
            if (утилітиДляPDFToolStripMenuItem.DropDownItems
                .OfType<ToolStripItem>()
                .Any(x => x.Name == PdfToolUsageMenuItemName))
            {
                return;
            }

            if (утилітиДляPDFToolStripMenuItem.DropDownItems.Count > 0)
                утилітиДляPDFToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());

            var item = new ToolStripMenuItem("Статистика використання PDF-утиліт...");
            item.Name = PdfToolUsageMenuItemName;
            item.Click += PdfToolUsageMenuItem_Click;
            утилітиДляPDFToolStripMenuItem.DropDownItems.Add(item);
        }
        string iconsPath = Path.Combine(Application.StartupPath, "db", "resources", "pdftool_icons");
        Dictionary<string, Image> iconCache = new Dictionary<string, Image>();
        Image GetToolIcon(ToolInfo tool)
        {
            string iconName = tool.Meta.Icon;

            if (string.IsNullOrEmpty(iconName))
                iconName = tool.ToolType.Name;

            if (iconCache.TryGetValue(iconName, out var img))
                return img;

            string file = Path.Combine(iconsPath, iconName + ".png");

            if (File.Exists(file))
            {
                img = Image.FromFile(file);
            }
            else
            {
                img = Image.FromFile(Path.Combine(iconsPath, "default.png"));
            }

            iconCache[iconName] = img;

            return img;
        }


        async void Tool_Click(object sender, EventArgs e)
        {
            var toolInfo = (ToolInfo)((ToolStripItem)sender).Tag;

            var tool = toolInfo.Create();

            var context = CreateContext();

            if (tool is IPdfToolAsync toolAsync)
            {
                if (!await toolAsync.ConfigureAsync(context))
                {
                    return;
                }
            }
            else
            
            if (!tool.Configure(context))
                return;

            if (toolInfo.Meta.IsBackgroundTask)
            {
                BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask(toolInfo.Meta.MenuPath, new Action(() =>
                {
                    TrackPdfToolUsage(toolInfo);
                    tool.Execute(context);
                }), context.ProcessingFiles));
            }
            else
            {
                TrackPdfToolUsage(toolInfo);
                tool.Execute(context);
            }
        }

        private void TrackPdfToolUsage(ToolInfo toolInfo)
        {
            try
            {
                lock (PdfToolUsageSync)
                {
                    var settings = UserProfile.LoadSettings<PdfToolUsageStats>() ?? new PdfToolUsageStats();
                    var toolKey = toolInfo.ToolType.FullName ?? toolInfo.ToolType.Name;

                    if (!settings.Tools.TryGetValue(toolKey, out var item) || item == null)
                    {
                        item = new PdfToolUsageItem
                        {
                            ToolType = toolKey
                        };
                    }

                    item.Name = toolInfo.Meta?.Name ?? toolInfo.ToolType.Name;
                    item.MenuPath = toolInfo.Meta?.MenuPath;
                    item.LaunchCount++;
                    item.LastStartedUtc = DateTime.UtcNow;

                    settings.Tools[toolKey] = item;
                    UserProfile.SaveSettings(settings);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Pdf tool usage tracking failed: {ex}");
            }
        }

        private PdfToolUsageStats LoadPdfToolUsageStats()
        {
            try
            {
                return UserProfile.LoadSettings<PdfToolUsageStats>() ?? new PdfToolUsageStats();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Pdf tool usage stats loading failed: {ex}");
                return new PdfToolUsageStats();
            }
        }

        private void PdfToolUsageMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FormPdfToolUsageStats(_fileManager.LoadPdfTools(), LoadPdfToolUsageStats(), GetToolIcon))
            {
                form.ShowDialog(this);
            }
        }

        public PdfJobContext CreateContext()
        {
            return new PdfJobContext
            {
                InputFiles = objectListView1.SelectedObjects.Cast<IFileSystemInfoExt>().ToList(),
                ProcessingFiles = objectListView1.SelectedObjects
                    .Cast<IFileSystemInfoExt>()
                    .Select(x => x?.FileInfo?.FullName)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct(StringComparer.InvariantCultureIgnoreCase)
                    .ToList(),
                FileManager = _fileManager,
                UserProfile = UserProfile
            };
        }

        #endregion

        private void InitListView()
        {
            var rbd = new RowBorderDecoration
            {
                BorderPen = new Pen(System.Drawing.Color.FromArgb(255, System.Drawing.Color.DarkGreen), 1),
                BoundsPadding = new Size(0, -1),
                CornerRounding = 3.0F,
            };
            objectListView1.CopySelectionOnControlC = false;
            objectListView1.SelectedRowDecoration = rbd;

            olvColumn_FileName.AspectGetter +=
                r => ((IFileSystemInfoExt)r).IsDir ? ((IFileSystemInfoExt)r).FileInfo.Name : Path.GetFileNameWithoutExtension(((IFileSystemInfoExt)r).FileInfo.Name);
            olvColumnType.AspectGetter += r => ((IFileSystemInfoExt)r).IsDir ? string.Empty : ((IFileSystemInfoExt)r).FileInfo.Extension;

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

            olvColumnCreatorApp.AspectGetter += r => ((IFileSystemInfoExt)r).CreatorApp;

            olvColumnColorSpaces.AspectGetter += r =>
            {
                var spaces = ((IFileSystemInfoExt)r).UsedColors;
                if (spaces == null || spaces.Count == 0) return string.Empty;

                var colors = spaces.ToList();
                return $"({GetTotalColorCount(colors)}) {string.Join(", ", colors)}";
            };

        }

        private static int GetTotalColorCount(IEnumerable<string> colors)
        {
            if (colors == null)
            {
                return 0;
            }

            int count = 0;

            foreach (var color in colors.Where(c => !string.IsNullOrWhiteSpace(c)))
            {
                if (IsCompactProcessColor(color))
                {
                    count += color.Count(ch => ch == 'C' || ch == 'M' || ch == 'Y' || ch == 'K');
                    continue;
                }

                count++;
            }

            return count;
        }

        private static bool IsCompactProcessColor(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            foreach (var ch in value)
            {
                if (ch != 'C' && ch != 'M' && ch != 'Y' && ch != 'K')
                {
                    return false;
                }
            }

            return true;
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
            objectListView1.EmptyListMsg = LOADING;
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
            objectListView1.EmptyListMsg = LOADING;
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
            // скопіювати список, щоб не було проблем з колекцією
            var list = new List<IFileSystemInfoExt>(e);

            foreach (var ext in list)
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

            try
            {
                // Wait for the task to finish (with timeout to avoid UI freeze)
                if (!_taskGetExtendedFileInfo.IsCompleted)
                {
                    _taskGetExtendedFileInfo.Wait(TimeSpan.FromSeconds(5));
                }
            }
            catch (AggregateException) { /* log if needed */ }
            finally
            {
                _taskGetExtendedFileInfo = null;
            }
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
            {
                var workPath = UserProfile.Settings.GetJobSettings().WorkPath;
                var jobPath = _fileManager.Settings.CurFolder;

                if (string.IsNullOrEmpty(workPath))
                {
                    kryptonLabelPath.Text = "\\";
                    return;
                }

                if (!string.IsNullOrEmpty(jobPath) && jobPath.StartsWith(workPath, StringComparison.InvariantCultureIgnoreCase))
                {
                    var relPath = jobPath.Remove(0, workPath.Length);
                    if (string.IsNullOrEmpty(relPath)) relPath = "\\";
                    kryptonLabelPath.Text = relPath;
                    return;
                }
                else
                {
                    kryptonLabelPath.Text = jobPath;
                }
            }
        }
        private string GetSelectedFilesSize()
        {
            // cnt files, cnt pages, size
            var res = FileBrowserSevices.File_GetSelectedFileSize(objectListView1.SelectedObjects);
            return $"{res.Item1} ({res.Item3.GetFileSizeInString()}, {res.Item2} pp.)";
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
                    FileBrowserSevices.Process_StartFromMenuSendTo(menuSendTo);
                }
            };
        }

        private void TtmClick(object sender, EventArgs eventArgs)
        {
            var menuSendTo = (MenuSendTo)((ToolStripItem)sender).Tag;
            FileBrowserSevices.Process_AppOrScript(UserProfile, menuSendTo, _fileManager, objectListView1.SelectedObjects);
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
            if (IsDropFromThisObjectListView(e))
            {
                e.Effect = DragDropEffects.None;
                return;
            }

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
                DropFiles(e);
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
        private void DropFiles(DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files == null) return;

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
            dataObject.SetData(ObjectListViewDragSourceFormat, objectListView1);

            objectListView1.DoDragDrop(dataObject, DragDropEffects.Copy);
        }

        private void ObjectListView1_DragOver(object sender, DragEventArgs e)
        {
            if (IsDropFromThisObjectListView(e))
            {
                e.Effect = DragDropEffects.None;
                return;
            }

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

        private bool IsDropFromThisObjectListView(DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(ObjectListViewDragSourceFormat))
            {
                return false;
            }

            return ReferenceEquals(e.Data.GetData(ObjectListViewDragSourceFormat), objectListView1);
        }

        private void DeleteFilesAndDirectories()
        {
            FileBrowserSevices.File_DeleteFilesAndDirectories(objectListView1.SelectedObjects, _fileManager);
        }
        private void КопирвоатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileBrowserSevices.Clipboard_CopyFiles(objectListView1.SelectedObjects);
        }
        private void ВставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileBrowserSevices.Clipboard_PasteFiles(_fileManager);
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
            objectListView1.EmptyListMsg = "LOADING...";
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

            SetToMoveFolders();
        }
        private void SetToMoveFolders()
        {
            переміститиДоToolStripMenuItem.Visible = false;

            if (objectListView1.SelectedObjects.Count == 0) return;

            var folders = UserProfile.Settings.GetFileBrowser().FolderNamesForCreate;

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
            if (objectListView1.SelectedObjects.Count == 0)
            {
                MessageBox.Show("Виберіть файли для відправки електронною поштою.");
                return;
            }

            FileBrowserSevices.Mail_SendFiles(UserProfile, objectListView1.SelectedObjects, ((ToolStripMenuItem)sender).Text);
        }
        async void SendMenuItem_ClickAsync(object sender, EventArgs e)
        {
            var sendToMenu = (MenuSendTo)((ToolStripMenuItem)sender).Tag;
            await FileBrowserSevices.File_CopyToAsync(UserProfile, sendToMenu, objectListView1.SelectedObjects);
        }
        private void ToolStripButton_Up_Click(object sender, EventArgs e)
        {
            objectListView1.ClearObjects();
            objectListView1.EmptyListMsg = LOADING;

            _fileManager.DirectoryUp();
        }
        private void PreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sc_list.Panel2Collapsed = false;
            tsb_preview.Checked = true;
            ShowFilePreview();
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
        private void ВырезатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileBrowserSevices.Clipboard_CutFiles(objectListView1.SelectedObjects);
        }
        private void ObjectListView1_KeyDown(object sender, KeyEventArgs e)
        {
            bool handled = true;

            if (e.Control && e.KeyCode == Keys.C)
            {
                FileBrowserSevices.Clipboard_CopyFiles(objectListView1.SelectedObjects);
            }
            else
            if (e.Control && e.KeyCode == Keys.V)
            {
                FileBrowserSevices.Clipboard_PasteFiles(_fileManager);
            }
            else if (e.Control && e.KeyCode == Keys.X)
            {
                FileBrowserSevices.Clipboard_CutFiles(objectListView1.SelectedObjects);
            }
            else if (e.KeyCode == Keys.Delete)
            {
                DeleteFilesAndDirectories();
            }
            else if (e.KeyCode == Keys.Add)
            {
                objectListView1.SelectObjects(FileBrowserSevices.File_SelectByExt(objectListView1.SelectedObjects, objectListView1.Objects));
            }
            else if (e.KeyCode == Keys.Enter)
            {
                if (objectListView1.SelectedObject is IFileSystemInfoExt file)
                {
                    _fileManager.OpenFileOrFolder(file);
                }
            }
            else
            {
                handled = false;
            }
            if (handled)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
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
            }
            else if (e.Column == olvColumnHeight)
            {
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

                SetObjectListViewFont();
                SetObjectListViewRowHeight();

                AddRightContextMenuToPdfTools();
                BuildUI(_fileManager.LoadPdfTools());
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
                Files = fileSystemInfoExts?
                    .Select(x => x?.FileInfo?.FullName)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct(StringComparer.InvariantCultureIgnoreCase)
                    .ToList(),
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

            if (Control.ModifierKeys == Keys.Shift)
            {
                FileBrowserSevices.File_DeleteFilesAndDirectories(files, _fileManager);
            }
            else
            {
                BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("move files to trash", new Action(
                    () => { _fileManager.MoveFilesToTrash(files); }
                    ), files.Select(x => x?.FileInfo?.FullName)));

            }
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
            
        }
        private void ObjectListView1_SelectionChanged(object sender, EventArgs e)
        {
            toolStripStatusLabelSelected.Text = GetSelectedFilesSize();
        }
        private void ОтправитьEmailToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            FileBrowserSevices.File_SendMail(objectListView1.SelectedObjects, UserProfile);
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
            
        }
        private Action CreateMoveToTrashAction(List<IFileSystemInfoExt> files)
        {
            Action moveToTrash = null;

            if (files.Count == 0) return moveToTrash;

            if (UserProfile.Settings.GetPdfConverterSettings().MoveOriginalsToTrash)
                moveToTrash = () => MoveToTrash(files.ToArray());

            return moveToTrash;
        }
        private void SplitPDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var context = CreateContext();
            var spliter = new Static.Pdf.Divide.PdfSplitPages();
            if (spliter.Configure(context))
            {
                BackgroundTaskService.AddTask(new BackgroundTaskItem()
                {
                    Name = "split pdf",
                    Files = context.ProcessingFiles,
                    BackgroundAction = () =>
                    {
                        spliter.Execute(context);
                    }
                });
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
                    _fileManager.MoveFolderContentsToHere(infoExt, false);
                }
            }
        }
        private void objectListView1_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObject is IFileSystemInfoExt file)
                UserProfile.Plugins.FileBrowserSelectObject(this, file);
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

            SetObjectListViewFont();
            SetObjectListViewRowHeight();
        }

        private void SetObjectListViewRowHeight()
        {
            if (_fileManager.Settings.RowHeight > 0)
                objectListView1.RowHeight = _fileManager.Settings.RowHeight;
        }

        private void SetObjectListViewFont()
        {
            if (_fileManager.Settings.FontSize != 0)
            {
                objectListView1.Font = new Font(
                    _fileManager.Settings.FontName,
                    _fileManager.Settings.FontSize,
                    _fileManager.Settings.FontStyle,
                    objectListView1.Font.Unit,
                    objectListView1.Font.GdiCharSet,
                    objectListView1.Font.GdiVerticalFont
                );
            }
        }

        private void toolStripButtonFileInfo_Click(object sender, EventArgs e) => GetFilesInfo();
        private void toolStripButtonCopyToClipboard_Click(object sender, EventArgs e) => FileBrowserSevices.Clipboard_CopyFiles(objectListView1.SelectedObjects);
        private void toolStripButtonCut_Click(object sender, EventArgs e) => FileBrowserSevices.Clipboard_CutFiles(objectListView1.SelectedObjects);
        private void toolStripButtonPaste_Click(object sender, EventArgs e) => FileBrowserSevices.Clipboard_PasteFiles(_fileManager);



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
            objectListView1.EmptyListMsg = LOADING;
            _fileManager.RefreshAsync();
        }
        private void копіюватиІмяФайлуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileBrowserSevices.Clipboard_CopyFileNames(objectListView1.SelectedObjects);
        }
        private void спускПолосToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileBrowserSevices.PDF_ShowImposDialog(objectListView1.SelectedObjects, new ImposInputParam
            {
                Job = UserProfile.Jobs?.CurrentJob,
                JobFolder = _fileManager.Settings.CurFolder,
                UserProfile = UserProfile,
            });
        }


        private void коміюватиІмяФайлуБезРозширенняToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileBrowserSevices.Clipboard_CopyFileNamesWithoutExtension(objectListView1.SelectedObjects);
        }

        private void перенестиВмістПапкиСюдипапкафайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileBrowserSevices.File_MoveFolderContentsToHere(objectListView1.SelectedObjects, _fileManager);
        }
        private void копіюватиІмяФайлуРозмірToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileBrowserSevices.Clipboard_CopyFileNamesWithFormat(objectListView1.SelectedObjects);
        }
        private void створитиМіткиДляПідборуToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


        private void вставитиЯкКопіюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileBrowserSevices.Clipboard_PasteLikeCopyFiles(_fileManager);
        }

        private void tsb_preview_Click(object sender, EventArgs e)
        {
            sc_list.Panel2Collapsed = !sc_list.Panel2Collapsed;
            ShowFilePreview();
        }
        private void objectListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowFilePreview();
        }
        private void ShowFilePreview()
        {
            // якщо активне cb_preview то показати превью
            if (sc_list.Panel2Collapsed == false)
            {
                if (objectListView1.SelectedObject is IFileSystemInfoExt f)
                {
                    ShowPreviewInControl(f);
                }
            }
            else
            {
                uc_PreviewBrowserFile1.ClearPreview();
            }
        }
        private void ShowPreviewInControl(IFileSystemInfoExt f)
        {
            uc_PreviewBrowserFile1.Show(f);
        }

        private void пошукзамінаТиражівToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects.Count == 0) return;
            FileBrowserSevices.File_FindReplaceTirag(objectListView1.SelectedObjects);
        }

        private void вставитиЗІменемФайлаПідКурсоромToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObject is IFileSystemInfoExt f)
            {
                FileBrowserSevices.Clipboard_PasteWithSpecificName(_fileManager, f);
            }
        }

        public void RefreshUI()
        {
            if (objectListView1.Objects != null)
            {
                var files = objectListView1.Objects.Cast<IFileSystemInfoExt>().ToArray();
                if (files.Length > 0)
                    objectListView1.UpdateObjects(files);
            }
        }
    }
}
