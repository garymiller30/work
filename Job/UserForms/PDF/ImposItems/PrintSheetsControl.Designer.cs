namespace JobSpace.UserForms.PDF.ImposItems
{
    partial class PrintSheetsControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintSheetsControl));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.objectListView1 = new BrightIdeasSoftware.ObjectListView();
            this.olvColumnId = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnDesc = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnFormat = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnPlaceType = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnCount = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnTemplatePlate = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsb_delete = new System.Windows.Forms.ToolStripButton();
            this.tsb_savePrintSheet = new System.Windows.Forms.ToolStripButton();
            this.tsb_loadPrintSheet = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_setPlate = new System.Windows.Forms.ToolStripButton();
            this.tsb_removeTemplatePlate = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_count = new System.Windows.Forms.ToolStripButton();
            this.tsb_loadFromOrderFolder = new System.Windows.Forms.ToolStripButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.objectListView1);
            this.groupBox1.Controls.Add(this.toolStrip1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(384, 171);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Друкарські листи";
            // 
            // objectListView1
            // 
            this.objectListView1.AllColumns.Add(this.olvColumnId);
            this.objectListView1.AllColumns.Add(this.olvColumnDesc);
            this.objectListView1.AllColumns.Add(this.olvColumnFormat);
            this.objectListView1.AllColumns.Add(this.olvColumnPlaceType);
            this.objectListView1.AllColumns.Add(this.olvColumnCount);
            this.objectListView1.AllColumns.Add(this.olvColumnTemplatePlate);
            this.objectListView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.objectListView1.CellEditUseWholeCell = false;
            this.objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnId,
            this.olvColumnDesc,
            this.olvColumnFormat,
            this.olvColumnPlaceType,
            this.olvColumnCount,
            this.olvColumnTemplatePlate});
            this.objectListView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListView1.FullRowSelect = true;
            this.objectListView1.GridLines = true;
            this.objectListView1.HideSelection = false;
            this.objectListView1.Location = new System.Drawing.Point(6, 44);
            this.objectListView1.Name = "objectListView1";
            this.objectListView1.ShowGroups = false;
            this.objectListView1.Size = new System.Drawing.Size(372, 121);
            this.objectListView1.TabIndex = 1;
            this.objectListView1.UseCompatibleStateImageBehavior = false;
            this.objectListView1.View = System.Windows.Forms.View.Details;
            // 
            // olvColumnId
            // 
            this.olvColumnId.Text = "№";
            this.olvColumnId.Width = 30;
            // 
            // olvColumnDesc
            // 
            this.olvColumnDesc.Text = "Опис";
            this.olvColumnDesc.Width = 124;
            // 
            // olvColumnFormat
            // 
            this.olvColumnFormat.Text = "Формат";
            // 
            // olvColumnPlaceType
            // 
            this.olvColumnPlaceType.Text = "Тип друку";
            this.olvColumnPlaceType.Width = 116;
            // 
            // olvColumnCount
            // 
            this.olvColumnCount.Text = "Тираж";
            // 
            // olvColumnTemplatePlate
            // 
            this.olvColumnTemplatePlate.AspectName = "";
            this.olvColumnTemplatePlate.Text = "Форма";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_delete,
            this.tsb_savePrintSheet,
            this.tsb_loadPrintSheet,
            this.tsb_loadFromOrderFolder,
            this.toolStripSeparator1,
            this.tsb_setPlate,
            this.tsb_removeTemplatePlate,
            this.toolStripSeparator2,
            this.tsb_count});
            this.toolStrip1.Location = new System.Drawing.Point(3, 16);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(378, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsb_delete
            // 
            this.tsb_delete.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsb_delete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_delete.Image = ((System.Drawing.Image)(resources.GetObject("tsb_delete.Image")));
            this.tsb_delete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_delete.Name = "tsb_delete";
            this.tsb_delete.Size = new System.Drawing.Size(23, 22);
            this.tsb_delete.ToolTipText = "Видалити лист \r\n(+Shift - видалити всі, \r\n+Alt - не переназначати сторінки)";
            this.tsb_delete.Click += new System.EventHandler(this.tsb_delete_Click);
            // 
            // tsb_savePrintSheet
            // 
            this.tsb_savePrintSheet.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_savePrintSheet.Image = ((System.Drawing.Image)(resources.GetObject("tsb_savePrintSheet.Image")));
            this.tsb_savePrintSheet.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_savePrintSheet.Name = "tsb_savePrintSheet";
            this.tsb_savePrintSheet.Size = new System.Drawing.Size(23, 22);
            this.tsb_savePrintSheet.Text = "зберегти друкарські листи";
            this.tsb_savePrintSheet.Click += new System.EventHandler(this.tsb_savePrintSheet_Click);
            // 
            // tsb_loadPrintSheet
            // 
            this.tsb_loadPrintSheet.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_loadPrintSheet.Image = ((System.Drawing.Image)(resources.GetObject("tsb_loadPrintSheet.Image")));
            this.tsb_loadPrintSheet.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_loadPrintSheet.Name = "tsb_loadPrintSheet";
            this.tsb_loadPrintSheet.Size = new System.Drawing.Size(23, 22);
            this.tsb_loadPrintSheet.Text = "завантажити друкарські листи";
            this.tsb_loadPrintSheet.Click += new System.EventHandler(this.tsb_loadPrintSheet_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsb_setPlate
            // 
            this.tsb_setPlate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_setPlate.Image = ((System.Drawing.Image)(resources.GetObject("tsb_setPlate.Image")));
            this.tsb_setPlate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_setPlate.Name = "tsb_setPlate";
            this.tsb_setPlate.Size = new System.Drawing.Size(23, 22);
            this.tsb_setPlate.Text = "додати форму для вибраних листів";
            this.tsb_setPlate.Click += new System.EventHandler(this.tsb_setPlate_Click);
            // 
            // tsb_removeTemplatePlate
            // 
            this.tsb_removeTemplatePlate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_removeTemplatePlate.Image = ((System.Drawing.Image)(resources.GetObject("tsb_removeTemplatePlate.Image")));
            this.tsb_removeTemplatePlate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_removeTemplatePlate.Name = "tsb_removeTemplatePlate";
            this.tsb_removeTemplatePlate.Size = new System.Drawing.Size(23, 22);
            this.tsb_removeTemplatePlate.Text = "видалити форми з листів";
            this.tsb_removeTemplatePlate.Click += new System.EventHandler(this.tsb_removeTemplatePlate_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsb_count
            // 
            this.tsb_count.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_count.Image = ((System.Drawing.Image)(resources.GetObject("tsb_count.Image")));
            this.tsb_count.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_count.Name = "tsb_count";
            this.tsb_count.Size = new System.Drawing.Size(23, 22);
            this.tsb_count.Text = "додати тираж";
            this.tsb_count.Click += new System.EventHandler(this.tsb_count_Click);
            // 
            // tsb_loadFromOrderFolder
            // 
            this.tsb_loadFromOrderFolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_loadFromOrderFolder.Image = ((System.Drawing.Image)(resources.GetObject("tsb_loadFromOrderFolder.Image")));
            this.tsb_loadFromOrderFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_loadFromOrderFolder.Name = "tsb_loadFromOrderFolder";
            this.tsb_loadFromOrderFolder.Size = new System.Drawing.Size(23, 22);
            this.tsb_loadFromOrderFolder.Text = "Відкрити з папки замовлення";
            this.tsb_loadFromOrderFolder.Click += new System.EventHandler(this.tsb_loadFromOrderFolder_Click);
            // 
            // PrintSheetsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "PrintSheetsControl";
            this.Size = new System.Drawing.Size(384, 171);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsb_delete;
        private BrightIdeasSoftware.ObjectListView objectListView1;
        private BrightIdeasSoftware.OLVColumn olvColumnId;
        private BrightIdeasSoftware.OLVColumn olvColumnDesc;
        private BrightIdeasSoftware.OLVColumn olvColumnPlaceType;
        private System.Windows.Forms.ToolStripButton tsb_savePrintSheet;
        private System.Windows.Forms.ToolStripButton tsb_loadPrintSheet;
        private BrightIdeasSoftware.OLVColumn olvColumnFormat;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsb_setPlate;
        private System.Windows.Forms.ToolStripButton tsb_removeTemplatePlate;
        private BrightIdeasSoftware.OLVColumn olvColumnTemplatePlate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsb_count;
        private BrightIdeasSoftware.OLVColumn olvColumnCount;
        private System.Windows.Forms.ToolStripButton tsb_loadFromOrderFolder;
    }
}
