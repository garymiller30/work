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
    public partial class SelectMarkSideControl : UserControl
    {
        public SelectMarkSideControl()
        {
            InitializeComponent();
        }

        public void SetParameters(PdfMarkParameters parameters)
        {
            cb_f_single_side.Checked = parameters.Front.SingleSide;
            cb_f_sheetwise.Checked = parameters.Front.Sheetwise;
            cb_f_workandturn.Checked = parameters.Front.WorkAndTurn;
            cb_f_perfecting.Checked = parameters.Front.Perfecting;

            cb_b_perfecting.Checked = parameters.Back.Perfecting;
            cb_b_workandturn.Checked = parameters.Back.WorkAndTurn;
            cb_b_sheetwise.Checked = parameters.Back.Sheetwise;
        }

        public void GetParameters(PdfMarkParameters parameters)
        {
            parameters.Front.SingleSide = cb_f_single_side.Checked;
            parameters.Front.Sheetwise = cb_f_sheetwise.Checked;
            parameters.Front.WorkAndTurn = cb_f_workandturn.Checked;
            parameters.Front.Perfecting = cb_f_perfecting.Checked;
            parameters.Back.Perfecting = cb_b_perfecting.Checked;
            parameters.Back.WorkAndTurn = cb_b_workandturn.Checked;
            parameters.Back.Sheetwise = cb_b_sheetwise.Checked;
        }
    }
}
