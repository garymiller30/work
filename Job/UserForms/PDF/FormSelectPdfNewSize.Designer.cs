namespace Job.UserForms
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
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBleed)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // numericUpDownWidth
            // 
            this.numericUpDownWidth.DecimalPlaces = 1;
            this.numericUpDownWidth.Location = new System.Drawing.Point(12, 31);
            this.numericUpDownWidth.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numericUpDownWidth.Name = "numericUpDownWidth";
            this.numericUpDownWidth.Size = new System.Drawing.Size(83, 20);
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
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Ширина";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(125, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Висота";
            // 
            // numericUpDownHeight
            // 
            this.numericUpDownHeight.DecimalPlaces = 1;
            this.numericUpDownHeight.Location = new System.Drawing.Point(130, 29);
            this.numericUpDownHeight.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numericUpDownHeight.Name = "numericUpDownHeight";
            this.numericUpDownHeight.Size = new System.Drawing.Size(83, 20);
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
            this.numericUpDownBleed.Location = new System.Drawing.Point(12, 72);
            this.numericUpDownBleed.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numericUpDownBleed.Name = "numericUpDownBleed";
            this.numericUpDownBleed.Size = new System.Drawing.Size(83, 20);
            this.numericUpDownBleed.TabIndex = 5;
            this.numericUpDownBleed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownBleed.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDownBleed.Enter += new System.EventHandler(this.numericUpDownWidth_Enter);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Поля на підрізку";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonNonProportial);
            this.groupBox1.Controls.Add(this.radioButtonProportial);
            this.groupBox1.Location = new System.Drawing.Point(12, 111);
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
            this.groupBox2.Location = new System.Drawing.Point(150, 111);
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
            this.buttonOk.Location = new System.Drawing.Point(128, 192);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(118, 39);
            this.buttonOk.TabIndex = 8;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.button1_Click);
            // 
            // ucSelectStandartPageFormat1
            // 
            this.ucSelectStandartPageFormat1.AutoSize = true;
            this.ucSelectStandartPageFormat1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ucSelectStandartPageFormat1.Location = new System.Drawing.Point(226, 4);
            this.ucSelectStandartPageFormat1.Name = "ucSelectStandartPageFormat1";
            this.ucSelectStandartPageFormat1.Size = new System.Drawing.Size(112, 48);
            this.ucSelectStandartPageFormat1.TabIndex = 9;
            this.ucSelectStandartPageFormat1.PaperFormatChanged += new System.EventHandler<Static.Pdf.Common.PaperFormat>(this.ucSelectStandartPageFormat1_PaperFormatChanged);
            // 
            // button1
            // 
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.Location = new System.Drawing.Point(101, 28);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(23, 23);
            this.button1.TabIndex = 10;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // FormSelectPdfNewSize
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 243);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ucSelectStandartPageFormat1);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.numericUpDownBleed);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numericUpDownHeight);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDownWidth);
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
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}