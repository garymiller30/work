namespace JobSpace.UserForms
{
    partial class FormCreateCollatingPageMark
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nud_x = new System.Windows.Forms.NumericUpDown();
            this.nud_y = new System.Windows.Forms.NumericUpDown();
            this.nud_len = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nud_height = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.nud_width = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.cb_position = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.nud_x)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_len)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_height)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_width)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Y";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(98, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "довжина";
            // 
            // nud_x
            // 
            this.nud_x.DecimalPlaces = 1;
            this.nud_x.Location = new System.Drawing.Point(29, 19);
            this.nud_x.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nud_x.Name = "nud_x";
            this.nud_x.Size = new System.Drawing.Size(54, 20);
            this.nud_x.TabIndex = 3;
            this.nud_x.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // nud_y
            // 
            this.nud_y.DecimalPlaces = 1;
            this.nud_y.Location = new System.Drawing.Point(29, 49);
            this.nud_y.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nud_y.Name = "nud_y";
            this.nud_y.Size = new System.Drawing.Size(54, 20);
            this.nud_y.TabIndex = 4;
            this.nud_y.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // nud_len
            // 
            this.nud_len.DecimalPlaces = 1;
            this.nud_len.Location = new System.Drawing.Point(98, 49);
            this.nud_len.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nud_len.Name = "nud_len";
            this.nud_len.Size = new System.Drawing.Size(54, 20);
            this.nud_len.TabIndex = 5;
            this.nud_len.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nud_height);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.nud_width);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(177, 17);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(145, 72);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "мітка";
            // 
            // nud_height
            // 
            this.nud_height.DecimalPlaces = 1;
            this.nud_height.Location = new System.Drawing.Point(81, 43);
            this.nud_height.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nud_height.Name = "nud_height";
            this.nud_height.Size = new System.Drawing.Size(54, 20);
            this.nud_height.TabIndex = 10;
            this.nud_height.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nud_height.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(78, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "висота";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nud_width
            // 
            this.nud_width.DecimalPlaces = 1;
            this.nud_width.Location = new System.Drawing.Point(10, 43);
            this.nud_width.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nud_width.Name = "nud_width";
            this.nud_width.Size = new System.Drawing.Size(54, 20);
            this.nud_width.TabIndex = 8;
            this.nud_width.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nud_width.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "ширина";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cb_position);
            this.groupBox2.Controls.Add(this.nud_x);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.nud_len);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.nud_y);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(6, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(163, 109);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "положення";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(134, 175);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 31);
            this.button1.TabIndex = 8;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cb_position
            // 
            this.cb_position.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_position.FormattingEnabled = true;
            this.cb_position.Location = new System.Drawing.Point(29, 82);
            this.cb_position.Name = "cb_position";
            this.cb_position.Size = new System.Drawing.Size(123, 21);
            this.cb_position.TabIndex = 6;
            // 
            // FormCreateCollatingPageMark
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 218);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCreateCollatingPageMark";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Створити мітки для підбору";
            ((System.ComponentModel.ISupportInitialize)(this.nud_x)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_len)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_height)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_width)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nud_x;
        private System.Windows.Forms.NumericUpDown nud_y;
        private System.Windows.Forms.NumericUpDown nud_len;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown nud_height;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nud_width;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox cb_position;
    }
}