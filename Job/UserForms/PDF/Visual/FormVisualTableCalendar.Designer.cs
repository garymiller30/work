namespace JobSpace.UserForms.PDF.Visual
{
    partial class FormVisualTableCalendar
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
            uc_SelectFileAndPageControl2 = new JobSpace.UC.PDF.Visual.Uc_SelectFileAndPageControl();
            uc_VisualRectangleControl_bottom = new JobSpace.UC.PDF.Visual.Uc_VisualRectangleControl();
            uc_SelectFileAndPageControl1 = new JobSpace.UC.PDF.Visual.Uc_SelectFileAndPageControl();
            groupBox3 = new System.Windows.Forms.GroupBox();
            nud_osnova = new System.Windows.Forms.NumericUpDown();
            uc_PreviewBrowserFile1 = new JobSpace.UC.Uc_FilePreviewControl();
            uc_SelectSpiralControl1 = new JobSpace.UC.PDF.Visual.Uc_SelectSpiralControl();
            btn_3d = new System.Windows.Forms.Button();
            flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            label1 = new System.Windows.Forms.Label();
            uc_VisualRectangleControl_top = new JobSpace.UC.PDF.Visual.Uc_VisualRectangleControl();
            label2 = new System.Windows.Forms.Label();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nud_osnova).BeginInit();
            flowLayoutPanel3.SuspendLayout();
            SuspendLayout();
            // 
            // uc_SelectFileAndPageControl2
            // 
            uc_SelectFileAndPageControl2.AutoSize = true;
            uc_SelectFileAndPageControl2.Location = new System.Drawing.Point(3, 243);
            uc_SelectFileAndPageControl2.Name = "uc_SelectFileAndPageControl2";
            uc_SelectFileAndPageControl2.Size = new System.Drawing.Size(219, 29);
            uc_SelectFileAndPageControl2.TabIndex = 5;
            // 
            // uc_VisualRectangleControl_bottom
            // 
            uc_VisualRectangleControl_bottom.AutoSize = true;
            uc_VisualRectangleControl_bottom.H = new decimal(new int[] { 100, 0, 0, 0 });
            uc_VisualRectangleControl_bottom.Location = new System.Drawing.Point(5, 278);
            uc_VisualRectangleControl_bottom.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            uc_VisualRectangleControl_bottom.Name = "uc_VisualRectangleControl_bottom";
            uc_VisualRectangleControl_bottom.RectEnabled = false;
            uc_VisualRectangleControl_bottom.Size = new System.Drawing.Size(107, 25);
            uc_VisualRectangleControl_bottom.TabIndex = 3;
            uc_VisualRectangleControl_bottom.W = new decimal(new int[] { 100, 0, 0, 0 });
            uc_VisualRectangleControl_bottom.X = new decimal(new int[] { 0, 0, 0, 0 });
            uc_VisualRectangleControl_bottom.Y = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // uc_SelectFileAndPageControl1
            // 
            uc_SelectFileAndPageControl1.AutoSize = true;
            uc_SelectFileAndPageControl1.Location = new System.Drawing.Point(3, 146);
            uc_SelectFileAndPageControl1.Name = "uc_SelectFileAndPageControl1";
            uc_SelectFileAndPageControl1.Size = new System.Drawing.Size(219, 29);
            uc_SelectFileAndPageControl1.TabIndex = 4;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(nud_osnova);
            groupBox3.Location = new System.Drawing.Point(4, 57);
            groupBox3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox3.Size = new System.Drawing.Size(227, 52);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "основа, мм";
            // 
            // nud_osnova
            // 
            nud_osnova.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            nud_osnova.DecimalPlaces = 1;
            nud_osnova.Location = new System.Drawing.Point(7, 22);
            nud_osnova.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            nud_osnova.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            nud_osnova.Name = "nud_osnova";
            nud_osnova.Size = new System.Drawing.Size(213, 23);
            nud_osnova.TabIndex = 1;
            nud_osnova.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            nud_osnova.ValueChanged += nud_osnova_ValueChanged;
            // 
            // uc_PreviewBrowserFile1
            // 
            uc_PreviewBrowserFile1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            uc_PreviewBrowserFile1.Location = new System.Drawing.Point(265, 14);
            uc_PreviewBrowserFile1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            uc_PreviewBrowserFile1.Name = "uc_PreviewBrowserFile1";
            uc_PreviewBrowserFile1.Size = new System.Drawing.Size(676, 667);
            uc_PreviewBrowserFile1.TabIndex = 1;
            // 
            // uc_SelectSpiralControl1
            // 
            uc_SelectSpiralControl1.Location = new System.Drawing.Point(5, 3);
            uc_SelectSpiralControl1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            uc_SelectSpiralControl1.Name = "uc_SelectSpiralControl1";
            uc_SelectSpiralControl1.Size = new System.Drawing.Size(227, 48);
            uc_SelectSpiralControl1.TabIndex = 3;
            // 
            // btn_3d
            // 
            btn_3d.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btn_3d.Location = new System.Drawing.Point(82, 649);
            btn_3d.Name = "btn_3d";
            btn_3d.Size = new System.Drawing.Size(102, 40);
            btn_3d.TabIndex = 4;
            btn_3d.Text = "3D";
            btn_3d.UseVisualStyleBackColor = true;
            btn_3d.Click += btn_3d_Click;
            // 
            // flowLayoutPanel3
            // 
            flowLayoutPanel3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            flowLayoutPanel3.AutoScroll = true;
            flowLayoutPanel3.Controls.Add(uc_SelectSpiralControl1);
            flowLayoutPanel3.Controls.Add(groupBox3);
            flowLayoutPanel3.Controls.Add(label1);
            flowLayoutPanel3.Controls.Add(uc_SelectFileAndPageControl1);
            flowLayoutPanel3.Controls.Add(uc_VisualRectangleControl_top);
            flowLayoutPanel3.Controls.Add(label2);
            flowLayoutPanel3.Controls.Add(uc_SelectFileAndPageControl2);
            flowLayoutPanel3.Controls.Add(uc_VisualRectangleControl_bottom);
            flowLayoutPanel3.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            flowLayoutPanel3.Location = new System.Drawing.Point(9, 14);
            flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            flowLayoutPanel3.Name = "flowLayoutPanel3";
            flowLayoutPanel3.Size = new System.Drawing.Size(251, 632);
            flowLayoutPanel3.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline);
            label1.Location = new System.Drawing.Point(10, 117);
            label1.Margin = new System.Windows.Forms.Padding(10, 5, 3, 5);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(115, 21);
            label1.TabIndex = 0;
            label1.Text = "верхній блок";
            // 
            // uc_VisualRectangleControl_top
            // 
            uc_VisualRectangleControl_top.AutoSize = true;
            uc_VisualRectangleControl_top.H = new decimal(new int[] { 100, 0, 0, 0 });
            uc_VisualRectangleControl_top.Location = new System.Drawing.Point(5, 181);
            uc_VisualRectangleControl_top.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            uc_VisualRectangleControl_top.Name = "uc_VisualRectangleControl_top";
            uc_VisualRectangleControl_top.RectEnabled = false;
            uc_VisualRectangleControl_top.Size = new System.Drawing.Size(107, 25);
            uc_VisualRectangleControl_top.TabIndex = 3;
            uc_VisualRectangleControl_top.W = new decimal(new int[] { 100, 0, 0, 0 });
            uc_VisualRectangleControl_top.X = new decimal(new int[] { 0, 0, 0, 0 });
            uc_VisualRectangleControl_top.Y = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline);
            label2.Location = new System.Drawing.Point(10, 214);
            label2.Margin = new System.Windows.Forms.Padding(10, 5, 3, 5);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(112, 21);
            label2.TabIndex = 5;
            label2.Text = "нижній блок";
            // 
            // FormVisualTableCalendar
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(955, 695);
            Controls.Add(flowLayoutPanel3);
            Controls.Add(btn_3d);
            Controls.Add(uc_PreviewBrowserFile1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormVisualTableCalendar";
            ShowIcon = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Настільний календар - ПРУЖИНА";
            Load += FormVisualTableCalendar_Load;
            groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)nud_osnova).EndInit();
            flowLayoutPanel3.ResumeLayout(false);
            flowLayoutPanel3.PerformLayout();
            ResumeLayout(false);

        }

        #endregion
        private UC.Uc_FilePreviewControl uc_PreviewBrowserFile1;
        private System.Windows.Forms.NumericUpDown nud_osnova;
        private System.Windows.Forms.GroupBox groupBox3;
        private UC.PDF.Visual.Uc_SelectSpiralControl uc_SelectSpiralControl1;
        private UC.PDF.Visual.Uc_VisualRectangleControl uc_VisualRectangleControl_bottom;
        private UC.PDF.Visual.Uc_SelectFileAndPageControl uc_SelectFileAndPageControl1;
        private UC.PDF.Visual.Uc_SelectFileAndPageControl uc_SelectFileAndPageControl2;
        private System.Windows.Forms.Button btn_3d;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private UC.PDF.Visual.Uc_VisualRectangleControl uc_VisualRectangleControl_top;
    }
}
