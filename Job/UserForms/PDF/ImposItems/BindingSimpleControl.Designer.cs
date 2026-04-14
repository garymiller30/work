namespace JobSpace.UserForms.PDF.ImposItems
{
    partial class BindingSimpleControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BindingSimpleControl));
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.btn_custom = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nud_Yofs = new System.Windows.Forms.NumericUpDown();
            this.nud_Xofs = new System.Windows.Forms.NumericUpDown();
            this.cb_centerHeight = new System.Windows.Forms.CheckBox();
            this.cb_centerWidth = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label_max_90 = new System.Windows.Forms.Label();
            this.label_max_0 = new System.Windows.Forms.Label();
            this.label_90 = new System.Windows.Forms.Label();
            this.label_0 = new System.Windows.Forms.Label();
            this.b_max_90 = new System.Windows.Forms.Button();
            this.b_max_0 = new System.Windows.Forms.Button();
            this.b_90 = new System.Windows.Forms.Button();
            this.b_0 = new System.Windows.Forms.Button();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Yofs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Xofs)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.btn_custom);
            this.groupBox7.Controls.Add(this.label2);
            this.groupBox7.Controls.Add(this.label1);
            this.groupBox7.Controls.Add(this.nud_Yofs);
            this.groupBox7.Controls.Add(this.nud_Xofs);
            this.groupBox7.Controls.Add(this.cb_centerHeight);
            this.groupBox7.Controls.Add(this.cb_centerWidth);
            this.groupBox7.Location = new System.Drawing.Point(3, 71);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(299, 47);
            this.groupBox7.TabIndex = 9;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Додаткові параметри";
            // 
            // btn_custom
            // 
            this.btn_custom.Location = new System.Drawing.Point(244, 15);
            this.btn_custom.Name = "btn_custom";
            this.btn_custom.Size = new System.Drawing.Size(49, 23);
            this.btn_custom.TabIndex = 13;
            this.btn_custom.Text = "Звіт";
            this.btn_custom.UseVisualStyleBackColor = true;
            this.btn_custom.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(154, 20);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "+Yofs";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "+Xofs";
            // 
            // nud_Yofs
            // 
            this.nud_Yofs.DecimalPlaces = 1;
            this.nud_Yofs.Location = new System.Drawing.Point(188, 16);
            this.nud_Yofs.Margin = new System.Windows.Forms.Padding(0);
            this.nud_Yofs.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nud_Yofs.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.nud_Yofs.Name = "nud_Yofs";
            this.nud_Yofs.Size = new System.Drawing.Size(50, 20);
            this.nud_Yofs.TabIndex = 8;
            this.nud_Yofs.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // nud_Xofs
            // 
            this.nud_Xofs.DecimalPlaces = 1;
            this.nud_Xofs.Location = new System.Drawing.Point(67, 16);
            this.nud_Xofs.Margin = new System.Windows.Forms.Padding(0);
            this.nud_Xofs.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nud_Xofs.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.nud_Xofs.Name = "nud_Xofs";
            this.nud_Xofs.Size = new System.Drawing.Size(50, 20);
            this.nud_Xofs.TabIndex = 7;
            this.nud_Xofs.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cb_centerHeight
            // 
            this.cb_centerHeight.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_centerHeight.AutoSize = true;
            this.cb_centerHeight.Checked = true;
            this.cb_centerHeight.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_centerHeight.Image = ((System.Drawing.Image)(resources.GetObject("cb_centerHeight.Image")));
            this.cb_centerHeight.Location = new System.Drawing.Point(126, 15);
            this.cb_centerHeight.Name = "cb_centerHeight";
            this.cb_centerHeight.Size = new System.Drawing.Size(22, 22);
            this.cb_centerHeight.TabIndex = 1;
            this.cb_centerHeight.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.cb_centerHeight.UseVisualStyleBackColor = true;
            // 
            // cb_centerWidth
            // 
            this.cb_centerWidth.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_centerWidth.AutoSize = true;
            this.cb_centerWidth.Checked = true;
            this.cb_centerWidth.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_centerWidth.Image = ((System.Drawing.Image)(resources.GetObject("cb_centerWidth.Image")));
            this.cb_centerWidth.Location = new System.Drawing.Point(8, 16);
            this.cb_centerWidth.Name = "cb_centerWidth";
            this.cb_centerWidth.Size = new System.Drawing.Size(22, 22);
            this.cb_centerWidth.TabIndex = 0;
            this.cb_centerWidth.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.cb_centerWidth.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label_max_90);
            this.groupBox1.Controls.Add(this.label_max_0);
            this.groupBox1.Controls.Add(this.label_90);
            this.groupBox1.Controls.Add(this.label_0);
            this.groupBox1.Controls.Add(this.b_max_90);
            this.groupBox1.Controls.Add(this.b_max_0);
            this.groupBox1.Controls.Add(this.b_90);
            this.groupBox1.Controls.Add(this.b_0);
            this.groupBox1.Location = new System.Drawing.Point(3, 2);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(299, 66);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "кількість на листі";
            // 
            // label_max_90
            // 
            this.label_max_90.Location = new System.Drawing.Point(220, 45);
            this.label_max_90.Name = "label_max_90";
            this.label_max_90.Size = new System.Drawing.Size(60, 18);
            this.label_max_90.TabIndex = 12;
            this.label_max_90.Text = "0";
            this.label_max_90.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_max_0
            // 
            this.label_max_0.Location = new System.Drawing.Point(154, 44);
            this.label_max_0.Name = "label_max_0";
            this.label_max_0.Size = new System.Drawing.Size(60, 18);
            this.label_max_0.TabIndex = 11;
            this.label_max_0.Text = "0";
            this.label_max_0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_90
            // 
            this.label_90.Location = new System.Drawing.Point(88, 45);
            this.label_90.Name = "label_90";
            this.label_90.Size = new System.Drawing.Size(60, 18);
            this.label_90.TabIndex = 10;
            this.label_90.Text = "0";
            this.label_90.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_0
            // 
            this.label_0.Location = new System.Drawing.Point(22, 45);
            this.label_0.Name = "label_0";
            this.label_0.Size = new System.Drawing.Size(60, 18);
            this.label_0.TabIndex = 9;
            this.label_0.Text = "0";
            this.label_0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // b_max_90
            // 
            this.b_max_90.Location = new System.Drawing.Point(220, 18);
            this.b_max_90.Name = "b_max_90";
            this.b_max_90.Size = new System.Drawing.Size(60, 23);
            this.b_max_90.TabIndex = 8;
            this.b_max_90.Text = "MAX: 90°";
            this.b_max_90.UseVisualStyleBackColor = true;
            this.b_max_90.Click += new System.EventHandler(this.b_max_90_Click);
            // 
            // b_max_0
            // 
            this.b_max_0.Location = new System.Drawing.Point(154, 18);
            this.b_max_0.Name = "b_max_0";
            this.b_max_0.Size = new System.Drawing.Size(60, 23);
            this.b_max_0.TabIndex = 7;
            this.b_max_0.Text = "MAX: 0°";
            this.b_max_0.UseVisualStyleBackColor = true;
            this.b_max_0.Click += new System.EventHandler(this.b_max_Click);
            // 
            // b_90
            // 
            this.b_90.Location = new System.Drawing.Point(88, 19);
            this.b_90.Name = "b_90";
            this.b_90.Size = new System.Drawing.Size(60, 23);
            this.b_90.TabIndex = 6;
            this.b_90.Text = "90°";
            this.b_90.UseVisualStyleBackColor = true;
            this.b_90.Click += new System.EventHandler(this.b_90_Click);
            // 
            // b_0
            // 
            this.b_0.Location = new System.Drawing.Point(22, 19);
            this.b_0.Name = "b_0";
            this.b_0.Size = new System.Drawing.Size(60, 23);
            this.b_0.TabIndex = 5;
            this.b_0.Text = "0°";
            this.b_0.UseVisualStyleBackColor = true;
            this.b_0.Click += new System.EventHandler(this.b_0_Click);
            // 
            // BindingSimpleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox1);
            this.Name = "BindingSimpleControl";
            this.Size = new System.Drawing.Size(307, 122);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Yofs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Xofs)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.NumericUpDown nud_Yofs;
        private System.Windows.Forms.NumericUpDown nud_Xofs;
        private System.Windows.Forms.CheckBox cb_centerHeight;
        private System.Windows.Forms.CheckBox cb_centerWidth;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button b_90;
        private System.Windows.Forms.Button b_0;
        private System.Windows.Forms.Button b_max_0;
        private System.Windows.Forms.Button b_max_90;
        private System.Windows.Forms.Label label_max_90;
        private System.Windows.Forms.Label label_max_0;
        private System.Windows.Forms.Label label_90;
        private System.Windows.Forms.Label label_0;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        protected System.Windows.Forms.Button btn_custom;
    }
}
