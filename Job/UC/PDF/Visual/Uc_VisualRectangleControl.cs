using JobSpace.Static.Pdf.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.UC.PDF.Visual
{
    public partial class Uc_VisualRectangleControl : UserControl
    {

        public EventHandler<SizeF> OnRectSizeChanged = delegate { };
        public EventHandler<PointF> OnRectPositionChanged = delegate { };
        public EventHandler<bool> OnRectEnabledChanged = delegate { };

        PdfPageInfo _pageInfo;
        public bool RectEnabled
        {
            get { return cb_rect.Checked; }
            set { cb_rect.Checked = value; }
        }
        public decimal X { get; set; }
        public decimal Y { get; set; }
        public decimal W { get; set; }
        public decimal H { get; set; }

        public void SetPdfPageInfo(PdfPageInfo pageInfo)
        {
            _pageInfo = pageInfo;
        }

        public Uc_VisualRectangleControl()
        {
            InitializeComponent();
        }

        private void nud_rect_w_ValueChanged(object sender, EventArgs e)
        {
            W = nud_rect_w.Value;
            H = nud_rect_h.Value;
            OnRectSizeChanged(this, new SizeF((float)W, (float)H));
        }

        private void nud_rect_x_ValueChanged(object sender, EventArgs e)
        {
            X = nud_rect_x.Value;
            Y = nud_rect_y.Value;

            OnRectPositionChanged(this, new PointF((float)X, (float)Y));
        }

        private void bnt_top_left_Click(object sender, EventArgs e)
        {
            X = 0;
            Y = 0;
            nud_rect_x.Value = X;
            nud_rect_y.Value = Y;
            OnRectPositionChanged(this, new PointF((float)X, (float)Y));
        }

        private void btn_top_center_Click(object sender, EventArgs e)
        {
            X = (((decimal)_pageInfo.Trimbox.wMM() - W) / 2);
            Y = 0;
            nud_rect_x.Value = X;
            nud_rect_y.Value = Y;
            OnRectPositionChanged(this, new PointF((float)X, (float)Y));
        }

        private void bnt_top_right_Click(object sender, EventArgs e)
        {
            X = ((decimal)_pageInfo.Trimbox.wMM() - W);
            Y = 0;
            nud_rect_x.Value = X;
            nud_rect_y.Value = Y;
            OnRectPositionChanged(this, new PointF((float)X, (float)Y));
        }

        private void btn_left_center_Click(object sender, EventArgs e)
        {
            X = 0;
            Y = (((decimal)_pageInfo.Trimbox.hMM() - H) / 2);
            nud_rect_x.Value = X;
            nud_rect_y.Value = Y;
            OnRectPositionChanged(this, new PointF((float)X, (float)Y));
        }

        private void btn_center_Click(object sender, EventArgs e)
        {
            X = (((decimal)_pageInfo.Trimbox.wMM() - W) / 2);
            Y = (((decimal)_pageInfo.Trimbox.hMM() - H) / 2);
            nud_rect_x.Value = X;
            nud_rect_y.Value = Y;
            OnRectPositionChanged(this, new PointF((float)X, (float)Y));
        }

        private void btn_right_center_Click(object sender, EventArgs e)
        {
            X = ((decimal)_pageInfo.Trimbox.wMM() - W);
            Y = (((decimal)_pageInfo.Trimbox.hMM() - H) / 2);
            nud_rect_x.Value = X;
            nud_rect_y.Value = Y;
            OnRectPositionChanged(this, new PointF((float)X, (float)Y));
        }

        private void bnt_bottom_left_Click(object sender, EventArgs e)
        {
            X = 0;
            Y = ((decimal)_pageInfo.Trimbox.hMM() - H);
            nud_rect_x.Value = X;
            nud_rect_y.Value = Y;
            OnRectPositionChanged(this, new PointF((float)X, (float)Y));
        }

        private void btn_bottom_center_Click(object sender, EventArgs e)
        {
            X = (((decimal)_pageInfo.Trimbox.wMM() - W) / 2);
            Y = ((decimal)_pageInfo.Trimbox.hMM() - H);
            nud_rect_x.Value = X;
            nud_rect_y.Value = Y;
            OnRectPositionChanged(this, new PointF((float)X, (float)Y));
        }

        private void btn_bottom_right_Click(object sender, EventArgs e)
        {
            X = ((decimal)_pageInfo.Trimbox.wMM() - W);
            Y = ((decimal)_pageInfo.Trimbox.hMM() - H);
            nud_rect_x.Value = X;
            nud_rect_y.Value = Y;
            OnRectPositionChanged(this, new PointF((float)X, (float)Y));
        }

        private void cb_rect_CheckedChanged(object sender, EventArgs e)
        {
            panel_rect_params.Enabled = cb_rect.Checked;
            OnRectEnabledChanged(this, cb_rect.Checked);
        }
    }
}
