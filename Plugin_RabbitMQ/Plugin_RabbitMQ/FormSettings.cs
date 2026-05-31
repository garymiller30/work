using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Plugin_RabbitMQ
{
    public partial class FormSettings : Form
    {

        readonly PluginRabbitMQSettings _settings;

        public FormSettings(PluginRabbitMQSettings settings)
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
            _settings = settings;

            textBoxHost.Text = _settings.VirtualHost;
            textBoxServer.Text = _settings.Server;
            textBoxUser.Text = _settings.User;
            textBoxPassword.Text = _settings.Password;


        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            _settings.VirtualHost = textBoxHost.Text;
            _settings.Server = textBoxServer.Text;
            _settings.User = textBoxUser.Text; 
            _settings.Password = textBoxPassword.Text;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
