using BrightIdeasSoftware;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PluginFileshareWeb
{
    public partial class FormSettings : Form
    {
        public FormSettings(FileShareWebSettings settings)
        {
            InitializeComponent();

            objectListView1.DragSource = new SimpleDragSource();
            objectListView1.DropSink = new RearrangingDropSink(false);

            Settings = settings;
            objectListView1.AddObjects(Settings.Links);
            DialogResult = DialogResult.Cancel; 
        }

        public FileShareWebSettings Settings { get; set; }

        private void tsb_remove_link_Click(object sender, EventArgs e)
        {
            objectListView1.RemoveObjects(objectListView1.SelectedObjects);
        }

        private void tsb_edit_link_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObject is LinkInfo linkInfo)
            {
                using (var form = new FormEditLink(linkInfo))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        objectListView1.RefreshObject(linkInfo);
                    }
                }
            }
        }

        private void tsb_add_link_Click(object sender, EventArgs e)
        {
            using (var form = new FormEditLink())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    objectListView1.AddObject(form.LinkInfo);
                }
            }
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            Settings.Links.Clear();
            if (objectListView1.Objects != null)
            {
                Settings.Links.AddRange(objectListView1.Objects.Cast<LinkInfo>());
            }
            DialogResult = DialogResult.OK;
            Close();


        }
    }
}
