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
            Static.Pdf.Imposition.Models.MarkColor markColor1 = new Static.Pdf.Imposition.Models.MarkColor();
            buttonCreate = new System.Windows.Forms.Button();
            groupBox1 = new System.Windows.Forms.GroupBox();
            cb_mirrorEven = new System.Windows.Forms.CheckBox();
            radioButtonVer = new System.Windows.Forms.RadioButton();
            radioButtonHor = new System.Windows.Forms.RadioButton();
            numLen = new System.Windows.Forms.NumericUpDown();
            numDistanse = new System.Windows.Forms.NumericUpDown();
            numBleed = new System.Windows.Forms.NumericUpDown();
            groupBox5 = new System.Windows.Forms.GroupBox();
            btn_add_to_center = new System.Windows.Forms.Button();
            textBoxBigovky = new System.Windows.Forms.TextBox();
            groupBox7 = new System.Windows.Forms.GroupBox();
            cb_files = new System.Windows.Forms.ComboBox();
            uc_PreviewBrowserFile1 = new JobSpace.UC.Uc_FilePreviewControl();
            btn_3d = new System.Windows.Forms.Button();
            uc_PdfColorSelector1 = new JobSpace.UC.PDF.Uc_PdfColorSelector();
            groupBox6 = new System.Windows.Forms.GroupBox();
            cb_draw_proofcolor = new System.Windows.Forms.CheckBox();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numLen).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numDistanse).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numBleed).BeginInit();
            groupBox5.SuspendLayout();
            groupBox7.SuspendLayout();
            groupBox6.SuspendLayout();
            SuspendLayout();
            // 
            // buttonCreate
            // 
            buttonCreate.Location = new System.Drawing.Point(189, 536);
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
            groupBox1.Size = new System.Drawing.Size(281, 118);
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
            // numLen
            // 
            numLen.DecimalPlaces = 1;
            numLen.Location = new System.Drawing.Point(186, 80);
            numLen.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            numLen.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numLen.Name = "numLen";
            numLen.Size = new System.Drawing.Size(71, 23);
            numLen.TabIndex = 0;
            numLen.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            numLen.Value = new decimal(new int[] { 2, 0, 0, 0 });
            numLen.Click += numDistanse_Enter;
            numLen.Enter += numDistanse_Enter;
            // 
            // numDistanse
            // 
            numDistanse.DecimalPlaces = 1;
            numDistanse.Location = new System.Drawing.Point(186, 51);
            numDistanse.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            numDistanse.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numDistanse.Name = "numDistanse";
            numDistanse.Size = new System.Drawing.Size(71, 23);
            numDistanse.TabIndex = 0;
            numDistanse.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            numDistanse.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numDistanse.Click += numDistanse_Enter;
            numDistanse.Enter += numDistanse_Enter;
            // 
            // numBleed
            // 
            numBleed.DecimalPlaces = 1;
            numBleed.Location = new System.Drawing.Point(186, 24);
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
            groupBox5.Size = new System.Drawing.Size(281, 91);
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
            textBoxBigovky.Size = new System.Drawing.Size(265, 23);
            textBoxBigovky.TabIndex = 0;
            textBoxBigovky.TextChanged += textBoxBigovky_TextChanged;
            // 
            // groupBox7
            // 
            groupBox7.Controls.Add(cb_files);
            groupBox7.Location = new System.Drawing.Point(14, 138);
            groupBox7.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox7.Name = "groupBox7";
            groupBox7.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox7.Size = new System.Drawing.Size(281, 55);
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
            cb_files.Size = new System.Drawing.Size(264, 23);
            cb_files.TabIndex = 3;
            cb_files.SelectedIndexChanged += cb_files_SelectedIndexChanged;
            // 
            // uc_PreviewBrowserFile1
            // 
            uc_PreviewBrowserFile1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            uc_PreviewBrowserFile1.Location = new System.Drawing.Point(304, 14);
            uc_PreviewBrowserFile1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            uc_PreviewBrowserFile1.Name = "uc_PreviewBrowserFile1";
            uc_PreviewBrowserFile1.Size = new System.Drawing.Size(599, 562);
            uc_PreviewBrowserFile1.TabIndex = 10;
            // 
            // btn_3d
            // 
            btn_3d.Location = new System.Drawing.Point(14, 536);
            btn_3d.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btn_3d.Name = "btn_3d";
            btn_3d.Size = new System.Drawing.Size(106, 40);
            btn_3d.TabIndex = 11;
            btn_3d.Text = "3D";
            btn_3d.UseVisualStyleBackColor = true;
            btn_3d.Click += btn_3d_Click;
            // 
            // uc_PdfColorSelector1
            // 
            uc_PdfColorSelector1.Location = new System.Drawing.Point(23, 109);
            markColor1.a = 0D;
            markColor1.b = 0D;
            markColor1.C = 0D;
            markColor1.ColorType = Models.ColorTypeEnum.CMYK;
            markColor1.Id = "6362da3b-fc13-40d7-9fd9-b28d236da442";
            markColor1.IsOverprint = false;
            markColor1.IsSpot = false;
            markColor1.K = 0D;
            markColor1.l = 0D;
            markColor1.M = 0D;
            markColor1.Name = "White";
            markColor1.Opasity = 100D;
            markColor1.Tint = 100D;
            markColor1.Y = 0D;
            uc_PdfColorSelector1.MarkColor = markColor1;
            uc_PdfColorSelector1.Name = "uc_PdfColorSelector1";
            uc_PdfColorSelector1.Size = new System.Drawing.Size(234, 71);
            uc_PdfColorSelector1.TabIndex = 12;
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(cb_draw_proofcolor);
            groupBox6.Controls.Add(numDistanse);
            groupBox6.Controls.Add(uc_PdfColorSelector1);
            groupBox6.Controls.Add(numLen);
            groupBox6.Controls.Add(numBleed);
            groupBox6.Controls.Add(label4);
            groupBox6.Controls.Add(label3);
            groupBox6.Controls.Add(label2);
            groupBox6.Location = new System.Drawing.Point(14, 298);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new System.Drawing.Size(281, 213);
            groupBox6.TabIndex = 13;
            groupBox6.TabStop = false;
            groupBox6.Text = "Налаштування міток";
            // 
            // cb_draw_proofcolor
            // 
            cb_draw_proofcolor.AutoSize = true;
            cb_draw_proofcolor.Checked = true;
            cb_draw_proofcolor.CheckState = System.Windows.Forms.CheckState.Checked;
            cb_draw_proofcolor.Location = new System.Drawing.Point(23, 186);
            cb_draw_proofcolor.Name = "cb_draw_proofcolor";
            cb_draw_proofcolor.Size = new System.Drawing.Size(202, 19);
            cb_draw_proofcolor.TabIndex = 13;
            cb_draw_proofcolor.Text = "малювати розмітку (ProofColor)";
            cb_draw_proofcolor.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(23, 82);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(105, 15);
            label4.TabIndex = 2;
            label4.Text = "довжина лінії, мм";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(23, 54);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(144, 15);
            label3.TabIndex = 1;
            label3.Text = "відстань від лінії різу, мм";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(23, 26);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(120, 15);
            label2.TabIndex = 0;
            label2.Text = "поля на підрізку, мм";
            // 
            // FormCreateBigovkaMarks
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(917, 589);
            Controls.Add(groupBox6);
            Controls.Add(btn_3d);
            Controls.Add(uc_PreviewBrowserFile1);
            Controls.Add(groupBox7);
            Controls.Add(groupBox5);
            Controls.Add(groupBox1);
            Controls.Add(buttonCreate);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormCreateBigovkaMarks";
            ShowIcon = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Створити мітки для біговки";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numLen).EndInit();
            ((System.ComponentModel.ISupportInitialize)numDistanse).EndInit();
            ((System.ComponentModel.ISupportInitialize)numBleed).EndInit();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            groupBox7.ResumeLayout(false);
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCreate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonVer;
        private System.Windows.Forms.RadioButton radioButtonHor;
        private System.Windows.Forms.NumericUpDown numLen;
        private System.Windows.Forms.NumericUpDown numDistanse;
        private System.Windows.Forms.NumericUpDown numBleed;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox textBoxBigovky;
        private System.Windows.Forms.CheckBox cb_mirrorEven;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.ComboBox cb_files;
        private System.Windows.Forms.Button btn_add_to_center;
        private UC.Uc_FilePreviewControl uc_PreviewBrowserFile1;
        private System.Windows.Forms.Button btn_3d;
        private UC.PDF.Uc_PdfColorSelector uc_PdfColorSelector1;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.CheckBox cb_draw_proofcolor;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
    }
}
