using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static JobSpace.UserForms.PDF.ImposItems.BindingSimpleOneFormatDifferentCount;

namespace JobSpace.UserForms.PDF.ImposItems
{
    public partial class FormSameFormatResult : Form
    {
        public FormSameFormatResult()
        {
            InitializeComponent();
        }

        internal void SetResult(CalcResult result)
        {
            textBox1.Text = result.SheetCount.ToString();
            textBox2.Text = result.TotalCount.ToString();

            objectListView1.AddObjects(result.Files);
        }
    }
}
