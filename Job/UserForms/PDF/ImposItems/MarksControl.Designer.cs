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
            this.tlv_MarksResources = new BrightIdeasSoftware.TreeListView();
            this.ovl_marks = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tlv_ProductMarks = new BrightIdeasSoftware.TreeListView();
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            ((System.ComponentModel.ISupportInitialize)(this.tlv_MarksResources)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tlv_ProductMarks)).BeginInit();
            this.SuspendLayout();
            // 
            // tlv_MarksResources
            // 
            this.tlv_MarksResources.AllColumns.Add(this.ovl_marks);
            this.tlv_MarksResources.CellEditUseWholeCell = false;
            this.tlv_MarksResources.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ovl_marks});
            this.tlv_MarksResources.Cursor = System.Windows.Forms.Cursors.Default;
            this.tlv_MarksResources.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlv_MarksResources.HideSelection = false;
            this.tlv_MarksResources.Location = new System.Drawing.Point(3, 16);
            this.tlv_MarksResources.Name = "tlv_MarksResources";
            this.tlv_MarksResources.ShowGroups = false;
            this.tlv_MarksResources.Size = new System.Drawing.Size(219, 283);
            this.tlv_MarksResources.TabIndex = 0;
            this.tlv_MarksResources.UseCompatibleStateImageBehavior = false;
            this.tlv_MarksResources.View = System.Windows.Forms.View.Details;
            this.tlv_MarksResources.VirtualMode = true;
            // 
            // ovl_marks
            // 
            this.ovl_marks.Text = "мітки";
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
            this.groupBox1.Controls.Add(this.tlv_MarksResources);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(225, 302);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Наявні мітки";
            // 
            // groupBox2
            // 
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
            this.tlv_ProductMarks.AllColumns.Add(this.olvColumn1);
            this.tlv_ProductMarks.CellEditUseWholeCell = false;
            this.tlv_ProductMarks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn1});
            this.tlv_ProductMarks.Cursor = System.Windows.Forms.Cursors.Default;
            this.tlv_ProductMarks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlv_ProductMarks.HideSelection = false;
            this.tlv_ProductMarks.Location = new System.Drawing.Point(3, 16);
            this.tlv_ProductMarks.Name = "tlv_ProductMarks";
            this.tlv_ProductMarks.ShowGroups = false;
            this.tlv_ProductMarks.Size = new System.Drawing.Size(219, 280);
            this.tlv_ProductMarks.TabIndex = 1;
            this.tlv_ProductMarks.UseCompatibleStateImageBehavior = false;
            this.tlv_ProductMarks.View = System.Windows.Forms.View.Details;
            this.tlv_ProductMarks.VirtualMode = true;
            // 
            // olvColumn1
            // 
            this.olvColumn1.Text = "мітки";
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
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tlv_ProductMarks)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.TreeListView tlv_MarksResources;
        private BrightIdeasSoftware.OLVColumn ovl_marks;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private BrightIdeasSoftware.TreeListView tlv_ProductMarks;
        private BrightIdeasSoftware.OLVColumn olvColumn1;
    }
}
