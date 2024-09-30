namespace JobSpace.UserForms
{
    partial class FormSelectPdfNewSize
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSelectPdfNewSize));
            this.numericUpDownWidth = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDownHeight = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownBleed = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonNonProportial = new System.Windows.Forms.RadioButton();
            this.radioButtonProportial = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButtonTrimbox = new System.Windows.Forms.RadioButton();
            this.radioButtonMediabox = new System.Windows.Forms.RadioButton();
            this.buttonOk = new System.Windows.Forms.Button();
            this.ucSelectStandartPageFormat1 = new UC.UcSelectStandartPageFormat();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label8 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxWorH = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDownVal = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.labelWorH = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonGarazd = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBleed)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVal)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // numericUpDownWidth
            // 
            this.numericUpDownWidth.DecimalPlaces = 1;
            this.numericUpDownWidth.Location = new System.Drawing.Point(14, 44);
            this.numericUpDownWidth.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numericUpDownWidth.Name = "numericUpDownWidth";
            this.numericUpDownWidth.Size = new System.Drawing.Size(70, 20);
            this.numericUpDownWidth.TabIndex = 0;
            this.numericUpDownWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownWidth.Value = new decimal(new int[] {
            210,
            0,
            0,
            0});
            this.numericUpDownWidth.Enter += new System.EventHandler(this.numericUpDownWidth_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Ширина";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(120, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Висота";
            // 
            // numericUpDownHeight
            // 
            this.numericUpDownHeight.DecimalPlaces = 1;
            this.numericUpDownHeight.Location = new System.Drawing.Point(123, 44);
            this.numericUpDownHeight.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numericUpDownHeight.Name = "numericUpDownHeight";
            this.numericUpDownHeight.Size = new System.Drawing.Size(70, 20);
            this.numericUpDownHeight.TabIndex = 3;
            this.numericUpDownHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownHeight.Value = new decimal(new int[] {
            297,
            0,
            0,
            0});
            this.numericUpDownHeight.Enter += new System.EventHandler(this.numericUpDownWidth_Enter);
            // 
            // numericUpDownBleed
            // 
            this.numericUpDownBleed.DecimalPlaces = 1;
            this.numericUpDownBleed.Location = new System.Drawing.Point(245, 44);
            this.numericUpDownBleed.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numericUpDownBleed.Name = "numericUpDownBleed";
            this.numericUpDownBleed.Size = new System.Drawing.Size(47, 20);
            this.numericUpDownBleed.TabIndex = 5;
            this.numericUpDownBleed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownBleed.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDownBleed.Enter += new System.EventHandler(this.numericUpDownWidth_Enter);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(206, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Поля на підрізку";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonNonProportial);
            this.groupBox1.Controls.Add(this.radioButtonProportial);
            this.groupBox1.Location = new System.Drawing.Point(98, 187);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(132, 69);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Як збільшуємо?";
            // 
            // radioButtonNonProportial
            // 
            this.radioButtonNonProportial.AutoSize = true;
            this.radioButtonNonProportial.Location = new System.Drawing.Point(6, 42);
            this.radioButtonNonProportial.Name = "radioButtonNonProportial";
            this.radioButtonNonProportial.Size = new System.Drawing.Size(101, 17);
            this.radioButtonNonProportial.TabIndex = 1;
            this.radioButtonNonProportial.Text = "Непропорційно";
            this.radioButtonNonProportial.UseVisualStyleBackColor = true;
            // 
            // radioButtonProportial
            // 
            this.radioButtonProportial.AutoSize = true;
            this.radioButtonProportial.Checked = true;
            this.radioButtonProportial.Location = new System.Drawing.Point(6, 19);
            this.radioButtonProportial.Name = "radioButtonProportial";
            this.radioButtonProportial.Size = new System.Drawing.Size(89, 17);
            this.radioButtonProportial.TabIndex = 0;
            this.radioButtonProportial.TabStop = true;
            this.radioButtonProportial.Text = "Пропорційно";
            this.radioButtonProportial.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButtonTrimbox);
            this.groupBox2.Controls.Add(this.radioButtonMediabox);
            this.groupBox2.Location = new System.Drawing.Point(246, 187);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(129, 69);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Відносно чого?";
            // 
            // radioButtonTrimbox
            // 
            this.radioButtonTrimbox.AutoSize = true;
            this.radioButtonTrimbox.Checked = true;
            this.radioButtonTrimbox.Location = new System.Drawing.Point(6, 42);
            this.radioButtonTrimbox.Name = "radioButtonTrimbox";
            this.radioButtonTrimbox.Size = new System.Drawing.Size(62, 17);
            this.radioButtonTrimbox.TabIndex = 3;
            this.radioButtonTrimbox.TabStop = true;
            this.radioButtonTrimbox.Text = "Trimbox";
            this.radioButtonTrimbox.UseVisualStyleBackColor = true;
            // 
            // radioButtonMediabox
            // 
            this.radioButtonMediabox.AutoSize = true;
            this.radioButtonMediabox.Location = new System.Drawing.Point(6, 19);
            this.radioButtonMediabox.Name = "radioButtonMediabox";
            this.radioButtonMediabox.Size = new System.Drawing.Size(71, 17);
            this.radioButtonMediabox.TabIndex = 2;
            this.radioButtonMediabox.Text = "Mediabox";
            this.radioButtonMediabox.UseVisualStyleBackColor = true;
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(98, 283);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(277, 39);
            this.buttonOk.TabIndex = 8;
            this.buttonOk.Text = "ЗМІНИТИ ФОРМАТ";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.button1_Click);
            // 
            // ucSelectStandartPageFormat1
            // 
            this.ucSelectStandartPageFormat1.AutoSize = true;
            this.ucSelectStandartPageFormat1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ucSelectStandartPageFormat1.Location = new System.Drawing.Point(17, 21);
            this.ucSelectStandartPageFormat1.Name = "ucSelectStandartPageFormat1";
            this.ucSelectStandartPageFormat1.Size = new System.Drawing.Size(112, 48);
            this.ucSelectStandartPageFormat1.TabIndex = 9;
            this.ucSelectStandartPageFormat1.PaperFormatChanged += new System.EventHandler<Static.Pdf.Common.PaperFormat>(this.ucSelectStandartPageFormat1_PaperFormatChanged);
            // 
            // button1
            // 
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.Location = new System.Drawing.Point(92, 41);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(23, 23);
            this.button1.TabIndex = 10;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.flowLayoutPanel1);
            this.groupBox3.Location = new System.Drawing.Point(12, 98);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(465, 71);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Порахувати";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.label8);
            this.flowLayoutPanel1.Controls.Add(this.label4);
            this.flowLayoutPanel1.Controls.Add(this.comboBoxWorH);
            this.flowLayoutPanel1.Controls.Add(this.label6);
            this.flowLayoutPanel1.Controls.Add(this.numericUpDownVal);
            this.flowLayoutPanel1.Controls.Add(this.label5);
            this.flowLayoutPanel1.Controls.Add(this.labelWorH);
            this.flowLayoutPanel1.Controls.Add(this.label7);
            this.flowLayoutPanel1.Controls.Add(this.textBox1);
            this.flowLayoutPanel1.Controls.Add(this.buttonGarazd);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(459, 52);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.Dock = System.Windows.Forms.DockStyle.Top;
            this.label8.Location = new System.Drawing.Point(3, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(453, 23);
            this.label8.TabIndex = 9;
            this.label8.Text = "Перед тим як рахувати, потрібно виставити поточну ширину і висоту документа";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(3, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 23);
            this.label4.TabIndex = 0;
            this.label4.Text = "Якщо";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxWorH
            // 
            this.comboBoxWorH.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxWorH.FormattingEnabled = true;
            this.comboBoxWorH.Items.AddRange(new object[] {
            "ширина",
            "висота"});
            this.comboBoxWorH.Location = new System.Drawing.Point(45, 26);
            this.comboBoxWorH.Name = "comboBoxWorH";
            this.comboBoxWorH.Size = new System.Drawing.Size(67, 21);
            this.comboBoxWorH.TabIndex = 1;
            this.comboBoxWorH.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(118, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(13, 23);
            this.label6.TabIndex = 5;
            this.label6.Text = "=";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numericUpDownVal
            // 
            this.numericUpDownVal.DecimalPlaces = 1;
            this.numericUpDownVal.Location = new System.Drawing.Point(137, 26);
            this.numericUpDownVal.Maximum = new decimal(new int[] {
            276447231,
            23283,
            0,
            0});
            this.numericUpDownVal.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownVal.Name = "numericUpDownVal";
            this.numericUpDownVal.Size = new System.Drawing.Size(61, 20);
            this.numericUpDownVal.TabIndex = 6;
            this.numericUpDownVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownVal.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownVal.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(204, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(18, 23);
            this.label5.TabIndex = 2;
            this.label5.Text = "то";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelWorH
            // 
            this.labelWorH.Location = new System.Drawing.Point(228, 23);
            this.labelWorH.Name = "labelWorH";
            this.labelWorH.Size = new System.Drawing.Size(61, 23);
            this.labelWorH.TabIndex = 3;
            this.labelWorH.Text = "label6";
            this.labelWorH.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(295, 23);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(13, 23);
            this.label7.TabIndex = 7;
            this.label7.Text = "=";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(314, 26);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(61, 20);
            this.textBox1.TabIndex = 4;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonGarazd
            // 
            this.buttonGarazd.Location = new System.Drawing.Point(381, 26);
            this.buttonGarazd.Name = "buttonGarazd";
            this.buttonGarazd.Size = new System.Drawing.Size(74, 23);
            this.buttonGarazd.TabIndex = 8;
            this.buttonGarazd.Text = "влаштовує";
            this.buttonGarazd.UseVisualStyleBackColor = true;
            this.buttonGarazd.Click += new System.EventHandler(this.buttonGarazd_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.ucSelectStandartPageFormat1);
            this.groupBox4.Location = new System.Drawing.Point(331, 8);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(146, 84);
            this.groupBox4.TabIndex = 12;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Вибрати зі стандартних";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.numericUpDownWidth);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.button1);
            this.groupBox5.Controls.Add(this.numericUpDownHeight);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.numericUpDownBleed);
            this.groupBox5.Location = new System.Drawing.Point(12, 8);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(302, 84);
            this.groupBox5.TabIndex = 13;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Новий формат документа";
            // 
            // FormSelectPdfNewSize
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 344);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSelectPdfNewSize";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Змінити формат документа";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBleed)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVal)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numericUpDownWidth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownHeight;
        private System.Windows.Forms.NumericUpDown numericUpDownBleed;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonNonProportial;
        private System.Windows.Forms.RadioButton radioButtonProportial;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButtonTrimbox;
        private System.Windows.Forms.RadioButton radioButtonMediabox;
        private System.Windows.Forms.Button buttonOk;
        private UC.UcSelectStandartPageFormat ucSelectStandartPageFormat1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxWorH;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDownVal;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelWorH;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button buttonGarazd;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label8;
    }
}