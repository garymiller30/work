using JobSpace.Static.Pdf.Create.CollatingPageMark;
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
    public partial class FormCreateCollatingPageMark : Form
    {
        public CreateCollatingPageMarkParams CreatePageCollationMarksParam { get;set;} = new CreateCollatingPageMarkParams();
        public FormCreateCollatingPageMark()
        {
            InitializeComponent();
            cb_position.DataSource = Enum.GetValues(typeof(PageCollatingMarkPositionEnum));
            DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CreatePageCollationMarksParam.X = (double)nud_x.Value;
            CreatePageCollationMarksParam.Y = (double)nud_y.Value;
            CreatePageCollationMarksParam.MarkWidth = (double)nud_width.Value;
            CreatePageCollationMarksParam.MarkHeight = (double)nud_height.Value;
            CreatePageCollationMarksParam.PathLen = (double)nud_len.Value;
            CreatePageCollationMarksParam.Position = (PageCollatingMarkPositionEnum)cb_position.SelectedItem;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
