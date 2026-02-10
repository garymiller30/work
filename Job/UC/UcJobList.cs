using Amazon;
using BrightIdeasSoftware;
using ExtensionMethods;
using Interfaces;
using Interfaces.MQ;
using JobSpace.Ext;
using JobSpace.Menus;
using JobSpace.Models;
using JobSpace.Profiles;
using JobSpace.Static;
using JobSpace.UserForms;
using Logger;
using Ookii.Dialogs.WinForms;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static JobSpace.Static.NaturalSorting;

namespace JobSpace.UC
{
    public sealed partial class UcJobList : UserControl, IUcJobList
    {
        private readonly IUserProfile _profile;
        private readonly RichTextBox _richTextBox = new RichTextBox();

        public EventHandler<int> OnChangeCountJobs { get; set; } = delegate { };

        public UcJobList()
        {
            InitializeComponent();
            var rbd = new RowBorderDecoration
            {
                BorderPen = new Pen(System.Drawing.Color.FromArgb(255, System.Drawing.Color.DarkBlue), 1),
                BoundsPadding = new Size(0, -1),
                CornerRounding = 3.0F,
            };
            UseTheme();
            SetTheme();

            objectListView_NewWorks.SelectedRowDecoration = rbd;
        }

        private void UseTheme()
        {
            ThemeController.ThemeChanged += ThemeController_ThemeChanged;
        }

        private void ThemeController_ThemeChanged(object sender, EventArgs e)
        {
            SetTheme();

            var objects = (ICollection)objectListView_NewWorks.Objects;
            objectListView_NewWorks.ClearObjects();
            objectListView_NewWorks.AddObjects(objects);
        }

        private void SetTheme()
        {
            objectListView_NewWorks.BackColor = ThemeController.Back;
            objectListView_NewWorks.ForeColor = ThemeController.Fore;

            objectListView_NewWorks.HeaderUsesThemes = false;
            objectListView_NewWorks.HeaderFormatStyle = new HeaderFormatStyle();
            objectListView_NewWorks.HeaderFormatStyle.SetForeColor(ThemeController.HeaderFore);
            objectListView_NewWorks.HeaderFormatStyle.SetBackColor(ThemeController.HeaderBack);
        }

        public UcJobList(IUserProfile userProfile) : this()
        {
            _profile = userProfile;
            objectListView_NewWorks.Font = _profile.Settings.GetJobListSettings().UserFont;
            InitOlvColumns();
            RestoreOLVState();
            InitObjectListviewImageList();

            olvColumn_Status.Renderer = new BaseRenderer();
            olvColumn_Status.ImageGetter += ImageGetter;
            olvColumnProcess.Renderer = new BarRenderer(0, 100);

            if (_profile.MenuManagers == null || _profile.MenuManagers.IsInitialized == false)
            {
                _profile.Events.Jobs.OnToolsMenuInitialized += (s, e) =>
                {
                    InitMenus();
                };
            }
            else
            {
                InitMenus();
            }
        }

        private void InitMenus()
        {
            InitMainToolStrip();
            AddingExtendedSettings();
        }

        private void ObjectListView_NewWorks_DoubleClick(object sender, EventArgs e)
        {
            EditJob2();
        }

        private void ObjectListView_NewWorks_Click(object sender, EventArgs e)
        {
            _profile.Jobs.SetCurrentJob(objectListView_NewWorks.SelectedObject as IJob);
        }

        private void EditJob2()
        {
            if (objectListView_NewWorks.SelectedObject is IJob j)
            {
                _profile.Jobs.LockJob(j);

                _profile.Plugins.BeforeJobChange(j);

                var jobParameters = new JobParameters(j);
                using (var faw = new FormAddWork2(_profile, jobParameters, false))
                {
                    var oldPath = _profile.Jobs.GetFullPathToWorkFolder(j);

                    if (faw.ShowDialog() == DialogResult.OK)
                    {
                        _profile.Plugins.AfterJobChange(jobParameters);
                        if (_profile.Jobs.RenameJobDirectory(oldPath, jobParameters))
                        {
                            jobParameters.ApplyToJob();
                            _profile.Jobs.UpdateJob(j);
                            objectListView_NewWorks.RefreshObject(j);

                            var newPath = _profile.Jobs.GetFullPathToWorkFolder(j);
                            _profile.FileBrowser.Browsers[0].SetRootFolder(newPath);
                            _profile.Plugins.MqController.PublishChanges(MessageEnum.JobChanged, j.Id);
                        }
                    }
                    _profile.Jobs.UnlockJob(j);

                }
            }
        }

        private void objectListView_NewWorks_ModelCanDrop(object sender, ModelDropEventArgs e)
        {
            e.Effect = ModifierKeys == Keys.Control ? DragDropEffects.Move : DragDropEffects.Copy;
        }


        private void ObjectListView_NewWorks_CanDrop(object sender, OlvDropEventArgs e)
        {
            e.Effect = ModifierKeys == Keys.Control ? DragDropEffects.Move : DragDropEffects.Copy;
        }

        private void ObjectListView_NewWorks_CellEditFinishing(object sender, CellEditEventArgs e)
        {
            if (e.Column == olvColumn_OrderNumber)
            {
                var str = e.NewValue.ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    var job = e.RowObject as IJob;
                    _profile.Jobs.ChangeJobNumber(job, str);
                }
                e.Cancel = true;
            }
        }

        private void RestoreOLVState()
        {
            if (_profile.Settings.OLVState != null)
            {
                var state = Convert.FromBase64String(_profile.Settings.OLVState);
                objectListView_NewWorks.RestoreState(state);
            }
        }

        private void ToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            var menuSendTo = (MenuSendTo)((ToolStripItem)sender).Tag;

            if (objectListView_NewWorks.SelectedObjects.Count == 0 || string.IsNullOrEmpty(menuSendTo.CommandLine))
            {
                Process.Start(menuSendTo.Path);
            }

            var jobs = objectListView_NewWorks.SelectedObjects.Cast<IJob>().ToArray();

            foreach (var job in jobs)
            {
                if (menuSendTo.CommandLine.Contains("{1}")) //folder
                {
                    ProcessFolder(job, menuSendTo);
                }
            }
            return;
        }

        private void ProcessFolder(IJob job, MenuSendTo menuSendTo)
        {
            if (_profile.ScriptEngine.IsScriptFile(menuSendTo.Path))
            {
                var scriptParam = _profile.ScriptEngine.JobList.PrepareScriptToStart(job, menuSendTo);
                _profile.ScriptEngine.JobList.RunScript(scriptParam);
            }
            else
            {
                var pii = new ProcessStartInfo
                {
                    WorkingDirectory = Path.GetDirectoryName(menuSendTo.Path),
                    FileName = menuSendTo.Path,
                    Arguments = _profile.ScriptEngine.JobList.PrepareCommandlineArguments(job, menuSendTo),
                };

                var p = Process.Start(pii);
                if (menuSendTo.EventOnFinish)
                {
                    p?.WaitForExit();
                }
            }
        }

        private void InitMainToolStrip()
        {
            toolStripWorks.Items.Clear();
            toolStripMainScriptPanel.Items.Clear();

            var menus = _profile.MenuManagers.Utils.GetToolStripButtons(0, ToolsStripMenuItem_Click, true);

            foreach (var menu in menus)
            {
                if (menu is ToolStripButton || menu is ToolStripSplitButton)
                {
                    if (menu.Tag is MenuSendTo menuSendTo)
                    {
                        if (menuSendTo.UsedInMainWindow && menuSendTo.Enable)
                        {
                            if (_profile.ScriptEngine.IsScriptFile(menuSendTo.Path))
                            {
                                toolStripMainScriptPanel.Items.Add(menu);
                            }
                            else
                            {
                                toolStripWorks.Items.Add(menu);
                            }
                        }
                    }
                }
            }

            toolStripWorks.Visible = toolStripWorks.Items.Count > 0;
            toolStripMainScriptPanel.Visible = toolStripMainScriptPanel.Items.Count > 0;
        }


        private void InitOlvColumns()
        {
            // без цього не малює прогрес бар
            objectListView_NewWorks.OwnerDraw = true;

            olvColumn_Date.AspectGetter += x => ((IJob)x).Date.ToLocalTime();
            olvColumnCategories.AspectGetter += r => _profile.Categories.GetCategoryById(((IJob)r).CategoryId)?.Name;
            olvColumnNote.AspectGetter += AspectGetterNote;


            olvColumn_Status.AspectGetter = r => _profile.StatusManager.GetJobStatusDescriptionByCode(((IJob)r).StatusCode);
            olvColumn_Status.GroupKeyGetter = delegate (object r) { return ((IJob)r).StatusCode; };
            olvColumn_Status.GroupKeyToTitleConverter = delegate (object r) { return _profile.StatusManager.GetJobStatusDescriptionByCode((int)r).ToString(); };

            olvColumn_Date.GroupKeyGetter += r => ((IJob)r).Date.ToString("yyyy-MM-dd");
            olvColumn_Date.GroupKeyToTitleConverter += key => key.ToString();

            olvColumn_Customer.GroupKeyGetter = r => ((Job)r).Customer;
            olvColumn_Customer.GroupKeyToTitleConverter = r => r.ToString();

            olvColumnCategories.GroupKeyGetter = r => _profile.Categories.GetCategoryById(((IJob)r).CategoryId)?.Name ?? "";
            olvColumnCategories.GroupKeyToTitleConverter = r => r.ToString();

            objectListView_NewWorks.CustomSorter = delegate (OLVColumn column, SortOrder order)
            {
                objectListView_NewWorks.PrimarySortColumn = column;
                objectListView_NewWorks.SecondarySortColumn = olvColumn_Date;

                if (column == olvColumn_Date)
                {
                    objectListView_NewWorks.ListViewItemSorter = new OrderDateComparer(order);

                }
                else if (column == olvColumn_Customer)
                {
                    objectListView_NewWorks.ListViewItemSorter = new OrderCustomerComparer(order);

                }
                else if (column == olvColumnCategories)
                {
                    objectListView_NewWorks.ListViewItemSorter = new OrderCategoryComparer(order);
                }
            };

            objectListView_NewWorks.RebuildColumns();
        }
        private object AspectGetterNote(object job)
        {
            var note = ((IJob)job).Note;

            if (!string.IsNullOrEmpty(note) && note.TrimStart().StartsWith(@"{\rtf1", StringComparison.Ordinal))
            {
                _richTextBox.Rtf = note;
                return _richTextBox.Text;
            }
            return note;
        }


        private void InitObjectListviewImageList()
        {
            objectListView_NewWorks.SmallImageList = _profile.StatusManager.GetImageList();
        }
        private object ImageGetter(object job)
        {
            int i = _profile.StatusManager.GetIndexImageByStatusCode(((IJob)job).StatusCode);
            return i;
        }

        void _onChangeCountJobs()
        {
            OnChangeCountJobs(this, objectListView_NewWorks.Items.Count);
        }


        #region JobEvents
        public void CreateEvents()
        {
            _profile.Events.Jobs.OnJobAdd += JobEventsOnJobAdd;
            _profile.Events.Jobs.OnJobsAdd += JobEventsOnJobsAdd;
            _profile.Events.Jobs.OnJobBeginEdit += JobEventsOnJobBeginEdit;
            _profile.Events.Jobs.OnJobChange += JobEventsOnJobChange;
            _profile.Events.Jobs.OnJobFinishEdit += JobEventsOnJobFinishEdit;
            _profile.Events.Jobs.OnJobDelete += JobEventsOnJobDelete;

            _profile.Events.Jobs.Init(_profile);
        }
        private void JobEventsOnJobAdd(object sender, IJob job)
        {
            this.InvokeIfNeeded(() =>
            {
                objectListView_NewWorks.AddObject(job);
                _onChangeCountJobs();
            });
        }
        private void JobEventsOnJobsAdd(object sender, ICollection jobs)
        {
            this.InvokeIfNeeded(() =>
            {
                objectListView_NewWorks.SetObjects(jobs);
                _onChangeCountJobs();
            });
        }
        private void JobEventsOnJobBeginEdit(object sender, IJob job)
        {
            this.InvokeIfNeeded(() => objectListView_NewWorks.DisableObject(job));
        }
        private void JobEventsOnJobChange(object sender, IJob job)
        {
            this.InvokeIfNeeded(() =>
            {

                var j = objectListView_NewWorks.Objects.Cast<IJob>().FirstOrDefault(x => x.Id.Equals(job.Id));
                if (j != null)
                {
                    //якщо змінився шлях і робота зараз вибрана
                    var isChangeFolder = _profile.Jobs.GetFullPathToWorkFolder(j)
                        .Equals(_profile.Jobs.GetFullPathToWorkFolder(job)) &&
                                         Equals(objectListView_NewWorks.SelectedObject, j);
                    j.Update(job);
                    objectListView_NewWorks.RefreshObject(j);

                    if (isChangeFolder)
                    {
                        _profile.Jobs.SetCurrentJob(j);
                    }

                    ApplyViewFilter(j);
                }
                else
                {
                    if (_profile.StatusManager.IsVisible(job.StatusCode))
                        objectListView_NewWorks.AddObject(job);
                }

                _onChangeCountJobs();
            });
        }
        private void JobEventsOnJobDelete(object sender, IJob job)
        {
            this.InvokeIfNeeded(() =>
            {
                objectListView_NewWorks.RemoveObject(job);
                _onChangeCountJobs();
            });
        }

        private void JobEventsOnJobFinishEdit(object sender, IJob job)
        {
            this.InvokeIfNeeded(() =>
            {
                objectListView_NewWorks.EnableObject(job);
            });
        }

        #endregion

        public string GetSelectedJobPath()
        {
            if (objectListView_NewWorks.SelectedObject is IJob j)
            {
                return _profile.Jobs.GetFullPathToWorkFolder(j);
            }
            return null;
        }

        public void ChangeSelectedJobsStatus(IJobStatus status)
        {
            if (objectListView_NewWorks.SelectedObjects.Count > 0)
            {
                foreach (IJob job in objectListView_NewWorks.SelectedObjects)
                {
                    _profile.Plugins.BeforeJobChange(job);

                    job.StatusCode = status.Code;
                    _profile.Jobs.UpdateJob(job);

                    _profile.Plugins.AfterJobChange(job);
                    _profile.CustomersNotifyManager.Notify(job);
                    _profile.StatusManager.OnChangeStatusesParams.Run(job);

                    _profile.Plugins.MqController.PublishChanges(Interfaces.MQ.MessageEnum.JobChanged, job.Id);
                }
                objectListView_NewWorks.RefreshObjects(objectListView_NewWorks.SelectedObjects);
            }
        }

        public void CreateJobFromFile(string file)
        {
            if (objectListView_NewWorks.SelectedObject is IJob j)
            {
                Factory.CreateJobFromFile(_profile, j, file);
            }
        }

        public IJob ChangeSelectedJobDescription(string description)
        {
            if (objectListView_NewWorks.SelectedObject is IJob job)
            {
                if (_profile.Jobs.ChangeJobDescription(job, description))
                {
                    return job;
                }
            }
            return null;
        }
        public void Close() => SaveOLVState();
        public void Search(string text)
        {
            objectListView_NewWorks.ClearObjects();
            _profile.Jobs.ApplyViewListFilterText(text);
        }

        public void RepeatSelectedJob() => RepeatJob();

        #region [ApplyViewFilter]
        //public void ApplyViewFilter()
        //{
        //    objectListView_NewWorks.ClearObjects();
        //    Profile.Jobs?.ApplyStatusViewFilter();
        //    _onChangeCountJobs();
        //}

        public void ApplyViewFilter(DateTime date)
        {
            //objectListView_NewWorks.ClearObjects();
            _profile.Jobs.ApplyViewListFilterDate(date);
        }

        public void ApplyViewFilter(IJob job)
        {
            var visibleStatuses = _profile.StatusManager.GetEnabledViewStatuses();
            if (!visibleStatuses.Contains(job.StatusCode))
            {
                objectListView_NewWorks.RemoveObject(job);
            }

        }
        #endregion

        public void ApplyJobListFilter(string filterText)
        {
            if (string.IsNullOrEmpty(filterText))
                objectListView_NewWorks.ModelFilter = null;
            else
            {
                objectListView_NewWorks.ModelFilter = TextMatchFilter.Regex(objectListView_NewWorks, filterText);
            }
        }

        private void ContextMenuStrip_NewJob_Opening(object sender, CancelEventArgs e)
        {
            if (objectListView_NewWorks.SelectedObjects.Count > 1)
            {
                toolStripSeparator3.Visible = false;
                обєднатиВОднеЗамовленняToolStripMenuItem.Visible = true;

                создатьSignaJobToolStripMenuItem.Visible = false;
                повторитьЗаказToolStripMenuItem.Visible = false;
                копироватьВБуферНомерЗаказаToolStripMenuItem.Visible = false;
                копироватьВБуферОписаниеЗаказаToolStripMenuItem.Visible = false;
                фільтрПоЗамовникуToolStripMenuItem.Visible = false;
            }
            else if (objectListView_NewWorks.SelectedObject != null)
            {
                toolStripSeparator3.Visible = true;
                обєднатиВОднеЗамовленняToolStripMenuItem.Visible = false;

                создатьSignaJobToolStripMenuItem.Visible = true;
                повторитьЗаказToolStripMenuItem.Visible = true;
                копироватьВБуферНомерЗаказаToolStripMenuItem.Visible = true;
                копироватьВБуферОписаниеЗаказаToolStripMenuItem.Visible = true;
                фільтрПоЗамовникуToolStripMenuItem.Visible = true;
                var job = objectListView_NewWorks.SelectedObject as IJob;

                var signaFiles = job.GetSignaFileNames(_profile);
                if (signaFiles.Length == 0)
                {
                    создатьSignaJobToolStripMenuItem.Text = "створити файл Prinect Signa";
                    создатьSignaJobToolStripMenuItem.Tag = null;
                }
                else
                {
                    создатьSignaJobToolStripMenuItem.Text = signaFiles[0];
                    создатьSignaJobToolStripMenuItem.Tag = signaFiles[0];
                }

            }
            else
            {
                e.Cancel = true;
            }

        }

        private void AddingExtendedSettings()
        {
            var s = _profile.Settings as ProfileSettings;
            if (s.ExtendedSettings.CanDeleteJobs)
            {
                var toolstripItem = new ToolStripMenuItem("(A) Видалити роботу");
                toolstripItem.Click += ToolstripItem_Click;
                contextMenuStrip_NewJob.Items.Add(toolstripItem);
            }
        }

        private void ToolstripItem_Click(object sender, EventArgs e)
        {
            if (objectListView_NewWorks.SelectedObjects.Count > 0)
            {
                TaskDialog td = new TaskDialog();
                td.Buttons.Add(new TaskDialogButton(ButtonType.Yes));
                td.Buttons.Add(new TaskDialogButton(ButtonType.Cancel));
                td.WindowTitle = "Видалення робіт";
                td.Content = "Видалити вибрані роботи?\nВидаляється тільки запис з бази данних, файли залишаються";
                var button = td.ShowDialog();

                if (button.ButtonType == ButtonType.Yes)
                {
                    var jobs = objectListView_NewWorks.SelectedObjects.Cast<IJob>().ToArray();
                    foreach (var job in jobs)
                    {
                        _profile.Jobs.DeleteJob(job);
                    }
                    objectListView_NewWorks.RemoveObjects(jobs);
                }
            }
        }

        private void SaveOLVState()
        {
            var state = objectListView_NewWorks.SaveState();
            _profile.Settings.OLVState = Convert.ToBase64String(state);
            ProfilesController.Save(_profile);
        }

        private void СоздатьSignaJobToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (objectListView_NewWorks.SelectedObject is IJob j)
            {
                _profile.Jobs.CreateOrOpenSignaJob(_profile.Settings.GetJobSettings().SignaJobsPath, j);
            }
        }

        private void ПовторитьЗаказToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RepeatJob();
        }

        private void RepeatJob()
        {
            if (objectListView_NewWorks.SelectedObject != null)
            {
                _profile.Jobs.Dublicate((IJob)objectListView_NewWorks.SelectedObject);
            }
        }


        private void КопироватьВБуферНомерЗаказаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView_NewWorks.SelectedObject is IJob o)
            {
                try
                {
                    Clipboard.SetText(o.Number);
                }
                catch (Exception ee)
                {
                    Log.Error(_profile, "Clipboard", ee.Message);
                }
            }
        }

        private void КопіюватиВБуферОписЗамовленняToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView_NewWorks.SelectedObject is IJob o)
            {
                try
                {
                    if (Control.ModifierKeys == Keys.Shift)
                    {
                        Clipboard.SetText(o.Description);
                    }
                    else
                    {
                        Clipboard.SetText(o.Description.Transliteration());
                    }

                }
                catch (Exception ee)
                {
                    Log.Error(_profile, "Clipboard", ee.Message);
                }

            }
        }

        private void ObjectListView_NewWorks_Dropped(object sender, OlvDropEventArgs e)
        {
            if (!(e.DropTargetItem.RowObject is IJob job)) return;

            if (e.DataObject is OLVDataObject olvobj)
            {
                var downloadParam = _profile.Ftp.FtpExplorer.GetDownloadFileParam(olvobj.ModelObjects);
                if (downloadParam != null)
                {
                    //todo: подумати над реалізацією закачки файлів з ftp через drag`n`drop
                    //StartDownloadFilesFromFtp(job, downloadParam);
                }
            }
            else if (((IDataObject)e.DataObject).GetDataPresent(DataFormats.FileDrop))
            {
                FileManager.CopyFiles((string[])((IDataObject)e.DataObject).GetData(DataFormats.FileDrop),
                    _profile.Jobs.GetFullPathToWorkFolder(job));
            }
            else if (((IDataObject)e.DataObject).GetDataPresent(DataFormats.StringFormat))
            {
                var link = ((IDataObject)e.DataObject).GetData(typeof(string));

                if (link != null)
                {
                    if (link.ToString().StartsWith("http", StringComparison.OrdinalIgnoreCase))
                    {
                        DownloadFromHttpLinkAsync(job, link.ToString());
                    }
                }
            }
        }

        public void UnlockJob(IJob job) => objectListView_NewWorks.EnableObject(job);
        public void LockJob(IJob job) => objectListView_NewWorks.DisableObject(job);

        private async void DownloadFromHttpLinkAsync(IJob job, string link)
        {
            LockJob(job);
            try
            {
                await Task.Run(() => _profile.Jobs.DownloadFromHttp(link, job)).ConfigureAwait(false);
            }
            catch (Exception)
            {
            }
            finally
            {
                UnlockJob(job);
            }
        }

        private void ОбєднатиВОднеЗамовленняToolStripMenuItem_Click(object sender, EventArgs e) =>
            _profile.Jobs.CombineOrdersInOne(objectListView_NewWorks.SelectedObjects);


        private void objectListView_NewWorks_FormatRow(object sender, FormatRowEventArgs e)
        {
            e.Item.ForeColor = ThemeController.Fore;
            e.Item.BackColor = ThemeController.Back;
            _profile.Plugins?.JobListFormatRow(e.Item);
        }


        private void копіюватиЗамавникаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView_NewWorks.SelectedObject is IJob o)
            {
                try
                {
                    if (Control.ModifierKeys == Keys.Shift)
                    {
                        Clipboard.SetText(o.Customer);
                    }
                    else
                    {
                        Clipboard.SetText(o.Customer.Transliteration());
                    }
                }
                catch (Exception ee)
                {
                    Log.Error(_profile, "Clipboard", ee.Message);
                }
            }
        }

        private void копіюватиКатегоріюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView_NewWorks.SelectedObject is IJob o)
            {
                var category = _profile.Categories.GetCategoryNameById(o.CategoryId);
                try
                {
                    if (Control.ModifierKeys == Keys.Shift)
                    {
                        Clipboard.SetText(category);
                    }
                    else
                    {
                        Clipboard.SetText(category.Transliteration());
                    }
                }
                catch (Exception ee)
                {
                    Log.Error(_profile, "Clipboard", ee.Message);
                }
            }
        }

        private void показатисховатиГрупиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            objectListView_NewWorks.ShowGroups = !objectListView_NewWorks.ShowGroups;
        }

        public void SelectJob(IJob job)
        {
            objectListView_NewWorks.DeselectAll();
            objectListView_NewWorks.SelectObject(job);
            _profile.Jobs.SetCurrentJob(job);
        }

        public void ApplyViewListFilterCustomer(string text)
        {
            _profile.Jobs.ApplyViewListFilterCustomer(text);
        }

        public void ApplyViewListFilterStatuses(int[] statuses)
        {
            _profile.Jobs.ApplyViewListFilterStatuses(statuses);
        }

        public void ApplyViewListFilterDate(DateTime date)
        {
            _profile.Jobs.ApplyViewListFilterDate(date);
        }

        public void ApplyViewListFilterText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                objectListView_NewWorks.ModelFilter = null;
            }
            else
            {
                objectListView_NewWorks.ModelFilter = TextMatchFilter.Regex(objectListView_NewWorks, text);
            }
            //_profile.Jobs.ApplyViewListFilterText(text);
        }

        public void ApplyViewFilter()
        {
            throw new NotImplementedException();
        }

        private void фільтрПоЗамовникуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView_NewWorks.SelectedObject is IJob o)
            {
                _profile.SearchManager.Search(o.Customer, "");
            }
        }

        public void ApplyJobListFontSettings()
        {
            objectListView_NewWorks.Font = _profile.Settings.GetJobListSettings().UserFont;
        }
    }
}
