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
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            tsb_previous_page = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            tst_cur_page = new System.Windows.Forms.ToolStripTextBox();
            tsl_count_pages = new System.Windows.Forms.ToolStripLabel();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            tsb_next_page = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            tsb_fit_to_window = new System.Windows.Forms.ToolStripButton();
            tsb_show_spread = new System.Windows.Forms.ToolStripButton();
            uc_PreviewControl1 = new Uc_PreviewControl();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tsb_previous_page, toolStripSeparator1, tst_cur_page, tsl_count_pages, toolStripSeparator2, tsb_next_page, toolStripSeparator3, tsb_fit_to_window, tsb_show_spread });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(272, 25);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // tsb_previous_page
            // 
            tsb_previous_page.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsb_previous_page.Image = (System.Drawing.Image)resources.GetObject("tsb_previous_page.Image");
            tsb_previous_page.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsb_previous_page.Name = "tsb_previous_page";
            tsb_previous_page.Size = new System.Drawing.Size(23, 22);
            tsb_previous_page.Text = "попередня сторінка";
            tsb_previous_page.Click += tsb_previous_page_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tst_cur_page
            // 
            tst_cur_page.Name = "tst_cur_page";
            tst_cur_page.Size = new System.Drawing.Size(34, 25);
            tst_cur_page.Text = "1";
            tst_cur_page.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            tst_cur_page.TextChanged += tst_cur_page_TextChanged;
            // 
            // tsl_count_pages
            // 
            tsl_count_pages.Name = "tsl_count_pages";
            tsl_count_pages.Size = new System.Drawing.Size(24, 22);
            tsl_count_pages.Text = "/00";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsb_next_page
            // 
            tsb_next_page.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsb_next_page.Image = (System.Drawing.Image)resources.GetObject("tsb_next_page.Image");
            tsb_next_page.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsb_next_page.Name = "tsb_next_page";
            tsb_next_page.Size = new System.Drawing.Size(23, 22);
            tsb_next_page.Text = "наступна сторінка";
            tsb_next_page.Click += tsb_next_page_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsb_fit_to_window
            // 
            tsb_fit_to_window.Checked = true;
            tsb_fit_to_window.CheckOnClick = true;
            tsb_fit_to_window.CheckState = System.Windows.Forms.CheckState.Checked;
            tsb_fit_to_window.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsb_fit_to_window.Image = (System.Drawing.Image)resources.GetObject("tsb_fit_to_window.Image");
            tsb_fit_to_window.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsb_fit_to_window.Name = "tsb_fit_to_window";
            tsb_fit_to_window.Size = new System.Drawing.Size(23, 22);
            tsb_fit_to_window.Text = "Зображення в розмір вікна";
            tsb_fit_to_window.CheckStateChanged += tsb_fit_to_window_CheckStateChanged;
            // 
            // tsb_show_spread
            // 
            tsb_show_spread.CheckOnClick = true;
            tsb_show_spread.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsb_show_spread.Image = (System.Drawing.Image)resources.GetObject("tsb_show_spread.Image");
            tsb_show_spread.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsb_show_spread.Name = "tsb_show_spread";
            tsb_show_spread.Size = new System.Drawing.Size(23, 22);
            tsb_show_spread.Text = "Розвороти";
            tsb_show_spread.ToolTipText = "Показати розворотами";
            tsb_show_spread.CheckedChanged += tsb_show_spread_CheckedChanged;
            // 
            // uc_PreviewControl1
            // 
            uc_PreviewControl1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            uc_PreviewControl1.FitToScreen = true;
            uc_PreviewControl1.Location = new System.Drawing.Point(0, 29);
            uc_PreviewControl1.Margin = new System.Windows.Forms.Padding(0);
            uc_PreviewControl1.Name = "uc_PreviewControl1";
            uc_PreviewControl1.Size = new System.Drawing.Size(272, 233);
            uc_PreviewControl1.TabIndex = 1;
            // 
            // Uc_FilePreviewControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(uc_PreviewControl1);
            Controls.Add(toolStrip1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "Uc_FilePreviewControl";
            Size = new System.Drawing.Size(272, 262);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

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
        private System.Windows.Forms.ToolStripButton tsb_show_spread;
    }
}
