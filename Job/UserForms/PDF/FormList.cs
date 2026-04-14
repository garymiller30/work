using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Interfaces;

namespace JobSpace.UserForms.PDF
{
    public partial class FormList : Form
    {
        public IEnumerable<IFileSystemInfoExt> ConvertFiles;

        public FormList()
        {
            InitializeComponent();

            objectListView1.DragSource = new SimpleDragSource();
            objectListView1.DropSink = new RearrangingDropSink(false);

            DialogResult = DialogResult.Cancel;

        }

        public FormList(IEnumerable<IFileSystemInfoExt> files): this()
        {
            AddFiles(files);
        }

        private void AddFiles(IEnumerable<IFileSystemInfoExt> files)
        {
            
            objectListView1.AddObjects(files.ToArray());
        }

        private void objectListView1_CanDrop(object sender, OlvDropEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void objectListView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;

          
        }

        private void toolStripButton_Play_Click(object sender, EventArgs e)
        {
            if (objectListView1.Objects != null)
            {
                ConvertFiles = objectListView1.Objects.Cast<IFileSystemInfoExt>().ToArray();
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void objectListView1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {

                var files = ((string[]) e.Data.GetData(DataFormats.FileDrop)).Select(x=>new {Name = Path.GetFileName(x),FullPath = x });

                objectListView1.AddObjects(files.ToArray());
            }
        }

        private void toolStripButton_AddFiles_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                objectListView1.AddObjects(openFileDialog1.FileNames);
            }
        }

        private void видалитиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteFiles();
        }
        /// <summary>
        /// видалити файли зі списку
        /// </summary>
        private void DeleteFiles()
        {
            if (objectListView1.SelectedObjects != null)
                objectListView1.RemoveObjects(objectListView1.SelectedObjects);
        }

        private void toolStripButton_Del_Click(object sender, EventArgs e)
        {
            DeleteFiles();
        }
    }
}
