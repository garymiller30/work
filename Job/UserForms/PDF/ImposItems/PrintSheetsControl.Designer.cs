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
            this.olvColumnTemplateId = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnDesc = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnPlaceType = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsb_delete = new System.Windows.Forms.ToolStripButton();
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
            this.objectListView1.AllColumns.Add(this.olvColumnTemplateId);
            this.objectListView1.AllColumns.Add(this.olvColumnDesc);
            this.objectListView1.AllColumns.Add(this.olvColumnPlaceType);
            this.objectListView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.objectListView1.CellEditUseWholeCell = false;
            this.objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnId,
            this.olvColumnTemplateId,
            this.olvColumnDesc,
            this.olvColumnPlaceType});
            this.objectListView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListView1.FullRowSelect = true;
            this.objectListView1.GridLines = true;
            this.objectListView1.HideSelection = false;
            this.objectListView1.Location = new System.Drawing.Point(6, 44);
            this.objectListView1.MultiSelect = false;
            this.objectListView1.Name = "objectListView1";
            this.objectListView1.ShowGroups = false;
            this.objectListView1.Size = new System.Drawing.Size(372, 121);
            this.objectListView1.TabIndex = 1;
            this.objectListView1.UseCompatibleStateImageBehavior = false;
            this.objectListView1.View = System.Windows.Forms.View.Details;
            // 
            // olvColumnId
            // 
            this.olvColumnId.Text = "Id";
            this.olvColumnId.Width = 30;
            // 
            // olvColumnTemplateId
            // 
            this.olvColumnTemplateId.Text = "TId";
            this.olvColumnTemplateId.Width = 35;
            // 
            // olvColumnDesc
            // 
            this.olvColumnDesc.Text = "Опис";
            this.olvColumnDesc.Width = 124;
            // 
            // olvColumnPlaceType
            // 
            this.olvColumnPlaceType.Text = "Тип друку";
            this.olvColumnPlaceType.Width = 116;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_delete});
            this.toolStrip1.Location = new System.Drawing.Point(3, 16);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(378, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsb_delete
            // 
            this.tsb_delete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_delete.Image = ((System.Drawing.Image)(resources.GetObject("tsb_delete.Image")));
            this.tsb_delete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_delete.Name = "tsb_delete";
            this.tsb_delete.Size = new System.Drawing.Size(23, 22);
            this.tsb_delete.ToolTipText = "Видалити лист";
            this.tsb_delete.Click += new System.EventHandler(this.tsb_delete_Click);
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
        private BrightIdeasSoftware.OLVColumn olvColumnTemplateId;
        private BrightIdeasSoftware.OLVColumn olvColumnDesc;
        private BrightIdeasSoftware.OLVColumn olvColumnPlaceType;
    }
}
