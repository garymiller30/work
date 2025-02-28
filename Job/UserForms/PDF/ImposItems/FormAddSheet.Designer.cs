namespace JobSpace.UserForms.PDF.ImposItems
{
    partial class FormAddSheet
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
            this.nud_Width = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.nud_Height = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.nud_FieldRight = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.nud_FieldLeft = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.nud_FieldBottom = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.nud_FileldTop = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.nud_ExtraSpace = new System.Windows.Forms.NumericUpDown();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tb_Description = new System.Windows.Forms.TextBox();
            this.btn_Save = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Width)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Height)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_FieldRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_FieldLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_FieldBottom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_FileldTop)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_ExtraSpace)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nud_Width);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.nud_Height);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 53);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(152, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Формат, мм";
            // 
            // nud_Width
            // 
            this.nud_Width.DecimalPlaces = 1;
            this.nud_Width.Location = new System.Drawing.Point(62, 25);
            this.nud_Width.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.nud_Width.Name = "nud_Width";
            this.nud_Width.Size = new System.Drawing.Size(74, 20);
            this.nud_Width.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Ширина";
            // 
            // nud_Height
            // 
            this.nud_Height.DecimalPlaces = 1;
            this.nud_Height.Location = new System.Drawing.Point(62, 52);
            this.nud_Height.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.nud_Height.Name = "nud_Height";
            this.nud_Height.Size = new System.Drawing.Size(74, 20);
            this.nud_Height.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Висота";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.nud_FieldRight);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.nud_FieldLeft);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.nud_FieldBottom);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.nud_FileldTop);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(170, 53);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(268, 100);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Поля, що не задруковуються, мм";
            // 
            // nud_FieldRight
            // 
            this.nud_FieldRight.DecimalPlaces = 1;
            this.nud_FieldRight.Location = new System.Drawing.Point(185, 52);
            this.nud_FieldRight.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.nud_FieldRight.Name = "nud_FieldRight";
            this.nud_FieldRight.Size = new System.Drawing.Size(74, 20);
            this.nud_FieldRight.TabIndex = 13;
            this.nud_FieldRight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(196, 36);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "Справа";
            // 
            // nud_FieldLeft
            // 
            this.nud_FieldLeft.DecimalPlaces = 1;
            this.nud_FieldLeft.Location = new System.Drawing.Point(6, 51);
            this.nud_FieldLeft.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.nud_FieldLeft.Name = "nud_FieldLeft";
            this.nud_FieldLeft.Size = new System.Drawing.Size(74, 20);
            this.nud_FieldLeft.TabIndex = 11;
            this.nud_FieldLeft.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 35);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Зліва";
            // 
            // nud_FieldBottom
            // 
            this.nud_FieldBottom.DecimalPlaces = 1;
            this.nud_FieldBottom.Location = new System.Drawing.Point(96, 74);
            this.nud_FieldBottom.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.nud_FieldBottom.Name = "nud_FieldBottom";
            this.nud_FieldBottom.Size = new System.Drawing.Size(74, 20);
            this.nud_FieldBottom.TabIndex = 9;
            this.nud_FieldBottom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(107, 58);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Знизу";
            // 
            // nud_FileldTop
            // 
            this.nud_FileldTop.DecimalPlaces = 1;
            this.nud_FileldTop.Location = new System.Drawing.Point(96, 32);
            this.nud_FileldTop.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.nud_FileldTop.Name = "nud_FileldTop";
            this.nud_FileldTop.Size = new System.Drawing.Size(74, 20);
            this.nud_FileldTop.TabIndex = 7;
            this.nud_FileldTop.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(107, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Зверху";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.nud_ExtraSpace);
            this.groupBox3.Location = new System.Drawing.Point(12, 159);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(235, 49);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Додаткове поле навколо сторінки, мм";
            // 
            // nud_ExtraSpace
            // 
            this.nud_ExtraSpace.DecimalPlaces = 1;
            this.nud_ExtraSpace.Location = new System.Drawing.Point(78, 19);
            this.nud_ExtraSpace.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.nud_ExtraSpace.Name = "nud_ExtraSpace";
            this.nud_ExtraSpace.Size = new System.Drawing.Size(74, 20);
            this.nud_ExtraSpace.TabIndex = 12;
            this.nud_ExtraSpace.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tb_Description);
            this.groupBox4.Location = new System.Drawing.Point(12, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(426, 43);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Опис";
            // 
            // tb_Description
            // 
            this.tb_Description.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_Description.Location = new System.Drawing.Point(3, 16);
            this.tb_Description.Name = "tb_Description";
            this.tb_Description.Size = new System.Drawing.Size(420, 20);
            this.tb_Description.TabIndex = 0;
            // 
            // btn_Save
            // 
            this.btn_Save.Location = new System.Drawing.Point(266, 159);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(172, 49);
            this.btn_Save.TabIndex = 4;
            this.btn_Save.Text = "Зберегти";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // FormAddSheet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 213);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAddSheet";
            this.ShowIcon = false;
            this.Text = "Додати лист";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Width)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Height)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_FieldRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_FieldLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_FieldBottom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_FileldTop)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nud_ExtraSpace)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nud_Width;
        private System.Windows.Forms.NumericUpDown nud_Height;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown nud_FieldRight;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown nud_FieldLeft;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nud_FieldBottom;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nud_FileldTop;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown nud_ExtraSpace;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox tb_Description;
        private System.Windows.Forms.Button btn_Save;
    }
}