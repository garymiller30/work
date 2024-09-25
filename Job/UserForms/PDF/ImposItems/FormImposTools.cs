using Job.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Job.UserForms.PDF.ImposItems
{
    public partial class FormImposTools : Form
    {
        ImposToolsParameters parameters;

        public FormImposTools(ImposToolsParameters param)
        {
            parameters = param;

            parameters.BackNumChanged += delegate (object sender, int num)
            {
                tb_back.Text = num.ToString();
            };

            parameters.FrontNumChanged += delegate (object sender, int num)
            {
                tb_front.Text = num.ToString();
            };

            InitializeComponent();
            tb_front.DataBindings.Add("Enabled", cb_EnableNumering, "Checked");
            tb_back.DataBindings.Add("Enabled", cb_EnableNumering, "Checked");
            btn_switch_front_back.DataBindings.Add("Enabled", cb_EnableNumering, "Checked");
        }

        private void tb_front_MouseEnter(object sender, EventArgs e)
        {
            tb_front.SelectAll();
        }

        private void tb_back_MouseEnter(object sender, EventArgs e)
        {
            tb_back.SelectAll();
        }

        private void tb_front_Enter(object sender, EventArgs e)
        {
            tb_front.SelectAll();
        }

        private void tb_front_MouseClick(object sender, MouseEventArgs e)
        {
            tb_front.SelectAll();
        }

        private void tb_back_MouseClick(object sender, MouseEventArgs e)
        {
            tb_back.SelectAll();
        }

        private void cb_rotate_180_CheckedChanged(object sender, EventArgs e)
        {
            parameters.IsFlipAngle = cb_rotate_180.Checked;
        }

        private void cb_EnableNumering_CheckedChanged(object sender, EventArgs e)
        {
            parameters.IsNumering = cb_EnableNumering.Checked;
        }

        private void tb_front_TextChanged(object sender, EventArgs e)
        {
            bool res = int.TryParse(tb_front.Text, out int val);
            if (res)
            {
                parameters.FrontNum = val;
            }
        }

        private void tb_back_TextChanged(object sender, EventArgs e)
        {
            bool res = int.TryParse(tb_back.Text, out int val);
            if (res)
            {
                parameters.BackNum = val;
            }
        }

        private void btn_switch_front_back_Click(object sender, EventArgs e)
        {
            (tb_front.Text, tb_back.Text) = (tb_back.Text, tb_front.Text);
        }
    }
}
