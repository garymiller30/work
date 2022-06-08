namespace CasheViewer.UC
{
    partial class UCCustomerJobReport
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
            this.treeListView1 = new BrightIdeasSoftware.TreeListView();
            this.olvColumn_Customer = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnDate = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn_OrderNumber = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnCategory = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn_Description = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn_Price = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            ((System.ComponentModel.ISupportInitialize)(this.treeListView1)).BeginInit();
            this.SuspendLayout();
            // 
            // treeListView1
            // 
            this.treeListView1.AllColumns.Add(this.olvColumn_Customer);
            this.treeListView1.AllColumns.Add(this.olvColumnDate);
            this.treeListView1.AllColumns.Add(this.olvColumn_OrderNumber);
            this.treeListView1.AllColumns.Add(this.olvColumnCategory);
            this.treeListView1.AllColumns.Add(this.olvColumn_Description);
            this.treeListView1.AllColumns.Add(this.olvColumn_Price);
            this.treeListView1.AllowColumnReorder = true;
            this.treeListView1.CellEditUseWholeCell = false;
            this.treeListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn_Customer,
            this.olvColumnDate,
            this.olvColumn_OrderNumber,
            this.olvColumnCategory,
            this.olvColumn_Description,
            this.olvColumn_Price});
            this.treeListView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.treeListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListView1.FullRowSelect = true;
            this.treeListView1.GridLines = true;
            this.treeListView1.HideSelection = false;
            this.treeListView1.Location = new System.Drawing.Point(0, 0);
            this.treeListView1.Name = "treeListView1";
            this.treeListView1.ShowGroups = false;
            this.treeListView1.Size = new System.Drawing.Size(436, 253);
            this.treeListView1.TabIndex = 1;
            this.treeListView1.UseCompatibleStateImageBehavior = false;
            this.treeListView1.View = System.Windows.Forms.View.Details;
            this.treeListView1.VirtualMode = true;
            this.treeListView1.FormatRow += new System.EventHandler<BrightIdeasSoftware.FormatRowEventArgs>(this.treeListView1_FormatRow);
            this.treeListView1.SelectionChanged += new System.EventHandler(this.treeListView1_SelectionChanged);
            // 
            // olvColumn_Customer
            // 
            this.olvColumn_Customer.Text = "Замовник";
            this.olvColumn_Customer.Width = 100;
            // 
            // olvColumnDate
            // 
            this.olvColumnDate.Text = "Дата";
            // 
            // olvColumn_OrderNumber
            // 
            this.olvColumn_OrderNumber.Text = "№ замовлення";
            this.olvColumn_OrderNumber.Width = 80;
            // 
            // olvColumnCategory
            // 
            this.olvColumnCategory.Text = "Категорія";
            // 
            // olvColumn_Description
            // 
            this.olvColumn_Description.Text = "Опис";
            this.olvColumn_Description.Width = 150;
            // 
            // olvColumn_Price
            // 
            this.olvColumn_Price.Text = "Ціна";
            this.olvColumn_Price.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // UCCustomerJobReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeListView1);
            this.Name = "UCCustomerJobReport";
            this.Size = new System.Drawing.Size(436, 253);
            ((System.ComponentModel.ISupportInitialize)(this.treeListView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.TreeListView treeListView1;
        private BrightIdeasSoftware.OLVColumn olvColumn_Customer;
        private BrightIdeasSoftware.OLVColumn olvColumnDate;
        private BrightIdeasSoftware.OLVColumn olvColumn_OrderNumber;
        private BrightIdeasSoftware.OLVColumn olvColumnCategory;
        private BrightIdeasSoftware.OLVColumn olvColumn_Description;
        private BrightIdeasSoftware.OLVColumn olvColumn_Price;
    }
}
