using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Divide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PDFManipulate.Forms
{
    public partial class FormDividerParams : Form
    {
        public PdfDividerParams Params { get; set; } = new PdfDividerParams();
        //public int CountPages { get; set; } = 1;
        public FormDividerParams()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            Params.Mode = radioButtonFixed.Checked
                ? DivideModeEnum.FixedCountPages
                : DivideModeEnum.CustomCountPages;


            switch (Params.Mode)
            {
                case DivideModeEnum.FixedCountPages:
                    Params.FixedCountPages = (int)numericUpDown1.Value;
                    Close();
                    break;
                case DivideModeEnum.CustomCountPages:
                    bool res = ConvertToIntArray(textBoxCustom.Text, out int[] customs);

                    if (res)
                    {
                        Params.CustomCountPages = customs;
                        Close();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool ConvertToIntArray(string text, out int[] ints)
        {

            var listInt = new List<int>();

            var splitted = text.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
            if (splitted.Any())
            {

                bool err = false;

                foreach (var s in splitted)
                {
                    var r = int.TryParse(s, out int res);
                    if (r)
                        listInt.Add(res);
                    else
                    {
                        err = true;
                        break;
                    }
                }

                if (err == false)
                {
                    ints = listInt.ToArray();
                    return true;
                }
            }

            ints = listInt.ToArray();
            return false;
        }

        private void FormSelectCountPages_Load(object sender, EventArgs e)
        {
            Activate();
        }
    }
}
