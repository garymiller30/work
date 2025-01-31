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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_SelectPdfFile = new System.Windows.Forms.Button();
            this.tb_markPath = new System.Windows.Forms.TextBox();
            this.tb_name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cb_foreground = new System.Windows.Forms.CheckBox();
            this.cb_Angle = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.cb_backMirror = new System.Windows.Forms.CheckBox();
            this.cb_back = new System.Windows.Forms.CheckBox();
            this.cb_front = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rb_parentSubject = new System.Windows.Forms.RadioButton();
            this.rb_parentSheet = new System.Windows.Forms.RadioButton();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.nud_yOfs = new System.Windows.Forms.NumericUpDown();
            this.nud_Xofs = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btn_ok = new System.Windows.Forms.Button();
            this.vistaOpenFileDialog1 = new Ookii.Dialogs.WinForms.VistaOpenFileDialog();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.nud_clip_left = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.nud_clip_right = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.nud_clip_top = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.nud_clip_bottom = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.cb_auto_clip_x = new System.Windows.Forms.CheckBox();
            this.cb_auto_clip_y = new System.Windows.Forms.CheckBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.rb_x_relative_sheet = new System.Windows.Forms.RadioButton();
            this.rb_x_relative_subjet = new System.Windows.Forms.RadioButton();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.rb_y_relative_subject = new System.Windows.Forms.RadioButton();
            this.rb_y_relative_sheet = new System.Windows.Forms.RadioButton();
            this.apc_parent = new JobSpace.UserForms.PDF.ImposItems.AnchorPointControl();
            this.apc_mark = new JobSpace.UserForms.PDF.ImposItems.AnchorPointControl();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_yOfs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Xofs)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_clip_left)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_clip_right)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_clip_top)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_clip_bottom)).BeginInit();
            this.groupBox9.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_SelectPdfFile);
            this.groupBox1.Controls.Add(this.tb_markPath);
            this.groupBox1.Location = new System.Drawing.Point(226, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(325, 49);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "шлях до мітки";
            // 
            // btn_SelectPdfFile
            // 
            this.btn_SelectPdfFile.Location = new System.Drawing.Point(278, 17);
            this.btn_SelectPdfFile.Name = "btn_SelectPdfFile";
            this.btn_SelectPdfFile.Size = new System.Drawing.Size(39, 23);
            this.btn_SelectPdfFile.TabIndex = 1;
            this.btn_SelectPdfFile.Text = "...";
            this.btn_SelectPdfFile.UseVisualStyleBackColor = true;
            this.btn_SelectPdfFile.Click += new System.EventHandler(this.btn_SelectPdfFile_Click);
            // 
            // tb_markPath
            // 
            this.tb_markPath.Location = new System.Drawing.Point(6, 19);
            this.tb_markPath.Name = "tb_markPath";
            this.tb_markPath.Size = new System.Drawing.Size(266, 20);
            this.tb_markPath.TabIndex = 0;
            // 
            // tb_name
            // 
            this.tb_name.Location = new System.Drawing.Point(64, 22);
            this.tb_name.Name = "tb_name";
            this.tb_name.Size = new System.Drawing.Size(152, 20);
            this.tb_name.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Назва";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox8);
            this.groupBox3.Controls.Add(this.cb_foreground);
            this.groupBox3.Controls.Add(this.cb_Angle);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.groupBox7);
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Location = new System.Drawing.Point(12, 58);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(539, 359);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Параметри";
            // 
            // cb_foreground
            // 
            this.cb_foreground.AutoSize = true;
            this.cb_foreground.Location = new System.Drawing.Point(405, 160);
            this.cb_foreground.Name = "cb_foreground";
            this.cb_foreground.Size = new System.Drawing.Size(60, 17);
            this.cb_foreground.TabIndex = 6;
            this.cb_foreground.Text = "зверху";
            this.cb_foreground.UseVisualStyleBackColor = true;
            // 
            // cb_Angle
            // 
            this.cb_Angle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Angle.FormattingEnabled = true;
            this.cb_Angle.Location = new System.Drawing.Point(418, 123);
            this.cb_Angle.Name = "cb_Angle";
            this.cb_Angle.Size = new System.Drawing.Size(87, 21);
            this.cb_Angle.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.Image = ((System.Drawing.Image)(resources.GetObject("label6.Image")));
            this.label6.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label6.Location = new System.Drawing.Point(339, 123);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 23);
            this.label6.TabIndex = 4;
            this.label6.Text = "Поворот";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.cb_backMirror);
            this.groupBox7.Controls.Add(this.cb_back);
            this.groupBox7.Controls.Add(this.cb_front);
            this.groupBox7.Location = new System.Drawing.Point(336, 14);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(175, 102);
            this.groupBox7.TabIndex = 3;
            this.groupBox7.TabStop = false;
            // 
            // cb_backMirror
            // 
            this.cb_backMirror.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_backMirror.Location = new System.Drawing.Point(6, 54);
            this.cb_backMirror.Name = "cb_backMirror";
            this.cb_backMirror.Size = new System.Drawing.Size(163, 38);
            this.cb_backMirror.TabIndex = 2;
            this.cb_backMirror.Text = "дзеркальне положення на звороті";
            this.cb_backMirror.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_backMirror.UseVisualStyleBackColor = true;
            // 
            // cb_back
            // 
            this.cb_back.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_back.Location = new System.Drawing.Point(92, 15);
            this.cb_back.Name = "cb_back";
            this.cb_back.Size = new System.Drawing.Size(77, 33);
            this.cb_back.TabIndex = 1;
            this.cb_back.Text = "зворот";
            this.cb_back.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_back.UseVisualStyleBackColor = true;
            // 
            // cb_front
            // 
            this.cb_front.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_front.Location = new System.Drawing.Point(6, 15);
            this.cb_front.Name = "cb_front";
            this.cb_front.Size = new System.Drawing.Size(80, 33);
            this.cb_front.TabIndex = 0;
            this.cb_front.Text = "лице";
            this.cb_front.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_front.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.groupBox2);
            this.groupBox4.Controls.Add(this.groupBox6);
            this.groupBox4.Controls.Add(this.groupBox5);
            this.groupBox4.Location = new System.Drawing.Point(6, 19);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(324, 170);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Положення відносно";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rb_parentSubject);
            this.groupBox2.Controls.Add(this.rb_parentSheet);
            this.groupBox2.Location = new System.Drawing.Point(6, 126);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(312, 38);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            // 
            // rb_parentSubject
            // 
            this.rb_parentSubject.AutoSize = true;
            this.rb_parentSubject.Location = new System.Drawing.Point(161, 15);
            this.rb_parentSubject.Name = "rb_parentSubject";
            this.rb_parentSubject.Size = new System.Drawing.Size(110, 17);
            this.rb_parentSubject.TabIndex = 1;
            this.rb_parentSubject.TabStop = true;
            this.rb_parentSubject.Text = "відносно сюжету";
            this.rb_parentSubject.UseVisualStyleBackColor = true;
            // 
            // rb_parentSheet
            // 
            this.rb_parentSheet.AutoSize = true;
            this.rb_parentSheet.Location = new System.Drawing.Point(40, 15);
            this.rb_parentSheet.Name = "rb_parentSheet";
            this.rb_parentSheet.Size = new System.Drawing.Size(101, 17);
            this.rb_parentSheet.TabIndex = 0;
            this.rb_parentSheet.TabStop = true;
            this.rb_parentSheet.Text = "відносно листа";
            this.rb_parentSheet.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label5);
            this.groupBox6.Controls.Add(this.label4);
            this.groupBox6.Controls.Add(this.nud_yOfs);
            this.groupBox6.Controls.Add(this.nud_Xofs);
            this.groupBox6.Controls.Add(this.label3);
            this.groupBox6.Controls.Add(this.label2);
            this.groupBox6.Controls.Add(this.apc_parent);
            this.groupBox6.Location = new System.Drawing.Point(102, 20);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(216, 100);
            this.groupBox6.TabIndex = 1;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "батьківського елемента";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(181, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "мм";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(182, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "мм";
            // 
            // nud_yOfs
            // 
            this.nud_yOfs.DecimalPlaces = 1;
            this.nud_yOfs.Location = new System.Drawing.Point(132, 62);
            this.nud_yOfs.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_yOfs.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nud_yOfs.Name = "nud_yOfs";
            this.nud_yOfs.Size = new System.Drawing.Size(43, 20);
            this.nud_yOfs.TabIndex = 5;
            this.nud_yOfs.Click += new System.EventHandler(this.nud_Xofs_Click);
            this.nud_yOfs.Enter += new System.EventHandler(this.nud_Xofs_Click);
            // 
            // nud_Xofs
            // 
            this.nud_Xofs.DecimalPlaces = 1;
            this.nud_Xofs.Location = new System.Drawing.Point(132, 28);
            this.nud_Xofs.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nud_Xofs.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nud_Xofs.Name = "nud_Xofs";
            this.nud_Xofs.Size = new System.Drawing.Size(43, 20);
            this.nud_Xofs.TabIndex = 4;
            this.nud_Xofs.Click += new System.EventHandler(this.nud_Xofs_Click);
            this.nud_Xofs.Enter += new System.EventHandler(this.nud_Xofs_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(92, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Y ofs:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(92, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "X ofs:";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.apc_mark);
            this.groupBox5.Location = new System.Drawing.Point(6, 20);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(90, 100);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "мітки";
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(215, 423);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(116, 38);
            this.btn_ok.TabIndex = 3;
            this.btn_ok.Text = "OK";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // vistaOpenFileDialog1
            // 
            this.vistaOpenFileDialog1.DefaultExt = "pdf";
            this.vistaOpenFileDialog1.Filter = "pdf|*.pdf";
            this.vistaOpenFileDialog1.SupportMultiDottedExtensions = true;
            this.vistaOpenFileDialog1.Title = "Вибрати PDF файл";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.groupBox10);
            this.groupBox8.Controls.Add(this.groupBox9);
            this.groupBox8.Controls.Add(this.cb_auto_clip_y);
            this.groupBox8.Controls.Add(this.cb_auto_clip_x);
            this.groupBox8.Controls.Add(this.nud_clip_bottom);
            this.groupBox8.Controls.Add(this.label10);
            this.groupBox8.Controls.Add(this.nud_clip_top);
            this.groupBox8.Controls.Add(this.label9);
            this.groupBox8.Controls.Add(this.nud_clip_right);
            this.groupBox8.Controls.Add(this.label7);
            this.groupBox8.Controls.Add(this.nud_clip_left);
            this.groupBox8.Controls.Add(this.label8);
            this.groupBox8.Location = new System.Drawing.Point(6, 195);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(505, 158);
            this.groupBox8.TabIndex = 7;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Обрізати, мм";
            // 
            // nud_clip_left
            // 
            this.nud_clip_left.DecimalPlaces = 1;
            this.nud_clip_left.Location = new System.Drawing.Point(12, 57);
            this.nud_clip_left.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nud_clip_left.Name = "nud_clip_left";
            this.nud_clip_left.Size = new System.Drawing.Size(43, 20);
            this.nud_clip_left.TabIndex = 8;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(17, 41);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(33, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "зліва";
            // 
            // nud_clip_right
            // 
            this.nud_clip_right.DecimalPlaces = 1;
            this.nud_clip_right.Location = new System.Drawing.Point(135, 57);
            this.nud_clip_right.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nud_clip_right.Name = "nud_clip_right";
            this.nud_clip_right.Size = new System.Drawing.Size(43, 20);
            this.nud_clip_right.TabIndex = 10;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(135, 41);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "справа";
            // 
            // nud_clip_top
            // 
            this.nud_clip_top.DecimalPlaces = 1;
            this.nud_clip_top.Location = new System.Drawing.Point(75, 34);
            this.nud_clip_top.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nud_clip_top.Name = "nud_clip_top";
            this.nud_clip_top.Size = new System.Drawing.Size(43, 20);
            this.nud_clip_top.TabIndex = 12;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(76, 19);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 13);
            this.label9.TabIndex = 11;
            this.label9.Text = "зверху";
            // 
            // nud_clip_bottom
            // 
            this.nud_clip_bottom.DecimalPlaces = 1;
            this.nud_clip_bottom.Location = new System.Drawing.Point(74, 82);
            this.nud_clip_bottom.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nud_clip_bottom.Name = "nud_clip_bottom";
            this.nud_clip_bottom.Size = new System.Drawing.Size(43, 20);
            this.nud_clip_bottom.TabIndex = 14;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(77, 66);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(36, 13);
            this.label10.TabIndex = 13;
            this.label10.Text = "знизу";
            // 
            // cb_auto_clip_x
            // 
            this.cb_auto_clip_x.AutoSize = true;
            this.cb_auto_clip_x.Location = new System.Drawing.Point(227, 41);
            this.cb_auto_clip_x.Name = "cb_auto_clip_x";
            this.cb_auto_clip_x.Size = new System.Drawing.Size(117, 17);
            this.cb_auto_clip_x.TabIndex = 15;
            this.cb_auto_clip_x.Text = "Автоматично по X";
            this.cb_auto_clip_x.UseVisualStyleBackColor = true;
            // 
            // cb_auto_clip_y
            // 
            this.cb_auto_clip_y.AutoSize = true;
            this.cb_auto_clip_y.Location = new System.Drawing.Point(227, 107);
            this.cb_auto_clip_y.Name = "cb_auto_clip_y";
            this.cb_auto_clip_y.Size = new System.Drawing.Size(117, 17);
            this.cb_auto_clip_y.TabIndex = 16;
            this.cb_auto_clip_y.Text = "Автоматично по Y";
            this.cb_auto_clip_y.UseVisualStyleBackColor = true;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.rb_x_relative_subjet);
            this.groupBox9.Controls.Add(this.rb_x_relative_sheet);
            this.groupBox9.Location = new System.Drawing.Point(350, 16);
            this.groupBox9.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(149, 60);
            this.groupBox9.TabIndex = 17;
            this.groupBox9.TabStop = false;
            // 
            // rb_x_relative_sheet
            // 
            this.rb_x_relative_sheet.AutoSize = true;
            this.rb_x_relative_sheet.Location = new System.Drawing.Point(7, 15);
            this.rb_x_relative_sheet.Name = "rb_x_relative_sheet";
            this.rb_x_relative_sheet.Size = new System.Drawing.Size(102, 17);
            this.rb_x_relative_sheet.TabIndex = 0;
            this.rb_x_relative_sheet.TabStop = true;
            this.rb_x_relative_sheet.Text = "Відносно листа";
            this.rb_x_relative_sheet.UseVisualStyleBackColor = true;
            // 
            // rb_x_relative_subjet
            // 
            this.rb_x_relative_subjet.AutoSize = true;
            this.rb_x_relative_subjet.Location = new System.Drawing.Point(7, 37);
            this.rb_x_relative_subjet.Name = "rb_x_relative_subjet";
            this.rb_x_relative_subjet.Size = new System.Drawing.Size(111, 17);
            this.rb_x_relative_subjet.TabIndex = 1;
            this.rb_x_relative_subjet.TabStop = true;
            this.rb_x_relative_subjet.Text = "Відносно сюжету";
            this.rb_x_relative_subjet.UseVisualStyleBackColor = true;
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.rb_y_relative_subject);
            this.groupBox10.Controls.Add(this.rb_y_relative_sheet);
            this.groupBox10.Location = new System.Drawing.Point(350, 82);
            this.groupBox10.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(149, 60);
            this.groupBox10.TabIndex = 18;
            this.groupBox10.TabStop = false;
            // 
            // rb_y_relative_subject
            // 
            this.rb_y_relative_subject.AutoSize = true;
            this.rb_y_relative_subject.Location = new System.Drawing.Point(7, 37);
            this.rb_y_relative_subject.Name = "rb_y_relative_subject";
            this.rb_y_relative_subject.Size = new System.Drawing.Size(111, 17);
            this.rb_y_relative_subject.TabIndex = 1;
            this.rb_y_relative_subject.TabStop = true;
            this.rb_y_relative_subject.Text = "Відносно сюжету";
            this.rb_y_relative_subject.UseVisualStyleBackColor = true;
            // 
            // rb_y_relative_sheet
            // 
            this.rb_y_relative_sheet.AutoSize = true;
            this.rb_y_relative_sheet.Location = new System.Drawing.Point(7, 15);
            this.rb_y_relative_sheet.Name = "rb_y_relative_sheet";
            this.rb_y_relative_sheet.Size = new System.Drawing.Size(102, 17);
            this.rb_y_relative_sheet.TabIndex = 0;
            this.rb_y_relative_sheet.TabStop = true;
            this.rb_y_relative_sheet.Text = "Відносно листа";
            this.rb_y_relative_sheet.UseVisualStyleBackColor = true;
            // 
            // apc_parent
            // 
            this.apc_parent.AnchorPointChanged = null;
            this.apc_parent.Location = new System.Drawing.Point(6, 14);
            this.apc_parent.Name = "apc_parent";
            this.apc_parent.Size = new System.Drawing.Size(80, 80);
            this.apc_parent.TabIndex = 1;
            // 
            // apc_mark
            // 
            this.apc_mark.AnchorPointChanged = null;
            this.apc_mark.Location = new System.Drawing.Point(6, 14);
            this.apc_mark.Name = "apc_mark";
            this.apc_mark.Size = new System.Drawing.Size(80, 80);
            this.apc_mark.TabIndex = 0;
            // 
            // FormAddPdfMark
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 473);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.tb_name);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAddPdfMark";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PDF Мітка";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_yOfs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Xofs)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_clip_left)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_clip_right)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_clip_top)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_clip_bottom)).EndInit();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.CheckBox cb_back;
        private System.Windows.Forms.CheckBox cb_front;
        private System.Windows.Forms.Button btn_ok;
        private Ookii.Dialogs.WinForms.VistaOpenFileDialog vistaOpenFileDialog1;
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
    }
}