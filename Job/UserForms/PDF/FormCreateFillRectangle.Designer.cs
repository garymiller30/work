namespace JobSpace.UserForms.PDF
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
            groupBox1 = new System.Windows.Forms.GroupBox();
            nud_bleed = new System.Windows.Forms.NumericUpDown();
            label7 = new System.Windows.Forms.Label();
            numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            button1 = new System.Windows.Forms.Button();
            uc_PdfColorSelector1 = new JobSpace.UC.PDF.Uc_PdfColorSelector();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nud_bleed).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(nud_bleed);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(numericUpDown2);
            groupBox1.Controls.Add(numericUpDown1);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new System.Drawing.Point(14, 14);
            groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Size = new System.Drawing.Size(189, 115);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "розмір";
            // 
            // nud_bleed
            // 
            nud_bleed.Location = new System.Drawing.Point(83, 85);
            nud_bleed.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            nud_bleed.Maximum = new decimal(new int[] { 9999999, 0, 0, 0 });
            nud_bleed.Name = "nud_bleed";
            nud_bleed.Size = new System.Drawing.Size(80, 23);
            nud_bleed.TabIndex = 5;
            nud_bleed.Value = new decimal(new int[] { 3, 0, 0, 0 });
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(23, 88);
            label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(52, 15);
            label7.TabIndex = 4;
            label7.Text = "на обріз";
            // 
            // numericUpDown2
            // 
            numericUpDown2.Location = new System.Drawing.Point(83, 52);
            numericUpDown2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            numericUpDown2.Maximum = new decimal(new int[] { 9999999, 0, 0, 0 });
            numericUpDown2.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new System.Drawing.Size(80, 23);
            numericUpDown2.TabIndex = 3;
            numericUpDown2.Value = new decimal(new int[] { 297, 0, 0, 0 });
            numericUpDown2.Click += numericUpDown1_Click;
            numericUpDown2.Enter += numericUpDown1_Click;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new System.Drawing.Point(83, 20);
            numericUpDown1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            numericUpDown1.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
            numericUpDown1.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new System.Drawing.Size(80, 23);
            numericUpDown1.TabIndex = 2;
            numericUpDown1.Value = new decimal(new int[] { 210, 0, 0, 0 });
            numericUpDown1.Click += numericUpDown1_Click;
            numericUpDown1.Enter += numericUpDown1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(23, 54);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(44, 15);
            label2.TabIndex = 1;
            label2.Text = "висота";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(23, 22);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(52, 15);
            label1.TabIndex = 0;
            label1.Text = "ширина";
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(162, 158);
            button1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(159, 45);
            button1.TabIndex = 2;
            button1.Text = "створити";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // uc_PdfColorSelector1
            // 
            uc_PdfColorSelector1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            uc_PdfColorSelector1.Location = new System.Drawing.Point(210, 18);
            uc_PdfColorSelector1.MarkColor = null;
            uc_PdfColorSelector1.Name = "uc_PdfColorSelector1";
            uc_PdfColorSelector1.Size = new System.Drawing.Size(265, 71);
            uc_PdfColorSelector1.TabIndex = 3;
            // 
            // FormCreateFillRectangle
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(481, 215);
            Controls.Add(uc_PdfColorSelector1);
            Controls.Add(button1);
            Controls.Add(groupBox1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormCreateFillRectangle";
            ShowIcon = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Створити плашку";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nud_bleed).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown nud_bleed;
        private System.Windows.Forms.Label label7;
        private UC.PDF.Uc_PdfColorSelector uc_PdfColorSelector1;
    }
}