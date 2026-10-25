using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace JobSpace.UC.PDF.Visual
{
    public partial class Uc_PositionControl : UserControl
    {
        public EventHandler<Tuple<decimal,decimal>> OnPositionChanged = delegate { };

        Dictionary<int, Control> rows = new Dictionary<int, Control>();

        public Uc_PositionControl()
        {
            InitializeComponent();
            rows.Add(0, panel_top);
            rows.Add(1, panel_center);
            rows.Add(2, panel_bottom);
        }

        public void DisableRows(params int[] rows)
        {
            foreach (var row in rows)
            {
                if (this.rows.ContainsKey(row))
                {
                    this.rows[row].Visible = false;
                }
            }
        }

        private void bnt_top_left_Click(object sender, EventArgs e)
        {
            OnPositionChanged?.Invoke(this, new Tuple<decimal, decimal>(0, 0));
        }

        private void btn_top_center_Click(object sender, EventArgs e)
        {
            OnPositionChanged?.Invoke(this, new Tuple<decimal, decimal>(0.5m, 0));
        }

        private void bnt_top_right_Click(object sender, EventArgs e)
        {
            OnPositionChanged?.Invoke(this, new Tuple<decimal, decimal>(1m, 0));
        }

        private void btn_left_center_Click(object sender, EventArgs e)
        {
            OnPositionChanged?.Invoke(this, new Tuple<decimal, decimal>(0, 0.5m));
        }

        private void btn_center_Click(object sender, EventArgs e)
        {
            OnPositionChanged?.Invoke(this, new Tuple<decimal, decimal>(0.5m, 0.5m));
        }

        private void btn_right_center_Click(object sender, EventArgs e)
        {
            OnPositionChanged?.Invoke(this, new Tuple<decimal, decimal>(1m, 0.5m));
        }

        private void btn_bottom_left_Click(object sender, EventArgs e)
        {
            OnPositionChanged?.Invoke(this, new Tuple<decimal, decimal>(0, 1m));
        }

        private void btn_bottom_center_Click(object sender, EventArgs e)
        {
            OnPositionChanged?.Invoke(this, new Tuple<decimal, decimal>(0.5m, 1m));
        }

        private void btn_bottom_right_Click(object sender, EventArgs e)
        {
            OnPositionChanged?.Invoke(this, new Tuple<decimal, decimal>(1m, 1m));
        }
    }
}
