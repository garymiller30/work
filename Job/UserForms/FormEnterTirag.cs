using BackgroundTaskServiceLib;
using Interfaces;
using JobSpace.Static.Pdf.MergeTemporary;
using JobSpace.UC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.UserForms
{
    public partial class FormEnterTirag : Form
    {
        UC.IFileManager _fileManager;
        Action<IFileManager,int, IFileSystemInfoExt> _renameAction;

        public FormEnterTirag()
        {
            InitializeComponent();
        }

        public FormEnterTirag(UC.IFileManager fileManager, IEnumerable<IFileSystemInfoExt> files,Action<IFileManager, int, IFileSystemInfoExt> renameAction) : this()
        {
            _fileManager = fileManager;
            _renameAction = renameAction;
            AddToList(files);

            //вибрати всі елементи
            objectListView1.SelectAll();
            // перейти до nud_tirag і вибрати все в ньому
            nud_tirag.Focus();
            nud_tirag.Select(0, nud_tirag.Text.Length);
        }

        private void AddToList(IEnumerable<IFileSystemInfoExt> files)
        {

            List<FileTirag> filesTirag = new List<FileTirag>();

            var reg = new Regex(@"#(\d+)\.");

            foreach (var file in files)
            {
                int tirag = 0;
                var match = reg.Match(file.FileInfo.Name);

                if (match.Success)
                {
                    int.TryParse(match.Groups[1].Value, out tirag);
                }
                var ft = new FileTirag
                {
                    FileInfo = file,
                    Tirag = tirag
                };

                filesTirag.Add(ft);


            }
            objectListView1.AddObjects(filesTirag);
            SetTotalLabel();
        }

        class FileTirag
        {
            public IFileSystemInfoExt FileInfo { get; set; }
            public int Tirag { get; set; }
        }

        private void objectListView1_CellEditFinished(object sender, BrightIdeasSoftware.CellEditEventArgs e)
        {
            if (e.RowObject is FileTirag ft)
            {
                if (e.Column == olvColumn_tirag)
                {
                    ft.Tirag = Convert.ToInt32(e.NewValue);
                    SetTotalLabel();
                }
            }
        }

        private void btn_set_tirag_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects.Count == 0) return;


            foreach (FileTirag ft in objectListView1.SelectedObjects)
            {
                ft.Tirag = Convert.ToInt32(nud_tirag.Value);
            }

            objectListView1.RefreshObjects(objectListView1.SelectedObjects);
            SetTotalLabel();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {

            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("Міняємо тиражі на файлах", new Action(
               () =>
               {
                   foreach (FileTirag ft in objectListView1.Objects)
                   {
                       _renameAction(_fileManager,ft.Tirag, ft.FileInfo);
                       //var reg = new Regex(@"#(\d+)\.");
                       //var match = reg.Match(ft.FileInfo.FileInfo.Name);
                       //string targetFile;
                       //if (match.Success)
                       //{
                       //    targetFile =
                       //        $"{Path.GetFileNameWithoutExtension(ft.FileInfo.FileInfo.Name).Substring(0, match.Index)}#{ft.Tirag}{ft.FileInfo.FileInfo.Extension}";
                       //}
                       //else
                       //{
                       //    targetFile = $"{Path.GetFileNameWithoutExtension(ft.FileInfo.FileInfo.Name)}#{ft.Tirag}{ft.FileInfo.FileInfo.Extension}";
                       //}
                       //_fileManager.MoveFileOrDirectoryToCurrentFolder(ft.FileInfo, targetFile);
                   }
               }
               )));
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btn_paste_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                var text = Clipboard.GetText();
                var files = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                
                int idx = 0;

                if (objectListView1.SelectedObjects.Count == 0)
                {
                    //select all objects
                    objectListView1.SelectAll();
                }


                foreach (FileTirag ft in objectListView1.SelectedObjects)
                {
                    if (idx >= files.Length) break;
                    ft.Tirag = Convert.ToInt32(files[idx]);
                    idx++;
                }

                objectListView1.RefreshObjects(objectListView1.SelectedObjects);
            }
            SetTotalLabel();
        }

   

        private void SetTotalLabel()
        {
            //set to label t_total sum of tirag
            int total = 0;
            if (objectListView1.Objects == null) {
            }
            else
            {
                foreach (FileTirag ft in objectListView1.Objects)
                {
                    total += ft.Tirag;
                }

            }
            l_total.Text = $"{total}";
        }
    }
}
