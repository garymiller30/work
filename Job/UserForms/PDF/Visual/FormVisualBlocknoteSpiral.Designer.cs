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
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(79, 160);
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
            // 
            // cb_place
            // 
            this.cb_place.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_place.FormattingEnabled = true;
            this.cb_place.Location = new System.Drawing.Point(6, 19);
            this.cb_place.Name = "cb_place";
            this.cb_place.Size = new System.Drawing.Size(244, 21);
            this.cb_place.TabIndex = 0;
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
            // FormVisualBlocknoteSpiral
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 214);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.btn_ok);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormVisualBlocknoteSpiral";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Параметри для пружини";
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox cb_files;
        private System.Windows.Forms.ComboBox cb_place;
        private System.Windows.Forms.GroupBox groupBox4;
    }
}