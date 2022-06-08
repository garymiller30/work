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
using Job.UC;

namespace Job.UserForms
{
    public partial class FormFileBrowserSettings : KryptonForm
    {
        public FormFileBrowserSettings(FileBrowserSettings fileManagerSettings)
        {
            InitializeComponent();

            propertyGrid1.SelectedObject = fileManagerSettings;
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
