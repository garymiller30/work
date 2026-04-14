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
            this.fastObjectListView1 = new BrightIdeasSoftware.FastObjectListView();
            this.olvColumIdxf = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnRunListPagesf = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnAsignf = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tssl_status = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsb_AddEmptyPage = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_RemovePage = new System.Windows.Forms.ToolStripButton();
            this.tsb_ShowOnlyUnassigned = new System.Windows.Forms.ToolStripButton();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fastObjectListView1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.fastObjectListView1);
            this.groupBox2.Controls.Add(this.statusStrip1);
            this.groupBox2.Controls.Add(this.toolStrip1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(124, 385);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Сторінки спуску";
            // 
            // fastObjectListView1
            // 
            this.fastObjectListView1.AllColumns.Add(this.olvColumIdxf);
            this.fastObjectListView1.AllColumns.Add(this.olvColumnRunListPagesf);
            this.fastObjectListView1.AllColumns.Add(this.olvColumnAsignf);
            this.fastObjectListView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fastObjectListView1.CellEditUseWholeCell = false;
            this.fastObjectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumIdxf,
            this.olvColumnRunListPagesf,
            this.olvColumnAsignf});
            this.fastObjectListView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.fastObjectListView1.FullRowSelect = true;
            this.fastObjectListView1.HideSelection = false;
            this.fastObjectListView1.Location = new System.Drawing.Point(3, 44);
            this.fastObjectListView1.Name = "fastObjectListView1";
            this.fastObjectListView1.ShowGroups = false;
            this.fastObjectListView1.Size = new System.Drawing.Size(118, 313);
            this.fastObjectListView1.TabIndex = 3;
            this.fastObjectListView1.UseCompatibleStateImageBehavior = false;
            this.fastObjectListView1.UseFiltering = true;
            this.fastObjectListView1.View = System.Windows.Forms.View.Details;
            this.fastObjectListView1.VirtualMode = true;
            this.fastObjectListView1.FormatRow += new System.EventHandler<BrightIdeasSoftware.FormatRowEventArgs>(this.fastObjectListView1_FormatRow);
            this.fastObjectListView1.Click += new System.EventHandler(this.fastObjectListView1_Click);
            // 
            // olvColumIdxf
            // 
            this.olvColumIdxf.Text = "№";
            this.olvColumIdxf.Width = 28;
            // 
            // olvColumnRunListPagesf
            // 
            this.olvColumnRunListPagesf.Text = "Сторінки";
            this.olvColumnRunListPagesf.Width = 45;
            // 
            // olvColumnAsignf
            // 
            this.olvColumnAsignf.Text = "";
            this.olvColumnAsignf.Width = 30;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssl_status});
            this.statusStrip1.Location = new System.Drawing.Point(3, 360);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(118, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tssl_status
            // 
            this.tssl_status.Name = "tssl_status";
            this.tssl_status.Size = new System.Drawing.Size(66, 17);
            this.tssl_status.Text = "◌ : 0 | ● : 0, ";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_AddEmptyPage,
            this.toolStripSeparator1,
            this.tsb_RemovePage,
            this.tsb_ShowOnlyUnassigned});
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
            // tsb_ShowOnlyUnassigned
            // 
            this.tsb_ShowOnlyUnassigned.CheckOnClick = true;
            this.tsb_ShowOnlyUnassigned.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsb_ShowOnlyUnassigned.Image = ((System.Drawing.Image)(resources.GetObject("tsb_ShowOnlyUnassigned.Image")));
            this.tsb_ShowOnlyUnassigned.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_ShowOnlyUnassigned.Name = "tsb_ShowOnlyUnassigned";
            this.tsb_ShowOnlyUnassigned.Size = new System.Drawing.Size(23, 22);
            this.tsb_ShowOnlyUnassigned.Text = "◌";
            this.tsb_ShowOnlyUnassigned.ToolTipText = "Показати тільки незадіяні сторінки";
            this.tsb_ShowOnlyUnassigned.CheckedChanged += new System.EventHandler(this.tsb_ShowOnlyUnassigned_CheckedChanged);
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
            ((System.ComponentModel.ISupportInitialize)(this.fastObjectListView1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsb_AddEmptyPage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsb_RemovePage;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tssl_status;
        private BrightIdeasSoftware.FastObjectListView fastObjectListView1;
        private BrightIdeasSoftware.OLVColumn olvColumIdxf;
        private BrightIdeasSoftware.OLVColumn olvColumnRunListPagesf;
        private BrightIdeasSoftware.OLVColumn olvColumnAsignf;
        private System.Windows.Forms.ToolStripButton tsb_ShowOnlyUnassigned;
    }
}
