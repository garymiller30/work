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
            this.btn_ok = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cb_files = new System.Windows.Forms.ComboBox();
            this.cb_place = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pb_preview = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label_total_pages = new System.Windows.Forms.Label();
            this.nud_page_number = new System.Windows.Forms.NumericUpDown();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_preview)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_page_number)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_ok
            // 
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
            this.groupBox5.Controls.Add(this.cb_files);
            this.groupBox5.Location = new System.Drawing.Point(12, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(256, 55);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "файл пружинки";
            // 
            // cb_files
            // 
            this.cb_files.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_files.FormattingEnabled = true;
            this.cb_files.Location = new System.Drawing.Point(6, 19);
            this.cb_files.Name = "cb_files";
            this.cb_files.Size = new System.Drawing.Size(244, 21);
            this.cb_files.TabIndex = 0;
            this.cb_files.SelectedIndexChanged += new System.EventHandler(this.cb_files_SelectedIndexChanged);
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
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pb_preview);
            this.panel1.Location = new System.Drawing.Point(274, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(688, 530);
            this.panel1.TabIndex = 5;
            // 
            // pb_preview
            // 
            this.pb_preview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pb_preview.Location = new System.Drawing.Point(3, 3);
            this.pb_preview.Name = "pb_preview";
            this.pb_preview.Size = new System.Drawing.Size(627, 419);
            this.pb_preview.TabIndex = 0;
            this.pb_preview.TabStop = false;
            this.pb_preview.Paint += new System.Windows.Forms.PaintEventHandler(this.pb_preview_Paint);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label_total_pages);
            this.groupBox1.Controls.Add(this.nud_page_number);
            this.groupBox1.Location = new System.Drawing.Point(12, 134);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(256, 77);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "сторінка";
            // 
            // label_total_pages
            // 
            this.label_total_pages.AutoSize = true;
            this.label_total_pages.Location = new System.Drawing.Point(102, 21);
            this.label_total_pages.Name = "label_total_pages";
            this.label_total_pages.Size = new System.Drawing.Size(27, 13);
            this.label_total_pages.TabIndex = 1;
            this.label_total_pages.Text = "/ 00";
            // 
            // nud_page_number
            // 
            this.nud_page_number.Location = new System.Drawing.Point(17, 19);
            this.nud_page_number.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_page_number.Name = "nud_page_number";
            this.nud_page_number.Size = new System.Drawing.Size(79, 20);
            this.nud_page_number.TabIndex = 0;
            this.nud_page_number.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nud_page_number.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_page_number.ValueChanged += new System.EventHandler(this.nud_page_number_ValueChanged);
            // 
            // FormVisualBlocknoteSpiral
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(974, 554);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.btn_ok);
            this.Name = "FormVisualBlocknoteSpiral";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Параметри для пружини";
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pb_preview)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_page_number)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox cb_files;
        private System.Windows.Forms.ComboBox cb_place;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pb_preview;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label_total_pages;
        private System.Windows.Forms.NumericUpDown nud_page_number;
    }
}