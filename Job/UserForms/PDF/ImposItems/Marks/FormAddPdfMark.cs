using JobSpace.Profiles;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Models.Marks;
using JobSpace.Static.Pdf.Imposition.Services;
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
        Profile _profile;
        private string[] angles = new [] {"0","90","180","270" };
        public PdfMark Mark { get; set; }

        public FormAddPdfMark(Profile profile)
        {
            _profile = profile;
            InitializeComponent();
            cb_Angle.DataSource = angles;
            

            Mark = new PdfMark();
            SetParams();
            DialogResult = DialogResult.Cancel;
            //Mark = mark;
        }

        public FormAddPdfMark(Profile profile, PdfMark mark)
        {
            _profile = profile;
            InitializeComponent();
            cb_Angle.DataSource = angles;
            Mark = mark;
            
            SetParams();
            DialogResult = DialogResult.Cancel;
        }

        private void SetParams()
        {
            selectMarkSideControl1.SetParameters(Mark.Parameters);

            tb_markPath.Text = Mark.File?.FileName;
            tb_name.Text = Mark.Name;
            //cb_front.Checked = Mark.Parameters.IsFront;
            //cb_back.Checked = Mark.Parameters.IsBack;
            cb_backMirror.Checked = Mark.Parameters.IsBackMirrored;
            nud_Xofs.Value = (decimal)Mark.Parameters.Xofs;
            nud_yOfs.Value = (decimal)Mark.Parameters.Yofs;
            cb_Angle.SelectedIndex = Array.IndexOf(angles, Mark.Angle.ToString());
            cb_foreground.Checked = Mark.IsForeground;
            rb_parentSheet.Checked = Mark.Parent == MarkParentEnum.Sheet;
            rb_parentSubject.Checked = Mark.Parent == MarkParentEnum.Subject;
            nud_clip_bottom.Value = (decimal)Mark.Parameters.ClipBox.Bottom;
            nud_clip_left.Value = (decimal)Mark.Parameters.ClipBox.Left;
            nud_clip_right.Value = (decimal)Mark.Parameters.ClipBox.Right;
            nud_clip_top.Value = (decimal)Mark.Parameters.ClipBox.Top;
            cb_auto_clip_x.Checked = Mark.Parameters.IsAutoClipX;
            cb_auto_clip_y.Checked = Mark.Parameters.IsAutoClipY;
            rb_x_relative_sheet.Checked = Mark.Parameters.AutoClipRelativeX == AutoClipMarkEnum.Sheet;
            rb_x_relative_subjet.Checked = Mark.Parameters.AutoClipRelativeX == AutoClipMarkEnum.Subject;
            rb_y_relative_sheet.Checked = Mark.Parameters.AutoClipRelativeY == AutoClipMarkEnum.Sheet;
            rb_y_relative_subject.Checked = Mark.Parameters.AutoClipRelativeY == AutoClipMarkEnum.Subject;

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
            
            //p.IsBack = cb_back.Checked;
            //p.IsFront = cb_front.Checked;
            p.IsBackMirrored = cb_backMirror.Checked;
            p.Xofs = (double)nud_Xofs.Value;
            p.Yofs = (double)nud_yOfs.Value;
            Mark.Angle = double.Parse( angles[cb_Angle.SelectedIndex]);
            Mark.IsForeground = cb_foreground.Checked;
            Mark.Parent = rb_parentSheet.Checked ? MarkParentEnum.Sheet : MarkParentEnum.Subject;
            p.ClipBox = new ClipBox
            {
                Bottom = (double)nud_clip_bottom.Value,
                Left = (double)nud_clip_left.Value,
                Right = (double)nud_clip_right.Value,
                Top = (double)nud_clip_top.Value
            };
            p.IsAutoClipX = cb_auto_clip_x.Checked;
            p.IsAutoClipY = cb_auto_clip_y.Checked;
            p.AutoClipRelativeX = rb_x_relative_sheet.Checked ? AutoClipMarkEnum.Sheet : AutoClipMarkEnum.Subject;
            p.AutoClipRelativeY = rb_y_relative_sheet.Checked ? AutoClipMarkEnum.Sheet : AutoClipMarkEnum.Subject;
            
            selectMarkSideControl1.GetParameters(p);

            return true;
        }

        private void btn_SelectPdfFile_Click(object sender, EventArgs e)
        {
            vistaOpenFileDialog1.Filter = "PDF files (*.pdf)|*.pdf";
            vistaOpenFileDialog1.FileName = _profile.ImposService.GetMarksPath() + "\\";

            if (vistaOpenFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Mark.File = new PdfFile(vistaOpenFileDialog1.FileName);
                tb_markPath.Text = vistaOpenFileDialog1.FileName;
            }
        }

        private void nud_Xofs_Click(object sender, EventArgs e)
        {
            ((NumericUpDown)sender).Select(0, ((NumericUpDown)sender).Text.Length);
        }
    }
}
