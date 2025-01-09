namespace JobSpace.UserForms.PDF.ImposItems
{
    partial class FormAddTemplatePlate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAddTemplatePlate));
            this.objectListView1 = new BrightIdeasSoftware.ObjectListView();
            this.olvColumnName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tb_name = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.nud_yOfs = new System.Windows.Forms.NumericUpDown();
            this.nud_xOfs = new System.Windows.Forms.NumericUpDown();
            this.nud_h = new System.Windows.Forms.NumericUpDown();
            this.nud_w = new System.Windows.Forms.NumericUpDown();
            this.cb_centerY = new System.Windows.Forms.CheckBox();
            this.cb_centerX = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.bnt_ok = new System.Windows.Forms.Button();
            this.btn_saveToList = new System.Windows.Forms.Button();
            this.btn_delete = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_yOfs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_xOfs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_h)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_w)).BeginInit();
            this.SuspendLayout();
            // 
            // objectListView1
            // 
            this.objectListView1.AllColumns.Add(this.olvColumnName);
            this.objectListView1.CellEditUseWholeCell = false;
            this.objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnName});
            this.objectListView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListView1.FullRowSelect = true;
            this.objectListView1.GridLines = true;
            this.objectListView1.HideSelection = false;
            this.objectListView1.Location = new System.Drawing.Point(12, 12);
            this.objectListView1.Name = "objectListView1";
            this.objectListView1.ShowGroups = false;
            this.objectListView1.Size = new System.Drawing.Size(182, 272);
            this.objectListView1.TabIndex = 0;
            this.objectListView1.UseCompatibleStateImageBehavior = false;
            this.objectListView1.View = System.Windows.Forms.View.Details;
            this.objectListView1.SelectedIndexChanged += new System.EventHandler(this.objectListView1_SelectedIndexChanged);
            // 
            // olvColumnName
            // 
            this.olvColumnName.AspectName = "Name";
            this.olvColumnName.Text = "Назва";
            this.olvColumnName.Width = 118;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tb_name);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.nud_yOfs);
            this.groupBox1.Controls.Add(this.nud_xOfs);
            this.groupBox1.Controls.Add(this.nud_h);
            this.groupBox1.Controls.Add(this.nud_w);
            this.groupBox1.Controls.Add(this.cb_centerY);
            this.groupBox1.Controls.Add(this.cb_centerX);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(200, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(257, 239);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Параметри";
            // 
            // tb_name
            // 
            this.tb_name.Location = new System.Drawing.Point(51, 26);
            this.tb_name.Name = "tb_name";
            this.tb_name.Size = new System.Drawing.Size(200, 20);
            this.tb_name.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "назва";
            // 
            // nud_yOfs
            // 
            this.nud_yOfs.DecimalPlaces = 1;
            this.nud_yOfs.Location = new System.Drawing.Point(154, 180);
            this.nud_yOfs.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nud_yOfs.Name = "nud_yOfs";
            this.nud_yOfs.Size = new System.Drawing.Size(70, 20);
            this.nud_yOfs.TabIndex = 9;
            this.nud_yOfs.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // nud_xOfs
            // 
            this.nud_xOfs.DecimalPlaces = 1;
            this.nud_xOfs.Location = new System.Drawing.Point(35, 180);
            this.nud_xOfs.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nud_xOfs.Name = "nud_xOfs";
            this.nud_xOfs.Size = new System.Drawing.Size(70, 20);
            this.nud_xOfs.TabIndex = 8;
            this.nud_xOfs.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // nud_h
            // 
            this.nud_h.DecimalPlaces = 1;
            this.nud_h.Location = new System.Drawing.Point(154, 85);
            this.nud_h.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nud_h.Name = "nud_h";
            this.nud_h.Size = new System.Drawing.Size(70, 20);
            this.nud_h.TabIndex = 7;
            this.nud_h.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // nud_w
            // 
            this.nud_w.DecimalPlaces = 1;
            this.nud_w.Location = new System.Drawing.Point(35, 85);
            this.nud_w.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nud_w.Name = "nud_w";
            this.nud_w.Size = new System.Drawing.Size(70, 20);
            this.nud_w.TabIndex = 6;
            this.nud_w.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cb_centerY
            // 
            this.cb_centerY.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_centerY.AutoSize = true;
            this.cb_centerY.Location = new System.Drawing.Point(140, 124);
            this.cb_centerY.Name = "cb_centerY";
            this.cb_centerY.Size = new System.Drawing.Size(99, 23);
            this.cb_centerY.TabIndex = 5;
            this.cb_centerY.Text = "центрувати по Y";
            this.cb_centerY.UseVisualStyleBackColor = true;
            // 
            // cb_centerX
            // 
            this.cb_centerX.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_centerX.AutoSize = true;
            this.cb_centerX.Location = new System.Drawing.Point(20, 124);
            this.cb_centerX.Name = "cb_centerX";
            this.cb_centerX.Size = new System.Drawing.Size(99, 23);
            this.cb_centerX.TabIndex = 4;
            this.cb_centerX.Text = "центрувати по X";
            this.cb_centerX.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(166, 164);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Y ofs";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(48, 164);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "X ofs";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(166, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "висота";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "ширина";
            // 
            // bnt_ok
            // 
            this.bnt_ok.Location = new System.Drawing.Point(58, 295);
            this.bnt_ok.Name = "bnt_ok";
            this.bnt_ok.Size = new System.Drawing.Size(90, 32);
            this.bnt_ok.TabIndex = 2;
            this.bnt_ok.Text = "Вибрати";
            this.bnt_ok.UseVisualStyleBackColor = true;
            this.bnt_ok.Click += new System.EventHandler(this.bnt_ok_Click);
            // 
            // btn_saveToList
            // 
            this.btn_saveToList.Location = new System.Drawing.Point(200, 261);
            this.btn_saveToList.Name = "btn_saveToList";
            this.btn_saveToList.Size = new System.Drawing.Size(224, 23);
            this.btn_saveToList.TabIndex = 12;
            this.btn_saveToList.Text = "змінити або додати до списку";
            this.btn_saveToList.UseVisualStyleBackColor = true;
            this.btn_saveToList.Click += new System.EventHandler(this.btn_saveToList_Click);
            // 
            // btn_delete
            // 
            this.btn_delete.Image = ((System.Drawing.Image)(resources.GetObject("btn_delete.Image")));
            this.btn_delete.Location = new System.Drawing.Point(433, 261);
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Size = new System.Drawing.Size(24, 23);
            this.btn_delete.TabIndex = 13;
            this.btn_delete.UseVisualStyleBackColor = true;
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // FormAddTemplatePlate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 339);
            this.Controls.Add(this.btn_delete);
            this.Controls.Add(this.btn_saveToList);
            this.Controls.Add(this.bnt_ok);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.objectListView1);
            this.Name = "FormAddTemplatePlate";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Форми";
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_yOfs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_xOfs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_h)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_w)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView objectListView1;
        private System.Windows.Forms.GroupBox groupBox1;
        private BrightIdeasSoftware.OLVColumn olvColumnName;
        private System.Windows.Forms.Button bnt_ok;
        private System.Windows.Forms.NumericUpDown nud_h;
        private System.Windows.Forms.NumericUpDown nud_w;
        private System.Windows.Forms.CheckBox cb_centerY;
        private System.Windows.Forms.CheckBox cb_centerX;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nud_yOfs;
        private System.Windows.Forms.NumericUpDown nud_xOfs;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_name;
        private System.Windows.Forms.Button btn_saveToList;
        private System.Windows.Forms.Button btn_delete;
    }
}