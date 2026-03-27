namespace JobSpace.UserForms.PDF
{
    partial class FormSpotColorRenamer
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
            this.cb_names = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tb_newName = new System.Windows.Forms.TextBox();
            this.btn_rename = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cb_names);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(221, 52);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "вибрати колір у файлі";
            // 
            // cb_names
            // 
            this.cb_names.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_names.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_names.FormattingEnabled = true;
            this.cb_names.Location = new System.Drawing.Point(3, 16);
            this.cb_names.Name = "cb_names";
            this.cb_names.Size = new System.Drawing.Size(215, 21);
            this.cb_names.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tb_newName);
            this.groupBox2.Location = new System.Drawing.Point(12, 70);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(221, 47);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "нове ім\'я";
            // 
            // tb_newName
            // 
            this.tb_newName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_newName.Location = new System.Drawing.Point(3, 16);
            this.tb_newName.Name = "tb_newName";
            this.tb_newName.Size = new System.Drawing.Size(215, 20);
            this.tb_newName.TabIndex = 0;
            // 
            // btn_rename
            // 
            this.btn_rename.Location = new System.Drawing.Point(66, 144);
            this.btn_rename.Name = "btn_rename";
            this.btn_rename.Size = new System.Drawing.Size(110, 33);
            this.btn_rename.TabIndex = 2;
            this.btn_rename.Text = "перейменувати";
            this.btn_rename.UseVisualStyleBackColor = true;
            this.btn_rename.Click += new System.EventHandler(this.btn_rename_Click);
            // 
            // FormSpotColorRenamer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(243, 189);
            this.Controls.Add(this.btn_rename);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSpotColorRenamer";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Перейменувати колір";
            this.Shown += new System.EventHandler(this.FormSpotColorRenamer_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cb_names;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tb_newName;
        private System.Windows.Forms.Button btn_rename;
    }
}