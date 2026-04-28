namespace JobSpace.UserForms.PDF
{
    partial class FormMergeCutAndDocument
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
            this.cb_cut_file = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cb_target_file = new System.Windows.Forms.ComboBox();
            this.btn_ok = new System.Windows.Forms.Button();
            this.uc_FilePreviewControl1 = new JobSpace.UC.Uc_FilePreviewControl();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cb_target_file);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cb_cut_file);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(233, 129);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // cb_cut_file
            // 
            this.cb_cut_file.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_cut_file.FormattingEnabled = true;
            this.cb_cut_file.Location = new System.Drawing.Point(9, 32);
            this.cb_cut_file.Name = "cb_cut_file";
            this.cb_cut_file.Size = new System.Drawing.Size(218, 21);
            this.cb_cut_file.TabIndex = 0;
            this.cb_cut_file.SelectedIndexChanged += new System.EventHandler(this.cb_cut_file_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "файл, що накладається";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "файл";
            // 
            // cb_target_file
            // 
            this.cb_target_file.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_target_file.FormattingEnabled = true;
            this.cb_target_file.Location = new System.Drawing.Point(9, 84);
            this.cb_target_file.Name = "cb_target_file";
            this.cb_target_file.Size = new System.Drawing.Size(218, 21);
            this.cb_target_file.TabIndex = 2;
            this.cb_target_file.SelectedIndexChanged += new System.EventHandler(this.cb_target_file_SelectedIndexChanged);
            // 
            // btn_ok
            // 
            this.btn_ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_ok.Location = new System.Drawing.Point(77, 403);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(113, 35);
            this.btn_ok.TabIndex = 2;
            this.btn_ok.Text = "OK";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // uc_FilePreviewControl1
            // 
            this.uc_FilePreviewControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uc_FilePreviewControl1.Location = new System.Drawing.Point(251, 12);
            this.uc_FilePreviewControl1.Name = "uc_FilePreviewControl1";
            this.uc_FilePreviewControl1.Size = new System.Drawing.Size(537, 426);
            this.uc_FilePreviewControl1.TabIndex = 0;
            // 
            // FormMergeCutAndDocument
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.uc_FilePreviewControl1);
            this.Name = "FormMergeCutAndDocument";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Комбінувати контур і документ";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private UC.Uc_FilePreviewControl uc_FilePreviewControl1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cb_cut_file;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cb_target_file;
        private System.Windows.Forms.Button btn_ok;
    }
}