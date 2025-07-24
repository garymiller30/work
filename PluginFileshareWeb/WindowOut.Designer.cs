namespace PluginFileshareWeb
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
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsb_add_tab = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripTextBoxUrl = new System.Windows.Forms.ToolStripTextBox();
            this.tsb_go = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tstb_zoomFactor = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tsb_zoomOk = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_Add = new System.Windows.Forms.ToolStripButton();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.tabControl1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(809, 428);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(809, 453);
            this.toolStripContainer1.TabIndex = 1;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // tabControl1
            // 
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(809, 428);
            this.tabControl1.TabIndex = 1;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_add_tab,
            this.toolStripSeparator2,
            this.toolStripTextBoxUrl,
            this.tsb_go,
            this.toolStripSeparator1,
            this.tstb_zoomFactor,
            this.toolStripLabel1,
            this.tsb_zoomOk,
            this.toolStripSeparator3,
            this.toolStripButton_Add});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(570, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsb_add_tab
            // 
            this.tsb_add_tab.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsb_add_tab.Image = ((System.Drawing.Image)(resources.GetObject("tsb_add_tab.Image")));
            this.tsb_add_tab.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_add_tab.Name = "tsb_add_tab";
            this.tsb_add_tab.Size = new System.Drawing.Size(73, 22);
            this.tsb_add_tab.Text = "+ закладку ";
            this.tsb_add_tab.Click += new System.EventHandler(this.tsb_add_tab_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripTextBoxUrl
            // 
            this.toolStripTextBoxUrl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripTextBoxUrl.Name = "toolStripTextBoxUrl";
            this.toolStripTextBoxUrl.Size = new System.Drawing.Size(300, 25);
            this.toolStripTextBoxUrl.Click += new System.EventHandler(this.toolStripTextBoxUrl_Click);
            // 
            // tsb_go
            // 
            this.tsb_go.BackColor = System.Drawing.Color.PaleGreen;
            this.tsb_go.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsb_go.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_go.Name = "tsb_go";
            this.tsb_go.Size = new System.Drawing.Size(26, 22);
            this.tsb_go.Text = "Go";
            this.tsb_go.Click += new System.EventHandler(this.toolStripButtonGo_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tstb_zoomFactor
            // 
            this.tstb_zoomFactor.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tstb_zoomFactor.Name = "tstb_zoomFactor";
            this.tstb_zoomFactor.Size = new System.Drawing.Size(40, 25);
            this.tstb_zoomFactor.Text = "80";
            this.tstb_zoomFactor.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(17, 22);
            this.toolStripLabel1.Text = "%";
            // 
            // tsb_zoomOk
            // 
            this.tsb_zoomOk.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsb_zoomOk.Image = ((System.Drawing.Image)(resources.GetObject("tsb_zoomOk.Image")));
            this.tsb_zoomOk.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_zoomOk.Name = "tsb_zoomOk";
            this.tsb_zoomOk.Size = new System.Drawing.Size(26, 22);
            this.tsb_zoomOk.Text = "Ok";
            this.tsb_zoomOk.Click += new System.EventHandler(this.tsb_zoomOk_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_Add
            // 
            this.toolStripButton_Add.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_Add.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Add.Image")));
            this.toolStripButton_Add.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Add.Name = "toolStripButton_Add";
            this.toolStripButton_Add.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_Add.Text = "+";
            this.toolStripButton_Add.Click += new System.EventHandler(this.toolStripButton_Add_Click);
            // 
            // WindowOut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "WindowOut";
            this.Size = new System.Drawing.Size(809, 453);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_Add;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxUrl;
        private System.Windows.Forms.ToolStripButton tsb_go;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripTextBox tstb_zoomFactor;
        private System.Windows.Forms.ToolStripButton tsb_zoomOk;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.ToolStripButton tsb_add_tab;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}
