namespace PluginAddWorkFromPolymix
{
    partial class FormAddOrder
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
            this.objectListViewFilter = new BrightIdeasSoftware.ObjectListView();
            this.olvColumnName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.kryptonButtonOk = new Krypton.Toolkit.KryptonButton();
            this.kryptonGroupBox1 = new Krypton.Toolkit.KryptonGroupBox();
            this.kryptonGroupBox2 = new Krypton.Toolkit.KryptonGroupBox();
            this.objectListViewOrderList = new BrightIdeasSoftware.ObjectListView();
            this.olvColumnNumber = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnCustomer = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnDescription = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            ((System.ComponentModel.ISupportInitialize)(this.objectListViewFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).BeginInit();
            this.kryptonGroupBox1.Panel.SuspendLayout();
            this.kryptonGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2.Panel)).BeginInit();
            this.kryptonGroupBox2.Panel.SuspendLayout();
            this.kryptonGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectListViewOrderList)).BeginInit();
            this.SuspendLayout();
            // 
            // objectListViewFilter
            // 
            this.objectListViewFilter.AllColumns.Add(this.olvColumnName);
            this.objectListViewFilter.CellEditUseWholeCell = false;
            this.objectListViewFilter.CheckBoxes = true;
            this.objectListViewFilter.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnName});
            this.objectListViewFilter.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListViewFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectListViewFilter.HideSelection = false;
            this.objectListViewFilter.Location = new System.Drawing.Point(0, 0);
            this.objectListViewFilter.Name = "objectListViewFilter";
            this.objectListViewFilter.Size = new System.Drawing.Size(214, 350);
            this.objectListViewFilter.TabIndex = 0;
            this.objectListViewFilter.UseCompatibleStateImageBehavior = false;
            this.objectListViewFilter.View = System.Windows.Forms.View.Details;
            // 
            // olvColumnName
            // 
            this.olvColumnName.AspectName = "Name";
            this.olvColumnName.AspectToStringFormat = "";
            this.olvColumnName.IsEditable = false;
            this.olvColumnName.Text = "Назва";
            this.olvColumnName.Width = 194;
            // 
            // kryptonButtonOk
            // 
            this.kryptonButtonOk.CornerRoundingRadius = -1F;
            this.kryptonButtonOk.Location = new System.Drawing.Point(429, 340);
            this.kryptonButtonOk.Name = "kryptonButtonOk";
            this.kryptonButtonOk.Size = new System.Drawing.Size(170, 46);
            this.kryptonButtonOk.TabIndex = 12;
            this.kryptonButtonOk.Values.Text = "Створити замовлення";
            this.kryptonButtonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // kryptonGroupBox1
            // 
            this.kryptonGroupBox1.Location = new System.Drawing.Point(12, 12);
            this.kryptonGroupBox1.Name = "kryptonGroupBox1";
            // 
            // kryptonGroupBox1.Panel
            // 
            this.kryptonGroupBox1.Panel.Controls.Add(this.objectListViewFilter);
            this.kryptonGroupBox1.Size = new System.Drawing.Size(218, 374);
            this.kryptonGroupBox1.TabIndex = 13;
            this.kryptonGroupBox1.Values.Heading = "Фільтр";
            // 
            // kryptonGroupBox2
            // 
            this.kryptonGroupBox2.Location = new System.Drawing.Point(245, 12);
            this.kryptonGroupBox2.Name = "kryptonGroupBox2";
            // 
            // kryptonGroupBox2.Panel
            // 
            this.kryptonGroupBox2.Panel.Controls.Add(this.objectListViewOrderList);
            this.kryptonGroupBox2.Size = new System.Drawing.Size(543, 310);
            this.kryptonGroupBox2.TabIndex = 14;
            this.kryptonGroupBox2.Values.Heading = "Роботи з Polymix";
            // 
            // objectListViewOrderList
            // 
            this.objectListViewOrderList.AllColumns.Add(this.olvColumnNumber);
            this.objectListViewOrderList.AllColumns.Add(this.olvColumnCustomer);
            this.objectListViewOrderList.AllColumns.Add(this.olvColumnDescription);
            this.objectListViewOrderList.CellEditUseWholeCell = false;
            this.objectListViewOrderList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnNumber,
            this.olvColumnCustomer,
            this.olvColumnDescription});
            this.objectListViewOrderList.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListViewOrderList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectListViewOrderList.FullRowSelect = true;
            this.objectListViewOrderList.HideSelection = false;
            this.objectListViewOrderList.Location = new System.Drawing.Point(0, 0);
            this.objectListViewOrderList.Name = "objectListViewOrderList";
            this.objectListViewOrderList.ShowGroups = false;
            this.objectListViewOrderList.Size = new System.Drawing.Size(539, 286);
            this.objectListViewOrderList.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.objectListViewOrderList.TabIndex = 1;
            this.objectListViewOrderList.UseCompatibleStateImageBehavior = false;
            this.objectListViewOrderList.View = System.Windows.Forms.View.Details;
            // 
            // olvColumnNumber
            // 
            this.olvColumnNumber.AspectName = "Number";
            this.olvColumnNumber.AspectToStringFormat = "{0:D5}";
            this.olvColumnNumber.Text = "#";
            // 
            // olvColumnCustomer
            // 
            this.olvColumnCustomer.AspectName = "Customer";
            this.olvColumnCustomer.Text = "Замовник";
            this.olvColumnCustomer.Width = 150;
            // 
            // olvColumnDescription
            // 
            this.olvColumnDescription.AspectName = "Description";
            this.olvColumnDescription.Text = "Опис";
            this.olvColumnDescription.Width = 300;
            // 
            // FormAddOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 398);
            this.Controls.Add(this.kryptonGroupBox2);
            this.Controls.Add(this.kryptonGroupBox1);
            this.Controls.Add(this.kryptonButtonOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAddOrder";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Створити замовлення з Polymix";
            ((System.ComponentModel.ISupportInitialize)(this.objectListViewFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).EndInit();
            this.kryptonGroupBox1.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).EndInit();
            this.kryptonGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2.Panel)).EndInit();
            this.kryptonGroupBox2.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2)).EndInit();
            this.kryptonGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.objectListViewOrderList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private BrightIdeasSoftware.ObjectListView objectListViewFilter;
        private BrightIdeasSoftware.OLVColumn olvColumnName;
        private Krypton.Toolkit.KryptonButton kryptonButtonOk;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBox1;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBox2;
        private BrightIdeasSoftware.ObjectListView objectListViewOrderList;
        private BrightIdeasSoftware.OLVColumn olvColumnNumber;
        private BrightIdeasSoftware.OLVColumn olvColumnCustomer;
        private BrightIdeasSoftware.OLVColumn olvColumnDescription;
    }
}