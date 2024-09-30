using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Krypton.Toolkit;
using JobSpace.UC;

namespace JobSpace.UserForms
{
    public sealed partial class FormFileBrowserSettings : KryptonForm
    {
        public FormFileBrowserSettings(FileBrowserSettings fileManagerSettings)
        {
            InitializeComponent();

            propertyGrid1.SelectedObject = fileManagerSettings;
            DialogResult = DialogResult.Cancel;
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
