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
using static JobSpace.UserForms.FormEnterTirag;

namespace JobSpace.UserForms
{
    public partial class FormEnterTirag : Form
    {
        public List<FileTirag> fileTirags { get;set;} = new List<FileTirag>();

        public FormEnterTirag()
        {
            InitializeComponent();
        }

        public FormEnterTirag(IEnumerable<IFileSystemInfoExt> files) : this()
        {
          
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

        public class FileTirag
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
            fileTirags = objectListView1.Objects.Cast<FileTirag>().ToList();
            DialogResult = DialogResult.OK;
            Close();

            //BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask("Міняємо тиражі на файлах", new Action(
            //   () =>
            //   {
            //       foreach (FileTirag ft in objectListView1.Objects)
            //       {
            //           _renameAction(_fileManager,ft.Tirag, ft.FileInfo);
                       
            //       }
            //   }
            //   )));
            //DialogResult = DialogResult.OK;
            //Close();
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

        private void txt_filter_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty( txt_filter.Text))            {
                objectListView1.ModelFilter = null;
            }
            else
                objectListView1.ModelFilter = new BrightIdeasSoftware.TextMatchFilter(objectListView1, txt_filter.Text);
        }
    }
}
