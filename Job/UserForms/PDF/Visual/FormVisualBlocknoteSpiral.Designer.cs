namespace JobSpace.UserForms.PDF.Visual
{
    partial class FormVisualBlocknoteSpiral
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormVisualBlocknoteSpiral));
            this.btn_ok = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cb_spiral_files = new System.Windows.Forms.ComboBox();
            this.cb_place = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_files = new System.Windows.Forms.ComboBox();
            this.label_total_pages = new System.Windows.Forms.Label();
            this.nud_page_number = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel_rect_params = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_center = new System.Windows.Forms.Button();
            this.btn_bottom_right = new System.Windows.Forms.Button();
            this.bnt_top_left = new System.Windows.Forms.Button();
            this.btn_bottom_center = new System.Windows.Forms.Button();
            this.btn_top_center = new System.Windows.Forms.Button();
            this.bnt_bottom_left = new System.Windows.Forms.Button();
            this.bnt_top_right = new System.Windows.Forms.Button();
            this.btn_right_center = new System.Windows.Forms.Button();
            this.btn_left_center = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.nud_rect_y = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nud_rect_x = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nud_rect_h = new System.Windows.Forms.NumericUpDown();
            this.nud_rect_w = new System.Windows.Forms.NumericUpDown();
            this.cb_rect = new System.Windows.Forms.CheckBox();
            this.cb_fit_to_panel = new System.Windows.Forms.CheckBox();
            this.uc_PreviewControl1 = new JobSpace.UC.Uc_PreviewControl();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_page_number)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.panel_rect_params.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_rect_y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_rect_x)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_rect_h)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_rect_w)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_ok
            // 
            this.btn_ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_ok.Location = new System.Drawing.Point(69, 500);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(118, 42);
            this.btn_ok.TabIndex = 0;
            this.btn_ok.Text = "OK";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cb_spiral_files);
            this.groupBox5.Location = new System.Drawing.Point(12, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(256, 55);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "файл пружинки";
            // 
            // cb_spiral_files
            // 
            this.cb_spiral_files.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_spiral_files.FormattingEnabled = true;
            this.cb_spiral_files.Location = new System.Drawing.Point(6, 19);
            this.cb_spiral_files.Name = "cb_spiral_files";
            this.cb_spiral_files.Size = new System.Drawing.Size(244, 21);
            this.cb_spiral_files.TabIndex = 0;
            this.cb_spiral_files.SelectedIndexChanged += new System.EventHandler(this.cb_files_SelectedIndexChanged);
            // 
            // cb_place
            // 
            this.cb_place.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_place.FormattingEnabled = true;
            this.cb_place.Location = new System.Drawing.Point(6, 19);
            this.cb_place.Name = "cb_place";
            this.cb_place.Size = new System.Drawing.Size(244, 21);
            this.cb_place.TabIndex = 0;
            this.cb_place.SelectedIndexChanged += new System.EventHandler(this.cb_place_SelectedIndexChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cb_place);
            this.groupBox4.Location = new System.Drawing.Point(12, 73);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(256, 55);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "розташування пружини";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cb_files);
            this.groupBox1.Controls.Add(this.label_total_pages);
            this.groupBox1.Controls.Add(this.nud_page_number);
            this.groupBox1.Location = new System.Drawing.Point(12, 134);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(256, 51);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "сторінка";
            // 
            // cb_files
            // 
            this.cb_files.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_files.FormattingEnabled = true;
            this.cb_files.Location = new System.Drawing.Point(11, 18);
            this.cb_files.Name = "cb_files";
            this.cb_files.Size = new System.Drawing.Size(154, 21);
            this.cb_files.TabIndex = 2;
            this.cb_files.SelectedIndexChanged += new System.EventHandler(this.cb_files_SelectedIndexChanged_1);
            // 
            // label_total_pages
            // 
            this.label_total_pages.AutoSize = true;
            this.label_total_pages.Location = new System.Drawing.Point(219, 21);
            this.label_total_pages.Name = "label_total_pages";
            this.label_total_pages.Size = new System.Drawing.Size(27, 13);
            this.label_total_pages.TabIndex = 1;
            this.label_total_pages.Text = "/ 00";
            // 
            // nud_page_number
            // 
            this.nud_page_number.Location = new System.Drawing.Point(171, 19);
            this.nud_page_number.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_page_number.Name = "nud_page_number";
            this.nud_page_number.Size = new System.Drawing.Size(42, 20);
            this.nud_page_number.TabIndex = 0;
            this.nud_page_number.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nud_page_number.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_page_number.ValueChanged += new System.EventHandler(this.nud_page_number_ValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panel_rect_params);
            this.groupBox2.Controls.Add(this.cb_rect);
            this.groupBox2.Location = new System.Drawing.Point(12, 191);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(256, 267);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "додатково";
            // 
            // panel_rect_params
            // 
            this.panel_rect_params.Controls.Add(this.panel1);
            this.panel_rect_params.Controls.Add(this.label4);
            this.panel_rect_params.Controls.Add(this.nud_rect_y);
            this.panel_rect_params.Controls.Add(this.label3);
            this.panel_rect_params.Controls.Add(this.nud_rect_x);
            this.panel_rect_params.Controls.Add(this.label2);
            this.panel_rect_params.Controls.Add(this.label1);
            this.panel_rect_params.Controls.Add(this.nud_rect_h);
            this.panel_rect_params.Controls.Add(this.nud_rect_w);
            this.panel_rect_params.Enabled = false;
            this.panel_rect_params.Location = new System.Drawing.Point(6, 41);
            this.panel_rect_params.Name = "panel_rect_params";
            this.panel_rect_params.Size = new System.Drawing.Size(244, 220);
            this.panel_rect_params.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btn_center);
            this.panel1.Controls.Add(this.btn_bottom_right);
            this.panel1.Controls.Add(this.bnt_top_left);
            this.panel1.Controls.Add(this.btn_bottom_center);
            this.panel1.Controls.Add(this.btn_top_center);
            this.panel1.Controls.Add(this.bnt_bottom_left);
            this.panel1.Controls.Add(this.bnt_top_right);
            this.panel1.Controls.Add(this.btn_right_center);
            this.panel1.Controls.Add(this.btn_left_center);
            this.panel1.Location = new System.Drawing.Point(25, 99);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(86, 86);
            this.panel1.TabIndex = 18;
            // 
            // btn_center
            // 
            this.btn_center.Image = ((System.Drawing.Image)(resources.GetObject("btn_center.Image")));
            this.btn_center.Location = new System.Drawing.Point(31, 30);
            this.btn_center.Margin = new System.Windows.Forms.Padding(2);
            this.btn_center.Name = "btn_center";
            this.btn_center.Size = new System.Drawing.Size(23, 23);
            this.btn_center.TabIndex = 13;
            this.btn_center.UseVisualStyleBackColor = true;
            this.btn_center.Click += new System.EventHandler(this.btn_center_Click);
            // 
            // btn_bottom_right
            // 
            this.btn_bottom_right.Image = ((System.Drawing.Image)(resources.GetObject("btn_bottom_right.Image")));
            this.btn_bottom_right.Location = new System.Drawing.Point(58, 57);
            this.btn_bottom_right.Margin = new System.Windows.Forms.Padding(2);
            this.btn_bottom_right.Name = "btn_bottom_right";
            this.btn_bottom_right.Size = new System.Drawing.Size(23, 23);
            this.btn_bottom_right.TabIndex = 17;
            this.btn_bottom_right.UseVisualStyleBackColor = true;
            this.btn_bottom_right.Click += new System.EventHandler(this.btn_bottom_right_Click);
            // 
            // bnt_top_left
            // 
            this.bnt_top_left.Image = ((System.Drawing.Image)(resources.GetObject("bnt_top_left.Image")));
            this.bnt_top_left.Location = new System.Drawing.Point(3, 3);
            this.bnt_top_left.Margin = new System.Windows.Forms.Padding(2);
            this.bnt_top_left.Name = "bnt_top_left";
            this.bnt_top_left.Size = new System.Drawing.Size(23, 23);
            this.bnt_top_left.TabIndex = 9;
            this.bnt_top_left.UseVisualStyleBackColor = true;
            this.bnt_top_left.Click += new System.EventHandler(this.bnt_top_left_Click);
            // 
            // btn_bottom_center
            // 
            this.btn_bottom_center.Image = ((System.Drawing.Image)(resources.GetObject("btn_bottom_center.Image")));
            this.btn_bottom_center.Location = new System.Drawing.Point(31, 57);
            this.btn_bottom_center.Margin = new System.Windows.Forms.Padding(2);
            this.btn_bottom_center.Name = "btn_bottom_center";
            this.btn_bottom_center.Size = new System.Drawing.Size(23, 23);
            this.btn_bottom_center.TabIndex = 16;
            this.btn_bottom_center.UseVisualStyleBackColor = true;
            this.btn_bottom_center.Click += new System.EventHandler(this.btn_bottom_center_Click);
            // 
            // btn_top_center
            // 
            this.btn_top_center.Image = ((System.Drawing.Image)(resources.GetObject("btn_top_center.Image")));
            this.btn_top_center.Location = new System.Drawing.Point(31, 3);
            this.btn_top_center.Margin = new System.Windows.Forms.Padding(2);
            this.btn_top_center.Name = "btn_top_center";
            this.btn_top_center.Size = new System.Drawing.Size(23, 23);
            this.btn_top_center.TabIndex = 10;
            this.btn_top_center.UseVisualStyleBackColor = true;
            this.btn_top_center.Click += new System.EventHandler(this.btn_top_center_Click);
            // 
            // bnt_bottom_left
            // 
            this.bnt_bottom_left.Image = ((System.Drawing.Image)(resources.GetObject("bnt_bottom_left.Image")));
            this.bnt_bottom_left.Location = new System.Drawing.Point(3, 57);
            this.bnt_bottom_left.Margin = new System.Windows.Forms.Padding(2);
            this.bnt_bottom_left.Name = "bnt_bottom_left";
            this.bnt_bottom_left.Size = new System.Drawing.Size(23, 23);
            this.bnt_bottom_left.TabIndex = 15;
            this.bnt_bottom_left.UseVisualStyleBackColor = true;
            this.bnt_bottom_left.Click += new System.EventHandler(this.bnt_bottom_left_Click);
            // 
            // bnt_top_right
            // 
            this.bnt_top_right.Image = ((System.Drawing.Image)(resources.GetObject("bnt_top_right.Image")));
            this.bnt_top_right.Location = new System.Drawing.Point(58, 3);
            this.bnt_top_right.Margin = new System.Windows.Forms.Padding(2);
            this.bnt_top_right.Name = "bnt_top_right";
            this.bnt_top_right.Size = new System.Drawing.Size(23, 23);
            this.bnt_top_right.TabIndex = 11;
            this.bnt_top_right.UseVisualStyleBackColor = true;
            this.bnt_top_right.Click += new System.EventHandler(this.bnt_top_right_Click);
            // 
            // btn_right_center
            // 
            this.btn_right_center.Image = ((System.Drawing.Image)(resources.GetObject("btn_right_center.Image")));
            this.btn_right_center.Location = new System.Drawing.Point(58, 30);
            this.btn_right_center.Margin = new System.Windows.Forms.Padding(2);
            this.btn_right_center.Name = "btn_right_center";
            this.btn_right_center.Size = new System.Drawing.Size(23, 23);
            this.btn_right_center.TabIndex = 14;
            this.btn_right_center.UseVisualStyleBackColor = true;
            this.btn_right_center.Click += new System.EventHandler(this.btn_right_center_Click);
            // 
            // btn_left_center
            // 
            this.btn_left_center.Image = ((System.Drawing.Image)(resources.GetObject("btn_left_center.Image")));
            this.btn_left_center.Location = new System.Drawing.Point(3, 30);
            this.btn_left_center.Margin = new System.Windows.Forms.Padding(2);
            this.btn_left_center.Name = "btn_left_center";
            this.btn_left_center.Size = new System.Drawing.Size(23, 23);
            this.btn_left_center.TabIndex = 12;
            this.btn_left_center.UseVisualStyleBackColor = true;
            this.btn_left_center.Click += new System.EventHandler(this.btn_left_center_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(143, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Y, мм";
            // 
            // nud_rect_y
            // 
            this.nud_rect_y.Location = new System.Drawing.Point(143, 66);
            this.nud_rect_y.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nud_rect_y.Name = "nud_rect_y";
            this.nud_rect_y.Size = new System.Drawing.Size(79, 20);
            this.nud_rect_y.TabIndex = 7;
            this.nud_rect_y.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nud_rect_y.ValueChanged += new System.EventHandler(this.cb_place_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "X, мм";
            // 
            // nud_rect_x
            // 
            this.nud_rect_x.Location = new System.Drawing.Point(25, 66);
            this.nud_rect_x.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nud_rect_x.Name = "nud_rect_x";
            this.nud_rect_x.Size = new System.Drawing.Size(79, 20);
            this.nud_rect_x.TabIndex = 5;
            this.nud_rect_x.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nud_rect_x.ValueChanged += new System.EventHandler(this.cb_place_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(143, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "висота, мм";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "ширина, мм";
            // 
            // nud_rect_h
            // 
            this.nud_rect_h.Location = new System.Drawing.Point(143, 27);
            this.nud_rect_h.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nud_rect_h.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_rect_h.Name = "nud_rect_h";
            this.nud_rect_h.Size = new System.Drawing.Size(79, 20);
            this.nud_rect_h.TabIndex = 2;
            this.nud_rect_h.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nud_rect_h.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_rect_h.ValueChanged += new System.EventHandler(this.cb_place_SelectedIndexChanged);
            // 
            // nud_rect_w
            // 
            this.nud_rect_w.Location = new System.Drawing.Point(25, 27);
            this.nud_rect_w.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nud_rect_w.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_rect_w.Name = "nud_rect_w";
            this.nud_rect_w.Size = new System.Drawing.Size(79, 20);
            this.nud_rect_w.TabIndex = 1;
            this.nud_rect_w.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nud_rect_w.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_rect_w.ValueChanged += new System.EventHandler(this.cb_place_SelectedIndexChanged);
            // 
            // cb_rect
            // 
            this.cb_rect.AutoSize = true;
            this.cb_rect.Location = new System.Drawing.Point(6, 19);
            this.cb_rect.Name = "cb_rect";
            this.cb_rect.Size = new System.Drawing.Size(92, 17);
            this.cb_rect.TabIndex = 0;
            this.cb_rect.Text = "прямокутник";
            this.cb_rect.UseVisualStyleBackColor = true;
            this.cb_rect.CheckedChanged += new System.EventHandler(this.cb_rect_CheckedChanged);
            // 
            // cb_fit_to_panel
            // 
            this.cb_fit_to_panel.AutoSize = true;
            this.cb_fit_to_panel.Checked = true;
            this.cb_fit_to_panel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_fit_to_panel.Location = new System.Drawing.Point(12, 464);
            this.cb_fit_to_panel.Name = "cb_fit_to_panel";
            this.cb_fit_to_panel.Size = new System.Drawing.Size(163, 17);
            this.cb_fit_to_panel.TabIndex = 8;
            this.cb_fit_to_panel.Text = "зображення в розмір вікна";
            this.cb_fit_to_panel.UseVisualStyleBackColor = true;
            this.cb_fit_to_panel.CheckedChanged += new System.EventHandler(this.cb_fit_to_panel_CheckedChanged);
            // 
            // uc_PreviewControl1
            // 
            this.uc_PreviewControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uc_PreviewControl1.FitToScreen = true;
            this.uc_PreviewControl1.Location = new System.Drawing.Point(274, 12);
            this.uc_PreviewControl1.Name = "uc_PreviewControl1";
            this.uc_PreviewControl1.Primitives = ((System.Collections.Generic.List<Interfaces.IScreenPrimitive>)(resources.GetObject("uc_PreviewControl1.Primitives")));
            this.uc_PreviewControl1.Size = new System.Drawing.Size(688, 530);
            this.uc_PreviewControl1.TabIndex = 9;
            // 
            // FormVisualBlocknoteSpiral
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(974, 554);
            this.Controls.Add(this.uc_PreviewControl1);
            this.Controls.Add(this.cb_fit_to_panel);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.btn_ok);
            this.Name = "FormVisualBlocknoteSpiral";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Параметри для пружини";
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_page_number)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel_rect_params.ResumeLayout(false);
            this.panel_rect_params.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nud_rect_y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_rect_x)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_rect_h)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_rect_w)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox cb_spiral_files;
        private System.Windows.Forms.ComboBox cb_place;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label_total_pages;
        private System.Windows.Forms.NumericUpDown nud_page_number;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel_rect_params;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nud_rect_h;
        private System.Windows.Forms.NumericUpDown nud_rect_w;
        private System.Windows.Forms.CheckBox cb_rect;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nud_rect_y;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nud_rect_x;
        private System.Windows.Forms.CheckBox cb_fit_to_panel;
        private System.Windows.Forms.ComboBox cb_files;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_bottom_right;
        private System.Windows.Forms.Button btn_bottom_center;
        private System.Windows.Forms.Button bnt_bottom_left;
        private System.Windows.Forms.Button btn_right_center;
        private System.Windows.Forms.Button btn_center;
        private System.Windows.Forms.Button btn_left_center;
        private System.Windows.Forms.Button bnt_top_right;
        private System.Windows.Forms.Button btn_top_center;
        private System.Windows.Forms.Button bnt_top_left;
        private UC.Uc_PreviewControl uc_PreviewControl1;
    }
}