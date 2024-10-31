namespace JobSpace.UserForms.PDF.ImposItems
{
    partial class MarksControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MarksControl));
            this.tlv_MarksResources = new BrightIdeasSoftware.TreeListView();
            this.ovl_ResourceMarks = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsb_addGroup = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_Delete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_addPdfMark = new System.Windows.Forms.ToolStripButton();
            this.tsb_addTextMark = new System.Windows.Forms.ToolStripButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tlv_ProductMarks = new BrightIdeasSoftware.TreeListView();
            this.olv_ProductMarkName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.tsb_sheet_deleteMark = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.tlv_MarksResources)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tlv_ProductMarks)).BeginInit();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlv_MarksResources
            // 
            this.tlv_MarksResources.AllColumns.Add(this.ovl_ResourceMarks);
            this.tlv_MarksResources.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlv_MarksResources.CellEditUseWholeCell = false;
            this.tlv_MarksResources.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ovl_ResourceMarks});
            this.tlv_MarksResources.Cursor = System.Windows.Forms.Cursors.Default;
            this.tlv_MarksResources.FullRowSelect = true;
            this.tlv_MarksResources.HideSelection = false;
            this.tlv_MarksResources.IsSimpleDragSource = true;
            this.tlv_MarksResources.Location = new System.Drawing.Point(3, 44);
            this.tlv_MarksResources.Name = "tlv_MarksResources";
            this.tlv_MarksResources.ShowGroups = false;
            this.tlv_MarksResources.Size = new System.Drawing.Size(219, 255);
            this.tlv_MarksResources.SmallImageList = this.imageList1;
            this.tlv_MarksResources.TabIndex = 0;
            this.tlv_MarksResources.UseCompatibleStateImageBehavior = false;
            this.tlv_MarksResources.View = System.Windows.Forms.View.Details;
            this.tlv_MarksResources.VirtualMode = true;
            this.tlv_MarksResources.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tlv_MarksResources_MouseDoubleClick);
            // 
            // ovl_ResourceMarks
            // 
            this.ovl_ResourceMarks.Text = "мітки";
            this.ovl_ResourceMarks.Width = 168;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Yusuke-Kamiyamane-Fugue-Folder-smiley.16.png");
            this.imageList1.Images.SetKeyName(1, "Emoopo-Darktheme-Folder-Folder-Text-PDF.16.png");
            this.imageList1.Images.SetKeyName(2, "Emoopo-Darktheme-Folder-Folder-Text.16.png");
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
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Size = new System.Drawing.Size(225, 605);
            this.splitContainer1.SplitterDistance = 302;
            this.splitContainer1.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.toolStrip1);
            this.groupBox1.Controls.Add(this.tlv_MarksResources);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(225, 302);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Наявні мітки";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_addGroup,
            this.toolStripSeparator1,
            this.tsb_Delete,
            this.toolStripSeparator2,
            this.tsb_addPdfMark,
            this.tsb_addTextMark});
            this.toolStrip1.Location = new System.Drawing.Point(3, 16);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(219, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsb_addGroup
            // 
            this.tsb_addGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_addGroup.Image = ((System.Drawing.Image)(resources.GetObject("tsb_addGroup.Image")));
            this.tsb_addGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_addGroup.Name = "tsb_addGroup";
            this.tsb_addGroup.Size = new System.Drawing.Size(23, 22);
            this.tsb_addGroup.Text = "Додати групу";
            this.tsb_addGroup.Click += new System.EventHandler(this.tsb_addGroup_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsb_Delete
            // 
            this.tsb_Delete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_Delete.Image = ((System.Drawing.Image)(resources.GetObject("tsb_Delete.Image")));
            this.tsb_Delete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Delete.Name = "tsb_Delete";
            this.tsb_Delete.Size = new System.Drawing.Size(23, 22);
            this.tsb_Delete.Text = "Видалити групу або мітку";
            this.tsb_Delete.Click += new System.EventHandler(this.tsb_Delete_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsb_addPdfMark
            // 
            this.tsb_addPdfMark.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_addPdfMark.Image = ((System.Drawing.Image)(resources.GetObject("tsb_addPdfMark.Image")));
            this.tsb_addPdfMark.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_addPdfMark.Name = "tsb_addPdfMark";
            this.tsb_addPdfMark.Size = new System.Drawing.Size(23, 22);
            this.tsb_addPdfMark.Text = "Додати PDF мітку";
            this.tsb_addPdfMark.Click += new System.EventHandler(this.tsb_addPdfMark_Click);
            // 
            // tsb_addTextMark
            // 
            this.tsb_addTextMark.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_addTextMark.Image = ((System.Drawing.Image)(resources.GetObject("tsb_addTextMark.Image")));
            this.tsb_addTextMark.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_addTextMark.Name = "tsb_addTextMark";
            this.tsb_addTextMark.Size = new System.Drawing.Size(23, 22);
            this.tsb_addTextMark.Text = "Додати текстову мітку";
            this.tsb_addTextMark.Click += new System.EventHandler(this.tsb_addTextMark_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.toolStrip2);
            this.groupBox2.Controls.Add(this.tlv_ProductMarks);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(225, 299);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Мітки шаблону";
            // 
            // tlv_ProductMarks
            // 
            this.tlv_ProductMarks.AllColumns.Add(this.olv_ProductMarkName);
            this.tlv_ProductMarks.AllowDrop = true;
            this.tlv_ProductMarks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlv_ProductMarks.CellEditUseWholeCell = false;
            this.tlv_ProductMarks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olv_ProductMarkName});
            this.tlv_ProductMarks.Cursor = System.Windows.Forms.Cursors.Default;
            this.tlv_ProductMarks.Enabled = false;
            this.tlv_ProductMarks.FullRowSelect = true;
            this.tlv_ProductMarks.HideSelection = false;
            this.tlv_ProductMarks.Location = new System.Drawing.Point(3, 44);
            this.tlv_ProductMarks.Name = "tlv_ProductMarks";
            this.tlv_ProductMarks.ShowGroups = false;
            this.tlv_ProductMarks.Size = new System.Drawing.Size(219, 252);
            this.tlv_ProductMarks.SmallImageList = this.imageList2;
            this.tlv_ProductMarks.TabIndex = 1;
            this.tlv_ProductMarks.UseCompatibleStateImageBehavior = false;
            this.tlv_ProductMarks.View = System.Windows.Forms.View.Details;
            this.tlv_ProductMarks.VirtualMode = true;
            // 
            // olv_ProductMarkName
            // 
            this.olv_ProductMarkName.Text = "мітки";
            this.olv_ProductMarkName.Width = 164;
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "1471082_base_data_document_office_page_icon.png");
            this.imageList2.Images.SetKeyName(1, "4017615_book_education_lesson_study_competitive strategy_icon.png");
            this.imageList2.Images.SetKeyName(2, "Emoopo-Darktheme-Folder-Folder-Text-PDF.16.png");
            this.imageList2.Images.SetKeyName(3, "Emoopo-Darktheme-Folder-Folder-Text.16.png");
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_sheet_deleteMark});
            this.toolStrip2.Location = new System.Drawing.Point(3, 16);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(219, 25);
            this.toolStrip2.TabIndex = 2;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // tsb_sheet_deleteMark
            // 
            this.tsb_sheet_deleteMark.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_sheet_deleteMark.Image = ((System.Drawing.Image)(resources.GetObject("tsb_sheet_deleteMark.Image")));
            this.tsb_sheet_deleteMark.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_sheet_deleteMark.Name = "tsb_sheet_deleteMark";
            this.tsb_sheet_deleteMark.Size = new System.Drawing.Size(23, 22);
            this.tsb_sheet_deleteMark.Text = "Видалити групу або мітку";
            this.tsb_sheet_deleteMark.Click += new System.EventHandler(this.tsb_sheet_deleteMark_Click);
            // 
            // MarksControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "MarksControl";
            this.Size = new System.Drawing.Size(225, 605);
            ((System.ComponentModel.ISupportInitialize)(this.tlv_MarksResources)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tlv_ProductMarks)).EndInit();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.TreeListView tlv_MarksResources;
        private BrightIdeasSoftware.OLVColumn ovl_ResourceMarks;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private BrightIdeasSoftware.TreeListView tlv_ProductMarks;
        private BrightIdeasSoftware.OLVColumn olv_ProductMarkName;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsb_addGroup;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsb_Delete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsb_addPdfMark;
        private System.Windows.Forms.ToolStripButton tsb_addTextMark;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton tsb_sheet_deleteMark;
    }
}
