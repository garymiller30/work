namespace JobSpace.UserForms.PDF
{
    partial class Form_PdfColorSelector
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
            gb_cmyk = new System.Windows.Forms.GroupBox();
            panel_cmyk = new System.Windows.Forms.Panel();
            cb_b = new System.Windows.Forms.CheckBox();
            cb_y = new System.Windows.Forms.CheckBox();
            cb_m = new System.Windows.Forms.CheckBox();
            cb_c = new System.Windows.Forms.CheckBox();
            numK = new System.Windows.Forms.NumericUpDown();
            numY = new System.Windows.Forms.NumericUpDown();
            numM = new System.Windows.Forms.NumericUpDown();
            numC = new System.Windows.Forms.NumericUpDown();
            comboBoxListColor = new System.Windows.Forms.ComboBox();
            comboBoxListTables = new System.Windows.Forms.ComboBox();
            radioButtonSpot = new System.Windows.Forms.RadioButton();
            radioButtonCMYK = new System.Windows.Forms.RadioButton();
            bnt_OK = new System.Windows.Forms.Button();
            gb_spot = new System.Windows.Forms.GroupBox();
            panel_spot = new System.Windows.Forms.Panel();
            gb_cmyk.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numK).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numY).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numM).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numC).BeginInit();
            gb_spot.SuspendLayout();
            SuspendLayout();
            // 
            // gb_cmyk
            // 
            gb_cmyk.Controls.Add(panel_cmyk);
            gb_cmyk.Controls.Add(cb_b);
            gb_cmyk.Controls.Add(cb_y);
            gb_cmyk.Controls.Add(cb_m);
            gb_cmyk.Controls.Add(cb_c);
            gb_cmyk.Controls.Add(numK);
            gb_cmyk.Controls.Add(numY);
            gb_cmyk.Controls.Add(numM);
            gb_cmyk.Controls.Add(numC);
            gb_cmyk.Location = new System.Drawing.Point(13, 44);
            gb_cmyk.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            gb_cmyk.Name = "gb_cmyk";
            gb_cmyk.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            gb_cmyk.Size = new System.Drawing.Size(237, 123);
            gb_cmyk.TabIndex = 7;
            gb_cmyk.TabStop = false;
            // 
            // panel_cmyk
            // 
            panel_cmyk.Location = new System.Drawing.Point(7, 82);
            panel_cmyk.Name = "panel_cmyk";
            panel_cmyk.Size = new System.Drawing.Size(221, 35);
            panel_cmyk.TabIndex = 11;
            // 
            // cb_b
            // 
            cb_b.Appearance = System.Windows.Forms.Appearance.Button;
            cb_b.AutoSize = true;
            cb_b.Location = new System.Drawing.Point(190, 22);
            cb_b.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cb_b.Name = "cb_b";
            cb_b.Size = new System.Drawing.Size(24, 25);
            cb_b.TabIndex = 10;
            cb_b.Text = "K";
            cb_b.UseVisualStyleBackColor = true;
            cb_b.CheckedChanged += cb_b_CheckedChanged;
            // 
            // cb_y
            // 
            cb_y.Appearance = System.Windows.Forms.Appearance.Button;
            cb_y.AutoSize = true;
            cb_y.Location = new System.Drawing.Point(133, 22);
            cb_y.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cb_y.Name = "cb_y";
            cb_y.Size = new System.Drawing.Size(24, 25);
            cb_y.TabIndex = 9;
            cb_y.Text = "Y";
            cb_y.UseVisualStyleBackColor = true;
            cb_y.CheckedChanged += cb_y_CheckedChanged;
            // 
            // cb_m
            // 
            cb_m.Appearance = System.Windows.Forms.Appearance.Button;
            cb_m.AutoSize = true;
            cb_m.Location = new System.Drawing.Point(76, 22);
            cb_m.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cb_m.Name = "cb_m";
            cb_m.Size = new System.Drawing.Size(28, 25);
            cb_m.TabIndex = 8;
            cb_m.Text = "M";
            cb_m.UseVisualStyleBackColor = true;
            cb_m.CheckedChanged += cb_m_CheckedChanged;
            // 
            // cb_c
            // 
            cb_c.Appearance = System.Windows.Forms.Appearance.Button;
            cb_c.AutoSize = true;
            cb_c.Location = new System.Drawing.Point(19, 22);
            cb_c.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cb_c.Name = "cb_c";
            cb_c.Size = new System.Drawing.Size(25, 25);
            cb_c.TabIndex = 7;
            cb_c.Text = "C";
            cb_c.UseVisualStyleBackColor = true;
            cb_c.CheckedChanged += cb_c_CheckedChanged;
            // 
            // numK
            // 
            numK.Location = new System.Drawing.Point(178, 53);
            numK.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            numK.Name = "numK";
            numK.Size = new System.Drawing.Size(50, 23);
            numK.TabIndex = 3;
            numK.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            numK.ValueChanged += numC_ValueChanged;
            numK.Click += numericUpDown1_Click;
            // 
            // numY
            // 
            numY.Location = new System.Drawing.Point(121, 53);
            numY.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            numY.Name = "numY";
            numY.Size = new System.Drawing.Size(50, 23);
            numY.TabIndex = 2;
            numY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            numY.ValueChanged += numC_ValueChanged;
            numY.Click += numericUpDown1_Click;
            // 
            // numM
            // 
            numM.Location = new System.Drawing.Point(64, 53);
            numM.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            numM.Name = "numM";
            numM.Size = new System.Drawing.Size(50, 23);
            numM.TabIndex = 1;
            numM.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            numM.ValueChanged += numC_ValueChanged;
            numM.Click += numericUpDown1_Click;
            // 
            // numC
            // 
            numC.Location = new System.Drawing.Point(7, 53);
            numC.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            numC.Name = "numC";
            numC.Size = new System.Drawing.Size(50, 23);
            numC.TabIndex = 0;
            numC.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            numC.ValueChanged += numC_ValueChanged;
            numC.Click += numericUpDown1_Click;
            // 
            // comboBoxListColor
            // 
            comboBoxListColor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            comboBoxListColor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            comboBoxListColor.FormattingEnabled = true;
            comboBoxListColor.Location = new System.Drawing.Point(7, 51);
            comboBoxListColor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            comboBoxListColor.Name = "comboBoxListColor";
            comboBoxListColor.Size = new System.Drawing.Size(252, 23);
            comboBoxListColor.TabIndex = 17;
            comboBoxListColor.SelectedIndexChanged += comboBoxListColor_SelectedIndexChanged;
            // 
            // comboBoxListTables
            // 
            comboBoxListTables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxListTables.FormattingEnabled = true;
            comboBoxListTables.Location = new System.Drawing.Point(7, 22);
            comboBoxListTables.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            comboBoxListTables.Name = "comboBoxListTables";
            comboBoxListTables.Size = new System.Drawing.Size(252, 23);
            comboBoxListTables.TabIndex = 16;
            comboBoxListTables.SelectedIndexChanged += comboBoxListTables_SelectedIndexChanged;
            // 
            // radioButtonSpot
            // 
            radioButtonSpot.AutoSize = true;
            radioButtonSpot.Location = new System.Drawing.Point(363, 19);
            radioButtonSpot.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            radioButtonSpot.Name = "radioButtonSpot";
            radioButtonSpot.Size = new System.Drawing.Size(49, 19);
            radioButtonSpot.TabIndex = 15;
            radioButtonSpot.Text = "Spot";
            radioButtonSpot.UseVisualStyleBackColor = true;
            radioButtonSpot.CheckedChanged += radioButtonCMYK_CheckedChanged;
            // 
            // radioButtonCMYK
            // 
            radioButtonCMYK.AutoSize = true;
            radioButtonCMYK.Checked = true;
            radioButtonCMYK.Location = new System.Drawing.Point(112, 19);
            radioButtonCMYK.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            radioButtonCMYK.Name = "radioButtonCMYK";
            radioButtonCMYK.Size = new System.Drawing.Size(58, 19);
            radioButtonCMYK.TabIndex = 14;
            radioButtonCMYK.TabStop = true;
            radioButtonCMYK.Text = "CMYK";
            radioButtonCMYK.UseVisualStyleBackColor = true;
            radioButtonCMYK.CheckedChanged += radioButtonCMYK_CheckedChanged;
            // 
            // bnt_OK
            // 
            bnt_OK.Location = new System.Drawing.Point(218, 206);
            bnt_OK.Name = "bnt_OK";
            bnt_OK.Size = new System.Drawing.Size(99, 39);
            bnt_OK.TabIndex = 18;
            bnt_OK.Text = "ОК";
            bnt_OK.UseVisualStyleBackColor = true;
            bnt_OK.Click += bnt_OK_Click;
            // 
            // gb_spot
            // 
            gb_spot.Controls.Add(panel_spot);
            gb_spot.Controls.Add(comboBoxListTables);
            gb_spot.Controls.Add(comboBoxListColor);
            gb_spot.Enabled = false;
            gb_spot.Location = new System.Drawing.Point(257, 44);
            gb_spot.Name = "gb_spot";
            gb_spot.Size = new System.Drawing.Size(266, 123);
            gb_spot.TabIndex = 19;
            gb_spot.TabStop = false;
            // 
            // panel_spot
            // 
            panel_spot.Location = new System.Drawing.Point(7, 80);
            panel_spot.Name = "panel_spot";
            panel_spot.Size = new System.Drawing.Size(252, 35);
            panel_spot.TabIndex = 18;
            // 
            // Form_PdfColorSelector
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(535, 257);
            Controls.Add(gb_spot);
            Controls.Add(bnt_OK);
            Controls.Add(radioButtonSpot);
            Controls.Add(radioButtonCMYK);
            Controls.Add(gb_cmyk);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form_PdfColorSelector";
            ShowIcon = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Вибрати колір";
            Load += Form_PdfColorSelector_Load;
            gb_cmyk.ResumeLayout(false);
            gb_cmyk.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numK).EndInit();
            ((System.ComponentModel.ISupportInitialize)numY).EndInit();
            ((System.ComponentModel.ISupportInitialize)numM).EndInit();
            ((System.ComponentModel.ISupportInitialize)numC).EndInit();
            gb_spot.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.GroupBox gb_cmyk;
        private System.Windows.Forms.CheckBox cb_b;
        private System.Windows.Forms.CheckBox cb_y;
        private System.Windows.Forms.CheckBox cb_m;
        private System.Windows.Forms.CheckBox cb_c;
        private System.Windows.Forms.NumericUpDown numK;
        private System.Windows.Forms.NumericUpDown numY;
        private System.Windows.Forms.NumericUpDown numM;
        private System.Windows.Forms.NumericUpDown numC;
        private System.Windows.Forms.ComboBox comboBoxListColor;
        private System.Windows.Forms.ComboBox comboBoxListTables;
        private System.Windows.Forms.RadioButton radioButtonSpot;
        private System.Windows.Forms.RadioButton radioButtonCMYK;
        private System.Windows.Forms.Button bnt_OK;
        private System.Windows.Forms.GroupBox gb_spot;
        private System.Windows.Forms.Panel panel_cmyk;
        private System.Windows.Forms.Panel panel_spot;
    }
}