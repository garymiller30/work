namespace PluginWorkProcessPlates.Forms
{
    partial class FormEdit
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
            this.components = new System.ComponentModel.Container();
            this.buttonOk = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBoxFormats = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownHeight = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownWidth = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonPlus4 = new System.Windows.Forms.Button();
            this.buttonPlus3 = new System.Windows.Forms.Button();
            this.buttonPlus2 = new System.Windows.Forms.Button();
            this.buttonPlus1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDownKomplekt = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownCount = new System.Windows.Forms.NumericUpDown();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.numericUpDownPrice = new System.Windows.Forms.NumericUpDown();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.objectListViewPays = new BrightIdeasSoftware.ObjectListView();
            this.olvColumnDate = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnSum = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.додатиПлатіжToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.видалитиПлатіжToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWidth)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownKomplekt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCount)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPrice)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectListViewPays)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOk
            // 
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(165, 287);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(101, 36);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBoxFormats);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.numericUpDownHeight);
            this.groupBox1.Controls.Add(this.numericUpDownWidth);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(325, 63);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Формат пластини";
            // 
            // comboBoxFormats
            // 
            this.comboBoxFormats.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFormats.FormattingEnabled = true;
            this.comboBoxFormats.Location = new System.Drawing.Point(7, 28);
            this.comboBoxFormats.Name = "comboBoxFormats";
            this.comboBoxFormats.Size = new System.Drawing.Size(157, 21);
            this.comboBoxFormats.TabIndex = 5;
            this.comboBoxFormats.SelectedIndexChanged += new System.EventHandler(this.comboBoxFormats_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(248, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "висота";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(170, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "ширина";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(233, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "x";
            // 
            // numericUpDownHeight
            // 
            this.numericUpDownHeight.Location = new System.Drawing.Point(251, 28);
            this.numericUpDownHeight.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numericUpDownHeight.Name = "numericUpDownHeight";
            this.numericUpDownHeight.Size = new System.Drawing.Size(56, 20);
            this.numericUpDownHeight.TabIndex = 1;
            this.numericUpDownHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownHeight.Enter += new System.EventHandler(this.numericUpDownWidth_Enter);
            // 
            // numericUpDownWidth
            // 
            this.numericUpDownWidth.Location = new System.Drawing.Point(171, 28);
            this.numericUpDownWidth.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numericUpDownWidth.Name = "numericUpDownWidth";
            this.numericUpDownWidth.Size = new System.Drawing.Size(56, 20);
            this.numericUpDownWidth.TabIndex = 0;
            this.numericUpDownWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownWidth.Enter += new System.EventHandler(this.numericUpDownWidth_Enter);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.buttonPlus4);
            this.groupBox2.Controls.Add(this.buttonPlus3);
            this.groupBox2.Controls.Add(this.buttonPlus2);
            this.groupBox2.Controls.Add(this.buttonPlus1);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.numericUpDownKomplekt);
            this.groupBox2.Controls.Add(this.numericUpDownCount);
            this.groupBox2.Location = new System.Drawing.Point(12, 81);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(412, 53);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "кількість";
            // 
            // buttonPlus4
            // 
            this.buttonPlus4.Location = new System.Drawing.Point(238, 16);
            this.buttonPlus4.Name = "buttonPlus4";
            this.buttonPlus4.Size = new System.Drawing.Size(28, 23);
            this.buttonPlus4.TabIndex = 11;
            this.buttonPlus4.Text = "+4";
            this.buttonPlus4.UseVisualStyleBackColor = true;
            this.buttonPlus4.Click += new System.EventHandler(this.buttonPlus4_Click);
            // 
            // buttonPlus3
            // 
            this.buttonPlus3.Location = new System.Drawing.Point(204, 16);
            this.buttonPlus3.Name = "buttonPlus3";
            this.buttonPlus3.Size = new System.Drawing.Size(28, 23);
            this.buttonPlus3.TabIndex = 10;
            this.buttonPlus3.Text = "+3";
            this.buttonPlus3.UseVisualStyleBackColor = true;
            this.buttonPlus3.Click += new System.EventHandler(this.buttonPlus3_Click);
            // 
            // buttonPlus2
            // 
            this.buttonPlus2.Location = new System.Drawing.Point(170, 16);
            this.buttonPlus2.Name = "buttonPlus2";
            this.buttonPlus2.Size = new System.Drawing.Size(28, 23);
            this.buttonPlus2.TabIndex = 9;
            this.buttonPlus2.Text = "+2";
            this.buttonPlus2.UseVisualStyleBackColor = true;
            this.buttonPlus2.Click += new System.EventHandler(this.buttonPlus2_Click);
            // 
            // buttonPlus1
            // 
            this.buttonPlus1.Location = new System.Drawing.Point(136, 16);
            this.buttonPlus1.Name = "buttonPlus1";
            this.buttonPlus1.Size = new System.Drawing.Size(28, 23);
            this.buttonPlus1.TabIndex = 8;
            this.buttonPlus1.Text = "+1";
            this.buttonPlus1.UseVisualStyleBackColor = true;
            this.buttonPlus1.Click += new System.EventHandler(this.buttonPlus1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(52, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "x";
            // 
            // numericUpDownKomplekt
            // 
            this.numericUpDownKomplekt.Location = new System.Drawing.Point(6, 19);
            this.numericUpDownKomplekt.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numericUpDownKomplekt.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownKomplekt.Name = "numericUpDownKomplekt";
            this.numericUpDownKomplekt.Size = new System.Drawing.Size(40, 20);
            this.numericUpDownKomplekt.TabIndex = 6;
            this.numericUpDownKomplekt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownKomplekt.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numericUpDownCount
            // 
            this.numericUpDownCount.Location = new System.Drawing.Point(70, 19);
            this.numericUpDownCount.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numericUpDownCount.Name = "numericUpDownCount";
            this.numericUpDownCount.Size = new System.Drawing.Size(58, 20);
            this.numericUpDownCount.TabIndex = 5;
            this.numericUpDownCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownCount.Enter += new System.EventHandler(this.numericUpDownWidth_Enter);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.numericUpDownPrice);
            this.groupBox3.Location = new System.Drawing.Point(343, 13);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(81, 62);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "ціна за шт.";
            // 
            // numericUpDownPrice
            // 
            this.numericUpDownPrice.Location = new System.Drawing.Point(6, 27);
            this.numericUpDownPrice.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numericUpDownPrice.Name = "numericUpDownPrice";
            this.numericUpDownPrice.Size = new System.Drawing.Size(69, 20);
            this.numericUpDownPrice.TabIndex = 6;
            this.numericUpDownPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownPrice.Enter += new System.EventHandler(this.numericUpDownWidth_Enter);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.objectListViewPays);
            this.groupBox4.Location = new System.Drawing.Point(12, 140);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(412, 129);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Оплата";
            // 
            // objectListViewPays
            // 
            this.objectListViewPays.AllColumns.Add(this.olvColumnDate);
            this.objectListViewPays.AllColumns.Add(this.olvColumnSum);
            this.objectListViewPays.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
            this.objectListViewPays.CellEditUseWholeCell = false;
            this.objectListViewPays.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnDate,
            this.olvColumnSum});
            this.objectListViewPays.ContextMenuStrip = this.contextMenuStrip1;
            this.objectListViewPays.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListViewPays.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectListViewPays.FullRowSelect = true;
            this.objectListViewPays.GridLines = true;
            this.objectListViewPays.HideSelection = false;
            this.objectListViewPays.Location = new System.Drawing.Point(3, 16);
            this.objectListViewPays.Name = "objectListViewPays";
            this.objectListViewPays.ShowGroups = false;
            this.objectListViewPays.Size = new System.Drawing.Size(406, 110);
            this.objectListViewPays.TabIndex = 0;
            this.objectListViewPays.UseCompatibleStateImageBehavior = false;
            this.objectListViewPays.View = System.Windows.Forms.View.Details;
            this.objectListViewPays.CellEditFinishing += new BrightIdeasSoftware.CellEditEventHandler(this.objectListViewPays_CellEditFinishing);
            // 
            // olvColumnDate
            // 
            this.olvColumnDate.AspectName = "Date";
            this.olvColumnDate.IsEditable = false;
            this.olvColumnDate.Text = "Дата оплати";
            this.olvColumnDate.Width = 230;
            // 
            // olvColumnSum
            // 
            this.olvColumnSum.AspectName = "Sum";
            this.olvColumnSum.CellEditUseWholeCell = true;
            this.olvColumnSum.Text = "сплачена сума";
            this.olvColumnSum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.olvColumnSum.Width = 100;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.додатиПлатіжToolStripMenuItem,
            this.видалитиПлатіжToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(173, 48);
            // 
            // додатиПлатіжToolStripMenuItem
            // 
            this.додатиПлатіжToolStripMenuItem.Name = "додатиПлатіжToolStripMenuItem";
            this.додатиПлатіжToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.додатиПлатіжToolStripMenuItem.Text = "додати платіж";
            this.додатиПлатіжToolStripMenuItem.Click += new System.EventHandler(this.додатиПлатіжToolStripMenuItem_Click);
            // 
            // видалитиПлатіжToolStripMenuItem
            // 
            this.видалитиПлатіжToolStripMenuItem.Name = "видалитиПлатіжToolStripMenuItem";
            this.видалитиПлатіжToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.видалитиПлатіжToolStripMenuItem.Text = "видалити платіж";
            this.видалитиПлатіжToolStripMenuItem.Click += new System.EventHandler(this.видалитиПлатіжToolStripMenuItem_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(272, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(28, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "+8";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(306, 16);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(36, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "+16";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // FormEdit
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 335);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormEdit";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Редагувати";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWidth)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownKomplekt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCount)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPrice)).EndInit();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.objectListViewPays)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownHeight;
        private System.Windows.Forms.NumericUpDown numericUpDownWidth;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown numericUpDownCount;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown numericUpDownPrice;
        private System.Windows.Forms.GroupBox groupBox4;
        private BrightIdeasSoftware.ObjectListView objectListViewPays;
        private BrightIdeasSoftware.OLVColumn olvColumnDate;
        private BrightIdeasSoftware.OLVColumn olvColumnSum;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem додатиПлатіжToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem видалитиПлатіжToolStripMenuItem;
        private System.Windows.Forms.ComboBox comboBoxFormats;
        private System.Windows.Forms.NumericUpDown numericUpDownKomplekt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonPlus4;
        private System.Windows.Forms.Button buttonPlus3;
        private System.Windows.Forms.Button buttonPlus2;
        private System.Windows.Forms.Button buttonPlus1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
    }
}