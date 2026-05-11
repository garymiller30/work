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
            numericUpDownWidth = new System.Windows.Forms.NumericUpDown();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            numericUpDownHeight = new System.Windows.Forms.NumericUpDown();
            numericUpDownBleed = new System.Windows.Forms.NumericUpDown();
            label3 = new System.Windows.Forms.Label();
            groupBox1 = new System.Windows.Forms.GroupBox();
            radioButtonNonProportial = new System.Windows.Forms.RadioButton();
            radioButtonProportial = new System.Windows.Forms.RadioButton();
            groupBox2 = new System.Windows.Forms.GroupBox();
            radioButtonTrimbox = new System.Windows.Forms.RadioButton();
            radioButtonMediabox = new System.Windows.Forms.RadioButton();
            buttonOk = new System.Windows.Forms.Button();
            ucSelectStandartPageFormat1 = new JobSpace.UC.UcSelectStandartPageFormat();
            button1 = new System.Windows.Forms.Button();
            groupBox3 = new System.Windows.Forms.GroupBox();
            flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            label8 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            comboBoxWorH = new System.Windows.Forms.ComboBox();
            label6 = new System.Windows.Forms.Label();
            numericUpDownVal = new System.Windows.Forms.NumericUpDown();
            label5 = new System.Windows.Forms.Label();
            labelWorH = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            textBox1 = new System.Windows.Forms.TextBox();
            buttonGarazd = new System.Windows.Forms.Button();
            groupBox4 = new System.Windows.Forms.GroupBox();
            groupBox5 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)numericUpDownWidth).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownHeight).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownBleed).BeginInit();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownVal).BeginInit();
            groupBox4.SuspendLayout();
            groupBox5.SuspendLayout();
            SuspendLayout();
            // 
            // numericUpDownWidth
            // 
            numericUpDownWidth.DecimalPlaces = 1;
            numericUpDownWidth.Location = new System.Drawing.Point(16, 51);
            numericUpDownWidth.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            numericUpDownWidth.Maximum = new decimal(new int[] { 9999999, 0, 0, 0 });
            numericUpDownWidth.Name = "numericUpDownWidth";
            numericUpDownWidth.Size = new System.Drawing.Size(82, 23);
            numericUpDownWidth.TabIndex = 0;
            numericUpDownWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            numericUpDownWidth.Value = new decimal(new int[] { 210, 0, 0, 0 });
            numericUpDownWidth.Click += numericUpDownWidth_Enter;
            numericUpDownWidth.Enter += numericUpDownWidth_Enter;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(18, 30);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(52, 15);
            label1.TabIndex = 1;
            label1.Text = "Ширина";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(140, 30);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(45, 15);
            label2.TabIndex = 2;
            label2.Text = "Висота";
            // 
            // numericUpDownHeight
            // 
            numericUpDownHeight.DecimalPlaces = 1;
            numericUpDownHeight.Location = new System.Drawing.Point(144, 51);
            numericUpDownHeight.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            numericUpDownHeight.Maximum = new decimal(new int[] { 9999999, 0, 0, 0 });
            numericUpDownHeight.Name = "numericUpDownHeight";
            numericUpDownHeight.Size = new System.Drawing.Size(82, 23);
            numericUpDownHeight.TabIndex = 3;
            numericUpDownHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            numericUpDownHeight.Value = new decimal(new int[] { 297, 0, 0, 0 });
            numericUpDownHeight.Click += numericUpDownWidth_Enter;
            numericUpDownHeight.Enter += numericUpDownWidth_Enter;
            // 
            // numericUpDownBleed
            // 
            numericUpDownBleed.DecimalPlaces = 1;
            numericUpDownBleed.Location = new System.Drawing.Point(246, 51);
            numericUpDownBleed.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            numericUpDownBleed.Maximum = new decimal(new int[] { 9999999, 0, 0, 0 });
            numericUpDownBleed.Name = "numericUpDownBleed";
            numericUpDownBleed.Size = new System.Drawing.Size(55, 23);
            numericUpDownBleed.TabIndex = 5;
            numericUpDownBleed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            numericUpDownBleed.Value = new decimal(new int[] { 2, 0, 0, 0 });
            numericUpDownBleed.Click += numericUpDownWidth_Enter;
            numericUpDownBleed.Enter += numericUpDownWidth_Enter;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(240, 30);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(98, 15);
            label3.TabIndex = 4;
            label3.Text = "Поля на підрізку";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(radioButtonNonProportial);
            groupBox1.Controls.Add(radioButtonProportial);
            groupBox1.Location = new System.Drawing.Point(114, 216);
            groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Size = new System.Drawing.Size(154, 80);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            groupBox1.Text = "Як збільшуємо?";
            // 
            // radioButtonNonProportial
            // 
            radioButtonNonProportial.AutoSize = true;
            radioButtonNonProportial.Location = new System.Drawing.Point(7, 48);
            radioButtonNonProportial.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            radioButtonNonProportial.Name = "radioButtonNonProportial";
            radioButtonNonProportial.Size = new System.Drawing.Size(113, 19);
            radioButtonNonProportial.TabIndex = 1;
            radioButtonNonProportial.Text = "Непропорційно";
            radioButtonNonProportial.UseVisualStyleBackColor = true;
            // 
            // radioButtonProportial
            // 
            radioButtonProportial.AutoSize = true;
            radioButtonProportial.Checked = true;
            radioButtonProportial.Location = new System.Drawing.Point(7, 22);
            radioButtonProportial.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            radioButtonProportial.Name = "radioButtonProportial";
            radioButtonProportial.Size = new System.Drawing.Size(100, 19);
            radioButtonProportial.TabIndex = 0;
            radioButtonProportial.TabStop = true;
            radioButtonProportial.Text = "Пропорційно";
            radioButtonProportial.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(radioButtonTrimbox);
            groupBox2.Controls.Add(radioButtonMediabox);
            groupBox2.Location = new System.Drawing.Point(287, 216);
            groupBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox2.Size = new System.Drawing.Size(150, 80);
            groupBox2.TabIndex = 7;
            groupBox2.TabStop = false;
            groupBox2.Text = "Відносно чого?";
            // 
            // radioButtonTrimbox
            // 
            radioButtonTrimbox.AutoSize = true;
            radioButtonTrimbox.Checked = true;
            radioButtonTrimbox.Location = new System.Drawing.Point(7, 48);
            radioButtonTrimbox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            radioButtonTrimbox.Name = "radioButtonTrimbox";
            radioButtonTrimbox.Size = new System.Drawing.Size(68, 19);
            radioButtonTrimbox.TabIndex = 3;
            radioButtonTrimbox.TabStop = true;
            radioButtonTrimbox.Text = "Trimbox";
            radioButtonTrimbox.UseVisualStyleBackColor = true;
            // 
            // radioButtonMediabox
            // 
            radioButtonMediabox.AutoSize = true;
            radioButtonMediabox.Location = new System.Drawing.Point(7, 22);
            radioButtonMediabox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            radioButtonMediabox.Name = "radioButtonMediabox";
            radioButtonMediabox.Size = new System.Drawing.Size(78, 19);
            radioButtonMediabox.TabIndex = 2;
            radioButtonMediabox.Text = "Mediabox";
            radioButtonMediabox.UseVisualStyleBackColor = true;
            // 
            // buttonOk
            // 
            buttonOk.Location = new System.Drawing.Point(114, 327);
            buttonOk.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonOk.Name = "buttonOk";
            buttonOk.Size = new System.Drawing.Size(323, 45);
            buttonOk.TabIndex = 8;
            buttonOk.Text = "ЗМІНИТИ ФОРМАТ";
            buttonOk.UseVisualStyleBackColor = true;
            buttonOk.Click += button1_Click;
            // 
            // ucSelectStandartPageFormat1
            // 
            ucSelectStandartPageFormat1.AutoSize = true;
            ucSelectStandartPageFormat1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            ucSelectStandartPageFormat1.Location = new System.Drawing.Point(33, 22);
            ucSelectStandartPageFormat1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            ucSelectStandartPageFormat1.Name = "ucSelectStandartPageFormat1";
            ucSelectStandartPageFormat1.Size = new System.Drawing.Size(131, 53);
            ucSelectStandartPageFormat1.TabIndex = 9;
            ucSelectStandartPageFormat1.PaperFormatChanged += ucSelectStandartPageFormat1_PaperFormatChanged;
            // 
            // button1
            // 
            button1.Image = (System.Drawing.Image)resources.GetObject("button1.Image");
            button1.Location = new System.Drawing.Point(107, 47);
            button1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(27, 27);
            button1.TabIndex = 10;
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_1;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(flowLayoutPanel1);
            groupBox3.Location = new System.Drawing.Point(14, 113);
            groupBox3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox3.Size = new System.Drawing.Size(572, 82);
            groupBox3.TabIndex = 11;
            groupBox3.TabStop = false;
            groupBox3.Text = "Порахувати";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(label8);
            flowLayoutPanel1.Controls.Add(label4);
            flowLayoutPanel1.Controls.Add(comboBoxWorH);
            flowLayoutPanel1.Controls.Add(label6);
            flowLayoutPanel1.Controls.Add(numericUpDownVal);
            flowLayoutPanel1.Controls.Add(label5);
            flowLayoutPanel1.Controls.Add(labelWorH);
            flowLayoutPanel1.Controls.Add(label7);
            flowLayoutPanel1.Controls.Add(textBox1);
            flowLayoutPanel1.Controls.Add(buttonGarazd);
            flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            flowLayoutPanel1.Location = new System.Drawing.Point(4, 19);
            flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new System.Drawing.Size(564, 60);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // label8
            // 
            label8.Dock = System.Windows.Forms.DockStyle.Top;
            label8.Location = new System.Drawing.Point(4, 0);
            label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(528, 27);
            label8.TabIndex = 9;
            label8.Text = "Перед тим як рахувати, потрібно виставити поточну ширину і висоту документа";
            label8.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label4
            // 
            label4.Location = new System.Drawing.Point(4, 27);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(42, 27);
            label4.TabIndex = 0;
            label4.Text = "Якщо";
            label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxWorH
            // 
            comboBoxWorH.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxWorH.FormattingEnabled = true;
            comboBoxWorH.Items.AddRange(new object[] { "ширина", "висота" });
            comboBoxWorH.Location = new System.Drawing.Point(54, 30);
            comboBoxWorH.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            comboBoxWorH.Name = "comboBoxWorH";
            comboBoxWorH.Size = new System.Drawing.Size(78, 23);
            comboBoxWorH.TabIndex = 1;
            comboBoxWorH.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // label6
            // 
            label6.Location = new System.Drawing.Point(140, 27);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(15, 27);
            label6.TabIndex = 5;
            label6.Text = "=";
            label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numericUpDownVal
            // 
            numericUpDownVal.DecimalPlaces = 1;
            numericUpDownVal.Location = new System.Drawing.Point(163, 30);
            numericUpDownVal.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            numericUpDownVal.Maximum = new decimal(new int[] { 276447231, 23283, 0, 0 });
            numericUpDownVal.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownVal.Name = "numericUpDownVal";
            numericUpDownVal.Size = new System.Drawing.Size(71, 23);
            numericUpDownVal.TabIndex = 6;
            numericUpDownVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            numericUpDownVal.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownVal.ValueChanged += numericUpDown1_ValueChanged;
            numericUpDownVal.Click += numericUpDownWidth_Enter;
            numericUpDownVal.Enter += numericUpDownWidth_Enter;
            // 
            // label5
            // 
            label5.Location = new System.Drawing.Point(242, 27);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(21, 27);
            label5.TabIndex = 2;
            label5.Text = "то";
            label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelWorH
            // 
            labelWorH.Location = new System.Drawing.Point(271, 27);
            labelWorH.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelWorH.Name = "labelWorH";
            labelWorH.Size = new System.Drawing.Size(71, 27);
            labelWorH.TabIndex = 3;
            labelWorH.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            label7.Location = new System.Drawing.Point(350, 27);
            label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(15, 27);
            label7.TabIndex = 7;
            label7.Text = "=";
            label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox1
            // 
            textBox1.Location = new System.Drawing.Point(373, 30);
            textBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new System.Drawing.Size(70, 23);
            textBox1.TabIndex = 4;
            textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonGarazd
            // 
            buttonGarazd.Location = new System.Drawing.Point(451, 30);
            buttonGarazd.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonGarazd.Name = "buttonGarazd";
            buttonGarazd.Size = new System.Drawing.Size(86, 27);
            buttonGarazd.TabIndex = 8;
            buttonGarazd.Text = "влаштовує";
            buttonGarazd.UseVisualStyleBackColor = true;
            buttonGarazd.Click += buttonGarazd_Click;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(ucSelectStandartPageFormat1);
            groupBox4.Location = new System.Drawing.Point(386, 9);
            groupBox4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox4.Name = "groupBox4";
            groupBox4.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox4.Size = new System.Drawing.Size(200, 97);
            groupBox4.TabIndex = 12;
            groupBox4.TabStop = false;
            groupBox4.Text = "Вибрати зі стандартних";
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(label1);
            groupBox5.Controls.Add(numericUpDownWidth);
            groupBox5.Controls.Add(label2);
            groupBox5.Controls.Add(button1);
            groupBox5.Controls.Add(numericUpDownHeight);
            groupBox5.Controls.Add(label3);
            groupBox5.Controls.Add(numericUpDownBleed);
            groupBox5.Location = new System.Drawing.Point(14, 9);
            groupBox5.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox5.Name = "groupBox5";
            groupBox5.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox5.Size = new System.Drawing.Size(352, 97);
            groupBox5.TabIndex = 13;
            groupBox5.TabStop = false;
            groupBox5.Text = "Новий формат документа";
            // 
            // FormSelectPdfNewSize
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(646, 397);
            Controls.Add(groupBox5);
            Controls.Add(groupBox4);
            Controls.Add(groupBox3);
            Controls.Add(buttonOk);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormSelectPdfNewSize";
            ShowIcon = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Змінити формат документа";
            ((System.ComponentModel.ISupportInitialize)numericUpDownWidth).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownHeight).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownBleed).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownVal).EndInit();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            ResumeLayout(false);

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