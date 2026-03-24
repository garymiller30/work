namespace JobSpace.UserForms.PDF.Visual
{
    partial class FormVisualSaddleStitchCreep
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
            this.uc_PreviewBrowserFile1 = new JobSpace.UC.Uc_FilePreviewControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nud_paper_thickness = new System.Windows.Forms.NumericUpDown();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_paper_thickness)).BeginInit();
            this.SuspendLayout();
            // 
            // uc_PreviewBrowserFile1
            // 
            this.uc_PreviewBrowserFile1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uc_PreviewBrowserFile1.Location = new System.Drawing.Point(270, 12);
            this.uc_PreviewBrowserFile1.Name = "uc_PreviewBrowserFile1";
            this.uc_PreviewBrowserFile1.Size = new System.Drawing.Size(798, 559);
            this.uc_PreviewBrowserFile1.TabIndex = 10;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.nud_paper_thickness);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(252, 51);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Товщина паперу, мм";
            // 
            // nud_paper_thickness
            // 
            this.nud_paper_thickness.DecimalPlaces = 2;
            this.nud_paper_thickness.Location = new System.Drawing.Point(191, 20);
            this.nud_paper_thickness.Name = "nud_paper_thickness";
            this.nud_paper_thickness.Size = new System.Drawing.Size(55, 20);
            this.nud_paper_thickness.TabIndex = 0;
            this.nud_paper_thickness.ValueChanged += new System.EventHandler(this.nud_paper_thickness_ValueChanged);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(6, 19);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(179, 21);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // FormVisualSaddleStitchCreep
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1080, 583);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.uc_PreviewBrowserFile1);
            this.Name = "FormVisualSaddleStitchCreep";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Візуал виштовхування при шитві скобою";
            this.Load += new System.EventHandler(this.FormVisualSaddleStitchCreep_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nud_paper_thickness)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UC.Uc_FilePreviewControl uc_PreviewBrowserFile1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown nud_paper_thickness;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}