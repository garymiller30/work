namespace JobSpace.UserForms.PDF
{
    partial class FormCreateBigovkaMarks
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
            buttonCreate = new System.Windows.Forms.Button();
            groupBox1 = new System.Windows.Forms.GroupBox();
            cb_mirrorEven = new System.Windows.Forms.CheckBox();
            radioButtonVer = new System.Windows.Forms.RadioButton();
            radioButtonHor = new System.Windows.Forms.RadioButton();
            groupBox2 = new System.Windows.Forms.GroupBox();
            label1 = new System.Windows.Forms.Label();
            numLen = new System.Windows.Forms.NumericUpDown();
            groupBox3 = new System.Windows.Forms.GroupBox();
            label2 = new System.Windows.Forms.Label();
            numDistanse = new System.Windows.Forms.NumericUpDown();
            groupBox4 = new System.Windows.Forms.GroupBox();
            label3 = new System.Windows.Forms.Label();
            numBleed = new System.Windows.Forms.NumericUpDown();
            groupBox5 = new System.Windows.Forms.GroupBox();
            btn_add_to_center = new System.Windows.Forms.Button();
            textBoxBigovky = new System.Windows.Forms.TextBox();
            groupBox6 = new System.Windows.Forms.GroupBox();
            cb_b = new System.Windows.Forms.CheckBox();
            cb_y = new System.Windows.Forms.CheckBox();
            cb_m = new System.Windows.Forms.CheckBox();
            cb_c = new System.Windows.Forms.CheckBox();
            numK = new System.Windows.Forms.NumericUpDown();
            numY = new System.Windows.Forms.NumericUpDown();
            numM = new System.Windows.Forms.NumericUpDown();
            numC = new System.Windows.Forms.NumericUpDown();
            groupBox7 = new System.Windows.Forms.GroupBox();
            cb_files = new System.Windows.Forms.ComboBox();
            uc_PreviewBrowserFile1 = new JobSpace.UC.Uc_FilePreviewControl();
            btn_3d = new System.Windows.Forms.Button();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numLen).BeginInit();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numDistanse).BeginInit();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numBleed).BeginInit();
            groupBox5.SuspendLayout();
            groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numK).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numY).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numM).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numC).BeginInit();
            groupBox7.SuspendLayout();
            SuspendLayout();
            // 
            // buttonCreate
            // 
            buttonCreate.Location = new System.Drawing.Point(238, 497);
            buttonCreate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonCreate.Name = "buttonCreate";
            buttonCreate.Size = new System.Drawing.Size(106, 40);
            buttonCreate.TabIndex = 0;
            buttonCreate.Text = "Створити";
            buttonCreate.UseVisualStyleBackColor = true;
            buttonCreate.Click += buttonCreate_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(cb_mirrorEven);
            groupBox1.Controls.Add(radioButtonVer);
            groupBox1.Controls.Add(radioButtonHor);
            groupBox1.Location = new System.Drawing.Point(14, 14);
            groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Size = new System.Drawing.Size(349, 118);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Напрямок";
            // 
            // cb_mirrorEven
            // 
            cb_mirrorEven.AutoSize = true;
            cb_mirrorEven.Location = new System.Drawing.Point(37, 48);
            cb_mirrorEven.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cb_mirrorEven.Name = "cb_mirrorEven";
            cb_mirrorEven.Size = new System.Drawing.Size(117, 19);
            cb_mirrorEven.TabIndex = 11;
            cb_mirrorEven.Text = "Дзеркальні поля";
            cb_mirrorEven.UseVisualStyleBackColor = true;
            cb_mirrorEven.CheckedChanged += cb_mirrorEven_CheckedChanged;
            // 
            // radioButtonVer
            // 
            radioButtonVer.AutoSize = true;
            radioButtonVer.Location = new System.Drawing.Point(8, 80);
            radioButtonVer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            radioButtonVer.Name = "radioButtonVer";
            radioButtonVer.Size = new System.Drawing.Size(171, 19);
            radioButtonVer.TabIndex = 1;
            radioButtonVer.Text = "Вертикально (знизу вгору)";
            radioButtonVer.UseVisualStyleBackColor = true;
            radioButtonVer.Click += radioButtonHor_Click;
            // 
            // radioButtonHor
            // 
            radioButtonHor.AutoSize = true;
            radioButtonHor.Checked = true;
            radioButtonHor.Location = new System.Drawing.Point(7, 22);
            radioButtonHor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            radioButtonHor.Name = "radioButtonHor";
            radioButtonHor.Size = new System.Drawing.Size(196, 19);
            radioButtonHor.TabIndex = 0;
            radioButtonHor.TabStop = true;
            radioButtonHor.Text = "Горизонтально (зліва направо)";
            radioButtonHor.UseVisualStyleBackColor = true;
            radioButtonHor.Click += radioButtonHor_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(numLen);
            groupBox2.Location = new System.Drawing.Point(14, 389);
            groupBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox2.Size = new System.Drawing.Size(105, 80);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "довжина лінії";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(68, 36);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(25, 15);
            label1.TabIndex = 1;
            label1.Text = "мм";
            // 
            // numLen
            // 
            numLen.DecimalPlaces = 1;
            numLen.Location = new System.Drawing.Point(12, 33);
            numLen.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            numLen.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numLen.Name = "numLen";
            numLen.Size = new System.Drawing.Size(49, 23);
            numLen.TabIndex = 0;
            numLen.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            numLen.Value = new decimal(new int[] { 2, 0, 0, 0 });
            numLen.Click += numDistanse_Enter;
            numLen.Enter += numDistanse_Enter;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(label2);
            groupBox3.Controls.Add(numDistanse);
            groupBox3.Location = new System.Drawing.Point(178, 299);
            groupBox3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox3.Size = new System.Drawing.Size(184, 80);
            groupBox3.TabIndex = 3;
            groupBox3.TabStop = false;
            groupBox3.Text = "відстань від лінії різу";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(114, 36);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(25, 15);
            label2.TabIndex = 2;
            label2.Text = "мм";
            // 
            // numDistanse
            // 
            numDistanse.DecimalPlaces = 1;
            numDistanse.Location = new System.Drawing.Point(38, 33);
            numDistanse.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            numDistanse.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numDistanse.Name = "numDistanse";
            numDistanse.Size = new System.Drawing.Size(69, 23);
            numDistanse.TabIndex = 0;
            numDistanse.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            numDistanse.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numDistanse.Click += numDistanse_Enter;
            numDistanse.Enter += numDistanse_Enter;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(label3);
            groupBox4.Controls.Add(numBleed);
            groupBox4.Location = new System.Drawing.Point(14, 299);
            groupBox4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox4.Name = "groupBox4";
            groupBox4.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox4.Size = new System.Drawing.Size(158, 80);
            groupBox4.TabIndex = 4;
            groupBox4.TabStop = false;
            groupBox4.Text = "поля на підрізку";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(108, 36);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(25, 15);
            label3.TabIndex = 2;
            label3.Text = "мм";
            // 
            // numBleed
            // 
            numBleed.DecimalPlaces = 1;
            numBleed.Location = new System.Drawing.Point(29, 33);
            numBleed.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            numBleed.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numBleed.Name = "numBleed";
            numBleed.Size = new System.Drawing.Size(71, 23);
            numBleed.TabIndex = 0;
            numBleed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            numBleed.Value = new decimal(new int[] { 2, 0, 0, 0 });
            numBleed.Click += numDistanse_Enter;
            numBleed.Enter += numDistanse_Enter;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(btn_add_to_center);
            groupBox5.Controls.Add(textBoxBigovky);
            groupBox5.Location = new System.Drawing.Point(14, 201);
            groupBox5.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox5.Name = "groupBox5";
            groupBox5.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox5.Size = new System.Drawing.Size(349, 91);
            groupBox5.TabIndex = 5;
            groupBox5.TabStop = false;
            groupBox5.Text = "Біговки (числа через пробіл)";
            // 
            // btn_add_to_center
            // 
            btn_add_to_center.Location = new System.Drawing.Point(127, 51);
            btn_add_to_center.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btn_add_to_center.Name = "btn_add_to_center";
            btn_add_to_center.Size = new System.Drawing.Size(88, 32);
            btn_add_to_center.TabIndex = 1;
            btn_add_to_center.Text = "+ по центру";
            btn_add_to_center.UseVisualStyleBackColor = true;
            btn_add_to_center.Click += btn_add_to_center_Click;
            // 
            // textBoxBigovky
            // 
            textBoxBigovky.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBoxBigovky.Location = new System.Drawing.Point(7, 22);
            textBoxBigovky.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxBigovky.Name = "textBoxBigovky";
            textBoxBigovky.Size = new System.Drawing.Size(333, 23);
            textBoxBigovky.TabIndex = 0;
            textBoxBigovky.TextChanged += textBoxBigovky_TextChanged;
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(cb_b);
            groupBox6.Controls.Add(cb_y);
            groupBox6.Controls.Add(cb_m);
            groupBox6.Controls.Add(cb_c);
            groupBox6.Controls.Add(numK);
            groupBox6.Controls.Add(numY);
            groupBox6.Controls.Add(numM);
            groupBox6.Controls.Add(numC);
            groupBox6.Location = new System.Drawing.Point(126, 385);
            groupBox6.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox6.Name = "groupBox6";
            groupBox6.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox6.Size = new System.Drawing.Size(237, 83);
            groupBox6.TabIndex = 6;
            groupBox6.TabStop = false;
            groupBox6.Text = "Колір";
            // 
            // cb_b
            // 
            cb_b.Appearance = System.Windows.Forms.Appearance.Button;
            cb_b.AutoSize = true;
            cb_b.Location = new System.Drawing.Point(189, 22);
            cb_b.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cb_b.Name = "cb_b";
            cb_b.Size = new System.Drawing.Size(24, 25);
            cb_b.TabIndex = 10;
            cb_b.Text = "K";
            cb_b.UseVisualStyleBackColor = true;
            cb_b.CheckedChanged += cb_b_CheckedChanged;
            // 
            // cb_y
            // 
            cb_y.Appearance = System.Windows.Forms.Appearance.Button;
            cb_y.AutoSize = true;
            cb_y.Location = new System.Drawing.Point(132, 22);
            cb_y.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cb_y.Name = "cb_y";
            cb_y.Size = new System.Drawing.Size(24, 25);
            cb_y.TabIndex = 9;
            cb_y.Text = "Y";
            cb_y.UseVisualStyleBackColor = true;
            cb_y.CheckedChanged += cb_y_CheckedChanged;
            // 
            // cb_m
            // 
            cb_m.Appearance = System.Windows.Forms.Appearance.Button;
            cb_m.AutoSize = true;
            cb_m.Location = new System.Drawing.Point(75, 22);
            cb_m.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cb_m.Name = "cb_m";
            cb_m.Size = new System.Drawing.Size(28, 25);
            cb_m.TabIndex = 8;
            cb_m.Text = "M";
            cb_m.UseVisualStyleBackColor = true;
            cb_m.CheckedChanged += cb_m_CheckedChanged;
            // 
            // cb_c
            // 
            cb_c.Appearance = System.Windows.Forms.Appearance.Button;
            cb_c.AutoSize = true;
            cb_c.Location = new System.Drawing.Point(18, 22);
            cb_c.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cb_c.Name = "cb_c";
            cb_c.Size = new System.Drawing.Size(25, 25);
            cb_c.TabIndex = 7;
            cb_c.Text = "C";
            cb_c.UseVisualStyleBackColor = true;
            cb_c.CheckedChanged += cb_c_CheckedChanged;
            // 
            // numK
            // 
            numK.Location = new System.Drawing.Point(178, 53);
            numK.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            numK.Name = "numK";
            numK.Size = new System.Drawing.Size(50, 23);
            numK.TabIndex = 3;
            numK.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            numK.Click += numDistanse_Enter;
            numK.Enter += numDistanse_Enter;
            // 
            // numY
            // 
            numY.Location = new System.Drawing.Point(121, 53);
            numY.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            numY.Name = "numY";
            numY.Size = new System.Drawing.Size(50, 23);
            numY.TabIndex = 2;
            numY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            numY.Click += numDistanse_Enter;
            numY.Enter += numDistanse_Enter;
            // 
            // numM
            // 
            numM.Location = new System.Drawing.Point(64, 53);
            numM.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            numM.Name = "numM";
            numM.Size = new System.Drawing.Size(50, 23);
            numM.TabIndex = 1;
            numM.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            numM.Click += numDistanse_Enter;
            numM.Enter += numDistanse_Enter;
            // 
            // numC
            // 
            numC.Location = new System.Drawing.Point(7, 53);
            numC.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            numC.Name = "numC";
            numC.Size = new System.Drawing.Size(50, 23);
            numC.TabIndex = 0;
            numC.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            numC.Click += numDistanse_Enter;
            numC.Enter += numDistanse_Enter;
            // 
            // groupBox7
            // 
            groupBox7.Controls.Add(cb_files);
            groupBox7.Location = new System.Drawing.Point(14, 138);
            groupBox7.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox7.Name = "groupBox7";
            groupBox7.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox7.Size = new System.Drawing.Size(349, 55);
            groupBox7.TabIndex = 8;
            groupBox7.TabStop = false;
            groupBox7.Text = "файл";
            // 
            // cb_files
            // 
            cb_files.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cb_files.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cb_files.FormattingEnabled = true;
            cb_files.Location = new System.Drawing.Point(8, 21);
            cb_files.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cb_files.Name = "cb_files";
            cb_files.Size = new System.Drawing.Size(332, 23);
            cb_files.TabIndex = 3;
            cb_files.SelectedIndexChanged += cb_files_SelectedIndexChanged;
            // 
            // uc_PreviewBrowserFile1
            // 
            uc_PreviewBrowserFile1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            uc_PreviewBrowserFile1.Location = new System.Drawing.Point(370, 14);
            uc_PreviewBrowserFile1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            uc_PreviewBrowserFile1.Name = "uc_PreviewBrowserFile1";
            uc_PreviewBrowserFile1.Size = new System.Drawing.Size(533, 531);
            uc_PreviewBrowserFile1.TabIndex = 10;
            // 
            // btn_3d
            // 
            btn_3d.Location = new System.Drawing.Point(125, 497);
            btn_3d.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btn_3d.Name = "btn_3d";
            btn_3d.Size = new System.Drawing.Size(106, 40);
            btn_3d.TabIndex = 11;
            btn_3d.Text = "3D";
            btn_3d.UseVisualStyleBackColor = true;
            btn_3d.Click += btn_3d_Click;
            // 
            // FormCreateBigovkaMarks
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(917, 558);
            Controls.Add(btn_3d);
            Controls.Add(uc_PreviewBrowserFile1);
            Controls.Add(groupBox7);
            Controls.Add(groupBox6);
            Controls.Add(groupBox5);
            Controls.Add(groupBox4);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(buttonCreate);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormCreateBigovkaMarks";
            ShowIcon = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Створити мітки для біговки";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numLen).EndInit();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numDistanse).EndInit();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numBleed).EndInit();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numK).EndInit();
            ((System.ComponentModel.ISupportInitialize)numY).EndInit();
            ((System.ComponentModel.ISupportInitialize)numM).EndInit();
            ((System.ComponentModel.ISupportInitialize)numC).EndInit();
            groupBox7.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCreate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonVer;
        private System.Windows.Forms.RadioButton radioButtonHor;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown numLen;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numDistanse;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numBleed;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox textBoxBigovky;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.NumericUpDown numK;
        private System.Windows.Forms.NumericUpDown numY;
        private System.Windows.Forms.NumericUpDown numM;
        private System.Windows.Forms.NumericUpDown numC;
        private System.Windows.Forms.CheckBox cb_b;
        private System.Windows.Forms.CheckBox cb_y;
        private System.Windows.Forms.CheckBox cb_m;
        private System.Windows.Forms.CheckBox cb_c;
        private System.Windows.Forms.CheckBox cb_mirrorEven;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.ComboBox cb_files;
        private System.Windows.Forms.Button btn_add_to_center;
        private UC.Uc_FilePreviewControl uc_PreviewBrowserFile1;
        private System.Windows.Forms.Button btn_3d;
    }
}
