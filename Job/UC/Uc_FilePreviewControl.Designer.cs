namespace JobSpace.UC
{
    partial class Uc_FilePreviewControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Uc_FilePreviewControl));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsb_previous_page = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tst_cur_page = new System.Windows.Forms.ToolStripTextBox();
            this.tsl_count_pages = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_next_page = new System.Windows.Forms.ToolStripButton();
            this.uc_PreviewControl1 = new JobSpace.UC.Uc_PreviewControl();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_fit_to_window = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
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
            this.tsb_next_page,
            this.toolStripSeparator3,
            this.tsb_fit_to_window});
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
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
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
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
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
            // uc_PreviewControl1
            // 
            this.uc_PreviewControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uc_PreviewControl1.FitToScreen = true;
            this.uc_PreviewControl1.Location = new System.Drawing.Point(0, 25);
            this.uc_PreviewControl1.Margin = new System.Windows.Forms.Padding(0);
            this.uc_PreviewControl1.Name = "uc_PreviewControl1";
            this.uc_PreviewControl1.Primitives = ((System.Collections.Generic.List<Interfaces.IScreenPrimitive>)(resources.GetObject("uc_PreviewControl1.Primitives")));
            this.uc_PreviewControl1.Size = new System.Drawing.Size(233, 202);
            this.uc_PreviewControl1.TabIndex = 1;
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsb_fit_to_window
            // 
            this.tsb_fit_to_window.Checked = true;
            this.tsb_fit_to_window.CheckOnClick = true;
            this.tsb_fit_to_window.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsb_fit_to_window.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_fit_to_window.Image = ((System.Drawing.Image)(resources.GetObject("tsb_fit_to_window.Image")));
            this.tsb_fit_to_window.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_fit_to_window.Name = "tsb_fit_to_window";
            this.tsb_fit_to_window.Size = new System.Drawing.Size(23, 22);
            this.tsb_fit_to_window.Text = "Зображення в розмір вікна";
            this.tsb_fit_to_window.CheckStateChanged += new System.EventHandler(this.tsb_fit_to_window_CheckStateChanged);
            // 
            // Uc_PreviewBrowserFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.uc_PreviewControl1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "Uc_FilePreviewControl";
            this.Size = new System.Drawing.Size(233, 227);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsb_previous_page;
        private System.Windows.Forms.ToolStripTextBox tst_cur_page;
        private System.Windows.Forms.ToolStripLabel tsl_count_pages;
        private System.Windows.Forms.ToolStripButton tsb_next_page;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private Uc_PreviewControl uc_PreviewControl1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsb_fit_to_window;
    }
}
