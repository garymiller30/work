// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com 


using ActiveWorks.Forms;
using BrightIdeasSoftware;
using Krypton.Toolkit;
using Interfaces;
using JobSpace;
using JobSpace.CustomForms;
using JobSpace.Menus;
using JobSpace.Profiles;
using JobSpace.Statuses;
using JobSpace.UserForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MailNotifier;
using MailNotifier.Shablons;
using Interfaces.Enums;

namespace ActiveWorks
{
    public partial class FormSettings : KryptonForm
    {

        private Profile _currentProfile;

        public FormSettings()
        {
            InitializeComponent();

            objectListViewProfiles.AddObjects(ProfilesController.GetProfiles());

            panelStatusParams.DataBindings.Add("Enabled", checkBoxStatusEnable, "Checked");
            groupBoxViewer.DataBindings.Add("Enabled", checkBoxUseViewer, "Checked");

            olvColumnUsedExplorer0.AspectGetter += r => ((MenuSendTo)r).UsedInExplorer[0];

            olvColumnUsedExplorerRight.AspectGetter += r => ((MenuSendTo)r).UsedInExplorer[1];
            olvColumnUsedExplorer2.AspectGetter += r => ((MenuSendTo)r).UsedInExplorer[2];
            olvColumnUsedExplorer3.AspectGetter += r => ((MenuSendTo)r).UsedInExplorer[3];
            olvColumnChangeStatus.AspectGetter += GetChangeStatus;

            olvColumnDeleteCategory.IsButton = true;
            olvColumnDeleteCategory.AspectGetter = s => "видалити";

            objectListViewStatuses.IsSimpleDragSource = true;
            objectListViewStatuses.DropSink = new RearrangingDropSink(true);
            
        }

        public FormSettings(Profile activeUserProfile): this()
        {
            if (activeUserProfile != null)
            {
                
                objectListViewProfiles.SelectObject(activeUserProfile);
                BindProfile();
                tabControlMain.Enabled = true;
                //_currentProfile = activeUserProfile;
            }
            else
            {
                tabControlMain.Enabled = false;
            }
        }

        private object GetChangeStatus(object r)
        {
            var status = _currentProfile.StatusManager.GetJobStatusByCode(((MenuSendTo)r).StatusCode);
            return status?.Name ?? string.Empty;
        }

        private void Button_Save_Click(object sender, EventArgs e)
        {

            if (_currentProfile != null)
            {
                ApplyProfileSetting();
            }

            ProfilesController.Save();
            Close();
        }


        /// <summary>
        /// добавить рабочую папку. В ней будут создаваться папки с заказами
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_WorkFolder_Click(object sender, EventArgs e)
        {
            if (vistaFolderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox_Work.Text = vistaFolderBrowserDialog1.SelectedPath;
            }
        }

        /// <summary>
        /// удалить формат пластины
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void УдалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBoxFolderNames.SelectedItems.Count == 0) return;

            var selIdx = new List<int>(listBoxFolderNames.SelectedIndices.Cast<int>());
            selIdx.Reverse();
            selIdx.ForEach(i =>listBoxFolderNames.Items.RemoveAt(i));
        }

        private void ДобавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            objectListViewSendTo.AddObject(_currentProfile.MenuManagers.SendTo.Add("новое меню", string.Empty));
        }

        private void УдалитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (objectListViewSendTo.SelectedObjects != null)
            {
                foreach (var menu in objectListViewSendTo.SelectedObjects)
                {
                    _currentProfile.MenuManagers.SendTo.Remove(menu);
                }
                objectListViewSendTo.RemoveObjects(objectListViewSendTo.SelectedObjects);
            }
        }

        private void ДобавитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            objectListView_Utils.AddObject(_currentProfile.MenuManagers.Utils.Add("нова програма", string.Empty, String.Empty));
        }

        private void УдалитьToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (objectListView_Utils.SelectedObjects != null)
            {
                foreach (var menu in objectListView_Utils.SelectedObjects)
                {
                    _currentProfile.MenuManagers.Utils.Remove(menu);
                }
                objectListView_Utils.RemoveObjects(objectListView_Utils.SelectedObjects);
            }
        }
        /// <summary>
        /// добавить почтовый адрес в список для отправки отчета
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_MailAdd_Click(object sender, EventArgs e)
        {
            MailAddToList();
        }
        /// <summary>
        /// добавить почтовый адрес в список для отправки отчета
        /// </summary>
        private void MailAddToList()
        {
            listBox_SendEmails.Items.Add(textBox_MailTo.Text);
        }



        private void Button_CustomButtonPath_Click(object sender, EventArgs e)
        {
            if (vistaFolderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                listBox_CustomButtonFolder.Items.Add(vistaFolderBrowserDialog1.SelectedPath);
            }
        }


        private void ВыбратьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView_Utils.SelectedObject is MenuSendTo t)
            {
                vistaOpenFileDialog1.Filter = "*.exe|*.exe";

                if (vistaOpenFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    t.Path = vistaOpenFileDialog1.FileName;
                    t.Image = System.Drawing.Icon.ExtractAssociatedIcon(t.Path)?.ToBitmap();

                    objectListView_Utils.RefreshObject(t);
                }
            }
        }

        private void ВыбратьПапкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListViewSendTo.SelectedObject is MenuSendTo t)
            {
                if (vistaFolderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    t.Path = vistaFolderBrowserDialog1.SelectedPath;
                    objectListViewSendTo.RefreshObject(t);
                }
            }
        }

        /// <summary>
        /// выбрать папку для хранения работ Signa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonOpenSignaJobsFolder_Click(object sender, EventArgs e)
        {
            if (vistaFolderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxFolderSignaJobs.Text = vistaFolderBrowserDialog1.SelectedPath;
            }
        }
        /// <summary>
        /// удалить из списка владельцев форм
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void УдалитьToolStripMenuItem4_Click(object sender, EventArgs e)
        {
        }

        private void УдалитьToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            if (listBox_CustomButtonFolder.SelectedItem != null)

                listBox_CustomButtonFolder.Items.Remove(listBox_CustomButtonFolder.SelectedItem);
        }

        private void buttonStatusSelectProgram_Click(object sender, EventArgs e)
        {

            if (objectListViewStatuses.SelectedObject is JobStatus status)
            {
                var param = _currentProfile.StatusManager.OnChangeStatusesParams.GetParam(status.Code);

                var res = vistaOpenFileDialog1.ShowDialog();
                if (res == DialogResult.OK)
                {
                    param.ProgramPath = vistaOpenFileDialog1.FileName;
                    textBoxStatusFileName.Text = vistaOpenFileDialog1.FileName;
                }
            }
        }

        private void buttonSelectViewer_Click(object sender, EventArgs e)
        {
            var res = vistaOpenFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                textBoxViewer.Text = vistaOpenFileDialog1.FileName;

            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox_SendEmails.SelectedItem != null)
            {
                listBox_SendEmails.Items.Remove(listBox_SendEmails.SelectedItem);
            }
        }

        private void buttonAddProfile_Click(object sender, EventArgs e)
        {
            var profile = ProfilesController.AddProfile();
            if (profile != null)
            {
                objectListViewProfiles.AddObject(profile);
            }
        }

        private void objectListViewProfiles_CellEditFinishing(object sender, BrightIdeasSoftware.CellEditEventArgs e)
        {
            if (e.Column == olvColumnName)
            {
                ((Profile)e.RowObject).Settings.ProfileName = e.NewValue.ToString();
                e.Cancel = true;
            }

        }

        private void objectListViewProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            // зберегти поточний профіль
            if (_currentProfile != null)
            {
                ApplyProfileSetting();
            }

            if (objectListViewProfiles.SelectedObject == null)
            {
                tabControlMain.Enabled = false;
                _currentProfile = null;
            }
            else
            {
                tabControlMain.Enabled = true;

                _currentProfile = objectListViewProfiles.SelectedObject as Profile;

                BindProfile();
            }
        }

        private void ApplyProfileSetting()
        {
            var setup = _currentProfile.Settings;

            setup.GetJobSettings().WorkPath = textBox_Work.Text;
            setup.GetJobSettings().SignaJobsPath = textBoxFolderSignaJobs.Text;
            setup.GetJobSettings().UseJobFolder = kryptonCheckBox1.Checked;
            setup.GetJobSettings().SubFolderForSignaFile = textBox_FolderForSignaFileInJob.Text;

            var baseSettings = setup.GetBaseSettings();

            baseSettings.MongoDbBaseName = textBoxBaseName.Text;
            baseSettings.MongoDbServer = textBox_mongoDB.Text;
            baseSettings.BaseTimeOut = (int)numericUpDownBaseTimeOut.Value;

            var mail = setup.GetMail();

            if (mail.MailTo == null)
            {
                mail.MailTo = new List<string>();
            }
            else
            {
                mail.MailTo.Clear();
            }
            mail.MailTo.AddRange(listBox_SendEmails.Items.Cast<string>());
            mail.MailFrom = textBox_MailFrom.Text;
            mail.MailFromPassword = textBox_MailPassword.Text;
            mail.MailImapPort = (int)numericUpDown_ImapPort.Value;
            mail.MailImapHost = textBox_ImapServer.Text;
            mail.MailSmtpPort = (int)numericUpDownSmtpPort.Value;
            mail.MailSmtpServer = textBoxSmtpServer.Text;
            mail.MailAutoRelogon = checkBoxMailAutoRelogon.Checked;

            _currentProfile.MailNotifier.SetMailTemplates(olv_mail_templates.Objects ?? new List<object>());
            mail.MailConnectType = (MailConnectTypeEnum)cb_mail_connection_type.SelectedItem;

            mail.ClientSecretFile = tb_mail_gmail_settings_secret_file.Text;

            var browser = setup.GetFileBrowser();

            browser.CustomButtonPath = new List<string>();
            browser.CustomButtonPath.AddRange(listBox_CustomButtonFolder.Items.Cast<string>().ToArray());

            browser.FolderNamesForCreate = new List<string>();
            browser.FolderNamesForCreate.AddRange(listBoxFolderNames.Items.Cast<string>().ToArray());


            setup.CountExplorers = numericUpDownCountExplorers.Value;
            //setup.ExplorerInRightPanel = checkBoxExplorerInRightPanel.Checked;

            browser.Viewer = textBoxViewer.Text;
            browser.ViewerCommandLine = textBoxViewerCommandLine.Text;
            browser.UseViewer = checkBoxUseViewer.Enabled;

            setup.GetJobSettings().SignaFileShablon = textBoxSignaShablon.Text;
            setup.GetJobSettings().StoreByYear = checkBoxStoreByYear.Checked;


            setup.HideCategory = checkBoxHideCategory.Checked;

            setup.GetPdfConverterSettings().MoveOriginalsToTrash = checkBoxMoveOriginalFileToTrash.Checked;

            _currentProfile.MenuManagers.SendTo.Save();
            _currentProfile.MenuManagers.Utils.Save();

            if (_currentProfile.StatusManager != null)
            {
                _currentProfile.StatusManager.SetJobStatuses(objectListViewStatuses.Objects?.Cast<JobStatus>() ?? new List<JobStatus>());
                _currentProfile.StatusManager.OnChangeStatusesParams.Save();
            }
            

            _currentProfile.Ftp?.FtpScriptController.SetList(objectListViewFtpScripts.Objects?.Cast<IFtpScript>() ??
                                                            new List<IFtpScript>());
            ReloadCategories();
        }

        private void BindProfile()
        {
            if (!_currentProfile.IsInitialized)
            {
                _currentProfile.InitProfile();
            }

            var setup = _currentProfile.Settings;

            textBox_Work.Text = setup.GetJobSettings().WorkPath;

            textBoxBaseName.Text = setup.GetBaseSettings().MongoDbBaseName;
            textBox_mongoDB.Text = setup.GetBaseSettings().MongoDbServer;

            objectListViewSendTo.ClearObjects();
            objectListViewSendTo.AddObjects(_currentProfile.MenuManagers.SendTo.Get());
            objectListView_Utils.ClearObjects();
            objectListView_Utils.AddObjects(_currentProfile.MenuManagers.Utils.Get());

            textBoxFolderSignaJobs.Text = setup.GetJobSettings().SignaJobsPath;
            kryptonCheckBox1.Checked = setup.GetJobSettings().UseJobFolder;
            textBox_FolderForSignaFileInJob.Text = setup.GetJobSettings().SubFolderForSignaFile;

            // mail

            cb_mail_connection_type.DataSource = Enum.GetValues(typeof(MailConnectTypeEnum));
            cb_mail_connection_type.SelectedIndex = (int)setup.GetMail().MailConnectType;

            tb_mail_gmail_settings_secret_file.Text = setup.GetMail().ClientSecretFile;

            textBox_MailFrom.Text = setup.GetMail().MailFrom;
            textBox_MailPassword.Text = setup.GetMail().MailFromPassword;
            textBox_ImapServer.Text = setup.GetMail().MailImapHost;
            numericUpDown_ImapPort.Value = setup.GetMail().MailImapPort;
            textBoxSmtpServer.Text = setup.GetMail().MailSmtpServer;
            numericUpDownSmtpPort.Value = setup.GetMail().MailSmtpPort;

            listBox_SendEmails.Items.Clear();
            if (setup.GetMail().MailTo.Any())
            {
                listBox_SendEmails.Items.AddRange(setup.GetMail().MailTo.ToArray());
            }
            olv_mail_templates.ClearObjects();
            olv_mail_templates.AddObjects(_currentProfile.MailNotifier.GetMailTemplates());


            listBox_CustomButtonFolder.Items.Clear();
            if (setup.GetFileBrowser().CustomButtonPath.Any())
            {
                listBox_CustomButtonFolder.Items.AddRange(setup.GetFileBrowser().CustomButtonPath.ToArray());
            }

            listBoxFolderNames.Items.Clear();
            listBoxFolderNames.Items.AddRange(setup.GetFileBrowser().FolderNamesForCreate?.ToArray());

            listBox_Ftp_Servers.Items.Clear();
            listBox_Ftp_Servers.DisplayMember = "Name";
            if (_currentProfile.Ftp != null)
                listBox_Ftp_Servers.Items.AddRange(_currentProfile.Ftp.GetCollection().ToArray());

            numericUpDownCountExplorers.Value = setup.CountExplorers;
            textBoxSignaShablon.Text = setup.GetJobSettings().SignaFileShablon;

            checkBoxMailAutoRelogon.Checked = setup.GetMail().MailAutoRelogon;

            numericUpDownBaseTimeOut.Value = setup.GetBaseSettings().BaseTimeOut;

            objectListViewStatuses.Objects = _currentProfile.StatusManager?.GetJobStatuses();

            checkBoxStoreByYear.Checked = setup.GetJobSettings().StoreByYear;
            checkBoxHideCategory.Checked = setup.HideCategory;

            textBoxViewer.Text = setup.GetFileBrowser().Viewer;
            textBoxViewerCommandLine.Text = setup.GetFileBrowser().ViewerCommandLine;
            checkBoxUseViewer.Checked = setup.GetFileBrowser().UseViewer;

            checkBoxMoveOriginalFileToTrash.Checked = setup.GetPdfConverterSettings().MoveOriginalsToTrash;
            //plugins
            LoadPluginsInfo();

            LoadFtpScripts();
            ReloadCategories();
        }

        private void LoadFtpScripts()
        {
            objectListViewFtpScripts.ClearObjects();
            objectListViewFtpScripts.AddObjects(_currentProfile.Ftp?.FtpScriptController.All());
        }

        private void LoadPluginsInfo()
        {
            var plugins = _currentProfile.Plugins.GetPluginsBase();

            objectListViewPlugins.ClearObjects();
            objectListViewPlugins.AddObjects(plugins);
        }

        private void buttonRemoveProfile_Click(object sender, EventArgs e)
        {
            if (objectListViewProfiles.SelectedObject != null)
            {
                ProfilesController.RemoveProfile((Profile)objectListViewProfiles.SelectedObject);
                objectListViewProfiles.RemoveObject(objectListViewProfiles.SelectedObject);
            }
        }

        private void buttonStatusAdd_Click(object sender, EventArgs e)
        {

            var ns = _currentProfile.StatusManager.Create();
            using (var f = new FormEditStatus(_currentProfile, ns))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    _currentProfile.StatusManager.Add(ns);
                    objectListViewStatuses.AddObject(ns);
                }
            }
        }

        private void buttonStatusDelete_Click(object sender, EventArgs e)
        {
            if (objectListViewStatuses.SelectedObject is JobStatus status)
            {
                _currentProfile.StatusManager.Remove(status);
                objectListViewStatuses.RemoveObject(status);
            }

        }

        private void buttonStatusEdit_Click(object sender, EventArgs e)
        {
            if (objectListViewStatuses.SelectedObject is JobStatus status)
            {
                using (var f = new FormEditStatus(_currentProfile, status))
                {
                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        _currentProfile.StatusManager.Refresh(status);
                        objectListViewStatuses.RefreshObject(status);
                    }
                }
            }
        }

        private void buttonStatusSetDefault_Click(object sender, EventArgs e)
        {
            if (objectListViewStatuses.SelectedObject is JobStatus status)
            {
                _currentProfile.StatusManager.SetDefaultStatus(status);
                objectListViewSendTo.RefreshObjects(_currentProfile.StatusManager.GetJobStatuses());
            }
        }

        private void objectListViewStatuses_Click(object sender, EventArgs e)
        {
            if (objectListViewStatuses.SelectedObject is JobStatus status)
            {
                groupBoxOnChangeStatuses.Enabled = true;
                var param = _currentProfile.StatusManager.OnChangeStatusesParams.GetParam(status.Code);

                checkBoxStatusEnable.Checked = param.Enable;
                panelStatusParams.Enabled = param.Enable;
                textBoxStatusFileName.Text = param.ProgramPath;
                textBoxStatusCommandLineParam.Text = param.CommandLineParams;
            }
            else
            {
                groupBoxOnChangeStatuses.Enabled = false;
            }

        }

        private void копіюватиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox_SendEmails.SelectedItem != null)
            {
                try
                {
                    Clipboard.SetText(listBox_SendEmails.SelectedItem.ToString());
                }
                catch
                {

                }

            }
        }

        private void objectListView_Utils_CellEditStarting(object sender, CellEditEventArgs e)
        {
            if (e.Column == olvColumnChangeStatus)
            {
                var cb = new ComboBox
                {
                    Bounds = e.CellBounds,
                    Font = ((ObjectListView)sender).Font,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    DisplayMember = "Name"
                };
                cb.Items.Add(new JobStatus { Code = 0, Name = "нічого" });
                cb.Items.AddRange(_currentProfile.StatusManager.GetJobStatuses());

                var sel = _currentProfile.StatusManager.GetJobStatusByCode(((MenuSendTo)e.RowObject).StatusCode);
                cb.SelectedItem = sel;
                e.Control = cb;

            }
        }

        private void objectListView_Utils_CellEditFinishing(object sender, CellEditEventArgs e)
        {
            if (e.Column == olvColumnChangeStatus)
            {
                if (e.Control is ComboBox cb && cb.SelectedItem is JobStatus status)
                {
                    ((MenuSendTo)e.RowObject).StatusCode = status.Code;
                }
            }
        }

        private void textBoxStatusCommandLineParam_TextChanged(object sender, EventArgs e)
        {
            if (objectListViewStatuses.SelectedObject is JobStatus status)
            {
                var param = _currentProfile.StatusManager.OnChangeStatusesParams.GetParam(status.Code);

                param.CommandLineParams = textBoxStatusCommandLineParam.Text;
            }
        }

        private void textBoxStatusFileName_TextChanged(object sender, EventArgs e)
        {
            if (objectListViewStatuses.SelectedObject is JobStatus status)
            {
                var param = _currentProfile.StatusManager.OnChangeStatusesParams.GetParam(status.Code);

                param.ProgramPath = textBoxStatusFileName.Text;
            }
        }

        private void checkBoxStatusEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (objectListViewStatuses.SelectedObject is JobStatus status)
            {
                var param = _currentProfile.StatusManager.OnChangeStatusesParams.GetParam(status.Code);

                param.Enable = checkBoxStatusEnable.Checked;
            }
        }

        private void listBox_Ftp_Servers_DoubleClick(object sender, EventArgs e)
        {
            if (listBox_Ftp_Servers.SelectedItem is FtpSettings fs)
            {
                using (var f = new FormFtpSettings(fs))
                {
                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        _currentProfile.Ftp.Update(fs);
                        listBox_Ftp_Servers.Refresh();
                    }
                }
            }
        }

        private void ButtonFtpSettingsAdd_Click(object sender, EventArgs e)
        {
            var fs = new FtpSettings();

            using (var f = new FormFtpSettings(fs))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    _currentProfile.Ftp.Add(fs);
                    listBox_Ftp_Servers.Items.Add(fs);
                }
            }
        }

        private void buttonFtpSettingsRemove_Click(object sender, EventArgs e)
        {

            if (listBox_Ftp_Servers.SelectedItem is FtpSettings fs)
            {
                _currentProfile.Ftp.Remove(fs);
                listBox_Ftp_Servers.Items.Remove(fs);
            }
        }

        private void buttonSetProfileDefault_Click(object sender, EventArgs e)
        {
            if (objectListViewProfiles.SelectedObject is Profile profile)
            {
                Properties.Settings.Default.DefaultProfile = profile.Settings.ProfileName;
                Properties.Settings.Default.Save();
                MessageBox.Show($"Профіль '{profile.Settings.ProfileName}' за замовчуванням встановлено");
            }
        }

        private void objectListViewPlugins_DoubleClick(object sender, EventArgs e)
        {
            if (objectListViewPlugins.SelectedObject is IPluginBase plugin)
            {
                plugin.ShowSettingsDlg();
            }
        }

        private void buttonScriptAdd_Click(object sender, EventArgs e)
        {
            if (vistaOpenFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //
                var script = _currentProfile.Ftp.FtpScriptController.Create(vistaOpenFileDialog1.FileName);
                objectListViewFtpScripts.AddObject(script);
            }
        }

        private void buttonScriptRemove_Click(object sender, EventArgs e)
        {
            if (objectListViewFtpScripts.SelectedObjects.Count > 0)
            {
                objectListViewFtpScripts.RemoveObjects(objectListViewFtpScripts.SelectedObjects);
            }

        }

        private void objectListView_Utils_SubItemChecking(object sender, SubItemCheckingEventArgs e)
        {
            if (e.Column == olvColumnUsedExplorer0)
            {
                ((MenuSendTo)e.RowObject).UsedInExplorer[0] = e.NewValue == CheckState.Checked;
            }
            else if (e.Column == olvColumnUsedExplorerRight)
            {
                ((MenuSendTo)e.RowObject).UsedInExplorer[1] = e.NewValue == CheckState.Checked;
            }
            else if (e.Column == olvColumnUsedExplorer2)
            {
                ((MenuSendTo)e.RowObject).UsedInExplorer[2] = e.NewValue == CheckState.Checked;
            }
            else if (e.Column == olvColumnUsedExplorer3)
            {
                ((MenuSendTo)e.RowObject).UsedInExplorer[3] = e.NewValue == CheckState.Checked;
            }
        }

        private void buttonExtBrowsersSettings_Click(object sender, EventArgs e)
        {
            using (var form = new FormExtBrowserSettings(_currentProfile))
            {

                form.ShowDialog();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox_addCategory.Text)) return;

            _ = _currentProfile.Categories.Add(textBox_addCategory.Text);

            ReloadCategories();
        }

        private void ReloadCategories()
        {
            objectListViewCategories.ClearObjects();
            
            objectListViewCategories.AddObjects(_currentProfile.Categories?.GetAll().ToArray());
        }

        private void objectListViewCategories_ButtonClick(object sender, CellClickEventArgs e)
        {
            _currentProfile.Categories.Remove((ICategory)e.Model);
            ReloadCategories();
        }

        private void buttonAddFolderName_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty( textBoxFolderName.Text)) return;
            
            listBoxFolderNames.Items.Add(textBoxFolderName.Text);
            textBoxFolderName.Clear();
        }

        private void kryptonButton_MoveSignaFileToOrder_Click(object sender, EventArgs e)
        {
            using (var form = new FormMoveSignaFileToOrder(_currentProfile))
            {
               form.ShowDialog();
            }
        }

        private void додатиШаблонToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var f = new FormTemplate())
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    olv_mail_templates.AddObject(f.Template);
                }
            }
        }

        private void редагуватиШаблонToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (olv_mail_templates.SelectedObject is MailTemplate template)
            {
                using (var f = new FormTemplate(template))
                {
                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        olv_mail_templates.RefreshObject(template);
                    }
                }
            }
        }

        private void видалитиШаблонToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (olv_mail_templates.SelectedObject is MailTemplate template)
            {
                olv_mail_templates.RemoveObject(template);
            }
        }

        private void btn_mail_gmail_settings_sel_secret_file_Click(object sender, EventArgs e)
        {
            using (var f = new Ookii.Dialogs.WinForms.VistaOpenFileDialog())
            {
                f.Filter = "*.json|*.json|*.*|*.*";
                if (f.ShowDialog() == DialogResult.OK)
                {
                    tb_mail_gmail_settings_secret_file.Text = f.FileName;
                }
            }
        }
    }
}
