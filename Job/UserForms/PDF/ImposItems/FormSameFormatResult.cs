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
        CalcResult _result;

        public FormSameFormatResult()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        internal void SetResult(CalcResult result)
        {
            _result = result;
            SetInfo();

            objectListView1.AddObjects(result.Files);
        }

        private void SetInfo()
        {
            textBox1.Text = _result.SheetCount.ToString();
            textBox2.Text = _result.TotalCount.ToString();
            tb_freeSpace.Text = _result.FreeCount.ToString();
        }

        //+1
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObject is FileResult info)
            {
                if (_result.FreeCount == 0) {

                    MessageBox.Show("Немає вільних місць на листі");
                    return;
                }

                _result.FreeCount--;
                info.PagesOnSheet++;

                _result.Recalc();

                objectListView1.RefreshObjects(_result.Files);
                SetInfo();
            }
        }
        //-1
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObject is FileResult info)
            {
                if (info.PagesOnSheet == 1){
                    MessageBox.Show("Видалення останньої позиції з листа неможливе");
                    return; 
                    }

                _result.FreeCount++;
                info.PagesOnSheet--;
                _result.Recalc();
                objectListView1.RefreshObjects(_result.Files);
                SetInfo();
            }
        }

        private void btn_close_and_apply_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            Close();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
