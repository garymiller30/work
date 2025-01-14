﻿using JobSpace.Static.Pdf.SplitSpread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.UserForms
{
    public partial class FormPdfSplitterParams : Form
    {

        public PdfSplitterParams Params { get;set; } = new PdfSplitterParams();

        public FormPdfSplitterParams()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Params.From = (int)numericUpDownFrom.Value;
            Params.To = (int)numericUpDownTo.Value;
            Params.Bleed = (double)numericUpDownBleed.Value;

            DialogResult = DialogResult.OK;
        }

        private void numericUpDownFrom_Click(object sender, EventArgs e)
        {
            ((NumericUpDown)sender).Select(0, ((NumericUpDown)sender).Text.Length);
        }
    }
}
