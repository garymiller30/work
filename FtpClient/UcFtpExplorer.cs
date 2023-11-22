using BrightIdeasSoftware;
using ExtensionMethods;
using Interfaces;
using Interfaces.Ftp;
using Interfaces.Plugins;
using Logger;
using PythonEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FtpClient
{
    public sealed partial class UcFtpExplorer : UserControl
    {
        public IUserProfile UserProfile { get; set; }

        private readonly IFtpSettings _settings;
        //private PythonEngine.PythonEngine _pythonEngine;
        private readonly Client _client;

        private readonly Font _fileNewFont = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
        private readonly Font _fileNormFont = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular);
        private readonly Font _fileChangeFont = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Italic);
        private readonly Font _fileDelFont = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Strikeout);

        private List<IFtpFileExt> _oldFtpFiles = new List<IFtpFileExt>();

        public event EventHandler<IDownloadTicket> OnCreateNewOrder = delegate { };
        public event EventHandler<IDownloadTicket> OnCreateOrderFromFolderLikeDescription = delegate { };
        public event EventHandler<IDownloadTicket> OnAddFilesToOrder = delegate { };
        public event EventHandler<IDownloadTicket> OnCreateOrderFromFolder = delegate { };
        public event EventHandler<int> OnNewFiles = delegate { };


        //readonly WMPLib.WindowsMediaPlayer _wplayer = new WMPLib.WindowsMediaPlayer();
        private int _defCount = 2 * 60; // таймер за замовчуванням 2 хв.
        private int _curCount;
        private FtpScript _timerFtpScript;

        public string FriendlyName { get; set; } = string.Empty;

        Timer _timer;

        //private readonly string _mp3AddFile;
        readonly List<FtpScript> _scripts = new List<FtpScript>();

        ~UcFtpExplorer()
        {
            _fileNewFont.Dispose();
            _fileNormFont.Dispose();
            _fileChangeFont.Dispose();
            _fileDelFont.Dispose();
        }
        public UcFtpExplorer(IFtpSettings settings)
        {
            _client = new Client();
            _settings = settings;


            _client.CreateConnection(_settings.Server, _settings.User, _settings.Password, _settings.RootFolder, _settings.IsActive, _settings.Encode);

            InitializeComponent();

            olvColumnFileName.AspectGetter = rowObject => ((FtpFileExt)rowObject).Name;
            olvColumnFileName.ImageGetter += r => ((FtpFileExt)r).IsDir ? 0 : 1;
            olvColumnSize.AspectGetter += r => ((FtpFileExt)r).IsDir ? "<DIR>" : ((FtpFileExt)r).Size.ToString("N0");
            olvColumnDate.AspectGetter = r => ((FtpFileExt)r).LastModified;

            olvColumnStatus.ImageGetter += r => imageListStatus2.Images[(int)((FtpFileExt)r).Status];

        }

        public void LoadScripts(List<IFtpScript> scripts)
        {
            _scripts.Clear();
            var enableScripts = scripts.Where(x => x.Enable);
            _scripts.AddRange(enableScripts.Cast<FtpScript>().ToList());

            if (_scripts.Any())
            {
                toolStripContainer1.BottomToolStripPanelVisible = true;
                toolStrip2FtpScripts.Items.Clear();

                toolStripComboBoxScripts.Items.Clear();
                toolStripComboBoxScripts.Items.Add(UserProfile.Ftp.FtpScriptController.Create(null));
                toolStripComboBoxScripts.SelectedIndex = 0;

                toolStripComboBoxScripts.ComboBox.DisplayMember = "Name";

                //set tool strip
                foreach (var script in _scripts)
                {
                    var button = new ToolStripSplitButton
                    {
                        Text = Path.GetFileNameWithoutExtension(script.ScriptPath),
                        Tag = script
                    };
                    button.Click += ClickScript;

                    var menu = new ToolStripMenuItem("редагувати")
                    {
                        Tag = script
                    };
                    menu.Click += Menu_Click;
                    button.DropDownItems.Add(menu);

                    toolStrip2FtpScripts.Items.Add(button);

                    toolStripComboBoxScripts.Items.Add(script);
                }
            }
            else
            {
                toolStripContainer1.BottomToolStripPanelVisible = false;
            }

        }

        private void Menu_Click(object sender, EventArgs e)
        {

            FtpScript script = (FtpScript)((ToolStripMenuItem)sender).Tag;
            Files.ShowOpenWithDialog(script.ScriptPath);
        }

        private void ClickScript(object sender, EventArgs e)
        {
            if (sender is ToolStripItem t)
            {
                var script = (FtpScript)t.Tag;

                ProcessScript(script);
            }
        }


        private void ProcessScript(FtpScript script)
        {
            if (!script.Enable) return;


            if (script.Parameters.Contains("{0}"))
            {
                ProcessScriptOnFile(script);
            }
            else if (script.Parameters.Contains("{1}"))
            {
                ProcessScriptOnDirectory(script);
            }
            else if (script.Parameters.Contains("{2}"))
            {
                ProcessScriptOnFileList(script);
            }
            else
            {
                MessageBox.Show(
                    "Параметр пустий або неправильний. Можливі варіанти:\n {0} - файл під курсором,\n {1} - поточна папка, \n {2} - список вибраних файлів");
            }
        }

        private void ProcessScriptOnFileList(FtpScript script)
        {
            if (objectListView1.SelectedObjects.Count > 0)
            {
                FtpFileExt[] files = objectListView1.SelectedObjects.Cast<FtpFileExt>().ToArray();

                var scriptParam = new ScriptRunParameters();
                FillScriptParameters(scriptParam);
                scriptParam.Values.FileList = files;
                scriptParam.ScriptPath = script.ScriptPath;
                //_pythonEngine.SetVariable("FileList", files);
                UserProfile.ScriptEngine.Ftp.RunScript(scriptParam);
                //_pythonEngine.ExecuteFile(script.ScriptPath);
            }
            else
            {
                MessageBox.Show("Не вибрано жодного файла");
            }

        }

        private void FillScriptParameters(ScriptRunParameters scriptParam)
        {
            scriptParam.Values.FileName = null;
            scriptParam.Values.FileList = null;
            scriptParam.Values.Server = _settings.Server;
            scriptParam.Values.User = _settings.User;
            scriptParam.Values.Password = _settings.Password;
            scriptParam.Values.IsActive = _settings.IsActive;
            scriptParam.Values.Encode = _settings.Encode;
            scriptParam.Values.CurrentDirectory = _client.CurrentDirectory;
            scriptParam.Values.FtpExplorer = this;

        }

        private void ProcessScriptOnDirectory(FtpScript script)
        {
            if (objectListView1.Objects != null)
            {
                var scriptParam = new ScriptRunParameters();
                FillScriptParameters(scriptParam);
                scriptParam.Values.FileList = objectListView1.Objects.Cast<FtpFileExt>();
                scriptParam.ScriptPath = script.ScriptPath;
                UserProfile.ScriptEngine.Ftp.RunScript(scriptParam);
                //_pythonEngine.SetVariable("FileList", objectListView1.Objects.Cast<FtpFileExt>());
                //_pythonEngine.ExecuteFile(script.ScriptPath);
            }
        }

        private void ProcessScriptOnFile(FtpScript script)
        {
            if (objectListView1.SelectedObjects.Count == 0)
            {
                MessageBox.Show("Не вибрано жодного файла");
                return;
            }


            foreach (FtpFileExt file in objectListView1.SelectedObjects)
            {
                var scriptParam = new ScriptRunParameters();
                FillScriptParameters(scriptParam);
                scriptParam.Values.FileName = file;
                scriptParam.ScriptPath = script.ScriptPath;
                UserProfile.ScriptEngine.Ftp.RunScript(scriptParam);
                //_pythonEngine.SetVariable("FileName", file);
                // _pythonEngine.ExecuteFile(script.ScriptPath);
            }
        }



        private void ToolStripButtonRefresh_Click(object sender, EventArgs e)
        {
            RefreshFtpObjectsList(true);
        }

        private void RefreshFtpObjectsList(bool notify)
        {
            CheckFtp(notify);
            toolStripStatusLabelPath.Text = _client.CurrentDirectory;
            SetToolTipCountFiles();
        }

        void SetToolTipCountFiles()
        {
            toolStripStatusLabelCntFiles.Text = objectListView1.GetItemCount().ToString();
        }

        private void ObjectListView1_DoubleClick(object sender, EventArgs e)
        {
            if (!toolStripButtonAutoCheck.Checked) // заборонити зміну каталога під час автоматичної перевірки

                if (objectListView1.SelectedObject is FtpFileExt f)
                {
                    if (f.IsDir)
                    {
                        objectListView1.ClearObjects();
                        toolStripTextBoxFilter.Text = null;


                        _oldFtpFiles = _client.ChangeDirectory(f).ToList();
                        objectListView1.AddObjects(_oldFtpFiles);
                        // ftp
                        //objectListView1.AddObjects(_client.ChangeDirectory(f.File).ToFtpFileExt());
                        toolStripStatusLabelPath.Text = f.FullPath;
                        SetToolTipCountFiles();
                        OnNewFilesEvent();
                    }
                }
        }

        private void ToolStripButtonBack_Click(object sender, EventArgs e)
        {
            objectListView1.ClearObjects();

            var sel = Path.GetFileName(_client.CurrentDirectory);

            _client.DirectoryUp();

            _oldFtpFiles = _client.GetDirectoriesAndFiles().ToList();
            objectListView1.AddObjects(_oldFtpFiles);


            var first = _oldFtpFiles.FirstOrDefault(x => x.Name.ToLower().Equals(sel.ToLower()));
            objectListView1.SelectObject(first);

            toolStripStatusLabelPath.Text = _client.CurrentDirectory;
            SetToolTipCountFiles();
            OnNewFilesEvent();
        }

        private void НовыйЗаказToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DownloadFiles(false,false,GetSelectedFiles);
            DownloadFiles(new DownloadFilesSettings
            {
                DownloadLikeDescription = false,
                SeparateDownload = false,
                Files = GetSelectedFiles()
            });

        }

        List<FtpFileExt> GetSelectedFiles()
        {
            if (objectListView1.SelectedObjects.Count > 0)
            {
                return objectListView1.SelectedObjects.Cast<FtpFileExt>().ToList();
            }
            return new List<FtpFileExt>();
        }


        //private void DownloadFiles(bool downloadLikeDescription, bool separateDownload, Func<IEnumerable<IFtpFileExt>> getFiles)
        public void DownloadFiles(DownloadFilesSettings settings)
        {

            if (settings.Files.Any())
            {
                // ftp
                var list = CreateDownloadFileParam();


                foreach (var ftpFile in settings.Files)
                {
                    if (ftpFile.IsDir)
                    {
                        NewOrderFromDir(ftpFile, settings);
                    }
                    else if (settings.SeparateDownload)
                    {
                        var singleFile = CreateDownloadFileParam();
                        singleFile.OrderNumber = settings.OrderNumber;
                        singleFile.File.Add(ftpFile);
                        OnCreateNewOrder(this, new DownloadTicket()
                        {
                            Sender = this,
                            DownloadFileParam = singleFile,
                            OnDownloaded = settings.OnDownloaded
                        });
                    }
                    else
                    {
                        list.File.Add(ftpFile);
                    }
                }

                if (list.File.Any())
                {
                    list.OrderNumber = settings.OrderNumber;
                    OnCreateNewOrder(this, new DownloadTicket() { Sender = this, DownloadFileParam = list, OnDownloaded = settings.OnDownloaded });
                }

                ResetFileStatus(settings.Files);

                objectListView1.RefreshObjects(settings.Files.ToArray());
            }
        }

        private DownloadFileParam CreateDownloadFileParam()
        {
            return new DownloadFileParam
            {
                ActiveMode = _settings.IsActive,
                CodePage = _settings.Encode,
                Server = _settings.Server,
                User = _settings.User,
                Password = _settings.Password,
                RootDirectory = _client.CurrentDirectory
            };
        }

        private void ResetFileStatus(IEnumerable<IFtpFileExt> files)
        {
            if (files.Any())
            {
                files.ToList().ForEach(x => x.Status = FtpFileExtStatus.NotChanged);
            }

            OnNewFilesEvent();


        }

        private void OnNewFilesEvent()
        {
            OnNewFiles(this, _oldFtpFiles.Count(x => x.Status == FtpFileExtStatus.New));
        }

        private void NewOrderFromDir(IFtpFileExt ftpDir, DownloadFilesSettings settings)
        {

            var files = _client.GetDirectoriesAndFiles(ftpDir).ToList();

            var list = CreateDownloadFileList(files);
            list.OrderNumber = settings.OrderNumber;
            list.RootDirectory = ftpDir.FullPath;

            if (list.File.Any())
            {

                var downloadProperties = new DownloadTicket()
                {
                    Sender = this,
                    DownloadFileParam = list,
                    FtpDir = ftpDir.Name,
                    OnDownloaded = settings.OnDownloaded,
                };


                if (settings.DownloadLikeDescription)
                {
                    OnCreateOrderFromFolderLikeDescription(this, downloadProperties);
                }
                else
                {
                    OnCreateOrderFromFolder(this, downloadProperties);
                }
            }

        }



        private void ДобавитьВТекущийЗаказToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects != null)
            {
                var files = objectListView1.SelectedObjects.Cast<IFtpFileExt>();

                var list = CreateDownloadFileList(files.ToList());

                if (list.File.Any())
                    OnAddFilesToOrder(this, new DownloadTicket() { DownloadFileParam = list });//list

                ResetFileStatus(files);

                objectListView1.RefreshObjects(files.ToArray());
            }
        }

        public DownloadFileParam CreateDownloadFileList(List<IFtpFileExt> files)
        {
            return _client.CreateDownloadFileList(files);
        }

        private void ToolStripTextBoxFilter_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(toolStripTextBoxFilter.Text))
            {
                ClearFilter();
            }
            else if (toolStripTextBoxFilter.Text.Length > 2)
            {
                objectListView1.ModelFilter = TextMatchFilter.Contains(objectListView1, toolStripTextBoxFilter.Text);
            }
        }

        private void ToolStripButtonClearFilter_Click(object sender, EventArgs e)
        {
            ClearFilter();
        }

        private void ClearFilter()
        {
            toolStripTextBoxFilter.Text = String.Empty;
            objectListView1.ModelFilter = null;
        }

        private void УдалитьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects != null)
            {

                _client.DeleteFiles(objectListView1.SelectedObjects.Cast<FtpFileExt>());
                RefreshFtpObjectsList(true);
            }
        }

        private void ToolStripButtonNewFolder_Click(object sender, EventArgs e)
        {
            CreateFtpFolder();
        }

        private void CreateFtpFolder()
        {
            using (var formText = new FormEditText())
            {
                if (formText.ShowDialog() == DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(formText.EditText))
                    {
                        _oldFtpFiles.Clear();
                        objectListView1.ClearObjects();

                        _client.CreateDirectory(formText.EditText);

                        RefreshFtpObjectsList(true);
                    }
                }
            }
        }

        private void DownloaderOnUploadingProgress(object sender, IDownloadTicket e)
        {
            this.InvokeIfNeeded(() =>
            {
                toolStripProgressBar1.Value = (int)e.currentProgress;

            });
        }

        private void ClientOnOnUploadingProgress(object sender, double e)
        {
            this.InvokeIfNeeded(() =>
            {
                toolStripProgressBar1.Value = (int)e;

            });
        }


        private void ObjectListView1_FormatRow(object sender, FormatRowEventArgs e)
        {
            var o = e.Model as FtpFileExt;

            switch (o.Status)
            {
                case FtpFileExtStatus.NotChanged:
                    e.Item.Font = _fileNormFont;
                    break;
                case FtpFileExtStatus.New:
                    e.Item.Font = _fileNewFont;
                    break;
                case FtpFileExtStatus.Changed:
                    e.Item.Font = _fileChangeFont;
                    break;
                case FtpFileExtStatus.Deleted:
                    e.Item.Font = _fileDelFont;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void НовыйЗаказпапкаОписаниеЗаказаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DownloadFiles(true,false,GetSelectedFiles);
            DownloadFiles(new DownloadFilesSettings()
            {
                DownloadLikeDescription = true,
                SeparateDownload = false,
                Files = GetSelectedFiles()
            });
        }

        private void ToolStripButtonAutoCheck_CheckedChanged(object sender, EventArgs e)
        {
            StartAutoCheckFtp();
        }

        private void StartAutoCheckFtp()
        {
            var enable = !toolStripButtonAutoCheck.Checked;

            toolStripButtonRefresh.Enabled = enable;
            toolStripButtonBack.Enabled = enable;
            toolStripButtonCreateNewFolder.Enabled = enable;
            toolStripTextBoxCheckTime.Enabled = enable;
            toolStripLabelMin.Enabled = enable;
            toolStripButtonDisconnect.Enabled = enable;
            toolStripComboBoxScripts.Enabled = enable;

            if (toolStripButtonAutoCheck.Checked)
            {
                _timerFtpScript = (FtpScript)toolStripComboBoxScripts.SelectedItem;

                var res = int.TryParse(toolStripTextBoxCheckTime.Text, out _defCount);
                if (!res || (_defCount <= 0)) _defCount = 2 * 60;

                _defCount *= 60;

                _curCount = _defCount;

                _timer = new Timer { Interval = 1000 };
                _timer.Tick += TimerOnTick;

                _timer.Start();
            }
            else
            {
                _timer?.Stop();
                _timer?.Dispose();

                toolStripStatusLabelAutoCheck.Text = string.Empty;
            }
        }

        public void TimerStop()
        {
            if (toolStripButtonAutoCheck.Checked)
            {
                toolStripButtonAutoCheck.Checked = false;
                PlaySound(AvailableSound.Ftp_StopTimer, null);
            }

        }

        private void TimerOnTick(object sender, EventArgs e)
        {
            if (_curCount == 0)
            {
                toolStripStatusLabelAutoCheck.Text = "Checking...";

                if (!CheckFtp(true)) return;
                SetToolTipCountFiles();
                _curCount = _defCount;

                // запустимо скрипт
                if (_timerFtpScript.Enable)
                {
                    ProcessScript(_timerFtpScript);
                }
            }
            else
            {
                _curCount--;

                toolStripStatusLabelAutoCheck.Text = TimeSpan.FromSeconds(_curCount).ToString("g");
            }

        }

        private bool CheckFtp(bool notify)
        {
            IList<IFtpFileExt> newFileExts;
            var playSound = false;

            var files = _client.GetDirectoriesAndFiles();
            var err = _client.GetLastException();
            if (err != null)
            {
                Log.Error(this, FriendlyName, err.Message);
                //MessageBox.Show(err.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!_oldFtpFiles.Any())
            {
                objectListView1.AddObjects(files.ToArray());
                _oldFtpFiles.AddRange(files);
                newFileExts = files.ToList();
                if (files.Any()) playSound = true;
            }
            else
            {
                //видалити файли, що помічені видаленими
                var forDel = _oldFtpFiles.Where(fileExt => fileExt.Status == FtpFileExtStatus.Deleted).ToList();
                if (forDel.Any())
                {
                    foreach (var ext in forDel)
                    {
                        _oldFtpFiles.Remove(ext);
                    }

                    objectListView1.RemoveObjects(forDel);
                }

                //нові файли
                var newFiles = files.Except(_oldFtpFiles, new CompareFileName());

                newFileExts = newFiles as IList<IFtpFileExt> ?? newFiles.ToList();
                if (newFileExts.Any())
                {
                    objectListView1.AddObjects(newFileExts.ToList());

                    if (notify) playSound = true;
                }

                //видалені файли
                _oldFtpFiles.AddRange(newFiles);
                var removedFiles = _oldFtpFiles.Except(files, new CompareFileName()).ToList();

                foreach (IFtpFileExt file in removedFiles)
                {
                    if (file.Status == FtpFileExtStatus.Deleted)
                    {
                        objectListView1.RemoveObject(file);
                    }
                    else
                    {
                        file.Status = FtpFileExtStatus.Deleted;
                        objectListView1.RefreshObject(file);
                    }
                }

                //знайти файли, що змінилися
                var commonOld = _oldFtpFiles.Intersect(files, new CompareFileName()).ToList(); // знайдемо загальні файли в списках


                foreach (IFtpFileExt ext in commonOld)
                {
                    var fileNew = files.FirstOrDefault(x => x.FullPath.Equals(ext.FullPath));

                    if (fileNew != null)
                    {
                        if (!CompareForChanges.Compare(ext, fileNew))
                        {
                            ext.Refresh(fileNew);
                            ext.Status = FtpFileExtStatus.Changed;
                        }
                        else if (ext.Status == FtpFileExtStatus.Changed)
                        {
                            ext.Status = FtpFileExtStatus.New;
                            if (notify) playSound = true;
                        }
                    }


                }
                objectListView1.RefreshObjects(commonOld);
            }

            if (playSound)
            {
                PlaySound(AvailableSound.Ftp_NewFile, newFileExts);
            }
            // оновимо інфо на табі
            OnNewFilesEvent();

            return true;
        }

        private void PlaySound(AvailableSound sound, IEnumerable<IFtpFileExt> newFiles)
        {
            UserProfile.Plugins.PlaySound(sound, newFiles);
        }

        private void MarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var files = objectListView1.SelectedObjects?.Cast<IFtpFileExt>().ToList() ?? new List<IFtpFileExt>();

            ResetFileStatus(files);

            objectListView1.RefreshObjects(files);
        }

        private void ObjectListView1_CanDrop(object sender, OlvDropEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
            e.InfoMessage = string.Empty;
        }

        private async void ObjectListView1_DroppedAsync(object sender, OlvDropEventArgs e)
        {
            if (e.DataObject is IDataObject dataObject)
            {
                if (dataObject.GetDataPresent(DataFormats.FileDrop))
                {
                    var files = (string[])dataObject.GetData(DataFormats.FileDrop);
                    if (files != null)
                    {
                        foreach (var file in files)
                        {
                            //var dest = _curFolder + "\\" + Path.GetFileName(file);
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
                                            _client.OnUploadingProgress += ClientOnOnUploadingProgress;
                                            toolStripProgressBar1.Visible = true;
                                            if (e.DropTargetItem?.RowObject is FtpFileExt target && target.IsDir)
                                            {
                                                await Task.Run(() => _client.Upload(target, file)).ConfigureAwait(false);
                                            }
                                            else
                                            {
                                                await Task.Run(() => _client.Upload(file)).ConfigureAwait(false);
                                            }


                                            _client.OnUploadingProgress -= ClientOnOnUploadingProgress;
                                            toolStripProgressBar1.Value = 0;
                                            toolStripProgressBar1.Visible = false;
                                        }
                                        else
                                        {
                                            //FileSystem.CopyDirectory(file, dest, UIOption.AllDialogs);

                                        }
                                        break;
                                }
                            }
                            catch (IOException ex)
                            {
                                MessageBox.Show(this, $@"Failed to perform the specified operation:\n\n{ex.Message}", @"File operation failed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            }
                        }
                        RefreshFtpObjectsList(false);
                    }
                }
            }
            // MessageBox.Show(e.DataObject.GetType().FullName);
        }

        private void ObjectListView1_ModelCanDrop(object sender, ModelDropEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
            e.InfoMessage = "objectListView1_ModelCanDrop: CopyTo...";
        }

        private void ObjectListView1_ModelDropped(object sender, ModelDropEventArgs e)
        {
            //MessageBox.Show("Dropped");
        }

        private void ToolStripButtonDisconnect_Click(object sender, EventArgs e)
        {
            objectListView1.ClearObjects();
            _oldFtpFiles.Clear();

            OnNewFilesEvent();
        }

        private void DownloadToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DownloadTo();
        }

        private void DownloadTo()
        {

            if (objectListView1.SelectedObjects != null)
            {
                var files = objectListView1.SelectedObjects.Cast<IFtpFileExt>().ToList();
                if (files.Any())
                {
                    using (var d = new Ookii.Dialogs.WinForms.VistaFolderBrowserDialog())
                    {
                        d.ShowNewFolderButton = true;

                        if (d.ShowDialog() == DialogResult.OK)
                        {
                            var targetDir = d.SelectedPath;

                            var downloader = new Downloader();

                            var ticket = new DownloadTicket();
                            ticket.DownloadFileParam = CreateDownloadFileList(files);
                            ticket.TargetDir = targetDir;
                            downloader.AddFile(ticket);

                            //downloader.StartDownload += Downloader_StartDownload;
                            downloader.ProcessDownloading += DownloaderOnUploadingProgress;
                            downloader.FinishDownload += DownloaderOnFinishDownload;
                            downloader.StartDownloadFile += Downloader_StartDownloadFile;

                            DownloadService.AddToQuery(downloader);
                            //await Task.Factory.StartNew( ()=>downloader.Download());



                        }
                    }
                }
            }

        }

        private void DownloaderOnFinishDownload(object sender, IDownloadTicket e)
        {
            this.InvokeIfNeeded(() =>
                {
                    toolStripProgressBar1.ToolTipText = string.Empty;
                    toolStripProgressBar1.Value = 0;
                    toolStripProgressBar1.Visible = false;
                }
            );

        }

        private void Downloader_StartDownloadFile(object sender, string e)
        {
            this.InvokeIfNeeded(() =>
            {
                toolStripProgressBar1.ToolTipText = e;

            });
        }



        private void НовыйЗаказстрокаНовыйЗаказToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // DownloadFiles(true,true,GetSelectedFiles);
            DownloadFiles(new DownloadFilesSettings()
            {
                DownloadLikeDescription = true,
                SeparateDownload = true,
                Files = GetSelectedFiles()
            });
        }

        private void objectListView1_SelectionChanged(object sender, EventArgs e)
        {
            toolStripStatusLabelSelectedFiles.Text = objectListView1.SelectedObjects.Count.ToString();
        }

        private void toolStripButtonCopySelectedFileListToClipboard_Click(object sender, EventArgs e)
        {
            CopyToClipboard();
        }

        private void CopyToClipboard()
        {
            if (objectListView1.SelectedObjects.Count > 0)
            {
                var files = objectListView1.SelectedObjects.Cast<FtpFileExt>().Select(x => x.Name);

                string str = string.Empty;

                foreach (var file in files)
                {
                    str += $"{file}{Environment.NewLine}";
                }

                Clipboard.Clear();
                Clipboard.SetText(str);
            }
        }

        private void toolStripButtonStartScript_Click(object sender, EventArgs e)
        {
            StartScript();
        }

        private void StartScript()
        {
            var script = (FtpScript)toolStripComboBoxScripts.SelectedItem;
            ProcessScript(script);
        }
    }
}

