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
    public partial class MarkColorControl : UserControl
    {
        TextMark _mark;



        public MarkColorControl()
        {
            InitializeComponent();

        }

        public void SetMark(TextMark mark)
        {
            _mark = mark;
            var c = _mark.Color;

            cb_isOverprint.Checked = c.IsOverprint;
            cb_isSpot.Checked = c.IsSpot;

            nud_c.Value = (decimal)c.C;
            nud_m.Value = (decimal)c.M;
            nud_y.Value = (decimal)c.Y;
            nud_k.Value = (decimal)c.K;

            tb_name.Text = c.Name;

            tb_name.DataBindings.Add("Enabled", cb_isSpot, "Checked");
            btn_selectPantone.DataBindings.Add("Enabled", cb_isSpot, "Checked");
        }

        public void UpdateMark()
        {
            var c = _mark.Color;
            c.IsOverprint = cb_isOverprint.Checked;
            c.IsSpot = cb_isSpot.Checked;
            c.Name = tb_name.Text;
            c.C = (double)nud_c.Value;
            c.M = (double)nud_m.Value;
            c.Y = (double)nud_y.Value;
            c.K = (double)nud_k.Value;

        }

        private void btn_selectPantone_Click(object sender, EventArgs e)
        {
            using (var form = new FormSelectSpotColor())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    var spot = form.SelectedSpotColor;

                    tb_name.Text = spot.Name;

                    nud_c.Value = spot.C;
                    nud_m.Value = spot.M;
                    nud_y.Value = spot.Y;
                    nud_k.Value = spot.K;
                }
            }
        }

        private void nud_c_Click(object sender, EventArgs e)
        {
            ((NumericUpDown)sender).Select(0,( (NumericUpDown)sender).Text.Length);
        }
    }

}
