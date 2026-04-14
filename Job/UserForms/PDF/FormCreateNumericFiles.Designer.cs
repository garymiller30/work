namespace JobSpace.UserForms.PDF
{
    partial class FormCreateNumericFiles
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
            this.objectListView1 = new BrightIdeasSoftware.ObjectListView();
            this.btn_ok = new System.Windows.Forms.Button();
            this.olvColumn_no = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnFileName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.label1 = new System.Windows.Forms.Label();
            this.nud_start_num = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nud_cnt_numbers = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_start_num)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_cnt_numbers)).BeginInit();
            this.SuspendLayout();
            // 
            // objectListView1
            // 
            this.objectListView1.AllColumns.Add(this.olvColumn_no);
            this.objectListView1.AllColumns.Add(this.olvColumnFileName);
            this.objectListView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.objectListView1.CellEditUseWholeCell = false;
            this.objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn_no,
            this.olvColumnFileName});
            this.objectListView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListView1.FullRowSelect = true;
            this.objectListView1.GridLines = true;
            this.objectListView1.HideSelection = false;
            this.objectListView1.Location = new System.Drawing.Point(12, 40);
            this.objectListView1.Name = "objectListView1";
            this.objectListView1.ShowGroups = false;
            this.objectListView1.Size = new System.Drawing.Size(444, 389);
            this.objectListView1.TabIndex = 0;
            this.objectListView1.UseCompatibleStateImageBehavior = false;
            this.objectListView1.View = System.Windows.Forms.View.Details;
            // 
            // btn_ok
            // 
            this.btn_ok.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_ok.Location = new System.Drawing.Point(197, 435);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(75, 38);
            this.btn_ok.TabIndex = 1;
            this.btn_ok.Text = "OK";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // olvColumn_no
            // 
            this.olvColumn_no.Text = "#";
            this.olvColumn_no.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.olvColumn_no.Width = 41;
            // 
            // olvColumnFileName
            // 
            this.olvColumnFileName.AspectName = "Name";
            this.olvColumnFileName.Text = "Файл";
            this.olvColumnFileName.Width = 314;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "починати нумерацію з";
            // 
            // nud_start_num
            // 
            this.nud_start_num.Location = new System.Drawing.Point(136, 7);
            this.nud_start_num.Maximum = new decimal(new int[] {
            1410065407,
            2,
            0,
            0});
            this.nud_start_num.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_start_num.Name = "nud_start_num";
            this.nud_start_num.Size = new System.Drawing.Size(60, 20);
            this.nud_start_num.TabIndex = 3;
            this.nud_start_num.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(212, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "кількість чисел в номері";
            // 
            // nud_cnt_numbers
            // 
            this.nud_cnt_numbers.Location = new System.Drawing.Point(348, 7);
            this.nud_cnt_numbers.Maximum = new decimal(new int[] {
            1410065407,
            2,
            0,
            0});
            this.nud_cnt_numbers.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_cnt_numbers.Name = "nud_cnt_numbers";
            this.nud_cnt_numbers.Size = new System.Drawing.Size(60, 20);
            this.nud_cnt_numbers.TabIndex = 5;
            this.nud_cnt_numbers.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // FormCreateNumericFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 485);
            this.Controls.Add(this.nud_cnt_numbers);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nud_start_num);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.objectListView1);
            this.Name = "FormCreateNumericFiles";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Пронумерувати файли";
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_start_num)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_cnt_numbers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView objectListView1;
        private System.Windows.Forms.Button btn_ok;
        private BrightIdeasSoftware.OLVColumn olvColumn_no;
        private BrightIdeasSoftware.OLVColumn olvColumnFileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nud_start_num;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nud_cnt_numbers;
    }
}