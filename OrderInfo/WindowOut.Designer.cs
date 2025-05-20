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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cb_cut = new System.Windows.Forms.CheckBox();
            this.cb_uv_lak = new System.Windows.Forms.CheckBox();
            this.cb_protected_lak = new System.Windows.Forms.CheckBox();
            this.ucNote1 = new JobSpace.UC.UcNote();
            this.ucAddWorkPluginsContainer1 = new JobSpace.UC.UcAddWorkPluginsContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.Controls.Add(this.ucNote1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ucAddWorkPluginsContainer1);
            this.splitContainer1.Size = new System.Drawing.Size(470, 282);
            this.splitContainer1.SplitterDistance = 205;
            this.splitContainer1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.cb_protected_lak);
            this.panel1.Controls.Add(this.cb_uv_lak);
            this.panel1.Controls.Add(this.cb_cut);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(464, 27);
            this.panel1.TabIndex = 1;
            // 
            // cb_cut
            // 
            this.cb_cut.AutoSize = true;
            this.cb_cut.Location = new System.Drawing.Point(4, 4);
            this.cb_cut.Name = "cb_cut";
            this.cb_cut.Size = new System.Drawing.Size(89, 17);
            this.cb_cut.TabIndex = 0;
            this.cb_cut.Text = "контур ножа";
            this.cb_cut.UseVisualStyleBackColor = true;
            // 
            // cb_uv_lak
            // 
            this.cb_uv_lak.AutoSize = true;
            this.cb_uv_lak.Location = new System.Drawing.Point(99, 4);
            this.cb_uv_lak.Name = "cb_uv_lak";
            this.cb_uv_lak.Size = new System.Drawing.Size(66, 17);
            this.cb_uv_lak.TabIndex = 1;
            this.cb_uv_lak.Text = "УФ лак";
            this.cb_uv_lak.UseVisualStyleBackColor = true;
            // 
            // cb_protected_lak
            // 
            this.cb_protected_lak.AutoSize = true;
            this.cb_protected_lak.Location = new System.Drawing.Point(171, 4);
            this.cb_protected_lak.Name = "cb_protected_lak";
            this.cb_protected_lak.Size = new System.Drawing.Size(94, 17);
            this.cb_protected_lak.TabIndex = 2;
            this.cb_protected_lak.Text = "захисний лак";
            this.cb_protected_lak.UseVisualStyleBackColor = true;
            // 
            // ucNote1
            // 
            this.ucNote1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ucNote1.Location = new System.Drawing.Point(3, 36);
            this.ucNote1.Name = "ucNote1";
            this.ucNote1.Size = new System.Drawing.Size(464, 166);
            this.ucNote1.TabIndex = 0;
            this.ucNote1.Leave += new System.EventHandler(this.RichTextBox1_Leave);
            // 
            // ucAddWorkPluginsContainer1
            // 
            this.ucAddWorkPluginsContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucAddWorkPluginsContainer1.Location = new System.Drawing.Point(0, 0);
            this.ucAddWorkPluginsContainer1.Name = "ucAddWorkPluginsContainer1";
            this.ucAddWorkPluginsContainer1.Size = new System.Drawing.Size(470, 73);
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
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private JobSpace.UC.UcNote ucNote1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private JobSpace.UC.UcAddWorkPluginsContainer ucAddWorkPluginsContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox cb_protected_lak;
        private System.Windows.Forms.CheckBox cb_uv_lak;
        private System.Windows.Forms.CheckBox cb_cut;
    }
}
