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
            cb_selectFile.Size = new System.Drawing.Size(129, 23);
            cb_selectFile.TabIndex = 0;
            // 
            // nud_pageNo
            // 
            nud_pageNo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            nud_pageNo.Location = new System.Drawing.Point(132, 3);
            nud_pageNo.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            nud_pageNo.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nud_pageNo.Name = "nud_pageNo";
            nud_pageNo.ReadOnly = true;
            nud_pageNo.Size = new System.Drawing.Size(63, 23);
            nud_pageNo.TabIndex = 1;
            nud_pageNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            nud_pageNo.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // Uc_SelectFileAndPageControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(nud_pageNo);
            Controls.Add(cb_selectFile);
            Name = "Uc_SelectFileAndPageControl";
            Size = new System.Drawing.Size(195, 29);
            ((System.ComponentModel.ISupportInitialize)nud_pageNo).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ComboBox cb_selectFile;
        private System.Windows.Forms.NumericUpDown nud_pageNo;
    }
}
