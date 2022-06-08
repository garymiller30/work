using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using Interfaces;

namespace PluginAddWorkFromPolymix
{
    public partial class FormSettings : KryptonForm
    {
        private PluginAddWorkFromPolymixSettings _addWorkFromPolymixSettings;
        private IUserProfile _userProfile;

        public FormSettings(PluginAddWorkFromPolymixSettings addWorkFromPolymixSettings, IUserProfile userProfile)
        {
            InitializeComponent();
            _userProfile = userProfile;
            _addWorkFromPolymixSettings = addWorkFromPolymixSettings;
            Bind();
        }

        private void Bind()
        {
            textBoxAddress.Text =  _addWorkFromPolymixSettings.ServerAddress;
            textBoxBaseName.Text = _addWorkFromPolymixSettings.BaseName;
            textBoxUser.Text =     _addWorkFromPolymixSettings.User;
            textBoxPassword.Text = _addWorkFromPolymixSettings.Password;
            //numericUpDownKindId.Value = _addWorkFromPolymixSettings.KindId;


        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Unbind();
            Close();
        }

        private void Unbind()
        {
            _addWorkFromPolymixSettings.ServerAddress = textBoxAddress.Text;
            _addWorkFromPolymixSettings.BaseName = textBoxBaseName.Text;
            _addWorkFromPolymixSettings.User = textBoxUser.Text ;
            _addWorkFromPolymixSettings.Password = textBoxPassword.Text;
            //_addWorkFromPolymixSettings.KindId = (int) numericUpDownKindId.Value;

        }
    }
}
