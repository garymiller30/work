
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Krypton.Toolkit;
using Interfaces;
using JobSpace.Profiles;
using MongoDB.Bson;
using JobSpace.Fasades;
using BrightIdeasSoftware;

namespace JobSpace.UserForms
{
    public sealed partial class FormAddWork2 : KryptonForm
    {
        #region ClipBoard

        [DllImport("User32.dll")]
        static extern int SetClipboardViewer(int hWndNewViewer);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        IntPtr _nextClipboardViewer;

        private bool _isFirstShow = true;
        private bool _isCloseAfterPaste;
        #endregion

        private IUserProfile UserProfile { get; set; }
        private readonly IJob _job;
        private readonly INoteControl _noteControl;

        public FormAddWork2(IUserProfile userProfile, IJob job, bool isNewJob)
        {
            UserProfile = userProfile;
            _job = job;
            DialogResult = DialogResult.Cancel;

            InitializeComponent();

            ToolTip btn_tooltip = new ToolTip();
            btn_tooltip.SetToolTip(btn_select_custom_folder, "Вибрати іншу папку для збереження файлів замовлення, якщо попередня була переміщена");

            _noteControl = ucNote1;
            InitializeUserInterface();

            AddPlugins(isNewJob);

            Bind(isNewJob);
        }

        private void Bind(bool isNewJob)
        {
            if (_job != null)
            {
                kryptonTextBoxNumber.Text = _job.Number;

                kryptonComboBox_Customers.SelectedItem = UserProfile.Customers.FindCustomer(_job.Customer);

                tb_category.Text = string.Empty;

                if (!UserProfile.Settings.HideCategory && !isNewJob)
                {
                    var category = UserProfile.Categories.GetCategoryById(_job.CategoryId);
                    if (category != null)
                    {
                        tb_category.Text = category.Name;
                    }
                }
                _noteControl.SetText(_job.Note);
                textBox_Description.Text = _job.Description;

                if (!string.IsNullOrEmpty(_job.PreviousOrder))
                {
                    this.Text += $" Повтор замовлення № {_job.PreviousOrder}";
                }
            }
        }

        private void InitializeUserInterface()
        {
            _isCloseAfterPaste = checkBoxCloseAfterPaste.Checked = UserProfile.Settings.CloseAfterPasteText;
            panelCategory.Visible = !UserProfile.Settings.HideCategory;

            var customers = UserProfile.Customers.Where(x => x.Show).ToList();
            kryptonComboBox_Customers.DataSource = customers;
            kryptonComboBox_Customers.DisplayMember = "Name";
        }

        private void AddPlugins(bool isNewJob)
        {
            SubscribePlugins(isNewJob);
        }

        private void SubscribePlugins(bool isNewJob)
        {
            var plugins = isNewJob
                ? new List<IPluginFormAddWork>()
                : UserProfile.Plugins.GetPluginFormAddWorks();

            if (plugins.Any())
            {
                ucAddWorkPluginsContainer1.Subscribe(UserProfile, _job);
            }
            else
            {
                // приховати пусте вікно та зменшити розмір вікна
                kryptonSplitContainer1.Panel2Collapsed = true;
            }
        }

        private void FormAddWork2_FormClosing(object sender, FormClosingEventArgs e)
        {
            ucAddWorkPluginsContainer1.Unsubscribe(UserProfile);
        }

        private void Button_Ok_Click(object sender, EventArgs e)
        {
            if (CheckCustomerPresent())
            {
                Unbind();
                DialogResult = DialogResult.OK;
                Close();
                return;
            }

            DialogResult = DialogResult.None;

        }
        private bool CheckCustomerPresent()
        {
            if (string.IsNullOrEmpty(kryptonComboBox_Customers.Text))
            {
                MessageBox.Show("Вибери замовника");
                return false;
            }

            return UserProfile.Customers.CheckCustomerPresent(kryptonComboBox_Customers.Text, false);
        }
        private void Unbind()
        {
            _job.Number = kryptonTextBoxNumber.Text;
            _job.Customer = kryptonComboBox_Customers.Text;

            var categoryName = tb_category.Text;

            if (!string.IsNullOrEmpty(categoryName))
            {
                _job.CategoryId = UserProfile.Categories.Add(categoryName);
                CategoryToCustomerAsignManager.SetCategory(UserProfile, ((Customer)kryptonComboBox_Customers.SelectedItem).Id, _job.CategoryId, true);
            }
            else
            {
                _job.CategoryId = ObjectId.Empty;
            }

            _job.Description = textBox_Description.Text;
            _job.Note = _noteControl.GetRtf();
        }


        private void InsertTextToTextbox(TextBox textBox, string txt)
        {
            var selStart = textBox.SelectionStart;

            if (textBox.SelectionLength > 0)
                textBox.Text = textBox.Text.Remove(textBox_Description.SelectionStart,
                    textBox.SelectionLength);
            textBox.Text = textBox.Text.Insert(selStart, txt);
            textBox.SelectionStart = selStart + txt.Length;
        }

        private void FormAddWork2_Shown(object sender, EventArgs e)
        {
            kryptonTextBoxNumber.Focus();
            _nextClipboardViewer = (IntPtr)SetClipboardViewer((int)Handle);
        }

        private void CheckBoxCloseAfterPaste_CheckedChanged(object sender, EventArgs e)
        {
            _isCloseAfterPaste = checkBoxCloseAfterPaste.Checked;
            UserProfile.Settings.CloseAfterPasteText = _isCloseAfterPaste;

            ProfilesController.Save(UserProfile);
        }

        protected override void WndProc(ref Message m)
        {
            // defined in winuser.h
            const int WM_DRAWCLIPBOARD = 0x308;
            const int WM_CHANGECBCHAIN = 0x030D;
            switch (m.Msg)
            {
                case WM_DRAWCLIPBOARD:
                    DisplayClipboardData();
                    SendMessage(_nextClipboardViewer, m.Msg, m.WParam, m.LParam);
                    break;

                case WM_CHANGECBCHAIN:
                    if (m.WParam == _nextClipboardViewer)
                        _nextClipboardViewer = m.LParam;
                    else
                        SendMessage(_nextClipboardViewer, m.Msg, m.WParam, m.LParam);
                    break;

                default:

                    base.WndProc(ref m);
                    break;
            }
        }
        void DisplayClipboardData()
        {
            if (_isFirstShow)
            {
                // при першому показі вікна ігноруємо те, що в буфері обміну
                _isFirstShow = false;
            }
            else
            {
                if (string.IsNullOrEmpty(textBox_Description.Text))
                {
                    InsertTextToTextbox(textBox_Description, true);

                    if (!string.IsNullOrEmpty(textBox_Description.Text))
                        if (_isCloseAfterPaste)
                        {
                            kryptonButton_OK.PerformClick();
                        }
                }
            }
        }
        private void InsertTextToTextbox(TextBox textBox, bool useTransliteration)
        {
            var iData = Clipboard.GetDataObject();

            if (iData != null && iData.GetDataPresent(DataFormats.Text))
            {
                var str = (string)iData.GetData(DataFormats.StringFormat);

                if (useTransliteration)
                {
                    try
                    {
                        str = Path.GetFileNameWithoutExtension(str);
                    }
                    catch
                    {
                        str = string.Empty;
                    }
                }

                InsertTextToTextbox(textBox, str);
            }
        }

        private void kryptonComboBox_Customers_SelectedIndexChanged(object sender, EventArgs e)
        {
            AssignCategories();
        }

        private void AssignCategories()
        {
            var customer = kryptonComboBox_Customers.SelectedItem as Customer;

            if (customer == null) return;

            if (UserProfile.Settings.HideCategory) return;

            var categories = CategoryToCustomerAsignManager.GetCustomerCategories(UserProfile, customer.Id);

            olv_categories.SetObjects(categories);
        }

        private void kryptonComboBox_Customers_Enter(object sender, EventArgs e)
        {
            // kryptonComboBox_Customers.DroppedDown = true;
        }


        private void olv_categories_SelectionChanged(object sender, EventArgs e)
        {
            if (olv_categories.SelectedObject is ICategory category)
            {
                tb_category.Text = category.Name;
            }
        }

        private void tb_category_KeyUp(object sender, KeyEventArgs e)
        {
            var str = tb_category.Text;
            if (string.IsNullOrEmpty(str))
            {
                olv_categories.ModelFilter = null;
            }
            else
            {
                olv_categories.ModelFilter = new TextMatchFilter(olv_categories, str);
            }
        }

        private void btn_select_custom_folder_Click(object sender, EventArgs e)
        {
            using (var form = new Ookii.Dialogs.WinForms.VistaFolderBrowserDialog())
            {
                form.Description = "Оберіть папку";

                if (form.ShowDialog() == DialogResult.OK)
                {
                    var path = form.SelectedPath;
                    if (Directory.Exists(path))
                    {
                        _job.UseCustomFolder = true;
                        _job.Folder = path;
                    }
                }
            }
        }

        private void btn_fix_wrong_keyboard_Click(object sender, EventArgs e)
        {
            // взяти вибраний текст із textBox_Description, поміняти розкладку і вставити назад

            var selStart = textBox_Description.SelectionStart;
            var selLength = textBox_Description.SelectionLength;

            string str;

            if (selLength == 0)
            {
                // вибрати увесь текст
                str = textBox_Description.Text;

                textBox_Description.Text = Commons.FixWrongKeyboardLayout(str);
            }
            else
            {
                // вибраний текст
                str = textBox_Description.Text.Substring(selStart, selLength);
                var fixedStr = Commons.FixWrongKeyboardLayout(str);
                textBox_Description.Text = textBox_Description.Text.Remove(selStart, selLength);
                textBox_Description.Text = textBox_Description.Text.Insert(selStart, fixedStr);
                textBox_Description.SelectionStart = selStart;
                textBox_Description.SelectionLength = fixedStr.Length;
            }
        }
    }
}


