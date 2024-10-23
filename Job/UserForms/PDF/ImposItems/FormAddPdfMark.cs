using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Models.Marks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF.ImposItems
{
    public partial class FormAddPdfMark : Form
    {
        private string[] angles = new [] {"0","90","180","270" };
        public PdfMark Mark { get; set; }

        public FormAddPdfMark()
        {
            InitializeComponent();
            cb_Angle.DataSource = angles;

            Mark = new PdfMark();
            SetParams();
            DialogResult = DialogResult.Cancel;
        }

        public FormAddPdfMark(PdfMark mark)
        {
            InitializeComponent();
            cb_Angle.DataSource = angles;
            Mark = mark;
            
            SetParams();
            DialogResult = DialogResult.Cancel;
        }

        private void SetParams()
        {
            tb_markPath.Text = Mark.File?.FileName;
            tb_name.Text = Mark.Name;
            cb_front.Checked = Mark.Parameters.IsFront;
            cb_back.Checked = Mark.Parameters.IsBack;
            cb_backMirror.Checked = Mark.Parameters.IsBackMirrored;
            nud_Xofs.Value = (decimal)Mark.Parameters.Xofs;
            nud_yOfs.Value = (decimal)Mark.Parameters.Yofs;
            cb_Angle.SelectedIndex = Array.IndexOf(angles, Mark.Angle.ToString());
            SetAnchors();

        }

        private void SetAnchors()
        {
            apc_mark.SetAnchor(Mark.Parameters.MarkAnchorPoint);
            apc_parent.SetAnchor(Mark.Parameters.ParentAnchorPoint);

            apc_mark.AnchorPointChanged += (s, point) => Mark.Parameters.MarkAnchorPoint = point;
            apc_parent.AnchorPointChanged += (s, point) => Mark.Parameters.ParentAnchorPoint = point;
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {

            UpdateParams();

            DialogResult = DialogResult.OK;
            Close();
        }

        private bool UpdateParams()
        {
            if (Mark.File == null) return false;

            if (string.IsNullOrEmpty(tb_name.Text))
            {
                Mark.Name = Mark.File.ShortName;
            }
            else
            {
                Mark.Name = tb_name.Text;
            }

            var p = Mark.Parameters;
            
            p.IsBack = cb_back.Checked;
            p.IsFront = cb_front.Checked;
            p.IsBackMirrored = cb_backMirror.Checked;
            p.Xofs = (double)nud_Xofs.Value;
            p.Yofs = (double)nud_yOfs.Value;
            Mark.Angle = double.Parse( angles[cb_Angle.SelectedIndex]);
             

            return true;
        }

        private void btn_SelectPdfFile_Click(object sender, EventArgs e)
        {
            if (vistaOpenFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Mark.File = new PdfFile(vistaOpenFileDialog1.FileName);
                tb_markPath.Text = vistaOpenFileDialog1.FileName;
            }
        }
    }
}
