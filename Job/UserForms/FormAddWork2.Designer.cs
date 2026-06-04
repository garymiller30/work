namespace JobSpace.UserForms
{
    partial class FormAddWork2
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
            components = new System.ComponentModel.Container();
            kryptonLabel3 = new Krypton.Toolkit.KryptonLabel();
            kryptonTextBoxNumber = new System.Windows.Forms.TextBox();
            kryptonPanel2 = new Krypton.Toolkit.KryptonPanel();
            kryptonComboBox_Customers = new Krypton.Toolkit.KryptonComboBox();
            kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            panelCategory = new Krypton.Toolkit.KryptonPanel();
            tb_category = new System.Windows.Forms.TextBox();
            olv_categories = new BrightIdeasSoftware.ObjectListView();
            olvColumn_category_name = new BrightIdeasSoftware.OLVColumn();
            kryptonLabel2 = new Krypton.Toolkit.KryptonLabel();
            panel4 = new System.Windows.Forms.Panel();
            label1 = new System.Windows.Forms.Label();
            label_language = new System.Windows.Forms.Label();
            btn_fix_wrong_keyboard = new System.Windows.Forms.Button();
            checkBoxCloseAfterPaste = new Krypton.Toolkit.KryptonCheckBox();
            kryptonLabel4 = new Krypton.Toolkit.KryptonLabel();
            textBox_Description = new JobSpace.UC.UcTexBox(components);
            kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            btn_select_custom_folder = new System.Windows.Forms.Button();
            kryptonPanel3 = new Krypton.Toolkit.KryptonPanel();
            kryptonSplitContainer1 = new Krypton.Toolkit.KryptonSplitContainer();
            kryptonGroupBox1 = new Krypton.Toolkit.KryptonGroupBox();
            ucNote1 = new JobSpace.UC.UcNote();
            kryptonGroupBox2 = new Krypton.Toolkit.KryptonGroupBox();
            ucAddWorkPluginsContainer1 = new JobSpace.UC.UcAddWorkPluginsContainer();
            kryptonButton_OK = new Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)kryptonPanel2).BeginInit();
            kryptonPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)kryptonComboBox_Customers).BeginInit();
            ((System.ComponentModel.ISupportInitialize)panelCategory).BeginInit();
            panelCategory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)olv_categories).BeginInit();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)kryptonPanel1).BeginInit();
            kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)kryptonPanel3).BeginInit();
            kryptonPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)kryptonSplitContainer1).BeginInit();
            (kryptonSplitContainer1.Panel1).BeginInit();
            kryptonSplitContainer1.Panel1.SuspendLayout();
            (kryptonSplitContainer1.Panel2).BeginInit();
            kryptonSplitContainer1.Panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)kryptonGroupBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)kryptonGroupBox1.Panel).BeginInit();
            kryptonGroupBox1.Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)kryptonGroupBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)kryptonGroupBox2.Panel).BeginInit();
            kryptonGroupBox2.Panel.SuspendLayout();
            SuspendLayout();
            // 
            // kryptonLabel3
            // 
            kryptonLabel3.Location = new System.Drawing.Point(15, 12);
            kryptonLabel3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            kryptonLabel3.Name = "kryptonLabel3";
            kryptonLabel3.Size = new System.Drawing.Size(59, 23);
            kryptonLabel3.TabIndex = 51;
            kryptonLabel3.Values.Text = "№ зам.";
            // 
            // kryptonTextBoxNumber
            // 
            kryptonTextBoxNumber.Location = new System.Drawing.Point(82, 12);
            kryptonTextBoxNumber.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            kryptonTextBoxNumber.Name = "kryptonTextBoxNumber";
            kryptonTextBoxNumber.Size = new System.Drawing.Size(98, 23);
            kryptonTextBoxNumber.TabIndex = 0;
            kryptonTextBoxNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // kryptonPanel2
            // 
            kryptonPanel2.Controls.Add(kryptonComboBox_Customers);
            kryptonPanel2.Controls.Add(kryptonLabel1);
            kryptonPanel2.Location = new System.Drawing.Point(214, 14);
            kryptonPanel2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            kryptonPanel2.Name = "kryptonPanel2";
            kryptonPanel2.Size = new System.Drawing.Size(310, 44);
            kryptonPanel2.TabIndex = 1;
            // 
            // kryptonComboBox_Customers
            // 
            kryptonComboBox_Customers.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            kryptonComboBox_Customers.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            kryptonComboBox_Customers.DropDownHeight = 500;
            kryptonComboBox_Customers.DropDownWidth = 147;
            kryptonComboBox_Customers.FormattingEnabled = true;
            kryptonComboBox_Customers.IntegralHeight = false;
            kryptonComboBox_Customers.Location = new System.Drawing.Point(86, 10);
            kryptonComboBox_Customers.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            kryptonComboBox_Customers.Name = "kryptonComboBox_Customers";
            kryptonComboBox_Customers.Size = new System.Drawing.Size(209, 22);
            kryptonComboBox_Customers.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            kryptonComboBox_Customers.TabIndex = 0;
            kryptonComboBox_Customers.SelectedIndexChanged += kryptonComboBox_Customers_SelectedIndexChanged;
            kryptonComboBox_Customers.Enter += kryptonComboBox_Customers_Enter;
            // 
            // kryptonLabel1
            // 
            kryptonLabel1.Location = new System.Drawing.Point(2, 12);
            kryptonLabel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            kryptonLabel1.Name = "kryptonLabel1";
            kryptonLabel1.Size = new System.Drawing.Size(77, 23);
            kryptonLabel1.TabIndex = 0;
            kryptonLabel1.Values.Text = "Замовник";
            // 
            // panelCategory
            // 
            panelCategory.Controls.Add(tb_category);
            panelCategory.Controls.Add(olv_categories);
            panelCategory.Controls.Add(kryptonLabel2);
            panelCategory.Location = new System.Drawing.Point(533, 14);
            panelCategory.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panelCategory.Name = "panelCategory";
            panelCategory.Size = new System.Drawing.Size(265, 562);
            panelCategory.TabIndex = 2;
            // 
            // tb_category
            // 
            tb_category.Location = new System.Drawing.Point(85, 12);
            tb_category.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tb_category.Name = "tb_category";
            tb_category.Size = new System.Drawing.Size(170, 23);
            tb_category.TabIndex = 53;
            tb_category.KeyUp += tb_category_KeyUp;
            // 
            // olv_categories
            // 
            olv_categories.AllColumns.Add(olvColumn_category_name);
            olv_categories.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            olv_categories.CellEditUseWholeCell = false;
            olv_categories.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { olvColumn_category_name });
            olv_categories.FullRowSelect = true;
            olv_categories.GridLines = true;
            olv_categories.Location = new System.Drawing.Point(14, 45);
            olv_categories.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            olv_categories.Name = "olv_categories";
            olv_categories.ShowGroups = false;
            olv_categories.Size = new System.Drawing.Size(241, 513);
            olv_categories.Sorting = System.Windows.Forms.SortOrder.Ascending;
            olv_categories.TabIndex = 52;
            olv_categories.UseCompatibleStateImageBehavior = false;
            olv_categories.UseFiltering = true;
            olv_categories.View = System.Windows.Forms.View.Details;
            olv_categories.SelectionChanged += olv_categories_SelectionChanged;
            // 
            // olvColumn_category_name
            // 
            olvColumn_category_name.AspectName = "Name";
            olvColumn_category_name.Text = "Назва";
            olvColumn_category_name.Width = 300;
            // 
            // kryptonLabel2
            // 
            kryptonLabel2.Location = new System.Drawing.Point(14, 9);
            kryptonLabel2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            kryptonLabel2.Name = "kryptonLabel2";
            kryptonLabel2.Size = new System.Drawing.Size(75, 23);
            kryptonLabel2.TabIndex = 51;
            kryptonLabel2.Values.Text = "Категорія";
            // 
            // panel4
            // 
            panel4.BackColor = System.Drawing.Color.Transparent;
            panel4.Controls.Add(label1);
            panel4.Controls.Add(label_language);
            panel4.Controls.Add(btn_fix_wrong_keyboard);
            panel4.Controls.Add(checkBoxCloseAfterPaste);
            panel4.Controls.Add(kryptonLabel4);
            panel4.Controls.Add(textBox_Description);
            panel4.Location = new System.Drawing.Point(14, 65);
            panel4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panel4.Name = "panel4";
            panel4.Size = new System.Drawing.Size(510, 90);
            panel4.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(4, 9);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(87, 15);
            label1.TabIndex = 54;
            label1.Text = "поточна мова:";
            // 
            // label_language
            // 
            label_language.AutoSize = true;
            label_language.Location = new System.Drawing.Point(103, 9);
            label_language.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label_language.Name = "label_language";
            label_language.Size = new System.Drawing.Size(17, 15);
            label_language.TabIndex = 53;
            label_language.Text = "--";
            // 
            // btn_fix_wrong_keyboard
            // 
            btn_fix_wrong_keyboard.Location = new System.Drawing.Point(351, 3);
            btn_fix_wrong_keyboard.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btn_fix_wrong_keyboard.Name = "btn_fix_wrong_keyboard";
            btn_fix_wrong_keyboard.Size = new System.Drawing.Size(155, 27);
            btn_fix_wrong_keyboard.TabIndex = 52;
            btn_fix_wrong_keyboard.Text = "виправити розкладку";
            btn_fix_wrong_keyboard.UseVisualStyleBackColor = true;
            btn_fix_wrong_keyboard.Click += btn_fix_wrong_keyboard_Click;
            // 
            // checkBoxCloseAfterPaste
            // 
            checkBoxCloseAfterPaste.Checked = true;
            checkBoxCloseAfterPaste.CheckState = System.Windows.Forms.CheckState.Checked;
            checkBoxCloseAfterPaste.Location = new System.Drawing.Point(57, 61);
            checkBoxCloseAfterPaste.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            checkBoxCloseAfterPaste.Name = "checkBoxCloseAfterPaste";
            checkBoxCloseAfterPaste.Size = new System.Drawing.Size(216, 23);
            checkBoxCloseAfterPaste.TabIndex = 51;
            checkBoxCloseAfterPaste.Values.Text = "закрити після вставки тексту";
            checkBoxCloseAfterPaste.CheckedChanged += CheckBoxCloseAfterPaste_CheckedChanged;
            // 
            // kryptonLabel4
            // 
            kryptonLabel4.Location = new System.Drawing.Point(4, 37);
            kryptonLabel4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            kryptonLabel4.Name = "kryptonLabel4";
            kryptonLabel4.Size = new System.Drawing.Size(47, 23);
            kryptonLabel4.TabIndex = 51;
            kryptonLabel4.Values.Text = "Опис";
            // 
            // textBox_Description
            // 
            textBox_Description.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBox_Description.Location = new System.Drawing.Point(57, 37);
            textBox_Description.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBox_Description.Name = "textBox_Description";
            textBox_Description.Size = new System.Drawing.Size(448, 23);
            textBox_Description.TabIndex = 0;
            textBox_Description.Enter += textBox_Description_Enter;
            // 
            // kryptonPanel1
            // 
            kryptonPanel1.Controls.Add(btn_select_custom_folder);
            kryptonPanel1.Controls.Add(panelCategory);
            kryptonPanel1.Controls.Add(kryptonPanel3);
            kryptonPanel1.Controls.Add(kryptonPanel2);
            kryptonPanel1.Controls.Add(kryptonSplitContainer1);
            kryptonPanel1.Controls.Add(kryptonButton_OK);
            kryptonPanel1.Controls.Add(panel4);
            kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            kryptonPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            kryptonPanel1.Name = "kryptonPanel1";
            kryptonPanel1.Size = new System.Drawing.Size(812, 590);
            kryptonPanel1.TabIndex = 49;
            // 
            // btn_select_custom_folder
            // 
            btn_select_custom_folder.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btn_select_custom_folder.Location = new System.Drawing.Point(18, 546);
            btn_select_custom_folder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btn_select_custom_folder.Name = "btn_select_custom_folder";
            btn_select_custom_folder.Size = new System.Drawing.Size(133, 27);
            btn_select_custom_folder.TabIndex = 51;
            btn_select_custom_folder.Text = "вибрати іншу папку";
            btn_select_custom_folder.UseVisualStyleBackColor = true;
            btn_select_custom_folder.Click += btn_select_custom_folder_Click;
            // 
            // kryptonPanel3
            // 
            kryptonPanel3.Controls.Add(kryptonLabel3);
            kryptonPanel3.Controls.Add(kryptonTextBoxNumber);
            kryptonPanel3.Location = new System.Drawing.Point(14, 14);
            kryptonPanel3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            kryptonPanel3.Name = "kryptonPanel3";
            kryptonPanel3.Size = new System.Drawing.Size(192, 44);
            kryptonPanel3.TabIndex = 0;
            // 
            // kryptonSplitContainer1
            // 
            kryptonSplitContainer1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            kryptonSplitContainer1.Location = new System.Drawing.Point(14, 162);
            kryptonSplitContainer1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            kryptonSplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // 
            // 
            kryptonSplitContainer1.Panel1.Controls.Add(kryptonGroupBox1);
            // 
            // 
            // 
            kryptonSplitContainer1.Panel2.Controls.Add(kryptonGroupBox2);
            kryptonSplitContainer1.Size = new System.Drawing.Size(512, 358);
            kryptonSplitContainer1.SplitterDistance = 209;
            kryptonSplitContainer1.TabIndex = 50;
            // 
            // kryptonGroupBox1
            // 
            kryptonGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            kryptonGroupBox1.Location = new System.Drawing.Point(0, 0);
            kryptonGroupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            // 
            // 
            // 
            kryptonGroupBox1.Panel.Controls.Add(ucNote1);
            kryptonGroupBox1.Size = new System.Drawing.Size(512, 209);
            kryptonGroupBox1.TabIndex = 0;
            kryptonGroupBox1.Values.Heading = "Примітка";
            // 
            // ucNote1
            // 
            ucNote1.Dock = System.Windows.Forms.DockStyle.Fill;
            ucNote1.Location = new System.Drawing.Point(0, 0);
            ucNote1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            ucNote1.Name = "ucNote1";
            ucNote1.Size = new System.Drawing.Size(508, 185);
            ucNote1.TabIndex = 0;
            // 
            // kryptonGroupBox2
            // 
            kryptonGroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            kryptonGroupBox2.Location = new System.Drawing.Point(0, 0);
            kryptonGroupBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            // 
            // 
            // 
            kryptonGroupBox2.Panel.Controls.Add(ucAddWorkPluginsContainer1);
            kryptonGroupBox2.Size = new System.Drawing.Size(512, 144);
            kryptonGroupBox2.TabIndex = 0;
            kryptonGroupBox2.Values.Heading = "Додатково";
            // 
            // ucAddWorkPluginsContainer1
            // 
            ucAddWorkPluginsContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            ucAddWorkPluginsContainer1.Location = new System.Drawing.Point(0, 0);
            ucAddWorkPluginsContainer1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            ucAddWorkPluginsContainer1.Name = "ucAddWorkPluginsContainer1";
            ucAddWorkPluginsContainer1.Size = new System.Drawing.Size(508, 120);
            ucAddWorkPluginsContainer1.TabIndex = 0;
            // 
            // kryptonButton_OK
            // 
            kryptonButton_OK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            kryptonButton_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            kryptonButton_OK.Location = new System.Drawing.Point(327, 531);
            kryptonButton_OK.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            kryptonButton_OK.Name = "kryptonButton_OK";
            kryptonButton_OK.Size = new System.Drawing.Size(156, 45);
            kryptonButton_OK.TabIndex = 4;
            kryptonButton_OK.Values.DropDownArrowColor = System.Drawing.Color.Empty;
            kryptonButton_OK.Values.Text = "OK";
            kryptonButton_OK.Click += Button_Ok_Click;
            // 
            // FormAddWork2
            // 
            AcceptButton = kryptonButton_OK;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(812, 590);
            Controls.Add(kryptonPanel1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormAddWork2";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Замовлення";
            FormClosing += FormAddWork2_FormClosing;
            Load += FormAddWork2_Load;
            Shown += FormAddWork2_Shown;
            ((System.ComponentModel.ISupportInitialize)kryptonPanel2).EndInit();
            kryptonPanel2.ResumeLayout(false);
            kryptonPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)kryptonComboBox_Customers).EndInit();
            ((System.ComponentModel.ISupportInitialize)panelCategory).EndInit();
            panelCategory.ResumeLayout(false);
            panelCategory.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)olv_categories).EndInit();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)kryptonPanel1).EndInit();
            kryptonPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)kryptonPanel3).EndInit();
            kryptonPanel3.ResumeLayout(false);
            kryptonPanel3.PerformLayout();
            (kryptonSplitContainer1.Panel1).EndInit();
            kryptonSplitContainer1.Panel1.ResumeLayout(false);
            (kryptonSplitContainer1.Panel2).EndInit();
            kryptonSplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)kryptonSplitContainer1).EndInit();
            ((System.ComponentModel.ISupportInitialize)kryptonGroupBox1.Panel).EndInit();
            kryptonGroupBox1.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)kryptonGroupBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)kryptonGroupBox2.Panel).EndInit();
            kryptonGroupBox2.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)kryptonGroupBox2).EndInit();
            ResumeLayout(false);

        }

        #endregion
        private global::JobSpace.UC.UcTexBox textBox_Description;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox kryptonTextBoxNumber;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonButton kryptonButton_OK;
        private Krypton.Toolkit.KryptonSplitContainer kryptonSplitContainer1;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBox1;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBox2;
        private Krypton.Toolkit.KryptonPanel kryptonPanel2;
        private Krypton.Toolkit.KryptonComboBox kryptonComboBox_Customers;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private Krypton.Toolkit.KryptonLabel kryptonLabel3;
        private Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private Krypton.Toolkit.KryptonLabel kryptonLabel4;
        private Krypton.Toolkit.KryptonCheckBox checkBoxCloseAfterPaste;
        private UC.UcNote ucNote1;
        private UC.UcAddWorkPluginsContainer ucAddWorkPluginsContainer1;
        private Krypton.Toolkit.KryptonPanel panelCategory;
        private Krypton.Toolkit.KryptonPanel kryptonPanel3;
        private BrightIdeasSoftware.ObjectListView olv_categories;
        private BrightIdeasSoftware.OLVColumn olvColumn_category_name;
        private System.Windows.Forms.TextBox tb_category;
        private System.Windows.Forms.Button btn_select_custom_folder;
        private System.Windows.Forms.Button btn_fix_wrong_keyboard;
        private System.Windows.Forms.Label label_language;
        private System.Windows.Forms.Label label1;
    }
}