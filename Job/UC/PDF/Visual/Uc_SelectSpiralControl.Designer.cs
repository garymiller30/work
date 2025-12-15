namespace JobSpace.UC.PDF.Visual
{
    partial class Uc_SelectSpiralControl
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
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cb_spiral_files = new System.Windows.Forms.ComboBox();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cb_spiral_files);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(0, 0);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(258, 42);
            this.groupBox5.TabIndex = 5;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "файл пружинки";
            // 
            // cb_spiral_files
            // 
            this.cb_spiral_files.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_spiral_files.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_spiral_files.FormattingEnabled = true;
            this.cb_spiral_files.Location = new System.Drawing.Point(3, 16);
            this.cb_spiral_files.Name = "cb_spiral_files";
            this.cb_spiral_files.Size = new System.Drawing.Size(252, 21);
            this.cb_spiral_files.TabIndex = 0;
            this.cb_spiral_files.SelectedIndexChanged += new System.EventHandler(this.cb_spiral_files_SelectedIndexChanged);
            // 
            // Uc_SelectSpiralControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox5);
            this.Name = "Uc_SelectSpiralControl";
            this.Size = new System.Drawing.Size(258, 42);
            this.Load += new System.EventHandler(this.Uc_SelectSpiralControl_Load);
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox cb_spiral_files;
    }
}
