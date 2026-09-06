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
            groupBox1 = new System.Windows.Forms.GroupBox();
            objectListView1 = new BrightIdeasSoftware.ObjectListView();
            olvColumnId = new BrightIdeasSoftware.OLVColumn();
            olvColumnDesc = new BrightIdeasSoftware.OLVColumn();
            olvColumnFormat = new BrightIdeasSoftware.OLVColumn();
            olvColumnPlaceType = new BrightIdeasSoftware.OLVColumn();
            olvColumnCount = new BrightIdeasSoftware.OLVColumn();
            olvColumnTemplatePlate = new BrightIdeasSoftware.OLVColumn();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            tsb_delete = new System.Windows.Forms.ToolStripButton();
            tsb_savePrintSheet = new System.Windows.Forms.ToolStripButton();
            tsb_loadPrintSheet = new System.Windows.Forms.ToolStripButton();
            tsb_loadFromOrderFolder = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            tsb_setPlate = new System.Windows.Forms.ToolStripButton();
            tsb_removeTemplatePlate = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            tsb_select_all = new System.Windows.Forms.ToolStripButton();
            tsb_count = new System.Windows.Forms.ToolStripButton();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)objectListView1).BeginInit();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(objectListView1);
            groupBox1.Controls.Add(toolStrip1);
            groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox1.Location = new System.Drawing.Point(0, 0);
            groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Size = new System.Drawing.Size(448, 197);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Друкарські листи";
            // 
            // objectListView1
            // 
            objectListView1.AllColumns.Add(olvColumnId);
            objectListView1.AllColumns.Add(olvColumnDesc);
            objectListView1.AllColumns.Add(olvColumnFormat);
            objectListView1.AllColumns.Add(olvColumnPlaceType);
            objectListView1.AllColumns.Add(olvColumnCount);
            objectListView1.AllColumns.Add(olvColumnTemplatePlate);
            objectListView1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            objectListView1.CellEditUseWholeCell = false;
            objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { olvColumnId, olvColumnDesc, olvColumnFormat, olvColumnPlaceType, olvColumnCount, olvColumnTemplatePlate });
            objectListView1.FullRowSelect = true;
            objectListView1.GridLines = true;
            objectListView1.Location = new System.Drawing.Point(7, 51);
            objectListView1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            objectListView1.Name = "objectListView1";
            objectListView1.ShowGroups = false;
            objectListView1.Size = new System.Drawing.Size(433, 139);
            objectListView1.TabIndex = 1;
            objectListView1.UseCompatibleStateImageBehavior = false;
            objectListView1.View = System.Windows.Forms.View.Details;
            objectListView1.Dropped += objectListView1_Dropped;
            // 
            // olvColumnId
            // 
            olvColumnId.Text = "№";
            olvColumnId.Width = 30;
            // 
            // olvColumnDesc
            // 
            olvColumnDesc.Text = "Опис";
            olvColumnDesc.Width = 124;
            // 
            // olvColumnFormat
            // 
            olvColumnFormat.Text = "Формат";
            // 
            // olvColumnPlaceType
            // 
            olvColumnPlaceType.Text = "Тип друку";
            olvColumnPlaceType.Width = 116;
            // 
            // olvColumnCount
            // 
            olvColumnCount.Text = "Тираж";
            // 
            // olvColumnTemplatePlate
            // 
            olvColumnTemplatePlate.AspectName = "";
            olvColumnTemplatePlate.Text = "Форма";
            // 
            // toolStrip1
            // 
            toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tsb_delete, tsb_savePrintSheet, tsb_loadPrintSheet, tsb_loadFromOrderFolder, toolStripSeparator1, tsb_setPlate, tsb_removeTemplatePlate, toolStripSeparator2, tsb_select_all, tsb_count });
            toolStrip1.Location = new System.Drawing.Point(4, 19);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            toolStrip1.Size = new System.Drawing.Size(440, 25);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // tsb_delete
            // 
            tsb_delete.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            tsb_delete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsb_delete.Image = (System.Drawing.Image)resources.GetObject("tsb_delete.Image");
            tsb_delete.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsb_delete.Name = "tsb_delete";
            tsb_delete.Size = new System.Drawing.Size(23, 22);
            tsb_delete.ToolTipText = "Видалити лист \r\n(+Shift - видалити всі, \r\n+Alt - не переназначати сторінки)";
            tsb_delete.Click += tsb_delete_Click;
            // 
            // tsb_savePrintSheet
            // 
            tsb_savePrintSheet.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsb_savePrintSheet.Image = (System.Drawing.Image)resources.GetObject("tsb_savePrintSheet.Image");
            tsb_savePrintSheet.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsb_savePrintSheet.Name = "tsb_savePrintSheet";
            tsb_savePrintSheet.Size = new System.Drawing.Size(23, 22);
            tsb_savePrintSheet.Text = "зберегти друкарські листи";
            tsb_savePrintSheet.Click += tsb_savePrintSheet_Click;
            // 
            // tsb_loadPrintSheet
            // 
            tsb_loadPrintSheet.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsb_loadPrintSheet.Image = (System.Drawing.Image)resources.GetObject("tsb_loadPrintSheet.Image");
            tsb_loadPrintSheet.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsb_loadPrintSheet.Name = "tsb_loadPrintSheet";
            tsb_loadPrintSheet.Size = new System.Drawing.Size(23, 22);
            tsb_loadPrintSheet.Text = "завантажити друкарські листи";
            tsb_loadPrintSheet.Click += tsb_loadPrintSheet_Click;
            // 
            // tsb_loadFromOrderFolder
            // 
            tsb_loadFromOrderFolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsb_loadFromOrderFolder.Image = (System.Drawing.Image)resources.GetObject("tsb_loadFromOrderFolder.Image");
            tsb_loadFromOrderFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsb_loadFromOrderFolder.Name = "tsb_loadFromOrderFolder";
            tsb_loadFromOrderFolder.Size = new System.Drawing.Size(23, 22);
            tsb_loadFromOrderFolder.Text = "Відкрити з папки замовлення";
            tsb_loadFromOrderFolder.Click += tsb_loadFromOrderFolder_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsb_setPlate
            // 
            tsb_setPlate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsb_setPlate.Image = (System.Drawing.Image)resources.GetObject("tsb_setPlate.Image");
            tsb_setPlate.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsb_setPlate.Name = "tsb_setPlate";
            tsb_setPlate.Size = new System.Drawing.Size(23, 22);
            tsb_setPlate.Text = "додати форму для вибраних листів";
            tsb_setPlate.Click += tsb_setPlate_Click;
            // 
            // tsb_removeTemplatePlate
            // 
            tsb_removeTemplatePlate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsb_removeTemplatePlate.Image = (System.Drawing.Image)resources.GetObject("tsb_removeTemplatePlate.Image");
            tsb_removeTemplatePlate.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsb_removeTemplatePlate.Name = "tsb_removeTemplatePlate";
            tsb_removeTemplatePlate.Size = new System.Drawing.Size(23, 22);
            tsb_removeTemplatePlate.Text = "видалити форми з листів";
            tsb_removeTemplatePlate.Click += tsb_removeTemplatePlate_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsb_select_all
            // 
            tsb_select_all.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsb_select_all.Image = (System.Drawing.Image)resources.GetObject("tsb_select_all.Image");
            tsb_select_all.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsb_select_all.Name = "tsb_select_all";
            tsb_select_all.Size = new System.Drawing.Size(23, 22);
            tsb_select_all.Text = "вибрати всі листи";
            tsb_select_all.Click += tsb_select_all_Click;
            // 
            // tsb_count
            // 
            tsb_count.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            tsb_count.Image = (System.Drawing.Image)resources.GetObject("tsb_count.Image");
            tsb_count.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsb_count.Name = "tsb_count";
            tsb_count.Size = new System.Drawing.Size(23, 22);
            tsb_count.Text = "додати тираж";
            tsb_count.Click += tsb_count_Click;
            // 
            // PrintSheetsControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(groupBox1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "PrintSheetsControl";
            Size = new System.Drawing.Size(448, 197);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)objectListView1).EndInit();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);

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
        private System.Windows.Forms.ToolStripButton tsb_select_all;
    }
}
