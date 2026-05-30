namespace JobSpace.UserForms.PDF.Visual
{
    partial class FormPdfSaddleStitchSpreads
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
            this.lblOutputTrimBox = new System.Windows.Forms.Label();
            this.lblInputTrimBox = new System.Windows.Forms.Label();
            this.lblPageCount = new System.Windows.Forms.Label();
            this.lblFileName = new System.Windows.Forms.Label();
            this.nudBleed = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lvSpreads = new System.Windows.Forms.ListView();
            this.chSheet = new System.Windows.Forms.ColumnHeader();
            this.chLeft = new System.Windows.Forms.ColumnHeader();
            this.chRight = new System.Windows.Forms.ColumnHeader();
            this.chOrder = new System.Windows.Forms.ColumnHeader();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.ucPreview = new JobSpace.UC.Uc_FilePreviewControl();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBleed)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.lblOutputTrimBox);
            this.groupBox1.Controls.Add(this.lblInputTrimBox);
            this.groupBox1.Controls.Add(this.lblPageCount);
            this.groupBox1.Controls.Add(this.lblFileName);
            this.groupBox1.Controls.Add(this.nudBleed);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.lvSpreads);
            this.groupBox1.Controls.Add(this.btnOk);
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(252, 559);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Макет";
            // 
            // lblOutputTrimBox
            // 
            this.lblOutputTrimBox.AutoSize = true;
            this.lblOutputTrimBox.Location = new System.Drawing.Point(9, 154);
            this.lblOutputTrimBox.Name = "lblOutputTrimBox";
            this.lblOutputTrimBox.Size = new System.Drawing.Size(10, 13);
            this.lblOutputTrimBox.TabIndex = 10;
            this.lblOutputTrimBox.Text = "-";
            // 
            // lblInputTrimBox
            // 
            this.lblInputTrimBox.AutoSize = true;
            this.lblInputTrimBox.Location = new System.Drawing.Point(9, 86);
            this.lblInputTrimBox.Name = "lblInputTrimBox";
            this.lblInputTrimBox.Size = new System.Drawing.Size(10, 13);
            this.lblInputTrimBox.TabIndex = 9;
            this.lblInputTrimBox.Text = "-";
            // 
            // lblPageCount
            // 
            this.lblPageCount.AutoSize = true;
            this.lblPageCount.Location = new System.Drawing.Point(9, 46);
            this.lblPageCount.Name = "lblPageCount";
            this.lblPageCount.Size = new System.Drawing.Size(10, 13);
            this.lblPageCount.TabIndex = 8;
            this.lblPageCount.Text = "-";
            // 
            // lblFileName
            // 
            this.lblFileName.AutoEllipsis = true;
            this.lblFileName.Location = new System.Drawing.Point(9, 20);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(234, 13);
            this.lblFileName.TabIndex = 7;
            this.lblFileName.Text = "-";
            // 
            // nudBleed
            // 
            this.nudBleed.DecimalPlaces = 1;
            this.nudBleed.Location = new System.Drawing.Point(169, 106);
            this.nudBleed.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudBleed.Name = "nudBleed";
            this.nudBleed.Size = new System.Drawing.Size(74, 20);
            this.nudBleed.TabIndex = 11;
            this.nudBleed.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nudBleed.ValueChanged += new System.EventHandler(this.nudBleed_ValueChanged);
            this.nudBleed.Enter += new System.EventHandler(this.nudBleed_Enter);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 108);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 13);
            this.label10.TabIndex = 12;
            this.label10.Text = "Bleed, мм";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 137);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(122, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "TrimBox вихідного PDF";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 69);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(116, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "TrimBox вхідного PDF";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Кількість сторінок";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 178);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Порядок розворотів";
            // 
            // lvSpreads
            // 
            this.lvSpreads.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lvSpreads.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chSheet,
            this.chLeft,
            this.chRight,
            this.chOrder});
            this.lvSpreads.FullRowSelect = true;
            this.lvSpreads.GridLines = true;
            this.lvSpreads.HideSelection = false;
            this.lvSpreads.Location = new System.Drawing.Point(9, 194);
            this.lvSpreads.MultiSelect = false;
            this.lvSpreads.Name = "lvSpreads";
            this.lvSpreads.Size = new System.Drawing.Size(234, 319);
            this.lvSpreads.TabIndex = 0;
            this.lvSpreads.UseCompatibleStateImageBehavior = false;
            this.lvSpreads.View = System.Windows.Forms.View.Details;
            // 
            // chSheet
            // 
            this.chSheet.Text = "лист";
            this.chSheet.Width = 40;
            // 
            // chLeft
            // 
            this.chLeft.Text = "ліва";
            this.chLeft.Width = 44;
            // 
            // chRight
            // 
            this.chRight.Text = "права";
            this.chRight.Width = 48;
            // 
            // chOrder
            // 
            this.chOrder.Text = "пара";
            this.chOrder.Width = 70;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOk.Location = new System.Drawing.Point(87, 528);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Створити";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Location = new System.Drawing.Point(168, 528);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Скасувати";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ucPreview
            // 
            this.ucPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ucPreview.Location = new System.Drawing.Point(270, 12);
            this.ucPreview.Name = "ucPreview";
            this.ucPreview.Size = new System.Drawing.Size(798, 559);
            this.ucPreview.TabIndex = 1;
            // 
            // FormPdfSaddleStitchSpreads
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1080, 583);
            this.Controls.Add(this.ucPreview);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormPdfSaddleStitchSpreads";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Макет розворотами для друку на скобу";
            this.Shown += new System.EventHandler(this.FormPdfSaddleStitchSpreads_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.nudBleed)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private JobSpace.UC.Uc_FilePreviewControl ucPreview;
        private System.Windows.Forms.ListView lvSpreads;
        private System.Windows.Forms.ColumnHeader chSheet;
        private System.Windows.Forms.ColumnHeader chLeft;
        private System.Windows.Forms.ColumnHeader chRight;
        private System.Windows.Forms.ColumnHeader chOrder;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblOutputTrimBox;
        private System.Windows.Forms.Label lblInputTrimBox;
        private System.Windows.Forms.Label lblPageCount;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.NumericUpDown nudBleed;
        private System.Windows.Forms.Label label10;
    }
}
