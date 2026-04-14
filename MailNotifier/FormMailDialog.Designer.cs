namespace MailNotifier
{
    partial class FormMailDialog
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMailDialog));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.objectListViewInbox = new BrightIdeasSoftware.ObjectListView();
            this.olvColumnDate = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnTime = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnFrom = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnSubject = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.contextMenuStripInbox = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.archiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.objectListViewAttachment = new BrightIdeasSoftware.ObjectListView();
            this.olvColumnFileName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnSize = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.contextMenuStripAttachment = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newOrderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.olvColumnId = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.toolStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectListViewInbox)).BeginInit();
            this.contextMenuStripInbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectListViewAttachment)).BeginInit();
            this.contextMenuStripAttachment.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(884, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(66, 22);
            this.toolStripButton1.Text = "Refresh";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 25);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(884, 536);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(876, 510);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Inbox";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.objectListViewInbox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(870, 504);
            this.splitContainer1.SplitterDistance = 510;
            this.splitContainer1.TabIndex = 1;
            // 
            // objectListViewInbox
            // 
            this.objectListViewInbox.AllColumns.Add(this.olvColumnId);
            this.objectListViewInbox.AllColumns.Add(this.olvColumnDate);
            this.objectListViewInbox.AllColumns.Add(this.olvColumnTime);
            this.objectListViewInbox.AllColumns.Add(this.olvColumnFrom);
            this.objectListViewInbox.AllColumns.Add(this.olvColumnSubject);
            this.objectListViewInbox.CellEditUseWholeCell = false;
            this.objectListViewInbox.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnId,
            this.olvColumnDate,
            this.olvColumnTime,
            this.olvColumnFrom,
            this.olvColumnSubject});
            this.objectListViewInbox.ContextMenuStrip = this.contextMenuStripInbox;
            this.objectListViewInbox.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListViewInbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectListViewInbox.FullRowSelect = true;
            this.objectListViewInbox.GridLines = true;
            this.objectListViewInbox.HideSelection = false;
            this.objectListViewInbox.Location = new System.Drawing.Point(0, 0);
            this.objectListViewInbox.Name = "objectListViewInbox";
            this.objectListViewInbox.Size = new System.Drawing.Size(510, 504);
            this.objectListViewInbox.TabIndex = 0;
            this.objectListViewInbox.UseCompatibleStateImageBehavior = false;
            this.objectListViewInbox.View = System.Windows.Forms.View.Details;
            this.objectListViewInbox.Click += new System.EventHandler(this.objectListViewInbox_Click);
            // 
            // olvColumnDate
            // 
            this.olvColumnDate.AspectName = "Date";
            this.olvColumnDate.Text = "Date";
            // 
            // olvColumnTime
            // 
            this.olvColumnTime.AspectName = "Time";
            this.olvColumnTime.Text = "Time";
            // 
            // olvColumnFrom
            // 
            this.olvColumnFrom.AspectName = "From";
            this.olvColumnFrom.Text = "From";
            this.olvColumnFrom.Width = 120;
            // 
            // olvColumnSubject
            // 
            this.olvColumnSubject.AspectName = "Subject";
            this.olvColumnSubject.Text = "Subject";
            this.olvColumnSubject.Width = 300;
            // 
            // contextMenuStripInbox
            // 
            this.contextMenuStripInbox.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.archiveToolStripMenuItem});
            this.contextMenuStripInbox.Name = "contextMenuStripInbox";
            this.contextMenuStripInbox.Size = new System.Drawing.Size(115, 26);
            // 
            // archiveToolStripMenuItem
            // 
            this.archiveToolStripMenuItem.Name = "archiveToolStripMenuItem";
            this.archiveToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.archiveToolStripMenuItem.Text = "Archive";
            this.archiveToolStripMenuItem.Click += new System.EventHandler(this.archiveToolStripMenuItem_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.webBrowser1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.objectListViewAttachment);
            this.splitContainer2.Size = new System.Drawing.Size(356, 504);
            this.splitContainer2.SplitterDistance = 397;
            this.splitContainer2.TabIndex = 1;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(356, 397);
            this.webBrowser1.TabIndex = 0;
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            // 
            // objectListViewAttachment
            // 
            this.objectListViewAttachment.AllColumns.Add(this.olvColumnFileName);
            this.objectListViewAttachment.AllColumns.Add(this.olvColumnSize);
            this.objectListViewAttachment.CellEditUseWholeCell = false;
            this.objectListViewAttachment.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnFileName,
            this.olvColumnSize});
            this.objectListViewAttachment.ContextMenuStrip = this.contextMenuStripAttachment;
            this.objectListViewAttachment.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListViewAttachment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectListViewAttachment.HideSelection = false;
            this.objectListViewAttachment.IsSimpleDragSource = true;
            this.objectListViewAttachment.Location = new System.Drawing.Point(0, 0);
            this.objectListViewAttachment.Name = "objectListViewAttachment";
            this.objectListViewAttachment.ShowGroups = false;
            this.objectListViewAttachment.Size = new System.Drawing.Size(356, 103);
            this.objectListViewAttachment.TabIndex = 0;
            this.objectListViewAttachment.UseCompatibleStateImageBehavior = false;
            this.objectListViewAttachment.View = System.Windows.Forms.View.Details;
            // 
            // olvColumnFileName
            // 
            this.olvColumnFileName.AspectName = "Name";
            this.olvColumnFileName.Text = "FileName";
            this.olvColumnFileName.Width = 230;
            // 
            // olvColumnSize
            // 
            this.olvColumnSize.Text = "Size";
            // 
            // contextMenuStripAttachment
            // 
            this.contextMenuStripAttachment.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newOrderToolStripMenuItem,
            this.saveToToolStripMenuItem});
            this.contextMenuStripAttachment.Name = "contextMenuStripAttachment";
            this.contextMenuStripAttachment.Size = new System.Drawing.Size(128, 48);
            // 
            // newOrderToolStripMenuItem
            // 
            this.newOrderToolStripMenuItem.Name = "newOrderToolStripMenuItem";
            this.newOrderToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.newOrderToolStripMenuItem.Text = "new order";
            // 
            // saveToToolStripMenuItem
            // 
            this.saveToToolStripMenuItem.Name = "saveToToolStripMenuItem";
            this.saveToToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.saveToToolStripMenuItem.Text = "save to...";
            this.saveToToolStripMenuItem.Click += new System.EventHandler(this.saveToToolStripMenuItem_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(876, 510);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "All";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // olvColumnId
            // 
            this.olvColumnId.AspectName = "Id";
            this.olvColumnId.Text = "Id";
            // 
            // FormMailDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 561);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "FormMailDialog";
            this.Text = "Mail";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMailDialog_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.objectListViewInbox)).EndInit();
            this.contextMenuStripInbox.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.objectListViewAttachment)).EndInit();
            this.contextMenuStripAttachment.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private BrightIdeasSoftware.ObjectListView objectListViewInbox;
        private BrightIdeasSoftware.OLVColumn olvColumnDate;
        private BrightIdeasSoftware.OLVColumn olvColumnFrom;
        private BrightIdeasSoftware.OLVColumn olvColumnSubject;
        private System.Windows.Forms.TabPage tabPage2;
        private BrightIdeasSoftware.OLVColumn olvColumnTime;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private BrightIdeasSoftware.ObjectListView objectListViewAttachment;
        private BrightIdeasSoftware.OLVColumn olvColumnFileName;
        private BrightIdeasSoftware.OLVColumn olvColumnSize;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripAttachment;
        private System.Windows.Forms.ToolStripMenuItem newOrderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripInbox;
        private System.Windows.Forms.ToolStripMenuItem archiveToolStripMenuItem;
        private BrightIdeasSoftware.OLVColumn olvColumnId;
    }
}