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
            uc_PreviewBrowserFile1 = new JobSpace.UC.Uc_FilePreviewControl();
            groupBox1 = new System.Windows.Forms.GroupBox();
            comboBox1 = new System.Windows.Forms.ComboBox();
            nud_paper_thickness = new System.Windows.Forms.NumericUpDown();
            btn_ok = new System.Windows.Forms.Button();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nud_paper_thickness).BeginInit();
            SuspendLayout();
            // 
            // uc_PreviewBrowserFile1
            // 
            uc_PreviewBrowserFile1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            uc_PreviewBrowserFile1.Location = new System.Drawing.Point(315, 14);
            uc_PreviewBrowserFile1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            uc_PreviewBrowserFile1.Name = "uc_PreviewBrowserFile1";
            uc_PreviewBrowserFile1.Size = new System.Drawing.Size(931, 645);
            uc_PreviewBrowserFile1.TabIndex = 10;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            groupBox1.Controls.Add(comboBox1);
            groupBox1.Controls.Add(nud_paper_thickness);
            groupBox1.Location = new System.Drawing.Point(14, 14);
            groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Size = new System.Drawing.Size(294, 59);
            groupBox1.TabIndex = 11;
            groupBox1.TabStop = false;
            groupBox1.Text = "Товщина паперу, мм";
            // 
            // comboBox1
            // 
            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new System.Drawing.Point(7, 22);
            comboBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new System.Drawing.Size(208, 23);
            comboBox1.TabIndex = 1;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // nud_paper_thickness
            // 
            nud_paper_thickness.DecimalPlaces = 2;
            nud_paper_thickness.Location = new System.Drawing.Point(223, 23);
            nud_paper_thickness.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            nud_paper_thickness.Name = "nud_paper_thickness";
            nud_paper_thickness.Size = new System.Drawing.Size(64, 23);
            nud_paper_thickness.TabIndex = 0;
            nud_paper_thickness.ValueChanged += nud_paper_thickness_ValueChanged;
            // 
            // btn_ok
            // 
            btn_ok.Location = new System.Drawing.Point(14, 88);
            btn_ok.Name = "btn_ok";
            btn_ok.Size = new System.Drawing.Size(294, 33);
            btn_ok.TabIndex = 12;
            btn_ok.Text = "підготувати файл";
            btn_ok.UseVisualStyleBackColor = true;
            btn_ok.Click += btn_ok_Click;
            // 
            // FormVisualSaddleStitchCreep
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1260, 673);
            Controls.Add(btn_ok);
            Controls.Add(groupBox1);
            Controls.Add(uc_PreviewBrowserFile1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormVisualSaddleStitchCreep";
            ShowIcon = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Візуал виштовхування при шитві скобою";
            Load += FormVisualSaddleStitchCreep_Load;
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)nud_paper_thickness).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private UC.Uc_FilePreviewControl uc_PreviewBrowserFile1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown nud_paper_thickness;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btn_ok;
    }
}