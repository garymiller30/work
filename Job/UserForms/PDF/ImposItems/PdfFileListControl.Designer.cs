namespace JobSpace.UserForms.PDF.ImposItems
{
    partial class PdfFileListControl
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.treeListViewFiles = new BrightIdeasSoftware.TreeListView();
            this.olvColumnName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnCenterMedia = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnTrim = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnCount = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeListViewFiles)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.treeListViewFiles);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(234, 400);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Файли";
            // 
            // treeListViewFiles
            // 
            this.treeListViewFiles.AllColumns.Add(this.olvColumnName);
            this.treeListViewFiles.AllColumns.Add(this.olvColumnCenterMedia);
            this.treeListViewFiles.AllColumns.Add(this.olvColumnTrim);
            this.treeListViewFiles.AllColumns.Add(this.olvColumnCount);
            this.treeListViewFiles.CellEditUseWholeCell = false;
            this.treeListViewFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnName,
            this.olvColumnCenterMedia,
            this.olvColumnTrim,
            this.olvColumnCount});
            this.treeListViewFiles.Cursor = System.Windows.Forms.Cursors.Default;
            this.treeListViewFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListViewFiles.FullRowSelect = true;
            this.treeListViewFiles.HideSelection = false;
            this.treeListViewFiles.Location = new System.Drawing.Point(3, 16);
            this.treeListViewFiles.Name = "treeListViewFiles";
            this.treeListViewFiles.ShowGroups = false;
            this.treeListViewFiles.ShowImagesOnSubItems = true;
            this.treeListViewFiles.Size = new System.Drawing.Size(228, 381);
            this.treeListViewFiles.TabIndex = 0;
            this.treeListViewFiles.UseCompatibleStateImageBehavior = false;
            this.treeListViewFiles.UseSubItemCheckBoxes = true;
            this.treeListViewFiles.View = System.Windows.Forms.View.Details;
            this.treeListViewFiles.VirtualMode = true;
            // 
            // olvColumnName
            // 
            this.olvColumnName.AspectName = "";
            this.olvColumnName.Text = "Ім\'я";
            this.olvColumnName.Width = 142;
            // 
            // olvColumnCenterMedia
            // 
            this.olvColumnCenterMedia.AspectName = "IsMediaboxCentered";
            this.olvColumnCenterMedia.CheckBoxes = true;
            this.olvColumnCenterMedia.HeaderCheckBox = true;
            this.olvColumnCenterMedia.Text = "Центрувати";
            this.olvColumnCenterMedia.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // olvColumnTrim
            // 
            this.olvColumnTrim.Text = "Trimbox";
            this.olvColumnTrim.Width = 79;
            // 
            // olvColumnCount
            // 
            this.olvColumnCount.Text = "Тираж";
            this.olvColumnCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // PdfFileListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "PdfFileListControl";
            this.Size = new System.Drawing.Size(234, 400);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeListViewFiles)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private BrightIdeasSoftware.TreeListView treeListViewFiles;
        private BrightIdeasSoftware.OLVColumn olvColumnName;
        private BrightIdeasSoftware.OLVColumn olvColumnTrim;
        private BrightIdeasSoftware.OLVColumn olvColumnCenterMedia;
        private BrightIdeasSoftware.OLVColumn olvColumnCount;
    }
}
