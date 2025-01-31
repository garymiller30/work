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
    public partial class FormChangePageMargins : Form
    {
        ClipBox cl;

        public FormChangePageMargins(ClipBox clipBox)
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
            cl = clipBox;
            Bind();

        }

        private void Bind()
        {
            nud_bottom.Value = (decimal)cl.Bottom;
            nud_left.Value = (decimal)cl.Left;
            nud_right.Value = (decimal)cl.Right;
            nud_top.Value = (decimal)cl.Top;
        }

        private void b_ok_Click(object sender, EventArgs e)
        {
            UnBind();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void UnBind()
        {
            cl.Bottom = (double)nud_bottom.Value;
            cl.Left = (double)nud_left.Value;
            cl.Right = (double)nud_right.Value;
            cl.Top = (double)nud_top.Value;
        }
    }
}
