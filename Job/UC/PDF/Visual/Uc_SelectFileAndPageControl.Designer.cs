namespace JobSpace.UC.PDF.Visual
{
    partial class Uc_SelectFileAndPageControl
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
            cb_selectFile = new System.Windows.Forms.ComboBox();
            nud_pageNo = new System.Windows.Forms.NumericUpDown();
            uc_PositionControl1 = new Uc_PositionControl();
            cb_position_enable = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)nud_pageNo).BeginInit();
            SuspendLayout();
            // 
            // cb_selectFile
            // 
            cb_selectFile.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cb_selectFile.DropDownHeight = 200;
            cb_selectFile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cb_selectFile.DropDownWidth = 200;
            cb_selectFile.FormattingEnabled = true;
            cb_selectFile.IntegralHeight = false;
            cb_selectFile.Location = new System.Drawing.Point(3, 3);
            cb_selectFile.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            cb_selectFile.Name = "cb_selectFile";
            cb_selectFile.Size = new System.Drawing.Size(146, 23);
            cb_selectFile.TabIndex = 0;
            // 
            // nud_pageNo
            // 
            nud_pageNo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            nud_pageNo.Location = new System.Drawing.Point(149, 3);
            nud_pageNo.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            nud_pageNo.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nud_pageNo.Name = "nud_pageNo";
            nud_pageNo.ReadOnly = true;
            nud_pageNo.Size = new System.Drawing.Size(48, 23);
            nud_pageNo.TabIndex = 1;
            nud_pageNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            nud_pageNo.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // uc_PositionControl1
            // 
            uc_PositionControl1.AutoSize = true;
            uc_PositionControl1.Location = new System.Drawing.Point(60, 32);
            uc_PositionControl1.Name = "uc_PositionControl1";
            uc_PositionControl1.Size = new System.Drawing.Size(100, 100);
            uc_PositionControl1.TabIndex = 2;
            uc_PositionControl1.Visible = false;
            // 
            // cb_position_enable
            // 
            cb_position_enable.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            cb_position_enable.AutoSize = true;
            cb_position_enable.Location = new System.Drawing.Point(204, 7);
            cb_position_enable.Margin = new System.Windows.Forms.Padding(0);
            cb_position_enable.Name = "cb_position_enable";
            cb_position_enable.Size = new System.Drawing.Size(15, 14);
            cb_position_enable.TabIndex = 3;
            cb_position_enable.UseVisualStyleBackColor = true;
            cb_position_enable.CheckedChanged += cb_position_enable_CheckedChanged;
            // 
            // Uc_SelectFileAndPageControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            Controls.Add(cb_position_enable);
            Controls.Add(uc_PositionControl1);
            Controls.Add(nud_pageNo);
            Controls.Add(cb_selectFile);
            Name = "Uc_SelectFileAndPageControl";
            Size = new System.Drawing.Size(225, 135);
            ((System.ComponentModel.ISupportInitialize)nud_pageNo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ComboBox cb_selectFile;
        private System.Windows.Forms.NumericUpDown nud_pageNo;
        private Uc_PositionControl uc_PositionControl1;
        private System.Windows.Forms.CheckBox cb_position_enable;
    }
}
