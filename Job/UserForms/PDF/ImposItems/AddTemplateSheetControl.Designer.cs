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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddTemplateSheetControl));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.objectListView1 = new BrightIdeasSoftware.ObjectListView();
            this.olvColumnDesc = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnPrintType = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnFormat = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.cms_SheetSideType = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tscb_sheetTemplates = new System.Windows.Forms.ToolStripComboBox();
            this.tscb_sheetType = new System.Windows.Forms.ToolStripComboBox();
            this.tsb_addToList = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_edit = new System.Windows.Forms.ToolStripButton();
            this.tsb_deleteTemplateSheet = new System.Windows.Forms.ToolStripButton();
            this.tsb_add = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.tsb_toPrint = new System.Windows.Forms.ToolStripButton();
            this.tsb_fillAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tbs_dublicate = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_delete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_saveTemplate = new System.Windows.Forms.ToolStripButton();
            this.tsb_save_all = new System.Windows.Forms.ToolStripButton();
            this.tsb_loadTemplate = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_addToQuickAccess = new System.Windows.Forms.ToolStripButton();
            this.tsb_QuickAccess = new System.Windows.Forms.ToolStripSplitButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
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
            this.groupBox1.Size = new System.Drawing.Size(329, 105);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Шаблони листів для друку";
            // 
            // objectListView1
            // 
            this.objectListView1.AllColumns.Add(this.olvColumnDesc);
            this.objectListView1.AllColumns.Add(this.olvColumnPrintType);
            this.objectListView1.AllColumns.Add(this.olvColumnFormat);
            this.objectListView1.CellEditUseWholeCell = false;
            this.objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnDesc,
            this.olvColumnPrintType,
            this.olvColumnFormat});
            this.objectListView1.ContextMenuStrip = this.cms_SheetSideType;
            this.objectListView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectListView1.FullRowSelect = true;
            this.objectListView1.GridLines = true;
            this.objectListView1.HideSelection = false;
            this.objectListView1.Location = new System.Drawing.Point(3, 16);
            this.objectListView1.Name = "objectListView1";
            this.objectListView1.ShowGroups = false;
            this.objectListView1.Size = new System.Drawing.Size(323, 86);
            this.objectListView1.TabIndex = 0;
            this.objectListView1.UseCompatibleStateImageBehavior = false;
            this.objectListView1.View = System.Windows.Forms.View.Details;
            this.objectListView1.DoubleClick += new System.EventHandler(this.objectListView1_DoubleClick);
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
            // olvColumnFormat
            // 
            this.olvColumnFormat.Text = "Формат";
            // 
            // cms_SheetSideType
            // 
            this.cms_SheetSideType.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cms_SheetSideType.Name = "cms_SheetSideType";
            this.cms_SheetSideType.Size = new System.Drawing.Size(61, 4);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tscb_sheetTemplates,
            this.tscb_sheetType,
            this.tsb_addToList,
            this.toolStripSeparator4,
            this.tsb_edit,
            this.tsb_deleteTemplateSheet,
            this.tsb_add,
            this.toolStripSeparator3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(335, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tscb_sheetTemplates
            // 
            this.tscb_sheetTemplates.DropDownHeight = 200;
            this.tscb_sheetTemplates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tscb_sheetTemplates.DropDownWidth = 150;
            this.tscb_sheetTemplates.IntegralHeight = false;
            this.tscb_sheetTemplates.Name = "tscb_sheetTemplates";
            this.tscb_sheetTemplates.Size = new System.Drawing.Size(90, 25);
            // 
            // tscb_sheetType
            // 
            this.tscb_sheetType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tscb_sheetType.Name = "tscb_sheetType";
            this.tscb_sheetType.Size = new System.Drawing.Size(90, 25);
            // 
            // tsb_addToList
            // 
            this.tsb_addToList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_addToList.Image = ((System.Drawing.Image)(resources.GetObject("tsb_addToList.Image")));
            this.tsb_addToList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_addToList.Name = "tsb_addToList";
            this.tsb_addToList.Size = new System.Drawing.Size(23, 22);
            this.tsb_addToList.Text = "додати до списку";
            this.tsb_addToList.ToolTipText = "додати до списку (+ Alt - додати чистий лист)";
            this.tsb_addToList.Click += new System.EventHandler(this.tsb_addToList_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // tsb_edit
            // 
            this.tsb_edit.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsb_edit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_edit.Image = ((System.Drawing.Image)(resources.GetObject("tsb_edit.Image")));
            this.tsb_edit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_edit.Name = "tsb_edit";
            this.tsb_edit.Size = new System.Drawing.Size(23, 22);
            this.tsb_edit.Text = "редагувати шаблон листа";
            this.tsb_edit.Click += new System.EventHandler(this.tsb_edit_Click);
            // 
            // tsb_deleteTemplateSheet
            // 
            this.tsb_deleteTemplateSheet.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsb_deleteTemplateSheet.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_deleteTemplateSheet.Image = ((System.Drawing.Image)(resources.GetObject("tsb_deleteTemplateSheet.Image")));
            this.tsb_deleteTemplateSheet.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_deleteTemplateSheet.Name = "tsb_deleteTemplateSheet";
            this.tsb_deleteTemplateSheet.Size = new System.Drawing.Size(23, 22);
            this.tsb_deleteTemplateSheet.Text = "Видалили зі списку форматів";
            this.tsb_deleteTemplateSheet.Click += new System.EventHandler(this.tsb_deleteTemplateSheet_Click);
            // 
            // tsb_add
            // 
            this.tsb_add.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsb_add.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_add.Image = ((System.Drawing.Image)(resources.GetObject("tsb_add.Image")));
            this.tsb_add.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_add.Name = "tsb_add";
            this.tsb_add.Size = new System.Drawing.Size(23, 22);
            this.tsb_add.Text = "додати шаблон листа";
            this.tsb_add.Click += new System.EventHandler(this.tsb_add_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_toPrint,
            this.tsb_fillAll,
            this.toolStripSeparator6,
            this.tbs_dublicate,
            this.toolStripSeparator1,
            this.tsb_delete,
            this.toolStripSeparator2,
            this.tsb_saveTemplate,
            this.tsb_save_all,
            this.tsb_loadTemplate,
            this.toolStripSeparator5,
            this.tsb_addToQuickAccess,
            this.tsb_QuickAccess});
            this.toolStrip2.Location = new System.Drawing.Point(0, 137);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(335, 25);
            this.toolStrip2.TabIndex = 2;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // tsb_toPrint
            // 
            this.tsb_toPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_toPrint.Image = ((System.Drawing.Image)(resources.GetObject("tsb_toPrint.Image")));
            this.tsb_toPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_toPrint.Name = "tsb_toPrint";
            this.tsb_toPrint.Size = new System.Drawing.Size(23, 22);
            this.tsb_toPrint.Text = "копіювати до друкарських листів";
            this.tsb_toPrint.ToolTipText = "копіювати до друкарських листів\r\n(+Alt - не переназначати автоматично нумерацію с" +
    "торінок)";
            this.tsb_toPrint.Click += new System.EventHandler(this.tsb_toPrint_Click);
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
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
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
            // tsb_saveTemplate
            // 
            this.tsb_saveTemplate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_saveTemplate.Image = ((System.Drawing.Image)(resources.GetObject("tsb_saveTemplate.Image")));
            this.tsb_saveTemplate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_saveTemplate.Name = "tsb_saveTemplate";
            this.tsb_saveTemplate.Size = new System.Drawing.Size(23, 22);
            this.tsb_saveTemplate.Text = "зберегти шаблон";
            this.tsb_saveTemplate.Click += new System.EventHandler(this.tsb_saveTemplate_Click_1);
            // 
            // tsb_save_all
            // 
            this.tsb_save_all.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_save_all.Image = ((System.Drawing.Image)(resources.GetObject("tsb_save_all.Image")));
            this.tsb_save_all.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_save_all.Name = "tsb_save_all";
            this.tsb_save_all.Size = new System.Drawing.Size(23, 22);
            this.tsb_save_all.Text = "зберегти список";
            this.tsb_save_all.ToolTipText = "зберегти увесь список";
            this.tsb_save_all.Click += new System.EventHandler(this.tsb_save_all_Click);
            // 
            // tsb_loadTemplate
            // 
            this.tsb_loadTemplate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_loadTemplate.Image = ((System.Drawing.Image)(resources.GetObject("tsb_loadTemplate.Image")));
            this.tsb_loadTemplate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_loadTemplate.Name = "tsb_loadTemplate";
            this.tsb_loadTemplate.Size = new System.Drawing.Size(23, 22);
            this.tsb_loadTemplate.Text = "завантажити шаблон";
            this.tsb_loadTemplate.Click += new System.EventHandler(this.tsb_loadTemplate_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // tsb_addToQuickAccess
            // 
            this.tsb_addToQuickAccess.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_addToQuickAccess.Image = ((System.Drawing.Image)(resources.GetObject("tsb_addToQuickAccess.Image")));
            this.tsb_addToQuickAccess.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_addToQuickAccess.Name = "tsb_addToQuickAccess";
            this.tsb_addToQuickAccess.Size = new System.Drawing.Size(23, 22);
            this.tsb_addToQuickAccess.Text = "Додати до швидкого доступу";
            this.tsb_addToQuickAccess.Click += new System.EventHandler(this.tsb_addToQuickAccess_Click);
            // 
            // tsb_QuickAccess
            // 
            this.tsb_QuickAccess.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_QuickAccess.Image = ((System.Drawing.Image)(resources.GetObject("tsb_QuickAccess.Image")));
            this.tsb_QuickAccess.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_QuickAccess.Name = "tsb_QuickAccess";
            this.tsb_QuickAccess.Size = new System.Drawing.Size(32, 22);
            this.tsb_QuickAccess.Text = "Меню швидкого доступу";
            this.tsb_QuickAccess.ButtonClick += new System.EventHandler(this.tsb_QuickAccess_ButtonClick);
            // 
            // AddTemplateSheetControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(335, 0);
            this.Name = "AddTemplateSheetControl";
            this.Size = new System.Drawing.Size(335, 162);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private BrightIdeasSoftware.ObjectListView objectListView1;
        private BrightIdeasSoftware.OLVColumn olvColumnDesc;
        private BrightIdeasSoftware.OLVColumn olvColumnPrintType;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsb_add;
        private System.Windows.Forms.ToolStripButton tsb_edit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripComboBox tscb_sheetTemplates;
        private System.Windows.Forms.ToolStripButton tsb_addToList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripComboBox tscb_sheetType;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton tbs_dublicate;
        private System.Windows.Forms.ToolStripButton tsb_toPrint;
        private System.Windows.Forms.ToolStripButton tsb_fillAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsb_delete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsb_saveTemplate;
        private System.Windows.Forms.ToolStripButton tsb_loadTemplate;
        private System.Windows.Forms.ToolStripButton tsb_deleteTemplateSheet;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton tsb_addToQuickAccess;
        private System.Windows.Forms.ToolStripSplitButton tsb_QuickAccess;
        private System.Windows.Forms.ContextMenuStrip cms_SheetSideType;
        private BrightIdeasSoftware.OLVColumn olvColumnFormat;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton tsb_save_all;
    }
}
