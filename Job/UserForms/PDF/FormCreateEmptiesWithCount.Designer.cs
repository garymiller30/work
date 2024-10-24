namespace JobSpace.UserForms
{
    partial class FormCreateEmptiesWithCount
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.nW = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nH = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nCount = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.nMul = new System.Windows.Forms.NumericUpDown();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.objectListView1 = new BrightIdeasSoftware.ObjectListView();
            this.olvColumnW = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnH = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnCnt = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnMul = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.buttonCreate = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nMul)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.flowLayoutPanel1);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(515, 52);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.nW);
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Controls.Add(this.nH);
            this.flowLayoutPanel1.Controls.Add(this.label3);
            this.flowLayoutPanel1.Controls.Add(this.nCount);
            this.flowLayoutPanel1.Controls.Add(this.label4);
            this.flowLayoutPanel1.Controls.Add(this.nMul);
            this.flowLayoutPanel1.Controls.Add(this.buttonAdd);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(509, 33);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "ширина";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nW
            // 
            this.nW.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nW.Location = new System.Drawing.Point(54, 3);
            this.nW.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.nW.Name = "nW";
            this.nW.Size = new System.Drawing.Size(60, 20);
            this.nW.TabIndex = 1;
            this.nW.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nW.Click += new System.EventHandler(this.nW_Enter);
            this.nW.Enter += new System.EventHandler(this.nW_Enter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(120, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 29);
            this.label2.TabIndex = 2;
            this.label2.Text = "висота";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nH
            // 
            this.nH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nH.Location = new System.Drawing.Point(168, 3);
            this.nH.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.nH.Name = "nH";
            this.nH.Size = new System.Drawing.Size(60, 20);
            this.nH.TabIndex = 3;
            this.nH.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nH.Click += new System.EventHandler(this.nW_Enter);
            this.nH.Enter += new System.EventHandler(this.nW_Enter);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(234, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 29);
            this.label3.TabIndex = 4;
            this.label3.Text = "тираж";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nCount
            // 
            this.nCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nCount.Location = new System.Drawing.Point(278, 3);
            this.nCount.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.nCount.Name = "nCount";
            this.nCount.Size = new System.Drawing.Size(60, 20);
            this.nCount.TabIndex = 5;
            this.nCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nCount.Click += new System.EventHandler(this.nW_Enter);
            this.nCount.Enter += new System.EventHandler(this.nW_Enter);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(344, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 29);
            this.label4.TabIndex = 6;
            this.label4.Text = "кількість";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nMul
            // 
            this.nMul.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nMul.Location = new System.Drawing.Point(402, 3);
            this.nMul.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.nMul.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nMul.Name = "nMul";
            this.nMul.Size = new System.Drawing.Size(60, 20);
            this.nMul.TabIndex = 7;
            this.nMul.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nMul.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nMul.Click += new System.EventHandler(this.nW_Enter);
            this.nMul.Enter += new System.EventHandler(this.nW_Enter);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(468, 3);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(38, 23);
            this.buttonAdd.TabIndex = 8;
            this.buttonAdd.Text = "+";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.objectListView1);
            this.groupBox2.Location = new System.Drawing.Point(13, 71);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(515, 233);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Список";
            // 
            // objectListView1
            // 
            this.objectListView1.AllColumns.Add(this.olvColumnW);
            this.objectListView1.AllColumns.Add(this.olvColumnH);
            this.objectListView1.AllColumns.Add(this.olvColumnCnt);
            this.objectListView1.AllColumns.Add(this.olvColumnMul);
            this.objectListView1.CellEditUseWholeCell = false;
            this.objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnW,
            this.olvColumnH,
            this.olvColumnCnt,
            this.olvColumnMul});
            this.objectListView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectListView1.FullRowSelect = true;
            this.objectListView1.GridLines = true;
            this.objectListView1.HideSelection = false;
            this.objectListView1.Location = new System.Drawing.Point(3, 16);
            this.objectListView1.Name = "objectListView1";
            this.objectListView1.ShowGroups = false;
            this.objectListView1.Size = new System.Drawing.Size(509, 214);
            this.objectListView1.TabIndex = 0;
            this.objectListView1.UseCompatibleStateImageBehavior = false;
            this.objectListView1.UseNotifyPropertyChanged = true;
            this.objectListView1.View = System.Windows.Forms.View.Details;
            this.objectListView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.objectListView1_KeyDown);
            // 
            // olvColumnW
            // 
            this.olvColumnW.AspectName = "Width";
            this.olvColumnW.Text = "ширина";
            // 
            // olvColumnH
            // 
            this.olvColumnH.AspectName = "Height";
            this.olvColumnH.Text = "висота";
            // 
            // olvColumnCnt
            // 
            this.olvColumnCnt.AspectName = "Count";
            this.olvColumnCnt.Text = "тираж";
            // 
            // olvColumnMul
            // 
            this.olvColumnMul.AspectName = "Multiplier";
            this.olvColumnMul.Text = "кількість";
            // 
            // buttonCreate
            // 
            this.buttonCreate.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonCreate.Location = new System.Drawing.Point(225, 310);
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Size = new System.Drawing.Size(90, 35);
            this.buttonCreate.TabIndex = 2;
            this.buttonCreate.Text = "Створити";
            this.buttonCreate.UseVisualStyleBackColor = true;
            this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
            // 
            // FormCreateEmptiesWithCount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 357);
            this.Controls.Add(this.buttonCreate);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormCreateEmptiesWithCount";
            this.ShowIcon = false;
            this.Text = "Створити пустишки з тиражами";
            this.groupBox1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nMul)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonCreate;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private BrightIdeasSoftware.ObjectListView objectListView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nW;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nH;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nCount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nMul;
        private System.Windows.Forms.Button buttonAdd;
        private BrightIdeasSoftware.OLVColumn olvColumnW;
        private BrightIdeasSoftware.OLVColumn olvColumnH;
        private BrightIdeasSoftware.OLVColumn olvColumnCnt;
        private BrightIdeasSoftware.OLVColumn olvColumnMul;
    }
}