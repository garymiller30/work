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
            btn_ok = new System.Windows.Forms.Button();
            cb_place = new System.Windows.Forms.ComboBox();
            groupBox4 = new System.Windows.Forms.GroupBox();
            groupBox1 = new System.Windows.Forms.GroupBox();
            cb_files = new System.Windows.Forms.ComboBox();
            uc_SelectFileAndPageControl1 = new JobSpace.UC.PDF.Visual.Uc_SelectFileAndPageControl();
            uc_VisualRectangleControl1 = new JobSpace.UC.PDF.Visual.Uc_VisualRectangleControl();
            uc_PreviewBrowserFile1 = new JobSpace.UC.Uc_FilePreviewControl();
            uc_SelectSpiralControl1 = new JobSpace.UC.PDF.Visual.Uc_SelectSpiralControl();
            flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            label1 = new System.Windows.Forms.Label();
            groupBox4.SuspendLayout();
            groupBox1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // btn_ok
            // 
            btn_ok.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btn_ok.Location = new System.Drawing.Point(51, 476);
            btn_ok.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btn_ok.Name = "btn_ok";
            btn_ok.Size = new System.Drawing.Size(138, 48);
            btn_ok.TabIndex = 0;
            btn_ok.Text = "OK";
            btn_ok.UseVisualStyleBackColor = true;
            btn_ok.Click += btn_ok_Click;
            // 
            // cb_place
            // 
            cb_place.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cb_place.FormattingEnabled = true;
            cb_place.Location = new System.Drawing.Point(7, 22);
            cb_place.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cb_place.Name = "cb_place";
            cb_place.Size = new System.Drawing.Size(201, 23);
            cb_place.TabIndex = 0;
            cb_place.SelectedIndexChanged += cb_place_SelectedIndexChanged;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(cb_place);
            groupBox4.Location = new System.Drawing.Point(4, 57);
            groupBox4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox4.Name = "groupBox4";
            groupBox4.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox4.Size = new System.Drawing.Size(216, 63);
            groupBox4.TabIndex = 2;
            groupBox4.TabStop = false;
            groupBox4.Text = "розташування пружини";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(cb_files);
            groupBox1.Location = new System.Drawing.Point(4, 126);
            groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Size = new System.Drawing.Size(216, 59);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            groupBox1.Text = "файл";
            // 
            // cb_files
            // 
            cb_files.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cb_files.FormattingEnabled = true;
            cb_files.Location = new System.Drawing.Point(7, 21);
            cb_files.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cb_files.Name = "cb_files";
            cb_files.Size = new System.Drawing.Size(201, 23);
            cb_files.TabIndex = 2;
            cb_files.SelectedIndexChanged += cb_files_SelectedIndexChanged_1;
            // 
            // uc_SelectFileAndPageControl1
            // 
            uc_SelectFileAndPageControl1.AutoSize = true;
            uc_SelectFileAndPageControl1.Location = new System.Drawing.Point(3, 216);
            uc_SelectFileAndPageControl1.Name = "uc_SelectFileAndPageControl1";
            uc_SelectFileAndPageControl1.Size = new System.Drawing.Size(195, 29);
            uc_SelectFileAndPageControl1.TabIndex = 11;
            // 
            // uc_VisualRectangleControl1
            // 
            uc_VisualRectangleControl1.AutoSize = true;
            uc_VisualRectangleControl1.H = new decimal(new int[] { 100, 0, 0, 0 });
            uc_VisualRectangleControl1.Location = new System.Drawing.Point(4, 251);
            uc_VisualRectangleControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            uc_VisualRectangleControl1.Name = "uc_VisualRectangleControl1";
            uc_VisualRectangleControl1.RectEnabled = false;
            uc_VisualRectangleControl1.Size = new System.Drawing.Size(107, 25);
            uc_VisualRectangleControl1.TabIndex = 11;
            uc_VisualRectangleControl1.W = new decimal(new int[] { 100, 0, 0, 0 });
            uc_VisualRectangleControl1.X = new decimal(new int[] { 0, 0, 0, 0 });
            uc_VisualRectangleControl1.Y = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // uc_PreviewBrowserFile1
            // 
            uc_PreviewBrowserFile1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            uc_PreviewBrowserFile1.Location = new System.Drawing.Point(260, 14);
            uc_PreviewBrowserFile1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            uc_PreviewBrowserFile1.Name = "uc_PreviewBrowserFile1";
            uc_PreviewBrowserFile1.Size = new System.Drawing.Size(569, 510);
            uc_PreviewBrowserFile1.TabIndex = 9;
            // 
            // uc_SelectSpiralControl1
            // 
            uc_SelectSpiralControl1.Location = new System.Drawing.Point(5, 3);
            uc_SelectSpiralControl1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            uc_SelectSpiralControl1.Name = "uc_SelectSpiralControl1";
            uc_SelectSpiralControl1.Size = new System.Drawing.Size(216, 48);
            uc_SelectSpiralControl1.TabIndex = 10;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Controls.Add(uc_SelectSpiralControl1);
            flowLayoutPanel1.Controls.Add(groupBox4);
            flowLayoutPanel1.Controls.Add(groupBox1);
            flowLayoutPanel1.Controls.Add(label1);
            flowLayoutPanel1.Controls.Add(uc_SelectFileAndPageControl1);
            flowLayoutPanel1.Controls.Add(uc_VisualRectangleControl1);
            flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            flowLayoutPanel1.Location = new System.Drawing.Point(12, 14);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new System.Drawing.Size(240, 456);
            flowLayoutPanel1.TabIndex = 11;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            label1.Location = new System.Drawing.Point(3, 198);
            label1.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(188, 15);
            label1.TabIndex = 11;
            label1.Text = "додатково файл / прямокутник";
            // 
            // FormVisualBlocknoteSpiral
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(842, 537);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(uc_PreviewBrowserFile1);
            Controls.Add(btn_ok);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormVisualBlocknoteSpiral";
            ShowIcon = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Параметри для пружини";
            groupBox4.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.ComboBox cb_place;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cb_files;
        private UC.Uc_FilePreviewControl uc_PreviewBrowserFile1;
        private UC.PDF.Visual.Uc_SelectSpiralControl uc_SelectSpiralControl1;
        private UC.PDF.Visual.Uc_VisualRectangleControl uc_VisualRectangleControl1;
        private UC.PDF.Visual.Uc_SelectFileAndPageControl uc_SelectFileAndPageControl1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label1;
    }
}