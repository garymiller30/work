using JobSpace.Static.Pdf.Imposition;
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
    public partial class PdfFileListControl : UserControl
    {
        public PdfFileListControl()
        {
            InitializeComponent();
            InitTree();
        }

        private void InitTree()
        {
            treeListViewFiles.IsSimpleDragSource = true;
            treeListViewFiles.CanExpandGetter += (r) => r is PdfFile;
            treeListViewFiles.ChildrenGetter += (r) => ((PdfFile)r).Pages;
            olvColumnName.AspectGetter = delegate (object r)
            {
                if (r is PdfFile file)
                {
                    return $"{file.Id}: {file.ShortName}";
                }
                else if (r is PdfFilePage page)
                {
                    return $"Page {page.Idx}";
                }
                return "???";
            };

            olvColumnTrim.AspectGetter = delegate (object r)
            {
                if (r is PdfFilePage p)
                {
                    return $"{p.Trim.W.ToString("N1")} x {p.Trim.H.ToString("N1")}";
                }
                else if (r is PdfFile file && file.Pages.Count() == 1)
                {
                    var page = file.Pages.First();
                    return $"{page.Trim.W.ToString("N1")} x {page.Trim.H.ToString("N1")}";
                }
                return null;
            };

            olvColumnCount.AspectGetter = delegate (object r)
            {
                if (r is PdfFile file)
                {
                    return file.Count;
                }
                return null;
            };

            treeListViewFiles.ModelCanDrop += TreeListViewFiles_ModelCanDrop;
        }

      

        private void TreeListViewFiles_ModelCanDrop(object sender, BrightIdeasSoftware.ModelDropEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            if (e.TargetModel == null) return;

            if (e.TargetModel is PdfFile || e.TargetModel is PdfFilePage)
            {
                e.Effect = e.StandardDropActionFromKeys;
            }
        }

        public void AddFiles(List<PdfFile> files)
        {
            treeListViewFiles.AddObjects(files);
        }

        public void SetControlBindParameters(GlobalImposParameters imposParam)
        {
            imposParam.ControlsBind.PdfFileList = treeListViewFiles;
        }
    }
}
