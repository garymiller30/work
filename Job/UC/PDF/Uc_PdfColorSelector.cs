using JobSpace.Static;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.UserForms.PDF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace JobSpace.UC.PDF
{
    public partial class Uc_PdfColorSelector : UserControl
    {

        public event EventHandler OnColorSelected = delegate { };
        public MarkColor MarkColor { get; set; }

        public Uc_PdfColorSelector()
        {
            InitializeComponent();
        }

        private void btn_select_color_Click(object sender, EventArgs e)
        {
            using var form = new Form_PdfColorSelector();
            if (form.ShowDialog() == DialogResult.OK)
            {
                MarkColor = form.MarkColor;
                SetLabelColorName();
                OnColorSelected(this,null);
            }
        }

        private void SetLabelColorName()
        {
            if (MarkColor.IsSpot)
            {
                SetLabelSpot();
            }
            else
            {
                SetLabelCMYK();
            }
            SetPanelColor();
        }

        private void SetPanelColor()
        {
            (int R, int G, int B) color;
            if (MarkColor.ColorType == Models.ColorTypeEnum.CMYK)
            {
                color = ColorUtil.CMYKToRGB(MarkColor);
                
            }
            else
            {
                color = ColorUtil.LabToRGB(MarkColor.l,MarkColor.a,MarkColor.b);
            }
            panel_color.BackColor = Color.FromArgb(color.R, color.G, color.B);
        }

        private void SetLabelCMYK()
        {
            label_selected_color.Text = $"C: {MarkColor.C}, M: {MarkColor.M}, Y: {MarkColor.Y}, K: {MarkColor.K}";
        }

        private void SetLabelSpot()
        {
            label_selected_color.Text = MarkColor.Name;
        }
    }
}
