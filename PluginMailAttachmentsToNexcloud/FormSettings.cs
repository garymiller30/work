using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PluginMailAttachmentsToNextcloud
{
    public partial class FormSettings : Form
    {
        private readonly CloudSettings _settings;

        public FormSettings(CloudSettings settings)
        {
            _settings = settings;
            InitializeComponent();
            BindSettings();

            DialogResult = DialogResult.Cancel;
        }

        private void BindSettings()
        {
            textBoxPass.Text = _settings.Password;
            textBoxRemoteFolder.Text = _settings.RemoteFolder;
            textBoxServer.Text = _settings.Server;
            textBoxUSer.Text = _settings.User;
            numericUpDownSize.Value = _settings.SizeLimit / (1024 * 1024);
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            _settings.Password = textBoxPass.Text;
            _settings.RemoteFolder = textBoxRemoteFolder.Text;
            _settings.Server = textBoxServer.Text;
            _settings.User = textBoxUSer.Text;
            _settings.SizeLimit = numericUpDownSize.Value * 1024 * 1024;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
