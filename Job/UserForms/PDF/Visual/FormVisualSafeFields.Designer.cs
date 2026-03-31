namespace JobSpace.UserForms.PDF.Visual
{
    partial class FormVisualSafeFields
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
            this.uc_FilePreviewControl1 = new JobSpace.UC.Uc_FilePreviewControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nud_top = new System.Windows.Forms.NumericUpDown();
            this.nud_left = new System.Windows.Forms.NumericUpDown();
            this.nud_right = new System.Windows.Forms.NumericUpDown();
            this.nud_bottom = new System.Windows.Forms.NumericUpDown();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_top)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_left)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_right)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_bottom)).BeginInit();
            this.SuspendLayout();
            // 
            // uc_FilePreviewControl1
            // 
            this.uc_FilePreviewControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uc_FilePreviewControl1.Location = new System.Drawing.Point(225, 12);
            this.uc_FilePreviewControl1.Name = "uc_FilePreviewControl1";
            this.uc_FilePreviewControl1.Size = new System.Drawing.Size(723, 586);
            this.uc_FilePreviewControl1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nud_bottom);
            this.groupBox1.Controls.Add(this.nud_right);
            this.groupBox1.Controls.Add(this.nud_left);
            this.groupBox1.Controls.Add(this.nud_top);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(207, 151);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "поля";
            // 
            // nud_top
            // 
            this.nud_top.DecimalPlaces = 1;
            this.nud_top.Location = new System.Drawing.Point(68, 19);
            this.nud_top.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.nud_top.Name = "nud_top";
            this.nud_top.Size = new System.Drawing.Size(70, 20);
            this.nud_top.TabIndex = 0;
            this.nud_top.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nud_top.ValueChanged += new System.EventHandler(this.nud_ValueChanged);
            // 
            // nud_left
            // 
            this.nud_left.DecimalPlaces = 1;
            this.nud_left.Location = new System.Drawing.Point(6, 68);
            this.nud_left.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.nud_left.Name = "nud_left";
            this.nud_left.Size = new System.Drawing.Size(70, 20);
            this.nud_left.TabIndex = 1;
            this.nud_left.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nud_left.ValueChanged += new System.EventHandler(this.nud_ValueChanged);
            // 
            // nud_right
            // 
            this.nud_right.DecimalPlaces = 1;
            this.nud_right.Location = new System.Drawing.Point(131, 68);
            this.nud_right.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.nud_right.Name = "nud_right";
            this.nud_right.Size = new System.Drawing.Size(70, 20);
            this.nud_right.TabIndex = 2;
            this.nud_right.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nud_right.ValueChanged += new System.EventHandler(this.nud_ValueChanged);
            // 
            // nud_bottom
            // 
            this.nud_bottom.DecimalPlaces = 1;
            this.nud_bottom.Location = new System.Drawing.Point(68, 117);
            this.nud_bottom.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.nud_bottom.Name = "nud_bottom";
            this.nud_bottom.Size = new System.Drawing.Size(70, 20);
            this.nud_bottom.TabIndex = 3;
            this.nud_bottom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nud_bottom.ValueChanged += new System.EventHandler(this.nud_ValueChanged);
            // 
            // FormVisualSafeFields
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 610);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.uc_FilePreviewControl1);
            this.Name = "FormVisualSafeFields";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Безпечні поля";
            this.Shown += new System.EventHandler(this.FormVisualSafeFields_Shown);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nud_top)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_left)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_right)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_bottom)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UC.Uc_FilePreviewControl uc_FilePreviewControl1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown nud_bottom;
        private System.Windows.Forms.NumericUpDown nud_right;
        private System.Windows.Forms.NumericUpDown nud_left;
        private System.Windows.Forms.NumericUpDown nud_top;
    }
}