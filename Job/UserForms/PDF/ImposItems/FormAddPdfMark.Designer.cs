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
            this.cb_Angle = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.cb_backMirror = new System.Windows.Forms.CheckBox();
            this.cb_back = new System.Windows.Forms.CheckBox();
            this.cb_front = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.nud_yOfs = new System.Windows.Forms.NumericUpDown();
            this.nud_Xofs = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.apc_parent = new JobSpace.UserForms.PDF.ImposItems.AnchorPointControl();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.apc_mark = new JobSpace.UserForms.PDF.ImposItems.AnchorPointControl();
            this.btn_ok = new System.Windows.Forms.Button();
            this.vistaOpenFileDialog1 = new Ookii.Dialogs.WinForms.VistaOpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_yOfs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Xofs)).BeginInit();
            this.groupBox5.SuspendLayout();
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
            this.groupBox3.Controls.Add(this.cb_Angle);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.groupBox7);
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Location = new System.Drawing.Point(12, 58);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(539, 157);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Параметри";
            // 
            // cb_Angle
            // 
            this.cb_Angle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Angle.FormattingEnabled = true;
            this.cb_Angle.Location = new System.Drawing.Point(418, 123);
            this.cb_Angle.Name = "cb_Angle";
            this.cb_Angle.Size = new System.Drawing.Size(62, 21);
            this.cb_Angle.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.Image = ((System.Drawing.Image)(resources.GetObject("label6.Image")));
            this.label6.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label6.Location = new System.Drawing.Point(339, 121);
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
            this.groupBox4.Controls.Add(this.groupBox6);
            this.groupBox4.Controls.Add(this.groupBox5);
            this.groupBox4.Location = new System.Drawing.Point(6, 19);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(324, 132);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Положення відносно";
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
            // apc_parent
            // 
            this.apc_parent.AnchorPointChanged = ((System.EventHandler<JobSpace.Static.Pdf.Imposition.Models.AnchorPoint>)(resources.GetObject("apc_parent.AnchorPointChanged")));
            this.apc_parent.Location = new System.Drawing.Point(6, 14);
            this.apc_parent.Name = "apc_parent";
            this.apc_parent.Size = new System.Drawing.Size(80, 80);
            this.apc_parent.TabIndex = 1;
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
            // apc_mark
            // 
            this.apc_mark.AnchorPointChanged = ((System.EventHandler<JobSpace.Static.Pdf.Imposition.Models.AnchorPoint>)(resources.GetObject("apc_mark.AnchorPointChanged")));
            this.apc_mark.Location = new System.Drawing.Point(6, 14);
            this.apc_mark.Name = "apc_mark";
            this.apc_mark.Size = new System.Drawing.Size(80, 80);
            this.apc_mark.TabIndex = 0;
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(220, 221);
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
            // FormAddPdfMark
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 271);
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
            this.groupBox7.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_yOfs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Xofs)).EndInit();
            this.groupBox5.ResumeLayout(false);
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
    }
}