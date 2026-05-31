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
            groupBox1 = new System.Windows.Forms.GroupBox();
            flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            label1 = new System.Windows.Forms.Label();
            nW = new System.Windows.Forms.NumericUpDown();
            label2 = new System.Windows.Forms.Label();
            nH = new System.Windows.Forms.NumericUpDown();
            label3 = new System.Windows.Forms.Label();
            nCount = new System.Windows.Forms.NumericUpDown();
            label4 = new System.Windows.Forms.Label();
            nMul = new System.Windows.Forms.NumericUpDown();
            buttonAdd = new System.Windows.Forms.Button();
            groupBox2 = new System.Windows.Forms.GroupBox();
            objectListView1 = new BrightIdeasSoftware.ObjectListView();
            olvColumnW = new BrightIdeasSoftware.OLVColumn();
            olvColumnH = new BrightIdeasSoftware.OLVColumn();
            olvColumnCnt = new BrightIdeasSoftware.OLVColumn();
            olvColumnMul = new BrightIdeasSoftware.OLVColumn();
            buttonCreate = new System.Windows.Forms.Button();
            bnt_import = new System.Windows.Forms.Button();
            groupBox1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nW).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nH).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nCount).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nMul).BeginInit();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)objectListView1).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            groupBox1.Controls.Add(flowLayoutPanel1);
            groupBox1.Location = new System.Drawing.Point(15, 15);
            groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Size = new System.Drawing.Size(769, 60);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(label1);
            flowLayoutPanel1.Controls.Add(nW);
            flowLayoutPanel1.Controls.Add(label2);
            flowLayoutPanel1.Controls.Add(nH);
            flowLayoutPanel1.Controls.Add(label3);
            flowLayoutPanel1.Controls.Add(nCount);
            flowLayoutPanel1.Controls.Add(label4);
            flowLayoutPanel1.Controls.Add(nMul);
            flowLayoutPanel1.Controls.Add(buttonAdd);
            flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            flowLayoutPanel1.Location = new System.Drawing.Point(4, 19);
            flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new System.Drawing.Size(761, 38);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = System.Windows.Forms.DockStyle.Fill;
            label1.Location = new System.Drawing.Point(4, 0);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(52, 33);
            label1.TabIndex = 0;
            label1.Text = "ширина";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nW
            // 
            nW.Dock = System.Windows.Forms.DockStyle.Fill;
            nW.Location = new System.Drawing.Point(64, 3);
            nW.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            nW.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
            nW.Name = "nW";
            nW.Size = new System.Drawing.Size(70, 23);
            nW.TabIndex = 1;
            nW.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            nW.Click += nW_Enter;
            nW.Enter += nW_Enter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = System.Windows.Forms.DockStyle.Fill;
            label2.Location = new System.Drawing.Point(142, 0);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(44, 33);
            label2.TabIndex = 2;
            label2.Text = "висота";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nH
            // 
            nH.Dock = System.Windows.Forms.DockStyle.Fill;
            nH.Location = new System.Drawing.Point(194, 3);
            nH.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            nH.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
            nH.Name = "nH";
            nH.Size = new System.Drawing.Size(70, 23);
            nH.TabIndex = 3;
            nH.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            nH.Click += nW_Enter;
            nH.Enter += nW_Enter;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = System.Windows.Forms.DockStyle.Fill;
            label3.Location = new System.Drawing.Point(272, 0);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(41, 33);
            label3.TabIndex = 4;
            label3.Text = "тираж";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nCount
            // 
            nCount.Dock = System.Windows.Forms.DockStyle.Fill;
            nCount.Location = new System.Drawing.Point(321, 3);
            nCount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            nCount.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
            nCount.Name = "nCount";
            nCount.Size = new System.Drawing.Size(70, 23);
            nCount.TabIndex = 5;
            nCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            nCount.Click += nW_Enter;
            nCount.Enter += nW_Enter;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = System.Windows.Forms.DockStyle.Fill;
            label4.Location = new System.Drawing.Point(399, 0);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(55, 33);
            label4.TabIndex = 6;
            label4.Text = "кількість";
            label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nMul
            // 
            nMul.Dock = System.Windows.Forms.DockStyle.Fill;
            nMul.Location = new System.Drawing.Point(462, 3);
            nMul.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            nMul.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            nMul.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nMul.Name = "nMul";
            nMul.Size = new System.Drawing.Size(70, 23);
            nMul.TabIndex = 7;
            nMul.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            nMul.Value = new decimal(new int[] { 1, 0, 0, 0 });
            nMul.Click += nW_Enter;
            nMul.Enter += nW_Enter;
            // 
            // buttonAdd
            // 
            buttonAdd.AutoSize = true;
            buttonAdd.Location = new System.Drawing.Point(540, 3);
            buttonAdd.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new System.Drawing.Size(113, 27);
            buttonAdd.TabIndex = 8;
            buttonAdd.Text = "+додати в список";
            buttonAdd.UseVisualStyleBackColor = true;
            buttonAdd.Click += buttonAdd_Click;
            // 
            // groupBox2
            // 
            groupBox2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            groupBox2.Controls.Add(objectListView1);
            groupBox2.Location = new System.Drawing.Point(15, 82);
            groupBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox2.Size = new System.Drawing.Size(769, 269);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Список";
            // 
            // objectListView1
            // 
            objectListView1.AllColumns.Add(olvColumnW);
            objectListView1.AllColumns.Add(olvColumnH);
            objectListView1.AllColumns.Add(olvColumnCnt);
            objectListView1.AllColumns.Add(olvColumnMul);
            objectListView1.CellEditUseWholeCell = false;
            objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { olvColumnW, olvColumnH, olvColumnCnt, olvColumnMul });
            objectListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            objectListView1.FullRowSelect = true;
            objectListView1.GridLines = true;
            objectListView1.Location = new System.Drawing.Point(4, 19);
            objectListView1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            objectListView1.Name = "objectListView1";
            objectListView1.ShowGroups = false;
            objectListView1.Size = new System.Drawing.Size(761, 247);
            objectListView1.TabIndex = 0;
            objectListView1.UseCompatibleStateImageBehavior = false;
            objectListView1.UseNotifyPropertyChanged = true;
            objectListView1.View = System.Windows.Forms.View.Details;
            objectListView1.KeyDown += objectListView1_KeyDown;
            // 
            // olvColumnW
            // 
            olvColumnW.AspectName = "Width";
            olvColumnW.Text = "ширина";
            // 
            // olvColumnH
            // 
            olvColumnH.AspectName = "Height";
            olvColumnH.Text = "висота";
            // 
            // olvColumnCnt
            // 
            olvColumnCnt.AspectName = "Count";
            olvColumnCnt.Text = "тираж";
            olvColumnCnt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // olvColumnMul
            // 
            olvColumnMul.AspectName = "Multiplier";
            olvColumnMul.Text = "кількість";
            olvColumnMul.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonCreate
            // 
            buttonCreate.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            buttonCreate.Location = new System.Drawing.Point(595, 358);
            buttonCreate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonCreate.Name = "buttonCreate";
            buttonCreate.Size = new System.Drawing.Size(105, 40);
            buttonCreate.TabIndex = 2;
            buttonCreate.Text = "Створити";
            buttonCreate.UseVisualStyleBackColor = true;
            buttonCreate.Click += buttonCreate_Click;
            // 
            // bnt_import
            // 
            bnt_import.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            bnt_import.Location = new System.Drawing.Point(19, 358);
            bnt_import.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            bnt_import.Name = "bnt_import";
            bnt_import.Size = new System.Drawing.Size(155, 40);
            bnt_import.TabIndex = 3;
            bnt_import.Text = "Імпортувати список";
            bnt_import.UseVisualStyleBackColor = true;
            bnt_import.Click += bnt_import_Click;
            // 
            // FormCreateEmptiesWithCount
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(798, 412);
            Controls.Add(bnt_import);
            Controls.Add(buttonCreate);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormCreateEmptiesWithCount";
            ShowIcon = false;
            Text = "Створити пустишки з тиражами";
            groupBox1.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nW).EndInit();
            ((System.ComponentModel.ISupportInitialize)nH).EndInit();
            ((System.ComponentModel.ISupportInitialize)nCount).EndInit();
            ((System.ComponentModel.ISupportInitialize)nMul).EndInit();
            groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)objectListView1).EndInit();
            ResumeLayout(false);

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
        private System.Windows.Forms.Button bnt_import;
    }
}