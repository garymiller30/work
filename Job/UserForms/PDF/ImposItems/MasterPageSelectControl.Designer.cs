namespace JobSpace.UserForms.PDF.ImposItems
{
    partial class MasterPageSelectControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MasterPageSelectControl));
            groupBox2 = new System.Windows.Forms.GroupBox();
            btn_add_page = new Krypton.Toolkit.KryptonButton();
            btn_change_margins = new Krypton.Toolkit.KryptonButton();
            b_bleed_to_margins = new Krypton.Toolkit.KryptonButton();
            ll_calc_h = new System.Windows.Forms.LinkLabel();
            ll_calc_x = new System.Windows.Forms.LinkLabel();
            cb_FileFormats = new System.Windows.Forms.ComboBox();
            nud_page_bleed = new System.Windows.Forms.NumericUpDown();
            label9 = new System.Windows.Forms.Label();
            nud_page_h = new System.Windows.Forms.NumericUpDown();
            nud_page_w = new System.Windows.Forms.NumericUpDown();
            label7 = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nud_page_bleed).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nud_page_h).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nud_page_w).BeginInit();
            SuspendLayout();
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(btn_add_page);
            groupBox2.Controls.Add(btn_change_margins);
            groupBox2.Controls.Add(b_bleed_to_margins);
            groupBox2.Controls.Add(ll_calc_h);
            groupBox2.Controls.Add(ll_calc_x);
            groupBox2.Controls.Add(cb_FileFormats);
            groupBox2.Controls.Add(nud_page_bleed);
            groupBox2.Controls.Add(label9);
            groupBox2.Controls.Add(nud_page_h);
            groupBox2.Controls.Add(nud_page_w);
            groupBox2.Controls.Add(label7);
            groupBox2.Controls.Add(label8);
            groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox2.Location = new System.Drawing.Point(0, 0);
            groupBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox2.Size = new System.Drawing.Size(393, 67);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "формат сторінки, мм";
            // 
            // btn_add_page
            // 
            btn_add_page.Location = new System.Drawing.Point(358, 17);
            btn_add_page.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btn_add_page.Name = "btn_add_page";
            btn_add_page.Size = new System.Drawing.Size(30, 30);
            btn_add_page.TabIndex = 29;
            btn_add_page.ToolTipValues.Description = "Додати на друкарський лист";
            btn_add_page.ToolTipValues.EnableToolTips = true;
            btn_add_page.ToolTipValues.Heading = "";
            btn_add_page.Values.DropDownArrowColor = System.Drawing.Color.Empty;
            btn_add_page.Values.Image = (System.Drawing.Image)resources.GetObject("btn_add_page.Values.Image");
            btn_add_page.Values.Text = "";
            btn_add_page.Click += btn_add_page_Click;
            // 
            // btn_change_margins
            // 
            btn_change_margins.Location = new System.Drawing.Point(324, 17);
            btn_change_margins.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btn_change_margins.Name = "btn_change_margins";
            btn_change_margins.Size = new System.Drawing.Size(30, 30);
            btn_change_margins.TabIndex = 28;
            btn_change_margins.ToolTipValues.Description = "Змінити зовнішні поля";
            btn_change_margins.ToolTipValues.EnableToolTips = true;
            btn_change_margins.ToolTipValues.Heading = "";
            btn_change_margins.Values.DropDownArrowColor = System.Drawing.Color.Empty;
            btn_change_margins.Values.Image = (System.Drawing.Image)resources.GetObject("btn_change_margins.Values.Image");
            btn_change_margins.Values.Text = "";
            btn_change_margins.Click += btn_change_margins_Click;
            // 
            // b_bleed_to_margins
            // 
            b_bleed_to_margins.Location = new System.Drawing.Point(289, 17);
            b_bleed_to_margins.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            b_bleed_to_margins.Name = "b_bleed_to_margins";
            b_bleed_to_margins.Size = new System.Drawing.Size(30, 30);
            b_bleed_to_margins.TabIndex = 27;
            b_bleed_to_margins.ToolTipValues.Description = "зовнішні поля = полю на підрізку";
            b_bleed_to_margins.ToolTipValues.EnableToolTips = true;
            b_bleed_to_margins.ToolTipValues.Heading = "";
            b_bleed_to_margins.Values.DropDownArrowColor = System.Drawing.Color.Empty;
            b_bleed_to_margins.Values.Text = ">";
            b_bleed_to_margins.Click += b_bleed_to_margins_Click;
            // 
            // ll_calc_h
            // 
            ll_calc_h.AutoSize = true;
            ll_calc_h.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
            ll_calc_h.Location = new System.Drawing.Point(166, 47);
            ll_calc_h.Margin = new System.Windows.Forms.Padding(0);
            ll_calc_h.Name = "ll_calc_h";
            ll_calc_h.Size = new System.Drawing.Size(25, 13);
            ll_calc_h.TabIndex = 26;
            ll_calc_h.TabStop = true;
            ll_calc_h.Text = "calc";
            ll_calc_h.LinkClicked += ll_calc_h_LinkClicked;
            // 
            // ll_calc_x
            // 
            ll_calc_x.AutoSize = true;
            ll_calc_x.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
            ll_calc_x.Location = new System.Drawing.Point(90, 47);
            ll_calc_x.Margin = new System.Windows.Forms.Padding(0);
            ll_calc_x.Name = "ll_calc_x";
            ll_calc_x.Size = new System.Drawing.Size(25, 13);
            ll_calc_x.TabIndex = 25;
            ll_calc_x.TabStop = true;
            ll_calc_x.Text = "calc";
            ll_calc_x.LinkClicked += ll_calc_x_LinkClicked;
            // 
            // cb_FileFormats
            // 
            cb_FileFormats.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cb_FileFormats.FormattingEnabled = true;
            cb_FileFormats.Location = new System.Drawing.Point(8, 22);
            cb_FileFormats.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cb_FileFormats.Name = "cb_FileFormats";
            cb_FileFormats.Size = new System.Drawing.Size(40, 23);
            cb_FileFormats.TabIndex = 20;
            cb_FileFormats.SelectedIndexChanged += cb_FileFormats_SelectedIndexChanged;
            // 
            // nud_page_bleed
            // 
            nud_page_bleed.DecimalPlaces = 1;
            nud_page_bleed.Location = new System.Drawing.Point(238, 22);
            nud_page_bleed.Margin = new System.Windows.Forms.Padding(0);
            nud_page_bleed.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nud_page_bleed.Name = "nud_page_bleed";
            nud_page_bleed.Size = new System.Drawing.Size(45, 23);
            nud_page_bleed.TabIndex = 18;
            nud_page_bleed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            nud_page_bleed.ValueChanged += nud_page_bleed_ValueChanged;
            nud_page_bleed.Click += nud_page_w_Click;
            nud_page_bleed.Enter += nud_page_w_Click;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(221, 25);
            label9.Margin = new System.Windows.Forms.Padding(0);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(16, 15);
            label9.TabIndex = 17;
            label9.Text = "О";
            // 
            // nud_page_h
            // 
            nud_page_h.DecimalPlaces = 1;
            nud_page_h.Location = new System.Drawing.Point(152, 21);
            nud_page_h.Margin = new System.Windows.Forms.Padding(0);
            nud_page_h.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nud_page_h.Name = "nud_page_h";
            nud_page_h.Size = new System.Drawing.Size(60, 23);
            nud_page_h.TabIndex = 16;
            nud_page_h.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            nud_page_h.ValueChanged += nud_page_h_ValueChanged;
            nud_page_h.Click += nud_page_w_Click;
            nud_page_h.Enter += nud_page_w_Click;
            // 
            // nud_page_w
            // 
            nud_page_w.DecimalPlaces = 1;
            nud_page_w.Location = new System.Drawing.Point(72, 21);
            nud_page_w.Margin = new System.Windows.Forms.Padding(0);
            nud_page_w.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nud_page_w.Name = "nud_page_w";
            nud_page_w.Size = new System.Drawing.Size(60, 23);
            nud_page_w.TabIndex = 15;
            nud_page_w.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            nud_page_w.ValueChanged += nud_page_w_ValueChanged;
            nud_page_w.Click += nud_page_w_Click;
            nud_page_w.Enter += nud_page_w_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(137, 25);
            label7.Margin = new System.Windows.Forms.Padding(0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(14, 15);
            label7.TabIndex = 14;
            label7.Text = "В";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(54, 25);
            label8.Margin = new System.Windows.Forms.Padding(0);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(18, 15);
            label8.TabIndex = 13;
            label8.Text = "Ш";
            // 
            // MasterPageSelectControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(groupBox2);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "MasterPageSelectControl";
            Size = new System.Drawing.Size(393, 67);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nud_page_bleed).EndInit();
            ((System.ComponentModel.ISupportInitialize)nud_page_h).EndInit();
            ((System.ComponentModel.ISupportInitialize)nud_page_w).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cb_FileFormats;
        private System.Windows.Forms.NumericUpDown nud_page_bleed;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown nud_page_h;
        private System.Windows.Forms.NumericUpDown nud_page_w;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.LinkLabel ll_calc_h;
        private System.Windows.Forms.LinkLabel ll_calc_x;
        private Krypton.Toolkit.KryptonButton b_bleed_to_margins;
        private Krypton.Toolkit.KryptonButton btn_change_margins;
        private Krypton.Toolkit.KryptonButton btn_add_page;
    }
}
