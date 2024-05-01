namespace Job.UserForms.PDF
{
    partial class FormCreateFillRectangle
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
			this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.comboBoxListColor = new System.Windows.Forms.ComboBox();
			this.comboBoxListTables = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.numK = new System.Windows.Forms.NumericUpDown();
			this.numY = new System.Windows.Forms.NumericUpDown();
			this.numM = new System.Windows.Forms.NumericUpDown();
			this.numC = new System.Windows.Forms.NumericUpDown();
			this.radioButtonSpot = new System.Windows.Forms.RadioButton();
			this.radioButtonCMYK = new System.Windows.Forms.RadioButton();
			this.button1 = new System.Windows.Forms.Button();
			this.panelColor = new System.Windows.Forms.Panel();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numK)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numY)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numM)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numC)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.numericUpDown2);
			this.groupBox1.Controls.Add(this.numericUpDown1);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(162, 100);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "розмір";
			// 
			// numericUpDown2
			// 
			this.numericUpDown2.Location = new System.Drawing.Point(71, 53);
			this.numericUpDown2.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
			this.numericUpDown2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericUpDown2.Name = "numericUpDown2";
			this.numericUpDown2.Size = new System.Drawing.Size(69, 20);
			this.numericUpDown2.TabIndex = 3;
			this.numericUpDown2.Value = new decimal(new int[] {
            297,
            0,
            0,
            0});
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.Location = new System.Drawing.Point(71, 25);
			this.numericUpDown1.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
			this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(69, 20);
			this.numericUpDown1.TabIndex = 2;
			this.numericUpDown1.Value = new decimal(new int[] {
            210,
            0,
            0,
            0});
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(20, 55);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(42, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "висота";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(20, 27);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(45, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "ширина";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.panelColor);
			this.groupBox2.Controls.Add(this.comboBoxListColor);
			this.groupBox2.Controls.Add(this.comboBoxListTables);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.numK);
			this.groupBox2.Controls.Add(this.numY);
			this.groupBox2.Controls.Add(this.numM);
			this.groupBox2.Controls.Add(this.numC);
			this.groupBox2.Controls.Add(this.radioButtonSpot);
			this.groupBox2.Controls.Add(this.radioButtonCMYK);
			this.groupBox2.Location = new System.Drawing.Point(180, 12);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(570, 100);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "колір";
			// 
			// comboBoxListColor
			// 
			this.comboBoxListColor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.comboBoxListColor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.comboBoxListColor.Enabled = false;
			this.comboBoxListColor.FormattingEnabled = true;
			this.comboBoxListColor.Location = new System.Drawing.Point(346, 65);
			this.comboBoxListColor.Name = "comboBoxListColor";
			this.comboBoxListColor.Size = new System.Drawing.Size(218, 21);
			this.comboBoxListColor.TabIndex = 13;
			this.comboBoxListColor.SelectedIndexChanged += new System.EventHandler(this.comboBoxListColor_SelectedIndexChanged);
			// 
			// comboBoxListTables
			// 
			this.comboBoxListTables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxListTables.Enabled = false;
			this.comboBoxListTables.FormattingEnabled = true;
			this.comboBoxListTables.Location = new System.Drawing.Point(90, 65);
			this.comboBoxListTables.Name = "comboBoxListTables";
			this.comboBoxListTables.Size = new System.Drawing.Size(239, 21);
			this.comboBoxListTables.TabIndex = 12;
			this.comboBoxListTables.SelectedIndexChanged += new System.EventHandler(this.comboBoxListTables_SelectedIndexChanged);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(343, 27);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(14, 13);
			this.label6.TabIndex = 11;
			this.label6.Text = "K";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(249, 27);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(14, 13);
			this.label5.TabIndex = 10;
			this.label5.Text = "Y";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(158, 27);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(16, 13);
			this.label4.TabIndex = 9;
			this.label4.Text = "M";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(70, 27);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(14, 13);
			this.label3.TabIndex = 8;
			this.label3.Text = "C";
			// 
			// numK
			// 
			this.numK.DecimalPlaces = 2;
			this.numK.Location = new System.Drawing.Point(363, 25);
			this.numK.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
			this.numK.Name = "numK";
			this.numK.Size = new System.Drawing.Size(60, 20);
			this.numK.TabIndex = 7;
			this.numK.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			// 
			// numY
			// 
			this.numY.DecimalPlaces = 2;
			this.numY.Location = new System.Drawing.Point(269, 25);
			this.numY.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
			this.numY.Name = "numY";
			this.numY.Size = new System.Drawing.Size(60, 20);
			this.numY.TabIndex = 6;
			// 
			// numM
			// 
			this.numM.DecimalPlaces = 2;
			this.numM.Location = new System.Drawing.Point(180, 25);
			this.numM.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
			this.numM.Name = "numM";
			this.numM.Size = new System.Drawing.Size(60, 20);
			this.numM.TabIndex = 5;
			// 
			// numC
			// 
			this.numC.DecimalPlaces = 2;
			this.numC.Location = new System.Drawing.Point(90, 25);
			this.numC.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
			this.numC.Name = "numC";
			this.numC.Size = new System.Drawing.Size(60, 20);
			this.numC.TabIndex = 4;
			// 
			// radioButtonSpot
			// 
			this.radioButtonSpot.AutoSize = true;
			this.radioButtonSpot.Location = new System.Drawing.Point(7, 65);
			this.radioButtonSpot.Name = "radioButtonSpot";
			this.radioButtonSpot.Size = new System.Drawing.Size(47, 17);
			this.radioButtonSpot.TabIndex = 1;
			this.radioButtonSpot.Text = "Spot";
			this.radioButtonSpot.UseVisualStyleBackColor = true;
			// 
			// radioButtonCMYK
			// 
			this.radioButtonCMYK.AutoSize = true;
			this.radioButtonCMYK.Checked = true;
			this.radioButtonCMYK.Location = new System.Drawing.Point(6, 25);
			this.radioButtonCMYK.Name = "radioButtonCMYK";
			this.radioButtonCMYK.Size = new System.Drawing.Size(55, 17);
			this.radioButtonCMYK.TabIndex = 0;
			this.radioButtonCMYK.TabStop = true;
			this.radioButtonCMYK.Text = "CMYK";
			this.radioButtonCMYK.UseVisualStyleBackColor = true;
			this.radioButtonCMYK.CheckedChanged += new System.EventHandler(this.radioButtonCMYK_CheckedChanged);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(331, 135);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(136, 39);
			this.button1.TabIndex = 2;
			this.button1.Text = "створити";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// panelColor
			// 
			this.panelColor.Location = new System.Drawing.Point(493, 12);
			this.panelColor.Name = "panelColor";
			this.panelColor.Size = new System.Drawing.Size(71, 33);
			this.panelColor.TabIndex = 14;
			// 
			// FormCreateFillRectangle
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(758, 186);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormCreateFillRectangle";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Створити плашку";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numK)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numY)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numM)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numC)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButtonSpot;
        private System.Windows.Forms.RadioButton radioButtonCMYK;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numK;
        private System.Windows.Forms.NumericUpDown numY;
        private System.Windows.Forms.NumericUpDown numM;
        private System.Windows.Forms.NumericUpDown numC;
        private System.Windows.Forms.ComboBox comboBoxListColor;
        private System.Windows.Forms.ComboBox comboBoxListTables;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panelColor;
    }
}