namespace JobSpace.UserForms.PDF.ImposItems
{
    partial class Form_AddTemplateSheet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_AddTemplateSheet));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.nud_info_extraSpace = new System.Windows.Forms.NumericUpDown();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.nud_info_fieldBottom = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.nud_info_fieldTop = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.nud_info_fieldRight = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.nud_info_fieldLeft = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nud_info_h = new System.Windows.Forms.NumericUpDown();
            this.nud_info_w = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonEditSheet = new System.Windows.Forms.Button();
            this.buttonAddSheet = new System.Windows.Forms.Button();
            this.comboBoxSheetPlaceType = new System.Windows.Forms.ComboBox();
            this.comboBoxSheets = new System.Windows.Forms.ComboBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.cb_DrawProofColor = new System.Windows.Forms.CheckBox();
            this.nud_Yofs = new System.Windows.Forms.NumericUpDown();
            this.nud_Xofs = new System.Windows.Forms.NumericUpDown();
            this.cb_centerHeight = new System.Windows.Forms.CheckBox();
            this.cb_centerWidth = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.nud_page_bleed = new System.Windows.Forms.NumericUpDown();
            this.nud_page_h = new System.Windows.Forms.NumericUpDown();
            this.nud_page_w = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.previewControl1 = new JobSpace.UserForms.PDF.ImposItems.PreviewControl();
            this.buttonSave = new System.Windows.Forms.Button();
            this.cb_OneCut = new System.Windows.Forms.CheckBox();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_info_extraSpace)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_info_fieldBottom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_info_fieldTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_info_fieldRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_info_fieldLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_info_h)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_info_w)).BeginInit();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Yofs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Xofs)).BeginInit();
            this.groupBox6.SuspendLayout();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_page_bleed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_page_h)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_page_w)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.panel1);
            this.groupBox3.Controls.Add(this.buttonEditSheet);
            this.groupBox3.Controls.Add(this.buttonAddSheet);
            this.groupBox3.Controls.Add(this.comboBoxSheetPlaceType);
            this.groupBox3.Controls.Add(this.comboBoxSheets);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(232, 262);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Формат матеріалу";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox5);
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Controls.Add(this.nud_info_h);
            this.panel1.Controls.Add(this.nud_info_w);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(6, 73);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(220, 184);
            this.panel1.TabIndex = 4;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.nud_info_extraSpace);
            this.groupBox5.Location = new System.Drawing.Point(42, 132);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(125, 45);
            this.groupBox5.TabIndex = 5;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "додаткове поле";
            // 
            // nud_info_extraSpace
            // 
            this.nud_info_extraSpace.DecimalPlaces = 1;
            this.nud_info_extraSpace.Location = new System.Drawing.Point(31, 19);
            this.nud_info_extraSpace.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nud_info_extraSpace.Name = "nud_info_extraSpace";
            this.nud_info_extraSpace.Size = new System.Drawing.Size(52, 20);
            this.nud_info_extraSpace.TabIndex = 11;
            this.nud_info_extraSpace.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.nud_info_fieldBottom);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.nud_info_fieldTop);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.nud_info_fieldRight);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.nud_info_fieldLeft);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Location = new System.Drawing.Point(7, 30);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 96);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "поля";
            // 
            // nud_info_fieldBottom
            // 
            this.nud_info_fieldBottom.DecimalPlaces = 1;
            this.nud_info_fieldBottom.Location = new System.Drawing.Point(73, 70);
            this.nud_info_fieldBottom.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nud_info_fieldBottom.Name = "nud_info_fieldBottom";
            this.nud_info_fieldBottom.Size = new System.Drawing.Size(52, 20);
            this.nud_info_fieldBottom.TabIndex = 10;
            this.nud_info_fieldBottom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(76, 54);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "знизу";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // nud_info_fieldTop
            // 
            this.nud_info_fieldTop.DecimalPlaces = 1;
            this.nud_info_fieldTop.Location = new System.Drawing.Point(73, 31);
            this.nud_info_fieldTop.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nud_info_fieldTop.Name = "nud_info_fieldTop";
            this.nud_info_fieldTop.Size = new System.Drawing.Size(52, 20);
            this.nud_info_fieldTop.TabIndex = 8;
            this.nud_info_fieldTop.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(78, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "зверху";
            // 
            // nud_info_fieldRight
            // 
            this.nud_info_fieldRight.DecimalPlaces = 1;
            this.nud_info_fieldRight.Location = new System.Drawing.Point(137, 51);
            this.nud_info_fieldRight.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nud_info_fieldRight.Name = "nud_info_fieldRight";
            this.nud_info_fieldRight.Size = new System.Drawing.Size(52, 20);
            this.nud_info_fieldRight.TabIndex = 6;
            this.nud_info_fieldRight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(144, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "справа";
            // 
            // nud_info_fieldLeft
            // 
            this.nud_info_fieldLeft.DecimalPlaces = 1;
            this.nud_info_fieldLeft.Location = new System.Drawing.Point(11, 49);
            this.nud_info_fieldLeft.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nud_info_fieldLeft.Name = "nud_info_fieldLeft";
            this.nud_info_fieldLeft.Size = new System.Drawing.Size(52, 20);
            this.nud_info_fieldLeft.TabIndex = 4;
            this.nud_info_fieldLeft.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "зліва";
            // 
            // nud_info_h
            // 
            this.nud_info_h.DecimalPlaces = 1;
            this.nud_info_h.Location = new System.Drawing.Point(159, 4);
            this.nud_info_h.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nud_info_h.Name = "nud_info_h";
            this.nud_info_h.Size = new System.Drawing.Size(48, 20);
            this.nud_info_h.TabIndex = 3;
            this.nud_info_h.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // nud_info_w
            // 
            this.nud_info_w.DecimalPlaces = 1;
            this.nud_info_w.Location = new System.Drawing.Point(54, 4);
            this.nud_info_w.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nud_info_w.Name = "nud_info_w";
            this.nud_info_w.Size = new System.Drawing.Size(52, 20);
            this.nud_info_w.TabIndex = 2;
            this.nud_info_w.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(112, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "висота";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "ширина";
            // 
            // buttonEditSheet
            // 
            this.buttonEditSheet.Image = ((System.Drawing.Image)(resources.GetObject("buttonEditSheet.Image")));
            this.buttonEditSheet.Location = new System.Drawing.Point(199, 17);
            this.buttonEditSheet.Name = "buttonEditSheet";
            this.buttonEditSheet.Size = new System.Drawing.Size(27, 27);
            this.buttonEditSheet.TabIndex = 3;
            this.buttonEditSheet.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonEditSheet.UseVisualStyleBackColor = true;
            // 
            // buttonAddSheet
            // 
            this.buttonAddSheet.Image = ((System.Drawing.Image)(resources.GetObject("buttonAddSheet.Image")));
            this.buttonAddSheet.Location = new System.Drawing.Point(165, 17);
            this.buttonAddSheet.Name = "buttonAddSheet";
            this.buttonAddSheet.Size = new System.Drawing.Size(27, 27);
            this.buttonAddSheet.TabIndex = 2;
            this.buttonAddSheet.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonAddSheet.UseVisualStyleBackColor = true;
            // 
            // comboBoxSheetPlaceType
            // 
            this.comboBoxSheetPlaceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSheetPlaceType.FormattingEnabled = true;
            this.comboBoxSheetPlaceType.Location = new System.Drawing.Point(6, 46);
            this.comboBoxSheetPlaceType.Name = "comboBoxSheetPlaceType";
            this.comboBoxSheetPlaceType.Size = new System.Drawing.Size(144, 21);
            this.comboBoxSheetPlaceType.TabIndex = 1;
            // 
            // comboBoxSheets
            // 
            this.comboBoxSheets.DisplayMember = "Description";
            this.comboBoxSheets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSheets.FormattingEnabled = true;
            this.comboBoxSheets.Location = new System.Drawing.Point(6, 21);
            this.comboBoxSheets.Name = "comboBoxSheets";
            this.comboBoxSheets.Size = new System.Drawing.Size(144, 21);
            this.comboBoxSheets.TabIndex = 0;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.cb_OneCut);
            this.groupBox7.Controls.Add(this.cb_DrawProofColor);
            this.groupBox7.Controls.Add(this.nud_Yofs);
            this.groupBox7.Controls.Add(this.nud_Xofs);
            this.groupBox7.Controls.Add(this.cb_centerHeight);
            this.groupBox7.Controls.Add(this.cb_centerWidth);
            this.groupBox7.Location = new System.Drawing.Point(11, 389);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(227, 131);
            this.groupBox7.TabIndex = 6;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Додаткові параметри";
            // 
            // cb_DrawProofColor
            // 
            this.cb_DrawProofColor.AutoSize = true;
            this.cb_DrawProofColor.Checked = true;
            this.cb_DrawProofColor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_DrawProofColor.Location = new System.Drawing.Point(43, 89);
            this.cb_DrawProofColor.Name = "cb_DrawProofColor";
            this.cb_DrawProofColor.Size = new System.Drawing.Size(133, 17);
            this.cb_DrawProofColor.TabIndex = 9;
            this.cb_DrawProofColor.Text = "ProofColor на спусках";
            this.cb_DrawProofColor.UseVisualStyleBackColor = true;
            // 
            // nud_Yofs
            // 
            this.nud_Yofs.DecimalPlaces = 1;
            this.nud_Yofs.Location = new System.Drawing.Point(131, 60);
            this.nud_Yofs.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nud_Yofs.Name = "nud_Yofs";
            this.nud_Yofs.Size = new System.Drawing.Size(52, 20);
            this.nud_Yofs.TabIndex = 8;
            // 
            // nud_Xofs
            // 
            this.nud_Xofs.DecimalPlaces = 1;
            this.nud_Xofs.Location = new System.Drawing.Point(131, 26);
            this.nud_Xofs.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nud_Xofs.Name = "nud_Xofs";
            this.nud_Xofs.Size = new System.Drawing.Size(52, 20);
            this.nud_Xofs.TabIndex = 7;
            // 
            // cb_centerHeight
            // 
            this.cb_centerHeight.AutoSize = true;
            this.cb_centerHeight.Checked = true;
            this.cb_centerHeight.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_centerHeight.Image = ((System.Drawing.Image)(resources.GetObject("cb_centerHeight.Image")));
            this.cb_centerHeight.Location = new System.Drawing.Point(43, 53);
            this.cb_centerHeight.Name = "cb_centerHeight";
            this.cb_centerHeight.Size = new System.Drawing.Size(88, 32);
            this.cb_centerHeight.TabIndex = 1;
            this.cb_centerHeight.Text = "+ Yofs";
            this.cb_centerHeight.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.cb_centerHeight.UseVisualStyleBackColor = true;
            // 
            // cb_centerWidth
            // 
            this.cb_centerWidth.AutoSize = true;
            this.cb_centerWidth.Checked = true;
            this.cb_centerWidth.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_centerWidth.Image = ((System.Drawing.Image)(resources.GetObject("cb_centerWidth.Image")));
            this.cb_centerWidth.Location = new System.Drawing.Point(43, 19);
            this.cb_centerWidth.Name = "cb_centerWidth";
            this.cb_centerWidth.Size = new System.Drawing.Size(88, 32);
            this.cb_centerWidth.TabIndex = 0;
            this.cb_centerWidth.Text = "+ Xofs";
            this.cb_centerWidth.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.cb_centerWidth.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.groupBox8);
            this.groupBox6.Controls.Add(this.nud_page_h);
            this.groupBox6.Controls.Add(this.nud_page_w);
            this.groupBox6.Controls.Add(this.label7);
            this.groupBox6.Controls.Add(this.label8);
            this.groupBox6.Location = new System.Drawing.Point(12, 275);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(232, 108);
            this.groupBox6.TabIndex = 5;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Формат сторінки";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.nud_page_bleed);
            this.groupBox8.Location = new System.Drawing.Point(46, 49);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(125, 45);
            this.groupBox8.TabIndex = 9;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "поле на підрізку";
            // 
            // nud_page_bleed
            // 
            this.nud_page_bleed.DecimalPlaces = 1;
            this.nud_page_bleed.Location = new System.Drawing.Point(38, 19);
            this.nud_page_bleed.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nud_page_bleed.Name = "nud_page_bleed";
            this.nud_page_bleed.Size = new System.Drawing.Size(52, 20);
            this.nud_page_bleed.TabIndex = 11;
            this.nud_page_bleed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // nud_page_h
            // 
            this.nud_page_h.DecimalPlaces = 1;
            this.nud_page_h.Location = new System.Drawing.Point(172, 23);
            this.nud_page_h.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nud_page_h.Name = "nud_page_h";
            this.nud_page_h.Size = new System.Drawing.Size(48, 20);
            this.nud_page_h.TabIndex = 7;
            this.nud_page_h.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // nud_page_w
            // 
            this.nud_page_w.DecimalPlaces = 1;
            this.nud_page_w.Location = new System.Drawing.Point(58, 23);
            this.nud_page_w.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nud_page_w.Name = "nud_page_w";
            this.nud_page_w.Size = new System.Drawing.Size(52, 20);
            this.nud_page_w.TabIndex = 6;
            this.nud_page_w.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(125, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "висота";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 23);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "ширина";
            // 
            // previewControl1
            // 
            this.previewControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.previewControl1.Location = new System.Drawing.Point(250, 12);
            this.previewControl1.Name = "previewControl1";
            this.previewControl1.Size = new System.Drawing.Size(745, 559);
            this.previewControl1.TabIndex = 7;
            // 
            // buttonSave
            // 
            this.buttonSave.Image = ((System.Drawing.Image)(resources.GetObject("buttonSave.Image")));
            this.buttonSave.Location = new System.Drawing.Point(12, 526);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(227, 45);
            this.buttonSave.TabIndex = 9;
            this.buttonSave.Text = "Зберегти";
            this.buttonSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.button2_Click);
            // 
            // cb_OneCut
            // 
            this.cb_OneCut.AutoSize = true;
            this.cb_OneCut.Checked = true;
            this.cb_OneCut.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_OneCut.Location = new System.Drawing.Point(43, 108);
            this.cb_OneCut.Name = "cb_OneCut";
            this.cb_OneCut.Size = new System.Drawing.Size(76, 17);
            this.cb_OneCut.TabIndex = 10;
            this.cb_OneCut.Text = "в один різ";
            this.cb_OneCut.UseVisualStyleBackColor = true;
            // 
            // Form_AddTemplateSheet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1007, 583);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.previewControl1);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox3);
            this.Name = "Form_AddTemplateSheet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Додати шаблон листа";
            this.groupBox3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nud_info_extraSpace)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_info_fieldBottom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_info_fieldTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_info_fieldRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_info_fieldLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_info_h)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_info_w)).EndInit();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Yofs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Xofs)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nud_page_bleed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_page_h)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_page_w)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.NumericUpDown nud_info_extraSpace;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.NumericUpDown nud_info_fieldBottom;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nud_info_fieldTop;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nud_info_fieldRight;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nud_info_fieldLeft;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nud_info_h;
        private System.Windows.Forms.NumericUpDown nud_info_w;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonEditSheet;
        private System.Windows.Forms.Button buttonAddSheet;
        private System.Windows.Forms.ComboBox comboBoxSheetPlaceType;
        private System.Windows.Forms.ComboBox comboBoxSheets;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.CheckBox cb_DrawProofColor;
        private System.Windows.Forms.NumericUpDown nud_Yofs;
        private System.Windows.Forms.NumericUpDown nud_Xofs;
        private System.Windows.Forms.CheckBox cb_centerHeight;
        private System.Windows.Forms.CheckBox cb_centerWidth;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.NumericUpDown nud_page_bleed;
        private System.Windows.Forms.NumericUpDown nud_page_h;
        private System.Windows.Forms.NumericUpDown nud_page_w;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private PreviewControl previewControl1;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.CheckBox cb_OneCut;
    }
}