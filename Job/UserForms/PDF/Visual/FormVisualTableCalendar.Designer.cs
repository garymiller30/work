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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.uc_VisualRectangleControl_bottom = new JobSpace.UC.PDF.Visual.Uc_VisualRectangleControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.uc_VisualRectangleControl_top = new JobSpace.UC.PDF.Visual.Uc_VisualRectangleControl();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.nud_osnova = new System.Windows.Forms.NumericUpDown();
            this.uc_PreviewBrowserFile1 = new JobSpace.UC.Uc_FilePreviewControl();
            this.uc_SelectSpiralControl1 = new JobSpace.UC.PDF.Visual.Uc_SelectSpiralControl();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_osnova)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(187, 479);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "параметри";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.uc_VisualRectangleControl_bottom);
            this.groupBox4.Location = new System.Drawing.Point(6, 299);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(174, 175);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "нижній блок";
            // 
            // uc_VisualRectangleControl_bottom
            // 
            this.uc_VisualRectangleControl_bottom.H = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.uc_VisualRectangleControl_bottom.Location = new System.Drawing.Point(5, 19);
            this.uc_VisualRectangleControl_bottom.Name = "uc_VisualRectangleControl_bottom";
            this.uc_VisualRectangleControl_bottom.RectEnabled = false;
            this.uc_VisualRectangleControl_bottom.Size = new System.Drawing.Size(166, 150);
            this.uc_VisualRectangleControl_bottom.TabIndex = 3;
            this.uc_VisualRectangleControl_bottom.W = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.uc_VisualRectangleControl_bottom.X = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.uc_VisualRectangleControl_bottom.Y = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.uc_VisualRectangleControl_top);
            this.groupBox2.Location = new System.Drawing.Point(6, 118);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(174, 175);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "верхній блок";
            // 
            // uc_VisualRectangleControl_top
            // 
            this.uc_VisualRectangleControl_top.H = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.uc_VisualRectangleControl_top.Location = new System.Drawing.Point(6, 19);
            this.uc_VisualRectangleControl_top.Name = "uc_VisualRectangleControl_top";
            this.uc_VisualRectangleControl_top.RectEnabled = false;
            this.uc_VisualRectangleControl_top.Size = new System.Drawing.Size(165, 153);
            this.uc_VisualRectangleControl_top.TabIndex = 3;
            this.uc_VisualRectangleControl_top.W = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.uc_VisualRectangleControl_top.X = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.uc_VisualRectangleControl_top.Y = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.nud_osnova);
            this.groupBox3.Location = new System.Drawing.Point(6, 67);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(162, 45);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "основа, мм";
            // 
            // nud_osnova
            // 
            this.nud_osnova.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nud_osnova.DecimalPlaces = 1;
            this.nud_osnova.Location = new System.Drawing.Point(6, 19);
            this.nud_osnova.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nud_osnova.Name = "nud_osnova";
            this.nud_osnova.Size = new System.Drawing.Size(150, 20);
            this.nud_osnova.TabIndex = 1;
            this.nud_osnova.ValueChanged += new System.EventHandler(this.nud_osnova_ValueChanged);
            // 
            // uc_PreviewBrowserFile1
            // 
            this.uc_PreviewBrowserFile1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uc_PreviewBrowserFile1.Location = new System.Drawing.Point(205, 12);
            this.uc_PreviewBrowserFile1.Name = "uc_PreviewBrowserFile1";
            this.uc_PreviewBrowserFile1.Size = new System.Drawing.Size(602, 617);
            this.uc_PreviewBrowserFile1.TabIndex = 1;
            // 
            // uc_SelectSpiralControl1
            // 
            this.uc_SelectSpiralControl1.Location = new System.Drawing.Point(18, 31);
            this.uc_SelectSpiralControl1.Name = "uc_SelectSpiralControl1";
            this.uc_SelectSpiralControl1.Size = new System.Drawing.Size(162, 42);
            this.uc_SelectSpiralControl1.TabIndex = 3;
            // 
            // FormVisualTableCalendar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(819, 641);
            this.Controls.Add(this.uc_SelectSpiralControl1);
            this.Controls.Add(this.uc_PreviewBrowserFile1);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormVisualTableCalendar";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Настільний календар - ПРУЖИНА";
            this.Load += new System.EventHandler(this.FormVisualTableCalendar_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nud_osnova)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private UC.Uc_FilePreviewControl uc_PreviewBrowserFile1;
        private System.Windows.Forms.NumericUpDown nud_osnova;
        private System.Windows.Forms.GroupBox groupBox3;
        private UC.PDF.Visual.Uc_SelectSpiralControl uc_SelectSpiralControl1;
        private System.Windows.Forms.GroupBox groupBox4;
        private UC.PDF.Visual.Uc_VisualRectangleControl uc_VisualRectangleControl_bottom;
        private System.Windows.Forms.GroupBox groupBox2;
        private UC.PDF.Visual.Uc_VisualRectangleControl uc_VisualRectangleControl_top;
    }
}