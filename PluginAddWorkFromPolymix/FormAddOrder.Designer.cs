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
            this.ucNote1 = new Job.UC.UcNote();
            this.objectListViewFilter = new BrightIdeasSoftware.ObjectListView();
            this.olvColumnName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.kryptonButtonOk = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonGroupBox1 = new ComponentFactory.Krypton.Toolkit.KryptonGroupBox();
            this.kryptonButton1 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonGroupBox2 = new ComponentFactory.Krypton.Toolkit.KryptonGroupBox();
            this.comboBoxPolymix = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.textBoxNumber = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.comboBoxCustomer = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.ucTexBoxDescripion = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.kryptonGroupBox3 = new ComponentFactory.Krypton.Toolkit.KryptonGroupBox();
            this.kryptonGroupBox4 = new ComponentFactory.Krypton.Toolkit.KryptonGroupBox();
            this.kryptonGroupBox5 = new ComponentFactory.Krypton.Toolkit.KryptonGroupBox();
            this.kryptonGroupBox6 = new ComponentFactory.Krypton.Toolkit.KryptonGroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.objectListViewFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).BeginInit();
            this.kryptonGroupBox1.Panel.SuspendLayout();
            this.kryptonGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2.Panel)).BeginInit();
            this.kryptonGroupBox2.Panel.SuspendLayout();
            this.kryptonGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxPolymix)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxCustomer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox3.Panel)).BeginInit();
            this.kryptonGroupBox3.Panel.SuspendLayout();
            this.kryptonGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox4.Panel)).BeginInit();
            this.kryptonGroupBox4.Panel.SuspendLayout();
            this.kryptonGroupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox5.Panel)).BeginInit();
            this.kryptonGroupBox5.Panel.SuspendLayout();
            this.kryptonGroupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox6.Panel)).BeginInit();
            this.kryptonGroupBox6.Panel.SuspendLayout();
            this.kryptonGroupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // ucNote1
            // 
            this.ucNote1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucNote1.Location = new System.Drawing.Point(0, 0);
            this.ucNote1.Name = "ucNote1";
            this.ucNote1.Size = new System.Drawing.Size(539, 125);
            this.ucNote1.TabIndex = 1;
            // 
            // objectListViewFilter
            // 
            this.objectListViewFilter.AllColumns.Add(this.olvColumnName);
            this.objectListViewFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.objectListViewFilter.CellEditUseWholeCell = false;
            this.objectListViewFilter.CheckBoxes = true;
            this.objectListViewFilter.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnName});
            this.objectListViewFilter.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListViewFilter.HideSelection = false;
            this.objectListViewFilter.Location = new System.Drawing.Point(0, 0);
            this.objectListViewFilter.Name = "objectListViewFilter";
            this.objectListViewFilter.Size = new System.Drawing.Size(216, 302);
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
            this.kryptonGroupBox1.Panel.Controls.Add(this.kryptonButton1);
            this.kryptonGroupBox1.Panel.Controls.Add(this.objectListViewFilter);
            this.kryptonGroupBox1.Size = new System.Drawing.Size(218, 374);
            this.kryptonGroupBox1.TabIndex = 13;
            this.kryptonGroupBox1.Values.Heading = "Фільтр";
            // 
            // kryptonButton1
            // 
            this.kryptonButton1.Location = new System.Drawing.Point(20, 311);
            this.kryptonButton1.Name = "kryptonButton1";
            this.kryptonButton1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.kryptonButton1.Size = new System.Drawing.Size(171, 31);
            this.kryptonButton1.TabIndex = 1;
            this.kryptonButton1.Values.Text = "застосувати фільтр";
            this.kryptonButton1.Click += new System.EventHandler(this.buttonApplyFilter_Click);
            // 
            // kryptonGroupBox2
            // 
            this.kryptonGroupBox2.Location = new System.Drawing.Point(245, 12);
            this.kryptonGroupBox2.Name = "kryptonGroupBox2";
            // 
            // kryptonGroupBox2.Panel
            // 
            this.kryptonGroupBox2.Panel.Controls.Add(this.comboBoxPolymix);
            this.kryptonGroupBox2.Size = new System.Drawing.Size(543, 52);
            this.kryptonGroupBox2.TabIndex = 14;
            this.kryptonGroupBox2.Values.Heading = "Роботи з Polymix";
            // 
            // comboBoxPolymix
            // 
            this.comboBoxPolymix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxPolymix.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPolymix.DropDownWidth = 539;
            this.comboBoxPolymix.IntegralHeight = false;
            this.comboBoxPolymix.Location = new System.Drawing.Point(0, 0);
            this.comboBoxPolymix.Name = "comboBoxPolymix";
            this.comboBoxPolymix.Size = new System.Drawing.Size(539, 23);
            this.comboBoxPolymix.StateCommon.ComboBox.Content.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxPolymix.TabIndex = 0;
            this.comboBoxPolymix.SelectedIndexChanged += new System.EventHandler(this.comboBoxPolymix_SelectedIndexChanged);
            // 
            // textBoxNumber
            // 
            this.textBoxNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxNumber.Location = new System.Drawing.Point(0, 0);
            this.textBoxNumber.Name = "textBoxNumber";
            this.textBoxNumber.ReadOnly = true;
            this.textBoxNumber.Size = new System.Drawing.Size(130, 21);
            this.textBoxNumber.StateCommon.Content.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxNumber.StateCommon.Content.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Inherit;
            this.textBoxNumber.TabIndex = 17;
            this.textBoxNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // comboBoxCustomer
            // 
            this.comboBoxCustomer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxCustomer.DropDownWidth = 210;
            this.comboBoxCustomer.IntegralHeight = false;
            this.comboBoxCustomer.Location = new System.Drawing.Point(0, 0);
            this.comboBoxCustomer.Name = "comboBoxCustomer";
            this.comboBoxCustomer.Size = new System.Drawing.Size(210, 23);
            this.comboBoxCustomer.StateCommon.ComboBox.Content.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxCustomer.TabIndex = 18;
            // 
            // ucTexBoxDescripion
            // 
            this.ucTexBoxDescripion.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.ucTexBoxDescripion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucTexBoxDescripion.Location = new System.Drawing.Point(0, 0);
            this.ucTexBoxDescripion.MaxLength = 100;
            this.ucTexBoxDescripion.Name = "ucTexBoxDescripion";
            this.ucTexBoxDescripion.Size = new System.Drawing.Size(539, 25);
            this.ucTexBoxDescripion.TabIndex = 19;
            // 
            // kryptonGroupBox3
            // 
            this.kryptonGroupBox3.Location = new System.Drawing.Point(245, 178);
            this.kryptonGroupBox3.Name = "kryptonGroupBox3";
            // 
            // kryptonGroupBox3.Panel
            // 
            this.kryptonGroupBox3.Panel.Controls.Add(this.ucNote1);
            this.kryptonGroupBox3.Size = new System.Drawing.Size(543, 150);
            this.kryptonGroupBox3.TabIndex = 21;
            this.kryptonGroupBox3.Values.Heading = "Нотатки";
            // 
            // kryptonGroupBox4
            // 
            this.kryptonGroupBox4.Location = new System.Drawing.Point(245, 124);
            this.kryptonGroupBox4.Name = "kryptonGroupBox4";
            // 
            // kryptonGroupBox4.Panel
            // 
            this.kryptonGroupBox4.Panel.Controls.Add(this.ucTexBoxDescripion);
            this.kryptonGroupBox4.Size = new System.Drawing.Size(543, 52);
            this.kryptonGroupBox4.TabIndex = 22;
            this.kryptonGroupBox4.Values.Heading = "Опис замовлення";
            // 
            // kryptonGroupBox5
            // 
            this.kryptonGroupBox5.Location = new System.Drawing.Point(245, 70);
            this.kryptonGroupBox5.Name = "kryptonGroupBox5";
            // 
            // kryptonGroupBox5.Panel
            // 
            this.kryptonGroupBox5.Panel.Controls.Add(this.textBoxNumber);
            this.kryptonGroupBox5.Size = new System.Drawing.Size(134, 52);
            this.kryptonGroupBox5.TabIndex = 23;
            this.kryptonGroupBox5.Values.Heading = "№ замовлення";
            // 
            // kryptonGroupBox6
            // 
            this.kryptonGroupBox6.Location = new System.Drawing.Point(385, 69);
            this.kryptonGroupBox6.Name = "kryptonGroupBox6";
            // 
            // kryptonGroupBox6.Panel
            // 
            this.kryptonGroupBox6.Panel.Controls.Add(this.comboBoxCustomer);
            this.kryptonGroupBox6.Size = new System.Drawing.Size(214, 53);
            this.kryptonGroupBox6.TabIndex = 24;
            this.kryptonGroupBox6.Values.Heading = "Замовник у Active Works";
            // 
            // FormAddOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 398);
            this.Controls.Add(this.kryptonGroupBox6);
            this.Controls.Add(this.kryptonGroupBox5);
            this.Controls.Add(this.kryptonGroupBox4);
            this.Controls.Add(this.kryptonGroupBox3);
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
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxPolymix)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxCustomer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox3.Panel)).EndInit();
            this.kryptonGroupBox3.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox3)).EndInit();
            this.kryptonGroupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox4.Panel)).EndInit();
            this.kryptonGroupBox4.Panel.ResumeLayout(false);
            this.kryptonGroupBox4.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox4)).EndInit();
            this.kryptonGroupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox5.Panel)).EndInit();
            this.kryptonGroupBox5.Panel.ResumeLayout(false);
            this.kryptonGroupBox5.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox5)).EndInit();
            this.kryptonGroupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox6.Panel)).EndInit();
            this.kryptonGroupBox6.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox6)).EndInit();
            this.kryptonGroupBox6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Job.UC.UcNote ucNote1;
        private BrightIdeasSoftware.ObjectListView objectListViewFilter;
        private BrightIdeasSoftware.OLVColumn olvColumnName;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButtonOk;
        private ComponentFactory.Krypton.Toolkit.KryptonGroupBox kryptonGroupBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton1;
        private ComponentFactory.Krypton.Toolkit.KryptonGroupBox kryptonGroupBox2;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox comboBoxPolymix;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox textBoxNumber;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox comboBoxCustomer;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox ucTexBoxDescripion;
        private ComponentFactory.Krypton.Toolkit.KryptonGroupBox kryptonGroupBox3;
        private ComponentFactory.Krypton.Toolkit.KryptonGroupBox kryptonGroupBox4;
        private ComponentFactory.Krypton.Toolkit.KryptonGroupBox kryptonGroupBox5;
        private ComponentFactory.Krypton.Toolkit.KryptonGroupBox kryptonGroupBox6;
    }
}