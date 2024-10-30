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
    public partial class FormAddTextMark : Form
    {
        public TextMark Mark { get; set; }
        public FormAddTextMark()
        {
            InitializeComponent();
        }

        private void bnt_fontSelect_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                tb_fontName.Text = fontDialog1.Font.Name;
                nud_fontSize.Value = (decimal)fontDialog1.Font.Size;
            }
        }
    }
}
