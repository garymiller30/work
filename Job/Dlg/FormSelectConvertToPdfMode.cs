using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Krypton.Toolkit;
using Interfaces;
using PDFManipulate.Converters;

namespace Job.Dlg
{
    public sealed partial class FormSelectConvertToPdfMode : KryptonForm
    {
        public ConvertModeEnum ConvertMode { get; private set; } = ConvertModeEnum.NotAssigned;
        public bool MoveToTrash { get; private set; }

        public double TrimBox { get; private set; } = 0;

        public FormSelectConvertToPdfMode()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        public FormSelectConvertToPdfMode(IPdfConverterSettings settings):this()
        {
            kryptonCheckBoxMoveToTrash.Checked = settings.MoveOriginalsToTrash;
        }

        private void kryptonButtonMultiple_Click(object sender, EventArgs e)
        {
            GetParameters();

            ConvertMode = ConvertModeEnum.MultipleFiles;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void GetParameters()
        {
            MoveToTrash = kryptonCheckBoxMoveToTrash.Checked;

            TrimBox = kryptonRadioButtonNone.Checked ? 0 :
                kryptonRadioButton1mm.Checked ? 1 :
                kryptonRadioButton2mm.Checked ? 2 :
                kryptonRadioButton3mm.Checked ? 3 : 5;

        }

        private void kryptonButtonSingle_Click(object sender, EventArgs e)
        {
            GetParameters();

            ConvertMode = ConvertModeEnum.SingleFile;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
