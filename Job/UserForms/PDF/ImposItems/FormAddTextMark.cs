using JobSpace.Static.Pdf.Imposition.Models.Marks;
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
    public partial class FormAddTextMark : Form
    {
        private string[] angles = new[] { "0", "90", "180", "270" };
        public TextMark Mark { get; set; }
        public FormAddTextMark()
        {
            InitializeComponent();
            cb_Angle.DataSource = angles;
            Mark = new TextMark();

            BindMark();
            DialogResult = DialogResult.Cancel;
        }

        public FormAddTextMark(TextMark mark)
        {
            InitializeComponent();
            cb_Angle.DataSource = angles;
            Mark = mark;

            BindMark();
            DialogResult = DialogResult.Cancel;
        }

        private void BindMark()
        {
            SetAnchors();
            tb_markName.Text = Mark.Name;
            tb_text.Text = Mark.Text;

            cb_front.Checked = Mark.Parameters.IsFront;
            cb_back.Checked = Mark.Parameters.IsBack;
            cb_backMirror.Checked = Mark.Parameters.IsBackMirrored;
            nud_xOfs.Value = (decimal)Mark.Parameters.Xofs;
            nud_yOfs.Value = (decimal)Mark.Parameters.Yofs;
            cb_Angle.SelectedIndex = Array.IndexOf(angles, Mark.Angle.ToString());
            tb_fontName.Text = Mark.FontName;
            nud_fontSize.Value = (decimal)Mark.FontSize;
            markColorControl1.SetMark(Mark);

        }

        private void SetAnchors()
        {
            apc_mark.SetAnchor(Mark.Parameters.MarkAnchorPoint);
            apc_parent.SetAnchor(Mark.Parameters.ParentAnchorPoint);

            apc_mark.AnchorPointChanged += (s, point) => Mark.Parameters.MarkAnchorPoint = point;
            apc_parent.AnchorPointChanged += (s, point) => Mark.Parameters.ParentAnchorPoint = point;

        }

        private bool UnbindMark()
        {
            if (string.IsNullOrEmpty(tb_text.Text)) return false;

            Mark.Text = tb_text.Text;

            if (string.IsNullOrEmpty(tb_markName.Text))
            {
                Mark.Name = tb_text.Text;
            }
            else
            {
                Mark.Name = tb_markName.Text;
            }
            
            Mark.Parameters.IsFront = cb_front.Checked;
            Mark.Parameters.IsBack = cb_back.Checked;
            Mark.Parameters.IsBackMirrored = cb_backMirror.Checked;

            Mark.Parameters.Xofs = (double)nud_xOfs.Value;
            Mark.Parameters.Yofs = (double)nud_yOfs.Value;

            Mark.FontName = tb_fontName.Text;
            Mark.FontSize = (double)nud_fontSize.Value;

            Mark.Angle = double.Parse(angles[cb_Angle.SelectedIndex]);
            markColorControl1.UpdateMark();

            return true;
        }

        private void bnt_fontSelect_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                tb_fontName.Text = fontDialog1.Font.Name;
                nud_fontSize.Value = (decimal)fontDialog1.Font.Size;
            }
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (!UnbindMark()) return;


            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
