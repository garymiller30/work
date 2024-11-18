namespace PluginGMail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WindowOut));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.tsb_ok = new System.Windows.Forms.ToolStripButton();
            this.webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.tstb_zoom = new System.Windows.Forms.ToolStripTextBox();
            this.tsb_okZoom = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.webView21)).BeginInit();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox1,
            this.tsb_ok});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(608, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(30, 25);
            this.toolStripTextBox1.Text = "80";
            this.toolStripTextBox1.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tsb_ok
            // 
            this.tsb_ok.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsb_ok.Image = ((System.Drawing.Image)(resources.GetObject("tsb_ok.Image")));
            this.tsb_ok.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_ok.Name = "tsb_ok";
            this.tsb_ok.Size = new System.Drawing.Size(26, 22);
            this.tsb_ok.Text = "Ok";
            this.tsb_ok.Click += new System.EventHandler(this.tsb_ok_Click);
            // 
            // webView21
            // 
            this.webView21.AllowExternalDrop = true;
            this.webView21.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webView21.CreationProperties = null;
            this.webView21.DefaultBackgroundColor = System.Drawing.Color.White;
            this.webView21.Location = new System.Drawing.Point(3, 28);
            this.webView21.Name = "webView21";
            this.webView21.Size = new System.Drawing.Size(602, 347);
            this.webView21.TabIndex = 0;
            this.webView21.ZoomFactor = 1D;
            // 
            // toolStrip2
            // 
            this.toolStrip2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tstb_zoom,
            this.tsb_okZoom});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(608, 25);
            this.toolStrip2.TabIndex = 1;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // tstb_zoom
            // 
            this.tstb_zoom.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tstb_zoom.Name = "tstb_zoom";
            this.tstb_zoom.Size = new System.Drawing.Size(30, 25);
            this.tstb_zoom.Text = "80";
            // 
            // tsb_okZoom
            // 
            this.tsb_okZoom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsb_okZoom.Image = ((System.Drawing.Image)(resources.GetObject("tsb_okZoom.Image")));
            this.tsb_okZoom.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_okZoom.Name = "tsb_okZoom";
            this.tsb_okZoom.Size = new System.Drawing.Size(26, 22);
            this.tsb_okZoom.Text = "Ok";
            this.tsb_okZoom.Click += new System.EventHandler(this.tsb_okZoom_Click);
            // 
            // WindowOut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.webView21);
            this.Name = "WindowOut";
            this.Size = new System.Drawing.Size(608, 378);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.webView21)).EndInit();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStripButton tsb_ok;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripTextBox tstb_zoom;
        private System.Windows.Forms.ToolStripButton tsb_okZoom;
    }
}
