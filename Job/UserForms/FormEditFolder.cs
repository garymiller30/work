using Interfaces;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace JobSpace.UserForms
{
    public sealed partial class FormEditFolder : Form
    {
        private IUserProfile _profile;

        public FormEditFolder()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        public FormEditFolder(IUserProfile profile, Action<string> action):this()
        {

            _profile = profile;

            var folders = _profile.Settings.GetFileBrowser().FolderNamesForCreate;

            if (folders.Count == 0 || action == null) return;

            buttonQuickMenu.Visible = true;

            foreach (var folderName in folders)
            {
                var mi = new ToolStripMenuItem(folderName,null, (sender, e) =>
                {
                    action(folderName);
                    Close();
                });
                contextMenuStripQuickMenu.Items.Add(mi);
            }

        }

        public FormEditFolder(string name) : this()
        {
            textBox_Name.Text = name;

            if (name != null) textBox_Name.Select(0, Path.GetFileNameWithoutExtension(name).Length);
        }

        private void buttonQuickMenu_Click(object sender, EventArgs e)
        {
            contextMenuStripQuickMenu.Show(buttonQuickMenu,new System.Drawing.Point(20,20));
        }
    }
}
