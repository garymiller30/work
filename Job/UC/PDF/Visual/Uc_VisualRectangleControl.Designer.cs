namespace JobSpace.UC.PDF.Visual
{
    partial class Uc_VisualRectangleControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel_rect_params = new System.Windows.Forms.Panel();
            uc_PositionControl1 = new Uc_PositionControl();
            label4 = new System.Windows.Forms.Label();
            nud_rect_y = new System.Windows.Forms.NumericUpDown();
            label3 = new System.Windows.Forms.Label();
            nud_rect_x = new System.Windows.Forms.NumericUpDown();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            nud_rect_h = new System.Windows.Forms.NumericUpDown();
            nud_rect_w = new System.Windows.Forms.NumericUpDown();
            cb_rect = new System.Windows.Forms.CheckBox();
            flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            panel_rect_params.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nud_rect_y).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nud_rect_x).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nud_rect_h).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nud_rect_w).BeginInit();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel_rect_params
            // 
            panel_rect_params.Controls.Add(uc_PositionControl1);
            panel_rect_params.Controls.Add(label4);
            panel_rect_params.Controls.Add(nud_rect_y);
            panel_rect_params.Controls.Add(label3);
            panel_rect_params.Controls.Add(nud_rect_x);
            panel_rect_params.Controls.Add(label2);
            panel_rect_params.Controls.Add(label1);
            panel_rect_params.Controls.Add(nud_rect_h);
            panel_rect_params.Controls.Add(nud_rect_w);
            panel_rect_params.Enabled = false;
            panel_rect_params.Location = new System.Drawing.Point(4, 28);
            panel_rect_params.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panel_rect_params.Name = "panel_rect_params";
            panel_rect_params.Size = new System.Drawing.Size(194, 152);
            panel_rect_params.TabIndex = 3;
            panel_rect_params.Visible = false;
            // 
            // uc_PositionControl1
            // 
            uc_PositionControl1.AutoSize = true;
            uc_PositionControl1.Location = new System.Drawing.Point(89, 46);
            uc_PositionControl1.Name = "uc_PositionControl1";
            uc_PositionControl1.Size = new System.Drawing.Size(100, 100);
            uc_PositionControl1.TabIndex = 9;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(4, 92);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(38, 15);
            label4.TabIndex = 8;
            label4.Text = "Y, мм";
            // 
            // nud_rect_y
            // 
            nud_rect_y.DecimalPlaces = 1;
            nud_rect_y.Location = new System.Drawing.Point(4, 111);
            nud_rect_y.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            nud_rect_y.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nud_rect_y.Name = "nud_rect_y";
            nud_rect_y.Size = new System.Drawing.Size(78, 23);
            nud_rect_y.TabIndex = 7;
            nud_rect_y.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            nud_rect_y.ValueChanged += nud_rect_x_ValueChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(4, 45);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(38, 15);
            label3.TabIndex = 6;
            label3.Text = "X, мм";
            // 
            // nud_rect_x
            // 
            nud_rect_x.DecimalPlaces = 1;
            nud_rect_x.Location = new System.Drawing.Point(4, 63);
            nud_rect_x.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            nud_rect_x.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nud_rect_x.Name = "nud_rect_x";
            nud_rect_x.Size = new System.Drawing.Size(78, 23);
            nud_rect_x.TabIndex = 5;
            nud_rect_x.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            nud_rect_x.ValueChanged += nud_rect_x_ValueChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(88, 1);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(68, 15);
            label2.TabIndex = 4;
            label2.Text = "висота, мм";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(4, 1);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(76, 15);
            label1.TabIndex = 3;
            label1.Text = "ширина, мм";
            // 
            // nud_rect_h
            // 
            nud_rect_h.DecimalPlaces = 1;
            nud_rect_h.Location = new System.Drawing.Point(88, 19);
            nud_rect_h.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            nud_rect_h.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nud_rect_h.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nud_rect_h.Name = "nud_rect_h";
            nud_rect_h.Size = new System.Drawing.Size(75, 23);
            nud_rect_h.TabIndex = 2;
            nud_rect_h.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            nud_rect_h.Value = new decimal(new int[] { 100, 0, 0, 0 });
            nud_rect_h.ValueChanged += nud_rect_w_ValueChanged;
            // 
            // nud_rect_w
            // 
            nud_rect_w.DecimalPlaces = 1;
            nud_rect_w.Location = new System.Drawing.Point(4, 19);
            nud_rect_w.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            nud_rect_w.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nud_rect_w.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nud_rect_w.Name = "nud_rect_w";
            nud_rect_w.Size = new System.Drawing.Size(78, 23);
            nud_rect_w.TabIndex = 1;
            nud_rect_w.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            nud_rect_w.Value = new decimal(new int[] { 100, 0, 0, 0 });
            nud_rect_w.ValueChanged += nud_rect_w_ValueChanged;
            // 
            // cb_rect
            // 
            cb_rect.AutoSize = true;
            cb_rect.Location = new System.Drawing.Point(4, 3);
            cb_rect.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cb_rect.Name = "cb_rect";
            cb_rect.Size = new System.Drawing.Size(99, 19);
            cb_rect.TabIndex = 2;
            cb_rect.Text = "прямокутник";
            cb_rect.UseVisualStyleBackColor = true;
            cb_rect.CheckedChanged += cb_rect_CheckedChanged;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.Controls.Add(cb_rect);
            flowLayoutPanel1.Controls.Add(panel_rect_params);
            flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new System.Drawing.Size(202, 184);
            flowLayoutPanel1.TabIndex = 4;
            // 
            // Uc_VisualRectangleControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(flowLayoutPanel1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "Uc_VisualRectangleControl";
            Size = new System.Drawing.Size(202, 184);
            panel_rect_params.ResumeLayout(false);
            panel_rect_params.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nud_rect_y).EndInit();
            ((System.ComponentModel.ISupportInitialize)nud_rect_x).EndInit();
            ((System.ComponentModel.ISupportInitialize)nud_rect_h).EndInit();
            ((System.ComponentModel.ISupportInitialize)nud_rect_w).EndInit();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel_rect_params;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nud_rect_y;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nud_rect_x;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nud_rect_h;
        private System.Windows.Forms.NumericUpDown nud_rect_w;
        private System.Windows.Forms.CheckBox cb_rect;
        private Uc_PositionControl uc_PositionControl1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}
