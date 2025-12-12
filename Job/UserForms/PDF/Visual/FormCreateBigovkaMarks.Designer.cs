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
            this.buttonCreate = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_mirrorEven = new System.Windows.Forms.CheckBox();
            this.radioButtonVer = new System.Windows.Forms.RadioButton();
            this.radioButtonHor = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numLen = new System.Windows.Forms.NumericUpDown();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numDistanse = new System.Windows.Forms.NumericUpDown();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.numBleed = new System.Windows.Forms.NumericUpDown();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btn_add_to_center = new System.Windows.Forms.Button();
            this.textBoxBigovky = new System.Windows.Forms.TextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.cb_b = new System.Windows.Forms.CheckBox();
            this.cb_y = new System.Windows.Forms.CheckBox();
            this.cb_m = new System.Windows.Forms.CheckBox();
            this.cb_c = new System.Windows.Forms.CheckBox();
            this.numK = new System.Windows.Forms.NumericUpDown();
            this.numY = new System.Windows.Forms.NumericUpDown();
            this.numM = new System.Windows.Forms.NumericUpDown();
            this.numC = new System.Windows.Forms.NumericUpDown();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.cb_files = new System.Windows.Forms.ComboBox();
            this.uc_PreviewBrowserFile1 = new JobSpace.UC.Uc_PreviewBrowserFile();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLen)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDistanse)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBleed)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numC)).BeginInit();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCreate
            // 
            this.buttonCreate.Location = new System.Drawing.Point(344, 365);
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Size = new System.Drawing.Size(91, 35);
            this.buttonCreate.TabIndex = 0;
            this.buttonCreate.Text = "Створити";
            this.buttonCreate.UseVisualStyleBackColor = true;
            this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cb_mirrorEven);
            this.groupBox1.Controls.Add(this.radioButtonVer);
            this.groupBox1.Controls.Add(this.radioButtonHor);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(423, 70);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Напрямок";
            // 
            // cb_mirrorEven
            // 
            this.cb_mirrorEven.AutoSize = true;
            this.cb_mirrorEven.Location = new System.Drawing.Point(32, 42);
            this.cb_mirrorEven.Name = "cb_mirrorEven";
            this.cb_mirrorEven.Size = new System.Drawing.Size(112, 17);
            this.cb_mirrorEven.TabIndex = 11;
            this.cb_mirrorEven.Text = "Дзеркальні поля";
            this.cb_mirrorEven.UseVisualStyleBackColor = true;
            this.cb_mirrorEven.CheckedChanged += new System.EventHandler(this.cb_mirrorEven_CheckedChanged);
            // 
            // radioButtonVer
            // 
            this.radioButtonVer.AutoSize = true;
            this.radioButtonVer.Location = new System.Drawing.Point(231, 19);
            this.radioButtonVer.Name = "radioButtonVer";
            this.radioButtonVer.Size = new System.Drawing.Size(160, 17);
            this.radioButtonVer.TabIndex = 1;
            this.radioButtonVer.Text = "Вертикально (знизу вгору)";
            this.radioButtonVer.UseVisualStyleBackColor = true;
            this.radioButtonVer.Click += new System.EventHandler(this.radioButtonHor_Click);
            // 
            // radioButtonHor
            // 
            this.radioButtonHor.AutoSize = true;
            this.radioButtonHor.Checked = true;
            this.radioButtonHor.Location = new System.Drawing.Point(6, 19);
            this.radioButtonHor.Name = "radioButtonHor";
            this.radioButtonHor.Size = new System.Drawing.Size(182, 17);
            this.radioButtonHor.TabIndex = 0;
            this.radioButtonHor.TabStop = true;
            this.radioButtonHor.Text = "Горизонтально (зліва направо)";
            this.radioButtonHor.UseVisualStyleBackColor = true;
            this.radioButtonHor.Click += new System.EventHandler(this.radioButtonHor_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.numLen);
            this.groupBox2.Location = new System.Drawing.Point(298, 259);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(137, 69);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "довжина лінії";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(108, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "мм";
            // 
            // numLen
            // 
            this.numLen.Location = new System.Drawing.Point(6, 29);
            this.numLen.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numLen.Name = "numLen";
            this.numLen.Size = new System.Drawing.Size(99, 20);
            this.numLen.TabIndex = 0;
            this.numLen.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numLen.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numLen.Click += new System.EventHandler(this.numDistanse_Enter);
            this.numLen.Enter += new System.EventHandler(this.numDistanse_Enter);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.numDistanse);
            this.groupBox3.Location = new System.Drawing.Point(155, 259);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(137, 69);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "відстань від лінії різу";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(108, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "мм";
            // 
            // numDistanse
            // 
            this.numDistanse.Location = new System.Drawing.Point(6, 29);
            this.numDistanse.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDistanse.Name = "numDistanse";
            this.numDistanse.Size = new System.Drawing.Size(99, 20);
            this.numDistanse.TabIndex = 0;
            this.numDistanse.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numDistanse.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDistanse.Click += new System.EventHandler(this.numDistanse_Enter);
            this.numDistanse.Enter += new System.EventHandler(this.numDistanse_Enter);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.numBleed);
            this.groupBox4.Location = new System.Drawing.Point(12, 259);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(137, 69);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "поля на підрізку";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(108, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "мм";
            // 
            // numBleed
            // 
            this.numBleed.Location = new System.Drawing.Point(6, 29);
            this.numBleed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numBleed.Name = "numBleed";
            this.numBleed.Size = new System.Drawing.Size(99, 20);
            this.numBleed.TabIndex = 0;
            this.numBleed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numBleed.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numBleed.Click += new System.EventHandler(this.numDistanse_Enter);
            this.numBleed.Enter += new System.EventHandler(this.numDistanse_Enter);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btn_add_to_center);
            this.groupBox5.Controls.Add(this.textBoxBigovky);
            this.groupBox5.Location = new System.Drawing.Point(12, 174);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(423, 79);
            this.groupBox5.TabIndex = 5;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Біговки (числа через пробіл)";
            // 
            // btn_add_to_center
            // 
            this.btn_add_to_center.Location = new System.Drawing.Point(7, 45);
            this.btn_add_to_center.Name = "btn_add_to_center";
            this.btn_add_to_center.Size = new System.Drawing.Size(75, 28);
            this.btn_add_to_center.TabIndex = 1;
            this.btn_add_to_center.Text = "+ по центру";
            this.btn_add_to_center.UseVisualStyleBackColor = true;
            this.btn_add_to_center.Click += new System.EventHandler(this.btn_add_to_center_Click);
            // 
            // textBoxBigovky
            // 
            this.textBoxBigovky.Location = new System.Drawing.Point(6, 19);
            this.textBoxBigovky.Name = "textBoxBigovky";
            this.textBoxBigovky.Size = new System.Drawing.Size(410, 20);
            this.textBoxBigovky.TabIndex = 0;
            this.textBoxBigovky.TextChanged += new System.EventHandler(this.textBoxBigovky_TextChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.cb_b);
            this.groupBox6.Controls.Add(this.cb_y);
            this.groupBox6.Controls.Add(this.cb_m);
            this.groupBox6.Controls.Add(this.cb_c);
            this.groupBox6.Controls.Add(this.numK);
            this.groupBox6.Controls.Add(this.numY);
            this.groupBox6.Controls.Add(this.numM);
            this.groupBox6.Controls.Add(this.numC);
            this.groupBox6.Location = new System.Drawing.Point(12, 334);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(213, 72);
            this.groupBox6.TabIndex = 6;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Колір";
            // 
            // cb_b
            // 
            this.cb_b.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_b.AutoSize = true;
            this.cb_b.Location = new System.Drawing.Point(169, 19);
            this.cb_b.Name = "cb_b";
            this.cb_b.Size = new System.Drawing.Size(24, 23);
            this.cb_b.TabIndex = 10;
            this.cb_b.Text = "K";
            this.cb_b.UseVisualStyleBackColor = true;
            this.cb_b.CheckedChanged += new System.EventHandler(this.cb_b_CheckedChanged);
            // 
            // cb_y
            // 
            this.cb_y.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_y.AutoSize = true;
            this.cb_y.Location = new System.Drawing.Point(120, 19);
            this.cb_y.Name = "cb_y";
            this.cb_y.Size = new System.Drawing.Size(24, 23);
            this.cb_y.TabIndex = 9;
            this.cb_y.Text = "Y";
            this.cb_y.UseVisualStyleBackColor = true;
            this.cb_y.CheckedChanged += new System.EventHandler(this.cb_y_CheckedChanged);
            // 
            // cb_m
            // 
            this.cb_m.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_m.AutoSize = true;
            this.cb_m.Location = new System.Drawing.Point(71, 19);
            this.cb_m.Name = "cb_m";
            this.cb_m.Size = new System.Drawing.Size(26, 23);
            this.cb_m.TabIndex = 8;
            this.cb_m.Text = "M";
            this.cb_m.UseVisualStyleBackColor = true;
            this.cb_m.CheckedChanged += new System.EventHandler(this.cb_m_CheckedChanged);
            // 
            // cb_c
            // 
            this.cb_c.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_c.AutoSize = true;
            this.cb_c.Location = new System.Drawing.Point(22, 19);
            this.cb_c.Name = "cb_c";
            this.cb_c.Size = new System.Drawing.Size(24, 23);
            this.cb_c.TabIndex = 7;
            this.cb_c.Text = "C";
            this.cb_c.UseVisualStyleBackColor = true;
            this.cb_c.CheckedChanged += new System.EventHandler(this.cb_c_CheckedChanged);
            // 
            // numK
            // 
            this.numK.Location = new System.Drawing.Point(160, 46);
            this.numK.Name = "numK";
            this.numK.Size = new System.Drawing.Size(43, 20);
            this.numK.TabIndex = 3;
            this.numK.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numK.Click += new System.EventHandler(this.numDistanse_Enter);
            this.numK.Enter += new System.EventHandler(this.numDistanse_Enter);
            // 
            // numY
            // 
            this.numY.Location = new System.Drawing.Point(111, 46);
            this.numY.Name = "numY";
            this.numY.Size = new System.Drawing.Size(43, 20);
            this.numY.TabIndex = 2;
            this.numY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numY.Click += new System.EventHandler(this.numDistanse_Enter);
            this.numY.Enter += new System.EventHandler(this.numDistanse_Enter);
            // 
            // numM
            // 
            this.numM.Location = new System.Drawing.Point(62, 46);
            this.numM.Name = "numM";
            this.numM.Size = new System.Drawing.Size(43, 20);
            this.numM.TabIndex = 1;
            this.numM.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numM.Click += new System.EventHandler(this.numDistanse_Enter);
            this.numM.Enter += new System.EventHandler(this.numDistanse_Enter);
            // 
            // numC
            // 
            this.numC.Location = new System.Drawing.Point(13, 46);
            this.numC.Name = "numC";
            this.numC.Size = new System.Drawing.Size(43, 20);
            this.numC.TabIndex = 0;
            this.numC.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numC.Click += new System.EventHandler(this.numDistanse_Enter);
            this.numC.Enter += new System.EventHandler(this.numDistanse_Enter);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.cb_files);
            this.groupBox7.Location = new System.Drawing.Point(12, 88);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(290, 48);
            this.groupBox7.TabIndex = 8;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "файл";
            // 
            // cb_files
            // 
            this.cb_files.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_files.FormattingEnabled = true;
            this.cb_files.Location = new System.Drawing.Point(7, 18);
            this.cb_files.Name = "cb_files";
            this.cb_files.Size = new System.Drawing.Size(273, 21);
            this.cb_files.TabIndex = 3;
            this.cb_files.SelectedIndexChanged += new System.EventHandler(this.cb_files_SelectedIndexChanged);
            // 
            // uc_PreviewBrowserFile1
            // 
            this.uc_PreviewBrowserFile1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uc_PreviewBrowserFile1.Location = new System.Drawing.Point(441, 12);
            this.uc_PreviewBrowserFile1.Name = "uc_PreviewBrowserFile1";
            this.uc_PreviewBrowserFile1.Size = new System.Drawing.Size(669, 404);
            this.uc_PreviewBrowserFile1.TabIndex = 10;
            // 
            // FormCreateBigovkaMarks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1122, 428);
            this.Controls.Add(this.uc_PreviewBrowserFile1);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonCreate);
            this.Name = "FormCreateBigovkaMarks";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Створити мітки для біговки";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLen)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDistanse)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBleed)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numC)).EndInit();
            this.groupBox7.ResumeLayout(false);
            this.ResumeLayout(false);

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
        private UC.Uc_PreviewBrowserFile uc_PreviewBrowserFile1;
    }
}