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
            cb_customers = new Krypton.Toolkit.KryptonComboBox();
            kryptonGroupBox1 = new Krypton.Toolkit.KryptonGroupBox();
            objectListView1 = new BrightIdeasSoftware.ObjectListView();
            olvColumn_number = new BrightIdeasSoftware.OLVColumn();
            olvColumn_description = new BrightIdeasSoftware.OLVColumn();
            btn_create = new Krypton.Toolkit.KryptonButton();
            btn_add_order = new Krypton.Toolkit.KryptonButton();
            btn_paste = new Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)cb_customers).BeginInit();
            ((System.ComponentModel.ISupportInitialize)kryptonGroupBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)kryptonGroupBox1.Panel).BeginInit();
            kryptonGroupBox1.Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)objectListView1).BeginInit();
            SuspendLayout();
            // 
            // cb_customers
            // 
            cb_customers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cb_customers.DropDownWidth = 75;
            cb_customers.Location = new System.Drawing.Point(4, 3);
            cb_customers.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cb_customers.Name = "cb_customers";
            cb_customers.Size = new System.Drawing.Size(322, 22);
            cb_customers.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            cb_customers.TabIndex = 0;
            // 
            // kryptonGroupBox1
            // 
            kryptonGroupBox1.Location = new System.Drawing.Point(14, 14);
            kryptonGroupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            // 
            // 
            // 
            kryptonGroupBox1.Panel.Controls.Add(cb_customers);
            kryptonGroupBox1.Size = new System.Drawing.Size(334, 67);
            kryptonGroupBox1.TabIndex = 2;
            kryptonGroupBox1.Values.Heading = "Замовник";
            // 
            // objectListView1
            // 
            objectListView1.AllColumns.Add(olvColumn_number);
            objectListView1.AllColumns.Add(olvColumn_description);
            objectListView1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            objectListView1.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClickAlways;
            objectListView1.CellEditUseWholeCell = false;
            objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { olvColumn_number, olvColumn_description });
            objectListView1.FullRowSelect = true;
            objectListView1.GridLines = true;
            objectListView1.Location = new System.Drawing.Point(14, 88);
            objectListView1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            objectListView1.Name = "objectListView1";
            objectListView1.ShowGroups = false;
            objectListView1.Size = new System.Drawing.Size(905, 350);
            objectListView1.TabIndex = 3;
            objectListView1.UseCompatibleStateImageBehavior = false;
            objectListView1.View = System.Windows.Forms.View.Details;
            // 
            // olvColumn_number
            // 
            olvColumn_number.AspectName = "Number";
            olvColumn_number.CellEditUseWholeCell = true;
            olvColumn_number.Text = "#";
            olvColumn_number.Width = 66;
            // 
            // olvColumn_description
            // 
            olvColumn_description.AspectName = "Description";
            olvColumn_description.CellEditUseWholeCell = true;
            olvColumn_description.Text = "Опис замовлення";
            olvColumn_description.Width = 394;
            // 
            // btn_create
            // 
            btn_create.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            btn_create.Location = new System.Drawing.Point(379, 458);
            btn_create.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btn_create.Name = "btn_create";
            btn_create.Size = new System.Drawing.Size(177, 47);
            btn_create.TabIndex = 4;
            btn_create.Values.DropDownArrowColor = System.Drawing.Color.Empty;
            btn_create.Values.Text = "Створити";
            btn_create.Click += kryptonButton1_Click;
            // 
            // btn_add_order
            // 
            btn_add_order.AutoSize = true;
            btn_add_order.Location = new System.Drawing.Point(379, 39);
            btn_add_order.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btn_add_order.Name = "btn_add_order";
            btn_add_order.Size = new System.Drawing.Size(183, 42);
            btn_add_order.TabIndex = 5;
            btn_add_order.Values.DropDownArrowColor = System.Drawing.Color.Empty;
            btn_add_order.Values.Text = "+ додати замовлення в список";
            btn_add_order.Click += btn_add_order_Click;
            // 
            // btn_paste
            // 
            btn_paste.Location = new System.Drawing.Point(762, 36);
            btn_paste.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btn_paste.Name = "btn_paste";
            btn_paste.Size = new System.Drawing.Size(158, 45);
            btn_paste.TabIndex = 7;
            btn_paste.ToolTipValues.Description = "формат такий:\r\n№зам. -> опис\r\n-> - табуляція";
            btn_paste.ToolTipValues.EnableToolTips = true;
            btn_paste.Values.DropDownArrowColor = System.Drawing.Color.Empty;
            btn_paste.Values.Text = "вставити з буфера";
            btn_paste.Click += btn_paste_Click;
            // 
            // FormAddWorkMany
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(933, 519);
            Controls.Add(btn_paste);
            Controls.Add(btn_add_order);
            Controls.Add(btn_create);
            Controls.Add(objectListView1);
            Controls.Add(kryptonGroupBox1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormAddWorkMany";
            ShowIcon = false;
            Text = "Додати кілька замовлень";
            ((System.ComponentModel.ISupportInitialize)cb_customers).EndInit();
            ((System.ComponentModel.ISupportInitialize)kryptonGroupBox1.Panel).EndInit();
            kryptonGroupBox1.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)kryptonGroupBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)objectListView1).EndInit();
            ResumeLayout(false);
            PerformLayout();

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