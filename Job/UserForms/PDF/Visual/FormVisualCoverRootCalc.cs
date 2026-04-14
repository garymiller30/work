using JobSpace.Static;
using Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF.Visual
{
    public partial class FormVisualCoverRootCalc : Krypton.Toolkit.KryptonForm
    {
        enum CoverType
        {
            Soft,
            Hard
        }

        Dictionary<string, decimal> thicknesses;
        CoverType coverType = CoverType.Soft;
        public FormVisualCoverRootCalc()
        {
            InitializeComponent();
            kryptonComboBox1.SelectedIndex = 0;
            //load paper thicknesses from db\paper_thicknesses.json
            thicknesses = SaveAndLoad.LoadPaperThicknesses();
            if (thicknesses.Count == 0)
            {
                cb_paper_thickness.Enabled = false;
            }
            else
            {
                cb_paper_thickness.Items.AddRange(thicknesses.Keys.ToArray());
                cb_paper_thickness.SelectedIndex = 0;
            }
        }

        private void cb_paper_thickness_SelectedIndexChanged(object sender, EventArgs e)
        {
            nud_paper_thickness.Value = thicknesses[cb_paper_thickness.SelectedItem.ToString()];
        }

        private void kryptonComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            coverType = (CoverType)kryptonComboBox1.SelectedIndex;
            nud_glue.Enabled = coverType == CoverType.Soft;
            nud_coeficient.Enabled = coverType == CoverType.Hard;
            Recalc();
        }

        private void nud_cnt_pages_ValueChanged(object sender, EventArgs e)
        {
            Recalc();
        }

        void Recalc()
        {
            if (nud_cnt_pages.Value == 0) return;

            switch (coverType)
            {
                case CoverType.Soft:
                    decimal thickness = Math.Ceiling(nud_cnt_pages.Value / 2) * nud_paper_thickness.Value + nud_glue.Value;
                    label_root.Text = $"{thickness:F1} мм";
                    break;
                case CoverType.Hard:
                    decimal thickness_hard = Math.Ceiling(nud_cnt_pages.Value / 2) * nud_paper_thickness.Value * nud_coeficient.Value; // для твердої палітурки можна використовувати коефіцієнт 1.5
                    label_root.Text = $"{thickness_hard:F1} мм";
                    break;
            }
        }
    }
}
