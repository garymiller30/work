namespace JobSpace.UserForms.PDF.ImposItems
{
    partial class RunListControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RunListControl));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsb_AddEmptyPage = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_RemovePage = new System.Windows.Forms.ToolStripButton();
            this.objectListViewRunList = new BrightIdeasSoftware.ObjectListView();
            this.olvColumIdx = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnRunListPages = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnAsign = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.groupBox2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectListViewRunList)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.toolStrip1);
            this.groupBox2.Controls.Add(this.objectListViewRunList);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(124, 385);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Сторінки спуску";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_AddEmptyPage,
            this.toolStripSeparator1,
            this.tsb_RemovePage});
            this.toolStrip1.Location = new System.Drawing.Point(3, 16);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(118, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsb_AddEmptyPage
            // 
            this.tsb_AddEmptyPage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_AddEmptyPage.Image = ((System.Drawing.Image)(resources.GetObject("tsb_AddEmptyPage.Image")));
            this.tsb_AddEmptyPage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_AddEmptyPage.Name = "tsb_AddEmptyPage";
            this.tsb_AddEmptyPage.Size = new System.Drawing.Size(23, 22);
            this.tsb_AddEmptyPage.Text = "Додати пусту сторінку";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsb_RemovePage
            // 
            this.tsb_RemovePage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_RemovePage.Image = ((System.Drawing.Image)(resources.GetObject("tsb_RemovePage.Image")));
            this.tsb_RemovePage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_RemovePage.Name = "tsb_RemovePage";
            this.tsb_RemovePage.Size = new System.Drawing.Size(23, 22);
            this.tsb_RemovePage.Text = "Видалити сторінку";
            // 
            // objectListViewRunList
            // 
            this.objectListViewRunList.AllColumns.Add(this.olvColumIdx);
            this.objectListViewRunList.AllColumns.Add(this.olvColumnRunListPages);
            this.objectListViewRunList.AllColumns.Add(this.olvColumnAsign);
            this.objectListViewRunList.AllowDrop = true;
            this.objectListViewRunList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.objectListViewRunList.CellEditUseWholeCell = false;
            this.objectListViewRunList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumIdx,
            this.olvColumnRunListPages,
            this.olvColumnAsign});
            this.objectListViewRunList.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListViewRunList.FullRowSelect = true;
            this.objectListViewRunList.HideSelection = false;
            this.objectListViewRunList.Location = new System.Drawing.Point(3, 44);
            this.objectListViewRunList.Name = "objectListViewRunList";
            this.objectListViewRunList.ShowGroups = false;
            this.objectListViewRunList.Size = new System.Drawing.Size(118, 338);
            this.objectListViewRunList.TabIndex = 0;
            this.objectListViewRunList.UseCompatibleStateImageBehavior = false;
            this.objectListViewRunList.View = System.Windows.Forms.View.Details;
            this.objectListViewRunList.Dropped += new System.EventHandler<BrightIdeasSoftware.OlvDropEventArgs>(this.objectListViewRunList_Dropped_1);
            // 
            // olvColumIdx
            // 
            this.olvColumIdx.Text = "№";
            this.olvColumIdx.Width = 28;
            // 
            // olvColumnRunListPages
            // 
            this.olvColumnRunListPages.Text = "Сторінки";
            this.olvColumnRunListPages.Width = 56;
            // 
            // olvColumnAsign
            // 
            this.olvColumnAsign.Text = "";
            this.olvColumnAsign.Width = 30;
            // 
            // RunListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Name = "RunListControl";
            this.Size = new System.Drawing.Size(124, 385);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectListViewRunList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsb_AddEmptyPage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsb_RemovePage;
        private BrightIdeasSoftware.ObjectListView objectListViewRunList;
        private BrightIdeasSoftware.OLVColumn olvColumIdx;
        private BrightIdeasSoftware.OLVColumn olvColumnRunListPages;
        private BrightIdeasSoftware.OLVColumn olvColumnAsign;
    }
}
