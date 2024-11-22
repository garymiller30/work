namespace JobSpace.UserForms.PDF.ImposItems
{
    partial class AddTemplateSheetControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddTemplateSheetControl));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.objectListView1 = new BrightIdeasSoftware.ObjectListView();
            this.olvColumnId = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnDesc = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnPrintType = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsb_add = new System.Windows.Forms.ToolStripButton();
            this.tsb_loadTemplate = new System.Windows.Forms.ToolStripButton();
            this.tsb_saveTemplate = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_edit = new System.Windows.Forms.ToolStripButton();
            this.tbs_dublicate = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_toPrint = new System.Windows.Forms.ToolStripButton();
            this.tsb_delete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_fillAll = new System.Windows.Forms.ToolStripButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.objectListView1);
            this.groupBox1.Location = new System.Drawing.Point(3, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(329, 141);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Шаблони листів для друку";
            // 
            // objectListView1
            // 
            this.objectListView1.AllColumns.Add(this.olvColumnId);
            this.objectListView1.AllColumns.Add(this.olvColumnDesc);
            this.objectListView1.AllColumns.Add(this.olvColumnPrintType);
            this.objectListView1.CellEditUseWholeCell = false;
            this.objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnId,
            this.olvColumnDesc,
            this.olvColumnPrintType});
            this.objectListView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectListView1.FullRowSelect = true;
            this.objectListView1.GridLines = true;
            this.objectListView1.HideSelection = false;
            this.objectListView1.Location = new System.Drawing.Point(3, 16);
            this.objectListView1.MultiSelect = false;
            this.objectListView1.Name = "objectListView1";
            this.objectListView1.ShowGroups = false;
            this.objectListView1.Size = new System.Drawing.Size(323, 122);
            this.objectListView1.TabIndex = 0;
            this.objectListView1.UseCompatibleStateImageBehavior = false;
            this.objectListView1.View = System.Windows.Forms.View.Details;
            // 
            // olvColumnId
            // 
            this.olvColumnId.Text = "Id";
            this.olvColumnId.Width = 29;
            // 
            // olvColumnDesc
            // 
            this.olvColumnDesc.Text = "Опис";
            this.olvColumnDesc.Width = 149;
            // 
            // olvColumnPrintType
            // 
            this.olvColumnPrintType.Text = "тип друку";
            this.olvColumnPrintType.Width = 104;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_add,
            this.tsb_loadTemplate,
            this.tsb_saveTemplate,
            this.toolStripSeparator3,
            this.tsb_edit,
            this.tbs_dublicate,
            this.toolStripSeparator1,
            this.tsb_toPrint,
            this.tsb_delete,
            this.toolStripSeparator2,
            this.tsb_fillAll});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(335, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsb_add
            // 
            this.tsb_add.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_add.Image = ((System.Drawing.Image)(resources.GetObject("tsb_add.Image")));
            this.tsb_add.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_add.Name = "tsb_add";
            this.tsb_add.Size = new System.Drawing.Size(23, 22);
            this.tsb_add.Text = "додати шаблон листа";
            this.tsb_add.Click += new System.EventHandler(this.tsb_add_Click);
            // 
            // tsb_loadTemplate
            // 
            this.tsb_loadTemplate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_loadTemplate.Image = ((System.Drawing.Image)(resources.GetObject("tsb_loadTemplate.Image")));
            this.tsb_loadTemplate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_loadTemplate.Name = "tsb_loadTemplate";
            this.tsb_loadTemplate.Size = new System.Drawing.Size(23, 22);
            this.tsb_loadTemplate.Text = "Завантажити шаблон";
            this.tsb_loadTemplate.Click += new System.EventHandler(this.tsb_loadTemplate_Click);
            // 
            // tsb_saveTemplate
            // 
            this.tsb_saveTemplate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_saveTemplate.Image = ((System.Drawing.Image)(resources.GetObject("tsb_saveTemplate.Image")));
            this.tsb_saveTemplate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_saveTemplate.Name = "tsb_saveTemplate";
            this.tsb_saveTemplate.Size = new System.Drawing.Size(23, 22);
            this.tsb_saveTemplate.Text = "Зберегти шаблон";
            this.tsb_saveTemplate.Click += new System.EventHandler(this.tsb_saveTemplate_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsb_edit
            // 
            this.tsb_edit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_edit.Image = ((System.Drawing.Image)(resources.GetObject("tsb_edit.Image")));
            this.tsb_edit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_edit.Name = "tsb_edit";
            this.tsb_edit.Size = new System.Drawing.Size(23, 22);
            this.tsb_edit.Text = "редагувати шаблон листа";
            this.tsb_edit.Click += new System.EventHandler(this.tsb_edit_Click);
            // 
            // tbs_dublicate
            // 
            this.tbs_dublicate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbs_dublicate.Image = ((System.Drawing.Image)(resources.GetObject("tbs_dublicate.Image")));
            this.tbs_dublicate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbs_dublicate.Name = "tbs_dublicate";
            this.tbs_dublicate.Size = new System.Drawing.Size(23, 22);
            this.tbs_dublicate.Text = "Дублювати лист";
            this.tbs_dublicate.Click += new System.EventHandler(this.tbs_dublicate_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsb_toPrint
            // 
            this.tsb_toPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_toPrint.Image = ((System.Drawing.Image)(resources.GetObject("tsb_toPrint.Image")));
            this.tsb_toPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_toPrint.Name = "tsb_toPrint";
            this.tsb_toPrint.Size = new System.Drawing.Size(23, 22);
            this.tsb_toPrint.Text = "копіювати до друкарських листів";
            this.tsb_toPrint.Click += new System.EventHandler(this.tsb_toPrint_Click);
            // 
            // tsb_delete
            // 
            this.tsb_delete.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsb_delete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_delete.Image = ((System.Drawing.Image)(resources.GetObject("tsb_delete.Image")));
            this.tsb_delete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_delete.Name = "tsb_delete";
            this.tsb_delete.Size = new System.Drawing.Size(23, 22);
            this.tsb_delete.Text = "видалити шаблон листа";
            this.tsb_delete.ToolTipText = "видалити шаблон листа (+Shift - видалити все)";
            this.tsb_delete.Click += new System.EventHandler(this.tsb_delete_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsb_fillAll
            // 
            this.tsb_fillAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_fillAll.Image = ((System.Drawing.Image)(resources.GetObject("tsb_fillAll.Image")));
            this.tsb_fillAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_fillAll.Name = "tsb_fillAll";
            this.tsb_fillAll.Size = new System.Drawing.Size(23, 22);
            this.tsb_fillAll.Text = "Заповнити все";
            this.tsb_fillAll.Click += new System.EventHandler(this.tsb_fillAll_Click);
            // 
            // AddTemplateSheetControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(335, 158);
            this.Name = "AddTemplateSheetControl";
            this.Size = new System.Drawing.Size(335, 172);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private BrightIdeasSoftware.ObjectListView objectListView1;
        private BrightIdeasSoftware.OLVColumn olvColumnId;
        private BrightIdeasSoftware.OLVColumn olvColumnDesc;
        private BrightIdeasSoftware.OLVColumn olvColumnPrintType;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsb_add;
        private System.Windows.Forms.ToolStripButton tsb_edit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsb_toPrint;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsb_delete;
        private System.Windows.Forms.ToolStripButton tsb_fillAll;
        private System.Windows.Forms.ToolStripButton tbs_dublicate;
        private System.Windows.Forms.ToolStripButton tsb_loadTemplate;
        private System.Windows.Forms.ToolStripButton tsb_saveTemplate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}
