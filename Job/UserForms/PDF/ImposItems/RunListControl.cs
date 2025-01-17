using BrightIdeasSoftware;
using JobSpace.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF.ImposItems
{
    public partial class RunListControl : UserControl
    {
        ControlBindParameters _bindParameters;

        //int UnassignedIdx = 0;

        public RunListControl()
        {
            InitializeComponent();

            tsb_AddEmptyPage.Click += tsb_AddEmptyPage_Click;
            tsb_RemovePage.Click += tsb_RemovePage_Click;

            InitRunList();
        }

        private void InitRunList()
        {
            fastObjectListView1.IsSimpleDragSource = true;
            fastObjectListView1.DropSink = new RearrangingDropSink(true);

            olvColumnRunListPagesf.AspectGetter += delegate (object r)
            {
                if (r is ImposRunPage page)
                {
                    return $"{page.FileId}:{page.PageIdx}";
                }
                return r.ToString();
            };

            olvColumIdxf.AspectGetter += delegate (object r)
            {
                var page = (ImposRunPage)r;
                var list = fastObjectListView1.Objects.Cast<ImposRunPage>().ToList();

                int idx = list.IndexOf(page) + 1;

                return idx;
            };

            olvColumnAsignf.AspectGetter += (r) => ((ImposRunPage)r).IsAssumed ? "●" : "◌";
        }

        public void SetControlBindParameters(ControlBindParameters bindParameters)
        {
            _bindParameters = bindParameters;
            fastObjectListView1.ModelCanDrop += ObjectListViewRunList_ModelCanDrop;
            fastObjectListView1.ModelDropped += ObjectListViewRunList_ModelDropped;
        }

        private void ObjectListViewRunList_ModelDropped(object sender, BrightIdeasSoftware.ModelDropEventArgs e)
        {
            if (e.SourceListView == _bindParameters.PdfFileList)
            {
                foreach (var item in e.SourceModels)
                {
                    if (item is PdfFile file)
                    {
                        List<ImposRunPage> pages = ImposRunList.CreatePagesFromFile(file);
                        fastObjectListView1.AddObjects(pages);
                    }
                    else if (item is PdfFilePage page)
                    {
                        int fileId = PdfFile.GetParentId(_bindParameters.PdfFiles, page);
                        if (fileId == -1) throw new Exception("No file id");
                        ImposRunPage runPage = new ImposRunPage(fileId, page.Idx);
                        fastObjectListView1.AddObject(runPage);
                    }
                }
                e.Handled = true;
            }
           
        }

        public List<ImposRunPage> GetRunPages()
        {
            return fastObjectListView1.Objects.Cast<ImposRunPage>().ToList();
        }

        private void ObjectListViewRunList_ModelCanDrop(object sender, BrightIdeasSoftware.ModelDropEventArgs e)
        {
            e.Effect = DragDropEffects.None;

            if (e.TargetModel == null) return;

            if (e.TargetModel is PdfFile || e.TargetModel is PdfFilePage)
            {
                e.Effect = e.StandardDropActionFromKeys;
            }
        }

        public void AddPages(List<ImposRunPage> pages)
        {
            fastObjectListView1.AddObjects(pages);
            UpdateStatusString();
        }

        private void tsb_RemovePage_Click(object sender, EventArgs e)
        {
            if (fastObjectListView1.SelectedObjects.Count > 0)
            {
                fastObjectListView1.RemoveObjects(fastObjectListView1.SelectedObjects);
                UpdateStatusString();
            }
        }

        private void tsb_AddEmptyPage_Click(object sender, EventArgs e)
        {
            fastObjectListView1.AddObject(new ImposRunPage() { FileId = 0, PageIdx = 0 });
            UpdateStatusString();
        }

        private void UpdateStatusString()
        {
            int assigned = fastObjectListView1.Objects.Cast<ImposRunPage>().Where(x => x.IsAssumed).Count();
            int unassigned = fastObjectListView1.Objects.Cast<ImposRunPage>().Where(x => !x.IsAssumed).Count();

            tssl_status.Text = $"◌ : {unassigned} | ● : {assigned}";

        }

        public int GetUnassignedPagesCount()
        {
            return fastObjectListView1.Objects.Cast<ImposRunPage>().Where(x=>x.IsAssumed == false).ToList().Count;
        }

        public void UpdateRunList()
        {
            UpdateStatusString();
            fastObjectListView1.RefreshObjects(fastObjectListView1.Objects.Cast<ImposRunPage>().ToList());
            ApplyFilter();
        }

        private void fastObjectListView1_FormatRow(object sender, FormatRowEventArgs e)
        {
            ImposRunPage imposRunPage = (ImposRunPage)e.Model;

            if (imposRunPage.IsAssumed)
            {
                e.Item.BackColor = imposRunPage.IsValidFormat ? default : Color.LightCoral;
            }
            else
            {
                e.Item.BackColor = Color.LightYellow;
            }
        }

        private void tsb_ShowOnlyUnassigned_CheckedChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        public void ApplyFilter()
        {
            if (tsb_ShowOnlyUnassigned.Checked)
            {
                
                fastObjectListView1.ModelFilter = new ModelFilter(delegate (object x)
                {
                    return ((ImposRunPage)x).IsAssumed == false;
                });
            }
            else
            {
                fastObjectListView1.ModelFilter = null;
            }
        }

        private void fastObjectListView1_SelectionChanged(object sender, EventArgs e)
        {
            
        }

        private void fastObjectListView1_Click(object sender, EventArgs e)
        {
            if (fastObjectListView1.SelectedObject is ImposRunPage page)
            {
                int idx = fastObjectListView1.Objects.Cast<ImposRunPage>().ToList().IndexOf(page);
                _bindParameters.SelectedImposRunPageIdx = idx + 1;
            }

        }
    }
}
