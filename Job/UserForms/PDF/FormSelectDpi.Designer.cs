namespace JobSpace.Dlg
{
    partial class FormSelectDpi
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
            buttonOk = new System.Windows.Forms.Button();
            numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            groupBox1 = new System.Windows.Forms.GroupBox();
            button3 = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            button1 = new System.Windows.Forms.Button();
            groupBox2 = new System.Windows.Forms.GroupBox();
            button6 = new System.Windows.Forms.Button();
            button5 = new System.Windows.Forms.Button();
            button4 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // buttonOk
            // 
            buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            buttonOk.Location = new System.Drawing.Point(94, 217);
            buttonOk.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            buttonOk.Name = "buttonOk";
            buttonOk.Size = new System.Drawing.Size(112, 35);
            buttonOk.TabIndex = 0;
            buttonOk.Text = "OK";
            buttonOk.UseVisualStyleBackColor = true;
            buttonOk.Click += buttonOk_Click;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new System.Drawing.Point(11, 27);
            numericUpDown1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            numericUpDown1.Maximum = new decimal(new int[] { 3000, 0, 0, 0 });
            numericUpDown1.Minimum = new decimal(new int[] { 36, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new System.Drawing.Size(101, 26);
            numericUpDown1.TabIndex = 1;
            numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            numericUpDown1.Value = new decimal(new int[] { 96, 0, 0, 0 });
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            numericUpDown1.Click += numericUpDown1_Click;
            numericUpDown1.Enter += numericUpDown1_Click;
            // 
            // numericUpDown2
            // 
            numericUpDown2.Location = new System.Drawing.Point(7, 27);
            numericUpDown2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            numericUpDown2.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new System.Drawing.Size(101, 26);
            numericUpDown2.TabIndex = 3;
            numericUpDown2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            numericUpDown2.Value = new decimal(new int[] { 80, 0, 0, 0 });
            numericUpDown2.Click += numericUpDown1_Click;
            numericUpDown2.Enter += numericUpDown1_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(button3);
            groupBox1.Controls.Add(button2);
            groupBox1.Controls.Add(button1);
            groupBox1.Controls.Add(numericUpDown1);
            groupBox1.Location = new System.Drawing.Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(125, 178);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            groupBox1.Text = "DPI";
            // 
            // button3
            // 
            button3.Location = new System.Drawing.Point(11, 143);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(101, 29);
            button3.TabIndex = 4;
            button3.Text = "600";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(63, 108);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(49, 29);
            button2.TabIndex = 3;
            button2.Text = "300";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button1_Click;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(11, 108);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(50, 29);
            button1.TabIndex = 2;
            button1.Text = "150";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(button6);
            groupBox2.Controls.Add(button5);
            groupBox2.Controls.Add(button4);
            groupBox2.Controls.Add(numericUpDown2);
            groupBox2.Location = new System.Drawing.Point(167, 12);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(117, 178);
            groupBox2.TabIndex = 7;
            groupBox2.TabStop = false;
            groupBox2.Text = "Якість, %";
            // 
            // button6
            // 
            button6.Location = new System.Drawing.Point(7, 143);
            button6.Name = "button6";
            button6.Size = new System.Drawing.Size(101, 29);
            button6.TabIndex = 6;
            button6.Text = "100";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button4_Click;
            // 
            // button5
            // 
            button5.Location = new System.Drawing.Point(59, 108);
            button5.Name = "button5";
            button5.Size = new System.Drawing.Size(49, 29);
            button5.TabIndex = 5;
            button5.Text = "90";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button4_Click;
            // 
            // button4
            // 
            button4.Location = new System.Drawing.Point(8, 108);
            button4.Name = "button4";
            button4.Size = new System.Drawing.Size(45, 29);
            button4.TabIndex = 4;
            button4.Text = "70";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // FormSelectDpi
            // 
            AcceptButton = buttonOk;
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(301, 263);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(buttonOk);
            Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormSelectDpi";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Вкажи DPI";
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
    }
}