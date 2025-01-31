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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_add_page = new System.Windows.Forms.Button();
            this.cb_FileFormats = new System.Windows.Forms.ComboBox();
            this.nud_page_bleed = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.nud_page_h = new System.Windows.Forms.NumericUpDown();
            this.nud_page_w = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btn_change_margins = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_page_bleed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_page_h)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_page_w)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_change_margins);
            this.groupBox2.Controls.Add(this.btn_add_page);
            this.groupBox2.Controls.Add(this.cb_FileFormats);
            this.groupBox2.Controls.Add(this.nud_page_bleed);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.nud_page_h);
            this.groupBox2.Controls.Add(this.nud_page_w);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(337, 71);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "формат сторінки, мм";
            // 
            // btn_add_page
            // 
            this.btn_add_page.FlatAppearance.BorderSize = 0;
            this.btn_add_page.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_add_page.Image = ((System.Drawing.Image)(resources.GetObject("btn_add_page.Image")));
            this.btn_add_page.Location = new System.Drawing.Point(305, 29);
            this.btn_add_page.Name = "btn_add_page";
            this.btn_add_page.Size = new System.Drawing.Size(26, 26);
            this.btn_add_page.TabIndex = 22;
            this.btn_add_page.UseVisualStyleBackColor = true;
            this.btn_add_page.Click += new System.EventHandler(this.btn_add_page_Click);
            // 
            // cb_FileFormats
            // 
            this.cb_FileFormats.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_FileFormats.FormattingEnabled = true;
            this.cb_FileFormats.Location = new System.Drawing.Point(7, 34);
            this.cb_FileFormats.Name = "cb_FileFormats";
            this.cb_FileFormats.Size = new System.Drawing.Size(46, 21);
            this.cb_FileFormats.TabIndex = 20;
            this.cb_FileFormats.SelectedIndexChanged += new System.EventHandler(this.cb_FileFormats_SelectedIndexChanged);
            // 
            // nud_page_bleed
            // 
            this.nud_page_bleed.DecimalPlaces = 1;
            this.nud_page_bleed.Location = new System.Drawing.Point(191, 35);
            this.nud_page_bleed.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nud_page_bleed.Name = "nud_page_bleed";
            this.nud_page_bleed.Size = new System.Drawing.Size(50, 20);
            this.nud_page_bleed.TabIndex = 18;
            this.nud_page_bleed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nud_page_bleed.ValueChanged += new System.EventHandler(this.nud_page_bleed_ValueChanged);
            this.nud_page_bleed.Click += new System.EventHandler(this.nud_page_w_Click);
            this.nud_page_bleed.Enter += new System.EventHandler(this.nud_page_w_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(188, 19);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(61, 13);
            this.label9.TabIndex = 17;
            this.label9.Text = "на підрізку";
            // 
            // nud_page_h
            // 
            this.nud_page_h.DecimalPlaces = 1;
            this.nud_page_h.Location = new System.Drawing.Point(124, 35);
            this.nud_page_h.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nud_page_h.Name = "nud_page_h";
            this.nud_page_h.Size = new System.Drawing.Size(60, 20);
            this.nud_page_h.TabIndex = 16;
            this.nud_page_h.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nud_page_h.ValueChanged += new System.EventHandler(this.nud_page_h_ValueChanged);
            this.nud_page_h.Click += new System.EventHandler(this.nud_page_w_Click);
            this.nud_page_h.Enter += new System.EventHandler(this.nud_page_w_Click);
            // 
            // nud_page_w
            // 
            this.nud_page_w.DecimalPlaces = 1;
            this.nud_page_w.Location = new System.Drawing.Point(59, 35);
            this.nud_page_w.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nud_page_w.Name = "nud_page_w";
            this.nud_page_w.Size = new System.Drawing.Size(60, 20);
            this.nud_page_w.TabIndex = 15;
            this.nud_page_w.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nud_page_w.ValueChanged += new System.EventHandler(this.nud_page_w_ValueChanged);
            this.nud_page_w.Click += new System.EventHandler(this.nud_page_w_Click);
            this.nud_page_w.Enter += new System.EventHandler(this.nud_page_w_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(131, 19);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "висота";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(67, 19);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "ширина";
            // 
            // btn_change_margins
            // 
            this.btn_change_margins.Image = ((System.Drawing.Image)(resources.GetObject("btn_change_margins.Image")));
            this.btn_change_margins.Location = new System.Drawing.Point(255, 30);
            this.btn_change_margins.Name = "btn_change_margins";
            this.btn_change_margins.Size = new System.Drawing.Size(26, 26);
            this.btn_change_margins.TabIndex = 23;
            this.btn_change_margins.UseVisualStyleBackColor = true;
            this.btn_change_margins.Click += new System.EventHandler(this.btn_change_margins_Click);
            // 
            // MasterPageSelectControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Name = "MasterPageSelectControl";
            this.Size = new System.Drawing.Size(337, 71);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_page_bleed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_page_h)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_page_w)).EndInit();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.Button btn_add_page;
        private System.Windows.Forms.Button btn_change_margins;
    }
}
