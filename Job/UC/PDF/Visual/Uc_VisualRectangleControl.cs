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

        PdfPageInfo? _pageInfo;
        public bool RectEnabled
        {
            get { return cb_rect.Checked; }
            set { cb_rect.Checked = value; }
        }
        public decimal X { get; set; } = 0;
        public decimal Y { get; set; } = 0;
        public decimal W { get; set; } = 100m;
        public decimal H { get; set; } = 100m;

        public void SetPdfPageInfo(PdfPageInfo? pageInfo)
        {
            _pageInfo = pageInfo;
        }

        public void DisableRows(params int[] rows)
        {
            uc_PositionControl1.DisableRows(rows);
        }

        public Uc_VisualRectangleControl()
        {
            InitializeComponent();
            uc_PositionControl1.OnPositionChanged += (s, p) =>
            {
                if (_pageInfo?.Trimbox == null) return;

                var x = ((decimal)_pageInfo.Trimbox.wMM()-W) * p.Item1;
                var y = ((decimal)_pageInfo.Trimbox.hMM()-H) * p.Item2;

                nud_rect_x.Value = x;
                nud_rect_y.Value = y;
                X = x;
                Y = y;
                OnRectPositionChanged(this, new PointF((float)x, (float)y));
            };
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
        private void cb_rect_CheckedChanged(object sender, EventArgs e)
        {
            panel_rect_params.Enabled = cb_rect.Checked;
            panel_rect_params.Visible = cb_rect.Checked;
            OnRectEnabledChanged(this, cb_rect.Checked);
        }
    }
}
