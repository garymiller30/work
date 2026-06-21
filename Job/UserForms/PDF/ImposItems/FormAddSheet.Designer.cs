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
            groupBox1 = new System.Windows.Forms.GroupBox();
            nud_Width = new System.Windows.Forms.NumericUpDown();
            label1 = new System.Windows.Forms.Label();
            nud_Height = new System.Windows.Forms.NumericUpDown();
            label2 = new System.Windows.Forms.Label();
            groupBox2 = new System.Windows.Forms.GroupBox();
            nud_FieldRight = new System.Windows.Forms.NumericUpDown();
            label8 = new System.Windows.Forms.Label();
            nud_FieldLeft = new System.Windows.Forms.NumericUpDown();
            label7 = new System.Windows.Forms.Label();
            nud_FieldBottom = new System.Windows.Forms.NumericUpDown();
            label6 = new System.Windows.Forms.Label();
            nud_FileldTop = new System.Windows.Forms.NumericUpDown();
            label5 = new System.Windows.Forms.Label();
            groupBox3 = new System.Windows.Forms.GroupBox();
            nud_ExtraSpace = new System.Windows.Forms.NumericUpDown();
            groupBox4 = new System.Windows.Forms.GroupBox();
            tb_Description = new System.Windows.Forms.TextBox();
            btn_Save = new System.Windows.Forms.Button();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nud_Width).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nud_Height).BeginInit();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nud_FieldRight).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nud_FieldLeft).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nud_FieldBottom).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nud_FileldTop).BeginInit();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nud_ExtraSpace).BeginInit();
            groupBox4.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(nud_Width);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(nud_Height);
            groupBox1.Controls.Add(label2);
            groupBox1.Location = new System.Drawing.Point(14, 61);
            groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Size = new System.Drawing.Size(177, 115);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Формат, мм";
            // 
            // nud_Width
            // 
            nud_Width.DecimalPlaces = 1;
            nud_Width.Location = new System.Drawing.Point(72, 29);
            nud_Width.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            nud_Width.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
            nud_Width.Name = "nud_Width";
            nud_Width.Size = new System.Drawing.Size(86, 23);
            nud_Width.TabIndex = 0;
            nud_Width.Enter += nud_Width_Enter;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 31);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(52, 15);
            label1.TabIndex = 0;
            label1.Text = "Ширина";
            // 
            // nud_Height
            // 
            nud_Height.DecimalPlaces = 1;
            nud_Height.Location = new System.Drawing.Point(72, 60);
            nud_Height.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            nud_Height.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
            nud_Height.Name = "nud_Height";
            nud_Height.Size = new System.Drawing.Size(86, 23);
            nud_Height.TabIndex = 1;
            nud_Height.Enter += nud_Width_Enter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(12, 60);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(45, 15);
            label2.TabIndex = 1;
            label2.Text = "Висота";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(nud_FieldRight);
            groupBox2.Controls.Add(label8);
            groupBox2.Controls.Add(nud_FieldLeft);
            groupBox2.Controls.Add(label7);
            groupBox2.Controls.Add(nud_FieldBottom);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(nud_FileldTop);
            groupBox2.Controls.Add(label5);
            groupBox2.Location = new System.Drawing.Point(198, 61);
            groupBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox2.Size = new System.Drawing.Size(313, 115);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Поля, що не задруковуються, мм";
            // 
            // nud_FieldRight
            // 
            nud_FieldRight.DecimalPlaces = 1;
            nud_FieldRight.Location = new System.Drawing.Point(216, 60);
            nud_FieldRight.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            nud_FieldRight.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
            nud_FieldRight.Name = "nud_FieldRight";
            nud_FieldRight.Size = new System.Drawing.Size(86, 23);
            nud_FieldRight.TabIndex = 1;
            nud_FieldRight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            nud_FieldRight.Enter += nud_Width_Enter;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(229, 42);
            label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(47, 15);
            label8.TabIndex = 12;
            label8.Text = "Справа";
            // 
            // nud_FieldLeft
            // 
            nud_FieldLeft.DecimalPlaces = 1;
            nud_FieldLeft.Location = new System.Drawing.Point(7, 59);
            nud_FieldLeft.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            nud_FieldLeft.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
            nud_FieldLeft.Name = "nud_FieldLeft";
            nud_FieldLeft.Size = new System.Drawing.Size(86, 23);
            nud_FieldLeft.TabIndex = 3;
            nud_FieldLeft.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            nud_FieldLeft.Enter += nud_Width_Enter;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(20, 40);
            label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(36, 15);
            label7.TabIndex = 10;
            label7.Text = "Зліва";
            // 
            // nud_FieldBottom
            // 
            nud_FieldBottom.DecimalPlaces = 1;
            nud_FieldBottom.Location = new System.Drawing.Point(112, 85);
            nud_FieldBottom.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            nud_FieldBottom.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
            nud_FieldBottom.Name = "nud_FieldBottom";
            nud_FieldBottom.Size = new System.Drawing.Size(86, 23);
            nud_FieldBottom.TabIndex = 2;
            nud_FieldBottom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            nud_FieldBottom.Enter += nud_Width_Enter;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(125, 67);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(39, 15);
            label6.TabIndex = 8;
            label6.Text = "Знизу";
            // 
            // nud_FileldTop
            // 
            nud_FileldTop.DecimalPlaces = 1;
            nud_FileldTop.Location = new System.Drawing.Point(112, 37);
            nud_FileldTop.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            nud_FileldTop.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
            nud_FileldTop.Name = "nud_FileldTop";
            nud_FileldTop.Size = new System.Drawing.Size(86, 23);
            nud_FileldTop.TabIndex = 0;
            nud_FileldTop.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            nud_FileldTop.Enter += nud_Width_Enter;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(125, 18);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(45, 15);
            label5.TabIndex = 6;
            label5.Text = "Зверху";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(nud_ExtraSpace);
            groupBox3.Location = new System.Drawing.Point(14, 183);
            groupBox3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox3.Size = new System.Drawing.Size(274, 57);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "Додаткове поле навколо сторінки, мм";
            // 
            // nud_ExtraSpace
            // 
            nud_ExtraSpace.DecimalPlaces = 1;
            nud_ExtraSpace.Location = new System.Drawing.Point(91, 22);
            nud_ExtraSpace.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            nud_ExtraSpace.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
            nud_ExtraSpace.Name = "nud_ExtraSpace";
            nud_ExtraSpace.Size = new System.Drawing.Size(86, 23);
            nud_ExtraSpace.TabIndex = 0;
            nud_ExtraSpace.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            nud_ExtraSpace.Enter += nud_Width_Enter;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(tb_Description);
            groupBox4.Location = new System.Drawing.Point(14, 5);
            groupBox4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox4.Name = "groupBox4";
            groupBox4.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox4.Size = new System.Drawing.Size(497, 50);
            groupBox4.TabIndex = 3;
            groupBox4.TabStop = false;
            groupBox4.Text = "Опис";
            // 
            // tb_Description
            // 
            tb_Description.Dock = System.Windows.Forms.DockStyle.Fill;
            tb_Description.Location = new System.Drawing.Point(4, 19);
            tb_Description.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tb_Description.Name = "tb_Description";
            tb_Description.Size = new System.Drawing.Size(489, 23);
            tb_Description.TabIndex = 0;
            // 
            // btn_Save
            // 
            btn_Save.Location = new System.Drawing.Point(310, 183);
            btn_Save.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btn_Save.Name = "btn_Save";
            btn_Save.Size = new System.Drawing.Size(201, 57);
            btn_Save.TabIndex = 0;
            btn_Save.Text = "Зберегти";
            btn_Save.UseVisualStyleBackColor = true;
            btn_Save.Click += btn_Save_Click;
            // 
            // FormAddSheet
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(519, 246);
            Controls.Add(btn_Save);
            Controls.Add(groupBox4);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormAddSheet";
            ShowIcon = false;
            Text = "Додати лист";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nud_Width).EndInit();
            ((System.ComponentModel.ISupportInitialize)nud_Height).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nud_FieldRight).EndInit();
            ((System.ComponentModel.ISupportInitialize)nud_FieldLeft).EndInit();
            ((System.ComponentModel.ISupportInitialize)nud_FieldBottom).EndInit();
            ((System.ComponentModel.ISupportInitialize)nud_FileldTop).EndInit();
            groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)nud_ExtraSpace).EndInit();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ResumeLayout(false);

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