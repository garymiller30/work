using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Job.UserForms
{
    public sealed partial class FormFtpSettings : Form
    {
        private readonly FtpSettings _settings;

        public FormFtpSettings()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;

            comboBox_Encoding.Items.Clear();
            comboBox_Encoding.DisplayMember = "Name";
            comboBox_Encoding.Items.AddRange(Encoding.GetEncodings());
        }

        public FormFtpSettings(FtpSettings settings) : this()
        {
            _settings = settings;
            BindFtpSettings();
        }

        private void BindFtpSettings()
        {
            if (_settings != null)
            {
                textBox_FtpName.Text = _settings.Name;
                textBox_FtpServer.Text = _settings.Server;
                textBox_FtpUser.Text = _settings.User;
                textBox_FtpPassword.Text = _settings.Password;
                checkBox_ActiveMode.Checked = _settings.IsActive;
                textBox_RootFolder.Text = _settings.RootFolder;

                var en = comboBox_Encoding.Items.Cast<EncodingInfo>().FirstOrDefault(x => x.CodePage == _settings.Encode);
                if (en == null) {comboBox_Encoding.SelectedIndex = -1;}
                else{comboBox_Encoding.SelectedItem = en;}

            }
        }

        private void button_FtpAddAplly_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;

            UnbindFtpSettings();
            Close();
        }

        private void UnbindFtpSettings()
        {
            var ei = GetFtpEncodingInfo();

            _settings.Name = textBox_FtpName.Text;
            _settings.Encode = ei.CodePage;
            _settings.Server = textBox_FtpServer.Text;
            _settings.User = textBox_FtpUser.Text;
            _settings.Password = textBox_FtpPassword.Text;
            _settings.IsActive = checkBox_ActiveMode.Checked;
            _settings.RootFolder = textBox_RootFolder.Text;
        }

        private EncodingInfo GetFtpEncodingInfo()
        {
            var ei = comboBox_Encoding.SelectedItem as EncodingInfo ??
                     Encoding.GetEncodings().First(x => x.CodePage == 1251);
            return ei;
        }

        private void button_SelectFTPFolder_Click(object sender, EventArgs e)
        {
            SelectFtpRootFolder();
        }

        private void SelectFtpRootFolder()
        {
            using (var ftp = new FormFtpDirectoryList( textBox_FtpServer.Text,textBox_FtpUser.Text,textBox_FtpPassword.Text,
                textBox_RootFolder.Text,checkBox_ActiveMode.Checked,GetFtpEncodingInfo().CodePage))
            {
                if (ftp.ShowDialog() == DialogResult.OK)
                {
                    
                    var rd = ftp.GetSelectedDirectory();
                    if (rd != null)
                    {
                        textBox_RootFolder.Text = rd.FullPath;
                    }
                }
            }
        }

        private void Button_Reset_Click(object sender, EventArgs e)
        {
            textBox_RootFolder.Text = null;
        }
    }
}
