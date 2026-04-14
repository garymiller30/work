namespace JobSpace.UserForms.PDF
{
    partial class FormSpotColorRenamer
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.objectListView1 = new BrightIdeasSoftware.ObjectListView();
            this.olvColumn_source_color = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn_target_color = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.btn_rename = new Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).BeginInit();
            this.SuspendLayout();
            // 
            // objectListView1
            // 
            this.objectListView1.AllColumns.Add(this.olvColumn_source_color);
            this.objectListView1.AllColumns.Add(this.olvColumn_target_color);
            this.objectListView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.objectListView1.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            this.objectListView1.CellEditUseWholeCell = false;
            this.objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn_source_color,
            this.olvColumn_target_color});
            this.objectListView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListView1.FullRowSelect = true;
            this.objectListView1.GridLines = true;
            this.objectListView1.HideSelection = false;
            this.objectListView1.Location = new System.Drawing.Point(12, 12);
            this.objectListView1.Name = "objectListView1";
            this.objectListView1.ShowGroups = false;
            this.objectListView1.Size = new System.Drawing.Size(334, 225);
            this.objectListView1.TabIndex = 4;
            this.objectListView1.UseCompatibleStateImageBehavior = false;
            this.objectListView1.View = System.Windows.Forms.View.Details;
            // 
            // olvColumn_source_color
            // 
            this.olvColumn_source_color.AspectName = "Source";
            this.olvColumn_source_color.IsEditable = false;
            this.olvColumn_source_color.Text = "оригінальний колір";
            this.olvColumn_source_color.Width = 166;
            // 
            // olvColumn_target_color
            // 
            this.olvColumn_target_color.AspectName = "Target";
            this.olvColumn_target_color.CellEditUseWholeCell = true;
            this.olvColumn_target_color.Text = "замінити на";
            this.olvColumn_target_color.Width = 169;
            // 
            // btn_rename
            // 
            this.btn_rename.Location = new System.Drawing.Point(119, 250);
            this.btn_rename.Name = "btn_rename";
            this.btn_rename.Size = new System.Drawing.Size(117, 39);
            this.btn_rename.TabIndex = 5;
            this.btn_rename.Values.DropDownArrowColor = System.Drawing.Color.Empty;
            this.btn_rename.Values.Text = "Перейменувати";
            this.btn_rename.Click += new System.EventHandler(this.btn_rename_Click);
            // 
            // FormSpotColorRenamer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 301);
            this.Controls.Add(this.btn_rename);
            this.Controls.Add(this.objectListView1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSpotColorRenamer";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Перейменувати колір";
            this.Shown += new System.EventHandler(this.FormSpotColorRenamer_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private BrightIdeasSoftware.ObjectListView objectListView1;
        private BrightIdeasSoftware.OLVColumn olvColumn_source_color;
        private BrightIdeasSoftware.OLVColumn olvColumn_target_color;
        private Krypton.Toolkit.KryptonButton btn_rename;
    }
}