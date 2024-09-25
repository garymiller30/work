using Job.Static.Pdf.Imposition.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Job.UserForms.PDF.ImposItems
{
    public partial class FormAddSheet : Form
    {
        public TemplateSheet Sheet { get; set; }
        public FormAddSheet()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        public FormAddSheet(TemplateSheet sheet) : this()
        {
            Sheet = sheet;

            tb_Description.Text = sheet.Description;
            nud_Width.Value = (decimal)sheet.W;
            nud_Height.Value = (decimal)sheet.H;
            nud_FieldLeft.Value = (decimal)sheet.SafeFields.Left;
            nud_FieldRight.Value = (decimal)sheet.SafeFields.Right;
            nud_FileldTop.Value = (decimal)sheet.SafeFields.Top;
            nud_FieldBottom.Value = (decimal)sheet.SafeFields.Bottom;
            nud_ExtraSpace.Value = (decimal)sheet.ExtraSpace;
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (Sheet == null)
            {
                Sheet = new TemplateSheet();
            }

            Sheet.Description = tb_Description.Text;
            Sheet.W = (double)nud_Width.Value;
            Sheet.H = (double)nud_Height.Value;
            Sheet.ExtraSpace = (double)nud_ExtraSpace.Value;
            Sheet.SafeFields.Left = (double)nud_FieldLeft.Value;
            Sheet.SafeFields.Right = (double)nud_FieldRight.Value;
            Sheet.SafeFields.Top = (double)nud_FileldTop.Value;
            Sheet.SafeFields.Bottom = (double)nud_FieldBottom.Value;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
