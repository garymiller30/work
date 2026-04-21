namespace JobSpace.UserForms
{
    partial class FormAddWorkMany
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
            this.cb_customers = new Krypton.Toolkit.KryptonComboBox();
            this.kryptonGroupBox1 = new Krypton.Toolkit.KryptonGroupBox();
            this.objectListView1 = new BrightIdeasSoftware.ObjectListView();
            this.olvColumn_number = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn_description = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.btn_create = new Krypton.Toolkit.KryptonButton();
            this.btn_add_order = new Krypton.Toolkit.KryptonButton();
            this.btn_paste = new Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)(this.cb_customers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).BeginInit();
            this.kryptonGroupBox1.Panel.SuspendLayout();
            this.kryptonGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).BeginInit();
            this.SuspendLayout();
            // 
            // cb_customers
            // 
            this.cb_customers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_customers.DropDownWidth = 75;
            this.cb_customers.Location = new System.Drawing.Point(3, 3);
            this.cb_customers.Name = "cb_customers";
            this.cb_customers.Size = new System.Drawing.Size(276, 22);
            this.cb_customers.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.cb_customers.TabIndex = 0;
            // 
            // kryptonGroupBox1
            // 
            this.kryptonGroupBox1.Location = new System.Drawing.Point(12, 12);
            // 
            // kryptonGroupBox1.Panel
            // 
            this.kryptonGroupBox1.Panel.Controls.Add(this.cb_customers);
            this.kryptonGroupBox1.Size = new System.Drawing.Size(286, 58);
            this.kryptonGroupBox1.TabIndex = 2;
            this.kryptonGroupBox1.Values.Heading = "Замовник";
            // 
            // objectListView1
            // 
            this.objectListView1.AllColumns.Add(this.olvColumn_number);
            this.objectListView1.AllColumns.Add(this.olvColumn_description);
            this.objectListView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.objectListView1.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClickAlways;
            this.objectListView1.CellEditUseWholeCell = false;
            this.objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn_number,
            this.olvColumn_description});
            this.objectListView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListView1.FullRowSelect = true;
            this.objectListView1.GridLines = true;
            this.objectListView1.HideSelection = false;
            this.objectListView1.Location = new System.Drawing.Point(12, 76);
            this.objectListView1.Name = "objectListView1";
            this.objectListView1.ShowGroups = false;
            this.objectListView1.Size = new System.Drawing.Size(776, 304);
            this.objectListView1.TabIndex = 3;
            this.objectListView1.UseCompatibleStateImageBehavior = false;
            this.objectListView1.View = System.Windows.Forms.View.Details;
            // 
            // olvColumn_number
            // 
            this.olvColumn_number.AspectName = "Number";
            this.olvColumn_number.CellEditUseWholeCell = true;
            this.olvColumn_number.Text = "#";
            this.olvColumn_number.Width = 66;
            // 
            // olvColumn_description
            // 
            this.olvColumn_description.AspectName = "Description";
            this.olvColumn_description.CellEditUseWholeCell = true;
            this.olvColumn_description.Text = "Опис замовлення";
            this.olvColumn_description.Width = 394;
            // 
            // btn_create
            // 
            this.btn_create.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_create.Location = new System.Drawing.Point(325, 397);
            this.btn_create.Name = "btn_create";
            this.btn_create.Size = new System.Drawing.Size(152, 41);
            this.btn_create.TabIndex = 4;
            this.btn_create.Values.DropDownArrowColor = System.Drawing.Color.Empty;
            this.btn_create.Values.Text = "Створити";
            this.btn_create.Click += new System.EventHandler(this.kryptonButton1_Click);
            // 
            // btn_add_order
            // 
            this.btn_add_order.Location = new System.Drawing.Point(325, 31);
            this.btn_add_order.Name = "btn_add_order";
            this.btn_add_order.Size = new System.Drawing.Size(140, 39);
            this.btn_add_order.TabIndex = 5;
            this.btn_add_order.Values.DropDownArrowColor = System.Drawing.Color.Empty;
            this.btn_add_order.Values.Text = "додати замовлення";
            this.btn_add_order.Click += new System.EventHandler(this.btn_add_order_Click);
            // 
            // btn_paste
            // 
            this.btn_paste.Location = new System.Drawing.Point(653, 31);
            this.btn_paste.Name = "btn_paste";
            this.btn_paste.Size = new System.Drawing.Size(135, 39);
            this.btn_paste.TabIndex = 7;
            this.btn_paste.ToolTipValues.Description = "формат такий:\r\n№зам. -> опис\r\n-> - табуляція";
            this.btn_paste.ToolTipValues.EnableToolTips = true;
            this.btn_paste.Values.DropDownArrowColor = System.Drawing.Color.Empty;
            this.btn_paste.Values.Text = "вставити з буфера";
            this.btn_paste.Click += new System.EventHandler(this.btn_paste_Click);
            // 
            // FormAddWorkMany
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_paste);
            this.Controls.Add(this.btn_add_order);
            this.Controls.Add(this.btn_create);
            this.Controls.Add(this.objectListView1);
            this.Controls.Add(this.kryptonGroupBox1);
            this.Name = "FormAddWorkMany";
            this.ShowIcon = false;
            this.Text = "Додати кілька замовлень";
            ((System.ComponentModel.ISupportInitialize)(this.cb_customers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).EndInit();
            this.kryptonGroupBox1.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).EndInit();
            this.kryptonGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonComboBox cb_customers;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBox1;
        private BrightIdeasSoftware.ObjectListView objectListView1;
        private Krypton.Toolkit.KryptonButton btn_create;
        private BrightIdeasSoftware.OLVColumn olvColumn_number;
        private BrightIdeasSoftware.OLVColumn olvColumn_description;
        private Krypton.Toolkit.KryptonButton btn_add_order;
        private Krypton.Toolkit.KryptonButton btn_paste;
    }
}