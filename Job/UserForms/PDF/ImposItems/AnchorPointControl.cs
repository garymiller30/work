using JobSpace.Static.Pdf.Imposition.Models;
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
    public partial class AnchorPointControl : UserControl
    {
        public EventHandler<AnchorPoint> AnchorPointChanged { get;set;} = delegate{};

    public AnchorPointControl()
        {
            InitializeComponent();
        }

        private void rb_tl_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
                AnchorPointChanged(this,AnchorPoint.TopLeft);
        }

        private void rb_tc_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
                AnchorPointChanged(this, AnchorPoint.TopCenter);
        }

        private void rb_tr_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
                AnchorPointChanged(this, AnchorPoint.TopRight);
        }

        private void rb_cl_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
                AnchorPointChanged(this, AnchorPoint.LeftCenter);
        }

        private void rb_cc_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
                AnchorPointChanged(this, AnchorPoint.Center);
        }

        private void rb_cr_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
                AnchorPointChanged(this, AnchorPoint.RightCenter);
        }

        private void rb_bl_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
                AnchorPointChanged(this, AnchorPoint.BottomLeft);
        }

        private void rb_bc_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
                AnchorPointChanged(this, AnchorPoint.BottomCenter);
        }

        private void rb_br_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
                AnchorPointChanged(this, AnchorPoint.BottomRight);
        }

        public void SetAnchor(AnchorPoint ap)
        {
            switch (ap)
            {
                case AnchorPoint.TopLeft:
                    rb_tl.Checked = true;
                    break;
                case AnchorPoint.TopRight:
                    rb_tr.Checked = true;
                    break;
                case AnchorPoint.TopCenter:
                    rb_tc.Checked = true;
                    break;
                case AnchorPoint.BottomLeft:
                    rb_bl.Checked = true;
                    break;
                case AnchorPoint.BottomRight:
                    rb_br.Checked = true;
                    break;
                case AnchorPoint.BottomCenter:
                    rb_bc.Checked = true;
                    break;
                case AnchorPoint.LeftCenter:
                    rb_lc.Checked = true;
                    break;
                case AnchorPoint.RightCenter:
                    rb_rc.Checked = true;
                    break;
                case AnchorPoint.Center:
                    rb_cc.Checked = true;
                    break;
                default:
                    break;
            }

            SetDelegates();
        }

        private void SetDelegates()
        {
            rb_tl.CheckedChanged += rb_tl_CheckedChanged;
            rb_tr.CheckedChanged += rb_tr_CheckedChanged;
            rb_tc.CheckedChanged += rb_tc_CheckedChanged;
            rb_bl.CheckedChanged += rb_bl_CheckedChanged;
            rb_br.CheckedChanged += rb_br_CheckedChanged;
            rb_bc.CheckedChanged += rb_bc_CheckedChanged;
            rb_lc.CheckedChanged += rb_cl_CheckedChanged;
            rb_rc.CheckedChanged += rb_cr_CheckedChanged;
            rb_cc.CheckedChanged += rb_cc_CheckedChanged;
        }
    }
}
