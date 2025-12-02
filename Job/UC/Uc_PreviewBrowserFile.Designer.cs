namespace JobSpace.UC
{
    partial class Uc_PreviewBrowserFile
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Uc_PreviewBrowserFile));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsb_previous_page = new System.Windows.Forms.ToolStripButton();
            this.tst_cur_page = new System.Windows.Forms.ToolStripTextBox();
            this.tsl_count_pages = new System.Windows.Forms.ToolStripLabel();
            this.tsb_next_page = new System.Windows.Forms.ToolStripButton();
            this.pb_preview = new System.Windows.Forms.PictureBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_preview)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_previous_page,
            this.toolStripSeparator1,
            this.tst_cur_page,
            this.tsl_count_pages,
            this.toolStripSeparator2,
            this.tsb_next_page});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(233, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsb_previous_page
            // 
            this.tsb_previous_page.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_previous_page.Image = ((System.Drawing.Image)(resources.GetObject("tsb_previous_page.Image")));
            this.tsb_previous_page.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_previous_page.Name = "tsb_previous_page";
            this.tsb_previous_page.Size = new System.Drawing.Size(23, 22);
            this.tsb_previous_page.Text = "попередня сторінка";
            this.tsb_previous_page.Click += new System.EventHandler(this.tsb_previous_page_Click);
            // 
            // tst_cur_page
            // 
            this.tst_cur_page.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tst_cur_page.Name = "tst_cur_page";
            this.tst_cur_page.Size = new System.Drawing.Size(30, 25);
            this.tst_cur_page.Text = "1";
            this.tst_cur_page.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tst_cur_page.TextChanged += new System.EventHandler(this.tst_cur_page_TextChanged);
            // 
            // tsl_count_pages
            // 
            this.tsl_count_pages.Name = "tsl_count_pages";
            this.tsl_count_pages.Size = new System.Drawing.Size(24, 22);
            this.tsl_count_pages.Text = "/00";
            // 
            // tsb_next_page
            // 
            this.tsb_next_page.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_next_page.Image = ((System.Drawing.Image)(resources.GetObject("tsb_next_page.Image")));
            this.tsb_next_page.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_next_page.Name = "tsb_next_page";
            this.tsb_next_page.Size = new System.Drawing.Size(23, 22);
            this.tsb_next_page.Text = "наступна сторінка";
            this.tsb_next_page.Click += new System.EventHandler(this.tsb_next_page_Click);
            // 
            // pb_preview
            // 
            this.pb_preview.BackColor = System.Drawing.Color.White;
            this.pb_preview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pb_preview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pb_preview.Location = new System.Drawing.Point(0, 25);
            this.pb_preview.Margin = new System.Windows.Forms.Padding(0);
            this.pb_preview.Name = "pb_preview";
            this.pb_preview.Size = new System.Drawing.Size(233, 202);
            this.pb_preview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pb_preview.TabIndex = 1;
            this.pb_preview.TabStop = false;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // Uc_PreviewBrowserFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pb_preview);
            this.Controls.Add(this.toolStrip1);
            this.Name = "Uc_PreviewBrowserFile";
            this.Size = new System.Drawing.Size(233, 227);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_preview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsb_previous_page;
        private System.Windows.Forms.ToolStripTextBox tst_cur_page;
        private System.Windows.Forms.ToolStripLabel tsl_count_pages;
        private System.Windows.Forms.ToolStripButton tsb_next_page;
        private System.Windows.Forms.PictureBox pb_preview;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}
