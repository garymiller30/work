using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDFManipulate.Forms
{
    public partial class FormSelectCountPages : Form
    {
        public int[] Pages { get; set; }

        //public int CountPages { get; set; } = 1;
        public FormSelectCountPages()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {

            if (radioButtonFixed.Checked)
            {
                Pages = new int[1] { (int)numericUpDown1.Value };
            }
            else
            {
                bool res = ConvertToIntArray(textBoxCustom.Text, out int[] customs);

                if (!res)
                {
                    return;
                }

                Pages = customs;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private bool ConvertToIntArray(string text, out int[] ints)
        {

            var listInt = new List<int>();

            var splitted = text.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (splitted.Any())
            {

                bool err = false;

                foreach (var s in splitted)
                {
                    if (s.Contains('-')) // вказаний діапазон, 
                    {
                        var diapazon = s.Split('-');

                        var rFrom = int.TryParse(diapazon[0], out int from);
                        var rTo = int.TryParse(diapazon[1], out int to);

                        if (rFrom == rTo && rTo)
                        {
                            for (int i = from; i <= to; i++)
                            {
                                listInt.Add(i); 
                            }
                        }

                    }
                    else
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


    }
}
