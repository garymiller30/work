namespace WeekReport
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WindowOut));
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonPreviousWeek = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_CurrentWeek = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_NextWeek = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabelCurWeek = new System.Windows.Forms.ToolStripLabel();
            this.treeListView_Week = new BrightIdeasSoftware.TreeListView();
            this.olvColumn_Name = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnMo = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnDi = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnMit = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnDon = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnFr = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnSam = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnSonn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnCompare = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.contextMenuStrip_Woche = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.развернутьВсёToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.свернутьВсёToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.barRenderer1 = new BrightIdeasSoftware.BarRenderer();
            this.toolStrip3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeListView_Week)).BeginInit();
            this.contextMenuStrip_Woche.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip3
            // 
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonPreviousWeek,
            this.toolStripButton_CurrentWeek,
            this.toolStripButton_NextWeek,
            this.toolStripLabelCurWeek});
            this.toolStrip3.Location = new System.Drawing.Point(0, 0);
            this.toolStrip3.Name = "toolStrip3";
            this.toolStrip3.Size = new System.Drawing.Size(415, 25);
            this.toolStrip3.TabIndex = 2;
            this.toolStrip3.Text = "toolStrip3";
            // 
            // toolStripButtonPreviousWeek
            // 
            this.toolStripButtonPreviousWeek.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPreviousWeek.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPreviousWeek.Image")));
            this.toolStripButtonPreviousWeek.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPreviousWeek.Name = "toolStripButtonPreviousWeek";
            this.toolStripButtonPreviousWeek.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonPreviousWeek.Text = "prev";
            this.toolStripButtonPreviousWeek.Click += new System.EventHandler(this.ToolStripButtonPreviousWeek_Click);
            // 
            // toolStripButton_CurrentWeek
            // 
            this.toolStripButton_CurrentWeek.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_CurrentWeek.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_CurrentWeek.Image")));
            this.toolStripButton_CurrentWeek.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_CurrentWeek.Name = "toolStripButton_CurrentWeek";
            this.toolStripButton_CurrentWeek.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_CurrentWeek.Text = "current week";
            this.toolStripButton_CurrentWeek.Click += new System.EventHandler(this.ToolStripButton_CurrentWeek_Click);
            // 
            // toolStripButton_NextWeek
            // 
            this.toolStripButton_NextWeek.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_NextWeek.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_NextWeek.Image")));
            this.toolStripButton_NextWeek.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_NextWeek.Name = "toolStripButton_NextWeek";
            this.toolStripButton_NextWeek.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_NextWeek.Text = "next week";
            this.toolStripButton_NextWeek.Click += new System.EventHandler(this.ToolStripButton_NextWeek_Click);
            // 
            // toolStripLabelCurWeek
            // 
            this.toolStripLabelCurWeek.Name = "toolStripLabelCurWeek";
            this.toolStripLabelCurWeek.Size = new System.Drawing.Size(24, 22);
            this.toolStripLabelCurWeek.Text = "0-0";
            // 
            // treeListView_Week
            // 
            this.treeListView_Week.AllColumns.Add(this.olvColumn_Name);
            this.treeListView_Week.AllColumns.Add(this.olvColumnMo);
            this.treeListView_Week.AllColumns.Add(this.olvColumnDi);
            this.treeListView_Week.AllColumns.Add(this.olvColumnMit);
            this.treeListView_Week.AllColumns.Add(this.olvColumnDon);
            this.treeListView_Week.AllColumns.Add(this.olvColumnFr);
            this.treeListView_Week.AllColumns.Add(this.olvColumnSam);
            this.treeListView_Week.AllColumns.Add(this.olvColumnSonn);
            this.treeListView_Week.AllColumns.Add(this.olvColumnCompare);
            this.treeListView_Week.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeListView_Week.CellEditUseWholeCell = false;
            this.treeListView_Week.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn_Name,
            this.olvColumnMo,
            this.olvColumnDi,
            this.olvColumnMit,
            this.olvColumnDon,
            this.olvColumnFr,
            this.olvColumnSam,
            this.olvColumnSonn,
            this.olvColumnCompare});
            this.treeListView_Week.ContextMenuStrip = this.contextMenuStrip_Woche;
            this.treeListView_Week.Cursor = System.Windows.Forms.Cursors.Default;
            this.treeListView_Week.FullRowSelect = true;
            this.treeListView_Week.GridLines = true;
            this.treeListView_Week.Location = new System.Drawing.Point(3, 28);
            this.treeListView_Week.Name = "treeListView_Week";
            this.treeListView_Week.ShowGroups = false;
            this.treeListView_Week.ShowImagesOnSubItems = true;
            this.treeListView_Week.Size = new System.Drawing.Size(409, 273);
            this.treeListView_Week.TabIndex = 3;
            this.treeListView_Week.UseCompatibleStateImageBehavior = false;
            this.treeListView_Week.View = System.Windows.Forms.View.Details;
            this.treeListView_Week.VirtualMode = true;
            // 
            // olvColumn_Name
            // 
            this.olvColumn_Name.AspectName = "Name";
            this.olvColumn_Name.Text = "Customer";
            this.olvColumn_Name.Width = 120;
            // 
            // olvColumnMo
            // 
            this.olvColumnMo.Text = "Mo";
            this.olvColumnMo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvColumnMo.Width = 45;
            // 
            // olvColumnDi
            // 
            this.olvColumnDi.Text = "Tu";
            this.olvColumnDi.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvColumnDi.Width = 45;
            // 
            // olvColumnMit
            // 
            this.olvColumnMit.Text = "We";
            this.olvColumnMit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvColumnMit.Width = 45;
            // 
            // olvColumnDon
            // 
            this.olvColumnDon.Text = "Thu";
            this.olvColumnDon.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvColumnDon.Width = 45;
            // 
            // olvColumnFr
            // 
            this.olvColumnFr.Text = "Fri";
            this.olvColumnFr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvColumnFr.Width = 45;
            // 
            // olvColumnSam
            // 
            this.olvColumnSam.Text = "Sat";
            this.olvColumnSam.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvColumnSam.Width = 45;
            // 
            // olvColumnSonn
            // 
            this.olvColumnSonn.Text = "Sun";
            this.olvColumnSonn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvColumnSonn.Width = 45;
            // 
            // olvColumnCompare
            // 
            this.olvColumnCompare.IsEditable = false;
            this.olvColumnCompare.Renderer = this.barRenderer1;
            this.olvColumnCompare.Text = "";
            // 
            // contextMenuStrip_Woche
            // 
            this.contextMenuStrip_Woche.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.развернутьВсёToolStripMenuItem,
            this.свернутьВсёToolStripMenuItem});
            this.contextMenuStrip_Woche.Name = "contextMenuStrip_Woche";
            this.contextMenuStrip_Woche.Size = new System.Drawing.Size(157, 48);
            // 
            // развернутьВсёToolStripMenuItem
            // 
            this.развернутьВсёToolStripMenuItem.Name = "развернутьВсёToolStripMenuItem";
            this.развернутьВсёToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.развернутьВсёToolStripMenuItem.Text = "развернуть всё";
            this.развернутьВсёToolStripMenuItem.Click += new System.EventHandler(this.РазвернутьВсёToolStripMenuItem_Click);
            // 
            // свернутьВсёToolStripMenuItem
            // 
            this.свернутьВсёToolStripMenuItem.Name = "свернутьВсёToolStripMenuItem";
            this.свернутьВсёToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.свернутьВсёToolStripMenuItem.Text = "свернуть всё";
            this.свернутьВсёToolStripMenuItem.Click += new System.EventHandler(this.СвернутьВсёToolStripMenuItem_Click);
            // 
            // barRenderer1
            // 
            this.barRenderer1.UseStandardBar = false;
            // 
            // WindowOut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeListView_Week);
            this.Controls.Add(this.toolStrip3);
            this.Name = "WindowOut";
            this.Size = new System.Drawing.Size(415, 304);
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeListView_Week)).EndInit();
            this.contextMenuStrip_Woche.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip3;
        private System.Windows.Forms.ToolStripButton toolStripButtonPreviousWeek;
        private System.Windows.Forms.ToolStripButton toolStripButton_CurrentWeek;
        private System.Windows.Forms.ToolStripButton toolStripButton_NextWeek;
        private System.Windows.Forms.ToolStripLabel toolStripLabelCurWeek;
        private BrightIdeasSoftware.TreeListView treeListView_Week;
        private BrightIdeasSoftware.OLVColumn olvColumn_Name;
        private BrightIdeasSoftware.OLVColumn olvColumnMo;
        private BrightIdeasSoftware.OLVColumn olvColumnDi;
        private BrightIdeasSoftware.OLVColumn olvColumnMit;
        private BrightIdeasSoftware.OLVColumn olvColumnDon;
        private BrightIdeasSoftware.OLVColumn olvColumnFr;
        private BrightIdeasSoftware.OLVColumn olvColumnSam;
        private BrightIdeasSoftware.OLVColumn olvColumnSonn;
        private BrightIdeasSoftware.OLVColumn olvColumnCompare;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_Woche;
        private System.Windows.Forms.ToolStripMenuItem развернутьВсёToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem свернутьВсёToolStripMenuItem;
        private BrightIdeasSoftware.BarRenderer barRenderer1;
    }
}
