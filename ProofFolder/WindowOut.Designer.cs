namespace ProofFolder
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.objectListViewOrders = new BrightIdeasSoftware.ObjectListView();
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn2 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnOrderState = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.objectListView_OrderAttachment = new BrightIdeasSoftware.ObjectListView();
            this.olvColumnFileName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.toolStripSplitButtonStatus = new System.Windows.Forms.ToolStripSplitButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectListViewOrders)).BeginInit();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectListView_OrderAttachment)).BeginInit();
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
            this.splitContainer1.Panel1.Controls.Add(this.objectListViewOrders);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.objectListView_OrderAttachment);
            this.splitContainer1.Size = new System.Drawing.Size(380, 377);
            this.splitContainer1.SplitterDistance = 281;
            this.splitContainer1.TabIndex = 0;
            // 
            // objectListViewOrders
            // 
            this.objectListViewOrders.AllColumns.Add(this.olvColumn1);
            this.objectListViewOrders.AllColumns.Add(this.olvColumn2);
            this.objectListViewOrders.AllColumns.Add(this.olvColumnOrderState);
            this.objectListViewOrders.CellEditUseWholeCell = false;
            this.objectListViewOrders.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn1,
            this.olvColumn2,
            this.olvColumnOrderState});
            this.objectListViewOrders.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListViewOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectListViewOrders.FullRowSelect = true;
            this.objectListViewOrders.GridLines = true;
            this.objectListViewOrders.Location = new System.Drawing.Point(0, 25);
            this.objectListViewOrders.Name = "objectListViewOrders";
            this.objectListViewOrders.ShowGroups = false;
            this.objectListViewOrders.Size = new System.Drawing.Size(380, 256);
            this.objectListViewOrders.TabIndex = 1;
            this.objectListViewOrders.UseCompatibleStateImageBehavior = false;
            this.objectListViewOrders.View = System.Windows.Forms.View.Details;
            this.objectListViewOrders.Click += new System.EventHandler(this.objectListViewOrders_Click);
            // 
            // olvColumn1
            // 
            this.olvColumn1.AspectName = "ID_number";
            this.olvColumn1.Text = "ID_number";
            this.olvColumn1.Width = 100;
            // 
            // olvColumn2
            // 
            this.olvColumn2.AspectName = "Comment";
            this.olvColumn2.Text = "Comment";
            this.olvColumn2.Width = 250;
            // 
            // olvColumnOrderState
            // 
            this.olvColumnOrderState.AspectName = "";
            this.olvColumnOrderState.Text = "OrderState";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSplitButtonStatus});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(380, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // objectListView_OrderAttachment
            // 
            this.objectListView_OrderAttachment.AllColumns.Add(this.olvColumnFileName);
            this.objectListView_OrderAttachment.AllowDrop = true;
            this.objectListView_OrderAttachment.CellEditUseWholeCell = false;
            this.objectListView_OrderAttachment.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnFileName});
            this.objectListView_OrderAttachment.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListView_OrderAttachment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectListView_OrderAttachment.FullRowSelect = true;
            this.objectListView_OrderAttachment.GridLines = true;
            this.objectListView_OrderAttachment.HideSelection = false;
            this.objectListView_OrderAttachment.Location = new System.Drawing.Point(0, 0);
            this.objectListView_OrderAttachment.Name = "objectListView_OrderAttachment";
            this.objectListView_OrderAttachment.ShowGroups = false;
            this.objectListView_OrderAttachment.Size = new System.Drawing.Size(380, 92);
            this.objectListView_OrderAttachment.TabIndex = 0;
            this.objectListView_OrderAttachment.UseCompatibleStateImageBehavior = false;
            this.objectListView_OrderAttachment.View = System.Windows.Forms.View.Details;
            this.objectListView_OrderAttachment.DragDrop += new System.Windows.Forms.DragEventHandler(this.objectListView_OrderAttachment_DragDrop);
            this.objectListView_OrderAttachment.DragOver += new System.Windows.Forms.DragEventHandler(this.objectListView_OrderAttachment_DragOver);
            // 
            // olvColumnFileName
            // 
            this.olvColumnFileName.AspectName = "FileName";
            this.olvColumnFileName.Text = "FileName";
            this.olvColumnFileName.Width = 200;
            // 
            // toolStripSplitButtonStatus
            // 
            this.toolStripSplitButtonStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButtonStatus.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButtonStatus.Image")));
            this.toolStripSplitButtonStatus.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButtonStatus.Name = "toolStripSplitButtonStatus";
            this.toolStripSplitButtonStatus.Size = new System.Drawing.Size(32, 22);
            this.toolStripSplitButtonStatus.Text = "toolStripSplitButton1";
            // 
            // WindowOut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "WindowOut";
            this.Size = new System.Drawing.Size(380, 377);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.objectListViewOrders)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectListView_OrderAttachment)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private BrightIdeasSoftware.ObjectListView objectListViewOrders;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private BrightIdeasSoftware.ObjectListView objectListView_OrderAttachment;
        private BrightIdeasSoftware.OLVColumn olvColumnFileName;
        private BrightIdeasSoftware.OLVColumn olvColumn1;
        private BrightIdeasSoftware.OLVColumn olvColumn2;
        private BrightIdeasSoftware.OLVColumn olvColumnOrderState;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButtonStatus;
    }
}
