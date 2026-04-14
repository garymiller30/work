using System;
using System.Windows.Forms;

namespace PluginRabbitMq
{
    public partial class FormSettings : Form
    {
        readonly Settings _settings;

        public FormSettings(Settings settings)
        {
            _settings = settings;

            InitializeComponent();
            DialogResult = DialogResult.Cancel;

            textBox_Pass.Text = _settings.RabbitPassword;
            textBox_Server.Text = _settings.RabbitServer;
            textBox_User.Text = _settings.RabbitUser;
            textBox_VirtualHost.Text = _settings.RabbitVirtualHost;

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {

            _settings.RabbitUser = textBox_User.Text;
            _settings.RabbitServer = textBox_Server.Text;
            _settings.RabbitPassword = textBox_Pass.Text;
            _settings.RabbitVirtualHost = textBox_VirtualHost.Text;

            DialogResult = DialogResult.OK;

            Close();
        }
    }
}
