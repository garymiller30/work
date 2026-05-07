namespace JobSpace.UC
{
    partial class UcAddWorkPluginsContainer
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
            components = new System.ComponentModel.Container();
            treeListView1 = new BrightIdeasSoftware.TreeListView();
            olvColumnName = new BrightIdeasSoftware.OLVColumn();
            olvColumnPrice = new BrightIdeasSoftware.OLVColumn();
            olvColumnPay = new BrightIdeasSoftware.OLVColumn();
            ((System.ComponentModel.ISupportInitialize)treeListView1).BeginInit();
            SuspendLayout();
            // 
            // treeListView1
            // 
            treeListView1.AllColumns.Add(olvColumnName);
            treeListView1.AllColumns.Add(olvColumnPrice);
            treeListView1.AllColumns.Add(olvColumnPay);
            treeListView1.CellEditUseWholeCell = false;
            treeListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { olvColumnName, olvColumnPrice, olvColumnPay });
            treeListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            treeListView1.FullRowSelect = true;
            treeListView1.Location = new System.Drawing.Point(0, 0);
            treeListView1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            treeListView1.Name = "treeListView1";
            treeListView1.ShowGroups = false;
            treeListView1.Size = new System.Drawing.Size(441, 239);
            treeListView1.TabIndex = 1;
            treeListView1.UseCompatibleStateImageBehavior = false;
            treeListView1.View = System.Windows.Forms.View.Details;
            treeListView1.VirtualMode = true;
            treeListView1.MouseClick += treeListView1_MouseClick;
            // 
            // olvColumnName
            // 
            olvColumnName.AspectName = "ToString";
            olvColumnName.IsTileViewColumn = true;
            olvColumnName.Text = "Назва";
            olvColumnName.Width = 200;
            // 
            // olvColumnPrice
            // 
            olvColumnPrice.AspectName = "Price";
            olvColumnPrice.Text = "Ціна";
            olvColumnPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // olvColumnPay
            // 
            olvColumnPay.AspectName = "Pay";
            olvColumnPay.Text = "Сплачено";
            olvColumnPay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // UcAddWorkPluginsContainer
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(treeListView1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "UcAddWorkPluginsContainer";
            Size = new System.Drawing.Size(441, 239);
            ((System.ComponentModel.ISupportInitialize)treeListView1).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.TreeListView treeListView1;
        private BrightIdeasSoftware.OLVColumn olvColumnName;
        private BrightIdeasSoftware.OLVColumn olvColumnPrice;
        private BrightIdeasSoftware.OLVColumn olvColumnPay;
    }
}
