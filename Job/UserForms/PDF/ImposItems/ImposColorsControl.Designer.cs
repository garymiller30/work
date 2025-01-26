namespace JobSpace.UserForms.PDF.ImposItems
{
    partial class ImposColorsControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImposColorsControl));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsb_addCMYK = new System.Windows.Forms.ToolStripButton();
            this.tsb_addPantone = new System.Windows.Forms.ToolStripButton();
            this.objectListView1 = new BrightIdeasSoftware.ObjectListView();
            this.olvColumn_Name = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn_Front = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn_Back = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_addCMYK,
            this.tsb_addPantone});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(150, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsb_addCMYK
            // 
            this.tsb_addCMYK.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsb_addCMYK.Image = ((System.Drawing.Image)(resources.GetObject("tsb_addCMYK.Image")));
            this.tsb_addCMYK.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_addCMYK.Name = "tsb_addCMYK";
            this.tsb_addCMYK.Size = new System.Drawing.Size(52, 22);
            this.tsb_addCMYK.Text = "+CMYK";
            this.tsb_addCMYK.Click += new System.EventHandler(this.tsb_addCMYK_Click);
            // 
            // tsb_addPantone
            // 
            this.tsb_addPantone.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsb_addPantone.Image = ((System.Drawing.Image)(resources.GetObject("tsb_addPantone.Image")));
            this.tsb_addPantone.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_addPantone.Name = "tsb_addPantone";
            this.tsb_addPantone.Size = new System.Drawing.Size(60, 22);
            this.tsb_addPantone.Text = "+Пантон";
            this.tsb_addPantone.Click += new System.EventHandler(this.tsb_addPantone_Click);
            // 
            // objectListView1
            // 
            this.objectListView1.AllColumns.Add(this.olvColumn_Name);
            this.objectListView1.AllColumns.Add(this.olvColumn_Front);
            this.objectListView1.AllColumns.Add(this.olvColumn_Back);
            this.objectListView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.objectListView1.CellEditUseWholeCell = false;
            this.objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn_Name,
            this.olvColumn_Front,
            this.olvColumn_Back});
            this.objectListView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListView1.FullRowSelect = true;
            this.objectListView1.GridLines = true;
            this.objectListView1.HideSelection = false;
            this.objectListView1.Location = new System.Drawing.Point(3, 28);
            this.objectListView1.Name = "objectListView1";
            this.objectListView1.ShowGroups = false;
            this.objectListView1.Size = new System.Drawing.Size(144, 188);
            this.objectListView1.TabIndex = 1;
            this.objectListView1.UseCompatibleStateImageBehavior = false;
            this.objectListView1.View = System.Windows.Forms.View.Details;
            // 
            // olvColumn_Name
            // 
            this.olvColumn_Name.Text = "Назва";
            // 
            // olvColumn_Front
            // 
            this.olvColumn_Front.CheckBoxes = true;
            this.olvColumn_Front.HeaderCheckBox = true;
            this.olvColumn_Front.Text = "Лице";
            this.olvColumn_Front.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvColumn_Front.Width = 44;
            // 
            // olvColumn_Back
            // 
            this.olvColumn_Back.CheckBoxes = true;
            this.olvColumn_Back.HeaderCheckBox = true;
            this.olvColumn_Back.Text = "Зворот";
            this.olvColumn_Back.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ImposColorsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.objectListView1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ImposColorsControl";
            this.Size = new System.Drawing.Size(150, 219);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsb_addCMYK;
        private BrightIdeasSoftware.ObjectListView objectListView1;
        private System.Windows.Forms.ToolStripButton tsb_addPantone;
        private BrightIdeasSoftware.OLVColumn olvColumn_Name;
        private BrightIdeasSoftware.OLVColumn olvColumn_Front;
        private BrightIdeasSoftware.OLVColumn olvColumn_Back;
    }
}
