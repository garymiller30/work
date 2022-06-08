namespace OrderInfo
{
    partial class WindowOut
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
            this.ucNote1 = new Job.UC.UcNote();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ucAddWorkPluginsContainer1 = new Job.UC.UcAddWorkPluginsContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ucNote1
            // 
            this.ucNote1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucNote1.Location = new System.Drawing.Point(0, 0);
            this.ucNote1.Name = "ucNote1";
            this.ucNote1.Size = new System.Drawing.Size(470, 141);
            this.ucNote1.TabIndex = 0;
            this.ucNote1.Leave += new System.EventHandler(this.RichTextBox1_Leave);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ucNote1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ucAddWorkPluginsContainer1);
            this.splitContainer1.Size = new System.Drawing.Size(470, 282);
            this.splitContainer1.SplitterDistance = 141;
            this.splitContainer1.TabIndex = 1;
            // 
            // ucAddWorkPluginsContainer1
            // 
            this.ucAddWorkPluginsContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucAddWorkPluginsContainer1.Location = new System.Drawing.Point(0, 0);
            this.ucAddWorkPluginsContainer1.Name = "ucAddWorkPluginsContainer1";
            this.ucAddWorkPluginsContainer1.Size = new System.Drawing.Size(470, 137);
            this.ucAddWorkPluginsContainer1.TabIndex = 0;
            // 
            // WindowOut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "WindowOut";
            this.Size = new System.Drawing.Size(470, 282);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Job.UC.UcNote ucNote1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Job.UC.UcAddWorkPluginsContainer ucAddWorkPluginsContainer1;
    }
}
