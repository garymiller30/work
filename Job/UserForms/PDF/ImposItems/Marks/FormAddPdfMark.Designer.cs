namespace JobSpace.UserForms.PDF.ImposItems
{
    partial class FormAddPdfMark
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAddPdfMark));
            groupBox1 = new System.Windows.Forms.GroupBox();
            btn_SelectPdfFile = new System.Windows.Forms.Button();
            tb_markPath = new System.Windows.Forms.TextBox();
            tb_name = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            groupBox3 = new System.Windows.Forms.GroupBox();
            cb_backMirror = new System.Windows.Forms.CheckBox();
            groupBox8 = new System.Windows.Forms.GroupBox();
            groupBox10 = new System.Windows.Forms.GroupBox();
            rb_y_relative_subject = new System.Windows.Forms.RadioButton();
            rb_y_relative_sheet = new System.Windows.Forms.RadioButton();
            groupBox9 = new System.Windows.Forms.GroupBox();
            rb_x_relative_subjet = new System.Windows.Forms.RadioButton();
            rb_x_relative_sheet = new System.Windows.Forms.RadioButton();
            cb_auto_clip_y = new System.Windows.Forms.CheckBox();
            cb_auto_clip_x = new System.Windows.Forms.CheckBox();
            nud_clip_bottom = new System.Windows.Forms.NumericUpDown();
            label10 = new System.Windows.Forms.Label();
            nud_clip_top = new System.Windows.Forms.NumericUpDown();
            label9 = new System.Windows.Forms.Label();
            nud_clip_right = new System.Windows.Forms.NumericUpDown();
            label7 = new System.Windows.Forms.Label();
            nud_clip_left = new System.Windows.Forms.NumericUpDown();
            label8 = new System.Windows.Forms.Label();
            cb_foreground = new System.Windows.Forms.CheckBox();
            cb_Angle = new System.Windows.Forms.ComboBox();
            label6 = new System.Windows.Forms.Label();
            groupBox7 = new System.Windows.Forms.GroupBox();
            selectMarkSideControl1 = new SelectMarkSideControl();
            groupBox4 = new System.Windows.Forms.GroupBox();
            groupBox2 = new System.Windows.Forms.GroupBox();
            rb_parentSubject = new System.Windows.Forms.RadioButton();
            rb_parentSheet = new System.Windows.Forms.RadioButton();
            groupBox6 = new System.Windows.Forms.GroupBox();
            label5 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            nud_yOfs = new System.Windows.Forms.NumericUpDown();
            nud_Xofs = new System.Windows.Forms.NumericUpDown();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            apc_parent = new AnchorPointControl();
            groupBox5 = new System.Windows.Forms.GroupBox();
            apc_mark = new AnchorPointControl();
            btn_ok = new System.Windows.Forms.Button();
            groupBox1.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox8.SuspendLayout();
            groupBox10.SuspendLayout();
            groupBox9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nud_clip_bottom).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nud_clip_top).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nud_clip_right).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nud_clip_left).BeginInit();
            groupBox7.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nud_yOfs).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nud_Xofs).BeginInit();
            groupBox5.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btn_SelectPdfFile);
            groupBox1.Controls.Add(tb_markPath);
            groupBox1.Location = new System.Drawing.Point(264, 3);
            groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Size = new System.Drawing.Size(489, 57);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "шлях до мітки";
            // 
            // btn_SelectPdfFile
            // 
            btn_SelectPdfFile.Location = new System.Drawing.Point(435, 18);
            btn_SelectPdfFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btn_SelectPdfFile.Name = "btn_SelectPdfFile";
            btn_SelectPdfFile.Size = new System.Drawing.Size(46, 27);
            btn_SelectPdfFile.TabIndex = 1;
            btn_SelectPdfFile.Text = "...";
            btn_SelectPdfFile.UseVisualStyleBackColor = true;
            btn_SelectPdfFile.Click += btn_SelectPdfFile_Click;
            // 
            // tb_markPath
            // 
            tb_markPath.Location = new System.Drawing.Point(7, 22);
            tb_markPath.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tb_markPath.Name = "tb_markPath";
            tb_markPath.Size = new System.Drawing.Size(420, 23);
            tb_markPath.TabIndex = 0;
            // 
            // tb_name
            // 
            tb_name.Location = new System.Drawing.Point(75, 25);
            tb_name.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tb_name.Name = "tb_name";
            tb_name.Size = new System.Drawing.Size(177, 23);
            tb_name.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(22, 29);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(39, 15);
            label1.TabIndex = 1;
            label1.Text = "Назва";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(cb_backMirror);
            groupBox3.Controls.Add(groupBox8);
            groupBox3.Controls.Add(cb_foreground);
            groupBox3.Controls.Add(cb_Angle);
            groupBox3.Controls.Add(label6);
            groupBox3.Controls.Add(groupBox7);
            groupBox3.Controls.Add(groupBox4);
            groupBox3.Location = new System.Drawing.Point(14, 67);
            groupBox3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox3.Size = new System.Drawing.Size(738, 414);
            groupBox3.TabIndex = 0;
            groupBox3.TabStop = false;
            groupBox3.Text = "Параметри";
            // 
            // cb_backMirror
            // 
            cb_backMirror.Location = new System.Drawing.Point(479, 134);
            cb_backMirror.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cb_backMirror.Name = "cb_backMirror";
            cb_backMirror.Size = new System.Drawing.Size(240, 30);
            cb_backMirror.TabIndex = 2;
            cb_backMirror.Text = "дзеркальне положення на звороті";
            cb_backMirror.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            groupBox8.Controls.Add(groupBox10);
            groupBox8.Controls.Add(groupBox9);
            groupBox8.Controls.Add(cb_auto_clip_y);
            groupBox8.Controls.Add(cb_auto_clip_x);
            groupBox8.Controls.Add(nud_clip_bottom);
            groupBox8.Controls.Add(label10);
            groupBox8.Controls.Add(nud_clip_top);
            groupBox8.Controls.Add(label9);
            groupBox8.Controls.Add(nud_clip_right);
            groupBox8.Controls.Add(label7);
            groupBox8.Controls.Add(nud_clip_left);
            groupBox8.Controls.Add(label8);
            groupBox8.Location = new System.Drawing.Point(7, 225);
            groupBox8.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox8.Name = "groupBox8";
            groupBox8.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox8.Size = new System.Drawing.Size(589, 182);
            groupBox8.TabIndex = 7;
            groupBox8.TabStop = false;
            groupBox8.Text = "Обрізати, мм";
            // 
            // groupBox10
            // 
            groupBox10.Controls.Add(rb_y_relative_subject);
            groupBox10.Controls.Add(rb_y_relative_sheet);
            groupBox10.Location = new System.Drawing.Point(408, 95);
            groupBox10.Margin = new System.Windows.Forms.Padding(0);
            groupBox10.Name = "groupBox10";
            groupBox10.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox10.Size = new System.Drawing.Size(174, 69);
            groupBox10.TabIndex = 18;
            groupBox10.TabStop = false;
            // 
            // rb_y_relative_subject
            // 
            rb_y_relative_subject.AutoSize = true;
            rb_y_relative_subject.Location = new System.Drawing.Point(8, 43);
            rb_y_relative_subject.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            rb_y_relative_subject.Name = "rb_y_relative_subject";
            rb_y_relative_subject.Size = new System.Drawing.Size(120, 19);
            rb_y_relative_subject.TabIndex = 1;
            rb_y_relative_subject.TabStop = true;
            rb_y_relative_subject.Text = "Відносно сюжету";
            rb_y_relative_subject.UseVisualStyleBackColor = true;
            // 
            // rb_y_relative_sheet
            // 
            rb_y_relative_sheet.AutoSize = true;
            rb_y_relative_sheet.Location = new System.Drawing.Point(8, 17);
            rb_y_relative_sheet.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            rb_y_relative_sheet.Name = "rb_y_relative_sheet";
            rb_y_relative_sheet.Size = new System.Drawing.Size(109, 19);
            rb_y_relative_sheet.TabIndex = 0;
            rb_y_relative_sheet.TabStop = true;
            rb_y_relative_sheet.Text = "Відносно листа";
            rb_y_relative_sheet.UseVisualStyleBackColor = true;
            // 
            // groupBox9
            // 
            groupBox9.Controls.Add(rb_x_relative_subjet);
            groupBox9.Controls.Add(rb_x_relative_sheet);
            groupBox9.Location = new System.Drawing.Point(408, 18);
            groupBox9.Margin = new System.Windows.Forms.Padding(0);
            groupBox9.Name = "groupBox9";
            groupBox9.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox9.Size = new System.Drawing.Size(174, 69);
            groupBox9.TabIndex = 17;
            groupBox9.TabStop = false;
            // 
            // rb_x_relative_subjet
            // 
            rb_x_relative_subjet.AutoSize = true;
            rb_x_relative_subjet.Location = new System.Drawing.Point(8, 43);
            rb_x_relative_subjet.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            rb_x_relative_subjet.Name = "rb_x_relative_subjet";
            rb_x_relative_subjet.Size = new System.Drawing.Size(120, 19);
            rb_x_relative_subjet.TabIndex = 1;
            rb_x_relative_subjet.TabStop = true;
            rb_x_relative_subjet.Text = "Відносно сюжету";
            rb_x_relative_subjet.UseVisualStyleBackColor = true;
            // 
            // rb_x_relative_sheet
            // 
            rb_x_relative_sheet.AutoSize = true;
            rb_x_relative_sheet.Location = new System.Drawing.Point(8, 17);
            rb_x_relative_sheet.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            rb_x_relative_sheet.Name = "rb_x_relative_sheet";
            rb_x_relative_sheet.Size = new System.Drawing.Size(109, 19);
            rb_x_relative_sheet.TabIndex = 0;
            rb_x_relative_sheet.TabStop = true;
            rb_x_relative_sheet.Text = "Відносно листа";
            rb_x_relative_sheet.UseVisualStyleBackColor = true;
            // 
            // cb_auto_clip_y
            // 
            cb_auto_clip_y.AutoSize = true;
            cb_auto_clip_y.Location = new System.Drawing.Point(265, 123);
            cb_auto_clip_y.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cb_auto_clip_y.Name = "cb_auto_clip_y";
            cb_auto_clip_y.Size = new System.Drawing.Size(127, 19);
            cb_auto_clip_y.TabIndex = 16;
            cb_auto_clip_y.Text = "Автоматично по Y";
            cb_auto_clip_y.UseVisualStyleBackColor = true;
            // 
            // cb_auto_clip_x
            // 
            cb_auto_clip_x.AutoSize = true;
            cb_auto_clip_x.Location = new System.Drawing.Point(265, 47);
            cb_auto_clip_x.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cb_auto_clip_x.Name = "cb_auto_clip_x";
            cb_auto_clip_x.Size = new System.Drawing.Size(127, 19);
            cb_auto_clip_x.TabIndex = 15;
            cb_auto_clip_x.Text = "Автоматично по X";
            cb_auto_clip_x.UseVisualStyleBackColor = true;
            // 
            // nud_clip_bottom
            // 
            nud_clip_bottom.DecimalPlaces = 1;
            nud_clip_bottom.Location = new System.Drawing.Point(86, 95);
            nud_clip_bottom.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            nud_clip_bottom.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            nud_clip_bottom.Name = "nud_clip_bottom";
            nud_clip_bottom.Size = new System.Drawing.Size(50, 23);
            nud_clip_bottom.TabIndex = 14;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new System.Drawing.Point(90, 76);
            label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(37, 15);
            label10.TabIndex = 13;
            label10.Text = "знизу";
            // 
            // nud_clip_top
            // 
            nud_clip_top.DecimalPlaces = 1;
            nud_clip_top.Location = new System.Drawing.Point(88, 39);
            nud_clip_top.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            nud_clip_top.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            nud_clip_top.Name = "nud_clip_top";
            nud_clip_top.Size = new System.Drawing.Size(50, 23);
            nud_clip_top.TabIndex = 12;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(89, 22);
            label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(43, 15);
            label9.TabIndex = 11;
            label9.Text = "зверху";
            // 
            // nud_clip_right
            // 
            nud_clip_right.DecimalPlaces = 1;
            nud_clip_right.Location = new System.Drawing.Point(158, 66);
            nud_clip_right.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            nud_clip_right.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            nud_clip_right.Name = "nud_clip_right";
            nud_clip_right.Size = new System.Drawing.Size(50, 23);
            nud_clip_right.TabIndex = 10;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(158, 47);
            label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(45, 15);
            label7.TabIndex = 9;
            label7.Text = "справа";
            // 
            // nud_clip_left
            // 
            nud_clip_left.DecimalPlaces = 1;
            nud_clip_left.Location = new System.Drawing.Point(14, 66);
            nud_clip_left.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            nud_clip_left.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            nud_clip_left.Name = "nud_clip_left";
            nud_clip_left.Size = new System.Drawing.Size(50, 23);
            nud_clip_left.TabIndex = 8;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(20, 47);
            label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(34, 15);
            label8.TabIndex = 7;
            label8.Text = "зліва";
            // 
            // cb_foreground
            // 
            cb_foreground.AutoSize = true;
            cb_foreground.Location = new System.Drawing.Point(399, 141);
            cb_foreground.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cb_foreground.Name = "cb_foreground";
            cb_foreground.Size = new System.Drawing.Size(62, 19);
            cb_foreground.TabIndex = 6;
            cb_foreground.Text = "зверху";
            cb_foreground.UseVisualStyleBackColor = true;
            // 
            // cb_Angle
            // 
            cb_Angle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cb_Angle.FormattingEnabled = true;
            cb_Angle.Location = new System.Drawing.Point(495, 167);
            cb_Angle.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cb_Angle.Name = "cb_Angle";
            cb_Angle.Size = new System.Drawing.Size(101, 23);
            cb_Angle.TabIndex = 5;
            // 
            // label6
            // 
            label6.Image = (System.Drawing.Image)resources.GetObject("label6.Image");
            label6.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            label6.Location = new System.Drawing.Point(402, 167);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(85, 27);
            label6.TabIndex = 4;
            label6.Text = "Поворот";
            label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox7
            // 
            groupBox7.Controls.Add(selectMarkSideControl1);
            groupBox7.Location = new System.Drawing.Point(392, 16);
            groupBox7.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox7.Name = "groupBox7";
            groupBox7.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox7.Size = new System.Drawing.Size(338, 118);
            groupBox7.TabIndex = 3;
            groupBox7.TabStop = false;
            // 
            // selectMarkSideControl1
            // 
            selectMarkSideControl1.Location = new System.Drawing.Point(7, 15);
            selectMarkSideControl1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            selectMarkSideControl1.Name = "selectMarkSideControl1";
            selectMarkSideControl1.Size = new System.Drawing.Size(328, 96);
            selectMarkSideControl1.TabIndex = 8;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(groupBox2);
            groupBox4.Controls.Add(groupBox6);
            groupBox4.Controls.Add(groupBox5);
            groupBox4.Location = new System.Drawing.Point(7, 22);
            groupBox4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox4.Name = "groupBox4";
            groupBox4.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox4.Size = new System.Drawing.Size(378, 196);
            groupBox4.TabIndex = 2;
            groupBox4.TabStop = false;
            groupBox4.Text = "Положення відносно";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(rb_parentSubject);
            groupBox2.Controls.Add(rb_parentSheet);
            groupBox2.Location = new System.Drawing.Point(7, 145);
            groupBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox2.Size = new System.Drawing.Size(364, 44);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            // 
            // rb_parentSubject
            // 
            rb_parentSubject.AutoSize = true;
            rb_parentSubject.Location = new System.Drawing.Point(188, 17);
            rb_parentSubject.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            rb_parentSubject.Name = "rb_parentSubject";
            rb_parentSubject.Size = new System.Drawing.Size(119, 19);
            rb_parentSubject.TabIndex = 1;
            rb_parentSubject.TabStop = true;
            rb_parentSubject.Text = "відносно сюжету";
            rb_parentSubject.UseVisualStyleBackColor = true;
            // 
            // rb_parentSheet
            // 
            rb_parentSheet.AutoSize = true;
            rb_parentSheet.Location = new System.Drawing.Point(47, 17);
            rb_parentSheet.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            rb_parentSheet.Name = "rb_parentSheet";
            rb_parentSheet.Size = new System.Drawing.Size(108, 19);
            rb_parentSheet.TabIndex = 0;
            rb_parentSheet.TabStop = true;
            rb_parentSheet.Text = "відносно листа";
            rb_parentSheet.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(label5);
            groupBox6.Controls.Add(label4);
            groupBox6.Controls.Add(nud_yOfs);
            groupBox6.Controls.Add(nud_Xofs);
            groupBox6.Controls.Add(label3);
            groupBox6.Controls.Add(label2);
            groupBox6.Controls.Add(apc_parent);
            groupBox6.Location = new System.Drawing.Point(119, 23);
            groupBox6.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox6.Name = "groupBox6";
            groupBox6.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox6.Size = new System.Drawing.Size(252, 115);
            groupBox6.TabIndex = 1;
            groupBox6.TabStop = false;
            groupBox6.Text = "батьківського елемента";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(211, 74);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(25, 15);
            label5.TabIndex = 7;
            label5.Text = "мм";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(212, 35);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(25, 15);
            label4.TabIndex = 6;
            label4.Text = "мм";
            // 
            // nud_yOfs
            // 
            nud_yOfs.DecimalPlaces = 1;
            nud_yOfs.Location = new System.Drawing.Point(154, 72);
            nud_yOfs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            nud_yOfs.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            nud_yOfs.Minimum = new decimal(new int[] { 1000000, 0, 0, int.MinValue });
            nud_yOfs.Name = "nud_yOfs";
            nud_yOfs.Size = new System.Drawing.Size(50, 23);
            nud_yOfs.TabIndex = 5;
            nud_yOfs.Click += nud_Xofs_Click;
            nud_yOfs.Enter += nud_Xofs_Click;
            // 
            // nud_Xofs
            // 
            nud_Xofs.DecimalPlaces = 1;
            nud_Xofs.Location = new System.Drawing.Point(154, 32);
            nud_Xofs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            nud_Xofs.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            nud_Xofs.Minimum = new decimal(new int[] { 1000000, 0, 0, int.MinValue });
            nud_Xofs.Name = "nud_Xofs";
            nud_Xofs.Size = new System.Drawing.Size(50, 23);
            nud_Xofs.TabIndex = 4;
            nud_Xofs.Click += nud_Xofs_Click;
            nud_Xofs.Enter += nud_Xofs_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(107, 74);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(36, 15);
            label3.TabIndex = 3;
            label3.Text = "Y ofs:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(107, 35);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(36, 15);
            label2.TabIndex = 2;
            label2.Text = "X ofs:";
            // 
            // apc_parent
            // 
            apc_parent.AnchorPointChanged = null;
            apc_parent.Location = new System.Drawing.Point(7, 16);
            apc_parent.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            apc_parent.Name = "apc_parent";
            apc_parent.Size = new System.Drawing.Size(93, 92);
            apc_parent.TabIndex = 1;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(apc_mark);
            groupBox5.Location = new System.Drawing.Point(7, 23);
            groupBox5.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox5.Name = "groupBox5";
            groupBox5.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox5.Size = new System.Drawing.Size(105, 115);
            groupBox5.TabIndex = 0;
            groupBox5.TabStop = false;
            groupBox5.Text = "мітки";
            // 
            // apc_mark
            // 
            apc_mark.AnchorPointChanged = null;
            apc_mark.Location = new System.Drawing.Point(7, 16);
            apc_mark.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            apc_mark.Name = "apc_mark";
            apc_mark.Size = new System.Drawing.Size(93, 92);
            apc_mark.TabIndex = 0;
            // 
            // btn_ok
            // 
            btn_ok.Location = new System.Drawing.Point(324, 488);
            btn_ok.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btn_ok.Name = "btn_ok";
            btn_ok.Size = new System.Drawing.Size(135, 44);
            btn_ok.TabIndex = 3;
            btn_ok.Text = "OK";
            btn_ok.UseVisualStyleBackColor = true;
            btn_ok.Click += btn_ok_Click;
            // 
            // FormAddPdfMark
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(766, 546);
            Controls.Add(btn_ok);
            Controls.Add(groupBox3);
            Controls.Add(tb_name);
            Controls.Add(label1);
            Controls.Add(groupBox1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormAddPdfMark";
            ShowIcon = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "PDF Мітка";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox8.ResumeLayout(false);
            groupBox8.PerformLayout();
            groupBox10.ResumeLayout(false);
            groupBox10.PerformLayout();
            groupBox9.ResumeLayout(false);
            groupBox9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nud_clip_bottom).EndInit();
            ((System.ComponentModel.ISupportInitialize)nud_clip_top).EndInit();
            ((System.ComponentModel.ISupportInitialize)nud_clip_right).EndInit();
            ((System.ComponentModel.ISupportInitialize)nud_clip_left).EndInit();
            groupBox7.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nud_yOfs).EndInit();
            ((System.ComponentModel.ISupportInitialize)nud_Xofs).EndInit();
            groupBox5.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tb_markPath;
        private System.Windows.Forms.Button btn_SelectPdfFile;
        private System.Windows.Forms.TextBox tb_name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private AnchorPointControl apc_parent;
        private AnchorPointControl apc_mark;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.NumericUpDown nud_Xofs;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nud_yOfs;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.CheckBox cb_backMirror;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.ComboBox cb_Angle;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rb_parentSubject;
        private System.Windows.Forms.RadioButton rb_parentSheet;
        private System.Windows.Forms.CheckBox cb_foreground;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.NumericUpDown nud_clip_bottom;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown nud_clip_top;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown nud_clip_right;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nud_clip_left;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox cb_auto_clip_y;
        private System.Windows.Forms.CheckBox cb_auto_clip_x;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.RadioButton rb_x_relative_sheet;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.RadioButton rb_y_relative_subject;
        private System.Windows.Forms.RadioButton rb_y_relative_sheet;
        private System.Windows.Forms.RadioButton rb_x_relative_subjet;
        private SelectMarkSideControl selectMarkSideControl1;
    }
}