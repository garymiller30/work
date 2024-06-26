﻿namespace Job.UserForms.PDF
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
			this.radioButtonHor = new System.Windows.Forms.RadioButton();
			this.radioButtonVer = new System.Windows.Forms.RadioButton();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.numLen = new System.Windows.Forms.NumericUpDown();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.numDistanse = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.label3 = new System.Windows.Forms.Label();
			this.numBleed = new System.Windows.Forms.NumericUpDown();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.textBoxBigovky = new System.Windows.Forms.TextBox();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.numC = new System.Windows.Forms.NumericUpDown();
			this.numM = new System.Windows.Forms.NumericUpDown();
			this.numY = new System.Windows.Forms.NumericUpDown();
			this.numK = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numLen)).BeginInit();
			this.groupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numDistanse)).BeginInit();
			this.groupBox4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numBleed)).BeginInit();
			this.groupBox5.SuspendLayout();
			this.groupBox6.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numC)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numM)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numY)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numK)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonCreate
			// 
			this.buttonCreate.Location = new System.Drawing.Point(179, 250);
			this.buttonCreate.Name = "buttonCreate";
			this.buttonCreate.Size = new System.Drawing.Size(91, 35);
			this.buttonCreate.TabIndex = 0;
			this.buttonCreate.Text = "Створити";
			this.buttonCreate.UseVisualStyleBackColor = true;
			this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radioButtonVer);
			this.groupBox1.Controls.Add(this.radioButtonHor);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(200, 70);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Напрямок";
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
			// 
			// radioButtonVer
			// 
			this.radioButtonVer.AutoSize = true;
			this.radioButtonVer.Location = new System.Drawing.Point(7, 43);
			this.radioButtonVer.Name = "radioButtonVer";
			this.radioButtonVer.Size = new System.Drawing.Size(160, 17);
			this.radioButtonVer.TabIndex = 1;
			this.radioButtonVer.Text = "Вертикально (знизу вгору)";
			this.radioButtonVer.UseVisualStyleBackColor = true;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.numLen);
			this.groupBox2.Location = new System.Drawing.Point(298, 88);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(137, 69);
			this.groupBox2.TabIndex = 2;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "довжина лінії";
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
            1,
            0,
            0,
            0});
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.label2);
			this.groupBox3.Controls.Add(this.numDistanse);
			this.groupBox3.Location = new System.Drawing.Point(155, 88);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(137, 69);
			this.groupBox3.TabIndex = 3;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "відстань від лінії різу";
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
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(108, 31);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(23, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "мм";
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.label3);
			this.groupBox4.Controls.Add(this.numBleed);
			this.groupBox4.Location = new System.Drawing.Point(12, 88);
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
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.textBoxBigovky);
			this.groupBox5.Location = new System.Drawing.Point(218, 12);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(217, 70);
			this.groupBox5.TabIndex = 5;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Біговки (числа через пробіл)";
			// 
			// textBoxBigovky
			// 
			this.textBoxBigovky.Location = new System.Drawing.Point(7, 29);
			this.textBoxBigovky.Name = "textBoxBigovky";
			this.textBoxBigovky.Size = new System.Drawing.Size(204, 20);
			this.textBoxBigovky.TabIndex = 0;
			// 
			// groupBox6
			// 
			this.groupBox6.Controls.Add(this.label7);
			this.groupBox6.Controls.Add(this.label6);
			this.groupBox6.Controls.Add(this.label5);
			this.groupBox6.Controls.Add(this.label4);
			this.groupBox6.Controls.Add(this.numK);
			this.groupBox6.Controls.Add(this.numY);
			this.groupBox6.Controls.Add(this.numM);
			this.groupBox6.Controls.Add(this.numC);
			this.groupBox6.Location = new System.Drawing.Point(123, 163);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(213, 72);
			this.groupBox6.TabIndex = 6;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "Колір";
			// 
			// numC
			// 
			this.numC.Location = new System.Drawing.Point(13, 46);
			this.numC.Name = "numC";
			this.numC.Size = new System.Drawing.Size(43, 20);
			this.numC.TabIndex = 0;
			this.numC.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// numM
			// 
			this.numM.Location = new System.Drawing.Point(62, 46);
			this.numM.Name = "numM";
			this.numM.Size = new System.Drawing.Size(43, 20);
			this.numM.TabIndex = 1;
			this.numM.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// numY
			// 
			this.numY.Location = new System.Drawing.Point(111, 46);
			this.numY.Name = "numY";
			this.numY.Size = new System.Drawing.Size(43, 20);
			this.numY.TabIndex = 2;
			this.numY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// numK
			// 
			this.numK.Location = new System.Drawing.Point(160, 46);
			this.numK.Name = "numK";
			this.numK.Size = new System.Drawing.Size(43, 20);
			this.numK.TabIndex = 3;
			this.numK.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.numK.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(22, 27);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(14, 13);
			this.label4.TabIndex = 4;
			this.label4.Text = "C";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(75, 27);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(16, 13);
			this.label5.TabIndex = 5;
			this.label5.Text = "M";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(121, 27);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(14, 13);
			this.label6.TabIndex = 6;
			this.label6.Text = "Y";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(172, 27);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(14, 13);
			this.label7.TabIndex = 7;
			this.label7.Text = "K";
			// 
			// FormCreateBigovkaMarks
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(444, 297);
			this.Controls.Add(this.groupBox6);
			this.Controls.Add(this.groupBox5);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.buttonCreate);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
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
			((System.ComponentModel.ISupportInitialize)(this.numC)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numM)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numY)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numK)).EndInit();
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
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numK;
        private System.Windows.Forms.NumericUpDown numY;
        private System.Windows.Forms.NumericUpDown numM;
        private System.Windows.Forms.NumericUpDown numC;
    }
}