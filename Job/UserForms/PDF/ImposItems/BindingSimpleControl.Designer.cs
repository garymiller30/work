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
            this.cb_OneCut = new System.Windows.Forms.CheckBox();
            this.nud_Yofs = new System.Windows.Forms.NumericUpDown();
            this.nud_Xofs = new System.Windows.Forms.NumericUpDown();
            this.cb_centerHeight = new System.Windows.Forms.CheckBox();
            this.cb_centerWidth = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.b_90 = new System.Windows.Forms.Button();
            this.b_0 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nud_y = new System.Windows.Forms.NumericUpDown();
            this.nud_x = new System.Windows.Forms.NumericUpDown();
            this.b_max = new System.Windows.Forms.Button();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Yofs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Xofs)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_x)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.cb_OneCut);
            this.groupBox7.Controls.Add(this.nud_Yofs);
            this.groupBox7.Controls.Add(this.nud_Xofs);
            this.groupBox7.Controls.Add(this.cb_centerHeight);
            this.groupBox7.Controls.Add(this.cb_centerWidth);
            this.groupBox7.Location = new System.Drawing.Point(3, 71);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(299, 93);
            this.groupBox7.TabIndex = 9;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Додаткові параметри";
            // 
            // cb_OneCut
            // 
            this.cb_OneCut.AutoSize = true;
            this.cb_OneCut.Location = new System.Drawing.Point(173, 46);
            this.cb_OneCut.Name = "cb_OneCut";
            this.cb_OneCut.Size = new System.Drawing.Size(76, 17);
            this.cb_OneCut.TabIndex = 10;
            this.cb_OneCut.Text = "в один різ";
            this.cb_OneCut.UseVisualStyleBackColor = true;
            // 
            // nud_Yofs
            // 
            this.nud_Yofs.DecimalPlaces = 1;
            this.nud_Yofs.Location = new System.Drawing.Point(95, 62);
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
            this.nud_Xofs.Location = new System.Drawing.Point(95, 27);
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
            this.cb_centerHeight.Location = new System.Drawing.Point(7, 55);
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
            this.cb_centerWidth.Location = new System.Drawing.Point(7, 20);
            this.cb_centerWidth.Name = "cb_centerWidth";
            this.cb_centerWidth.Size = new System.Drawing.Size(88, 32);
            this.cb_centerWidth.TabIndex = 0;
            this.cb_centerWidth.Text = "+ Xofs";
            this.cb_centerWidth.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.cb_centerWidth.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.b_max);
            this.groupBox1.Controls.Add(this.b_90);
            this.groupBox1.Controls.Add(this.b_0);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.nud_y);
            this.groupBox1.Controls.Add(this.nud_x);
            this.groupBox1.Location = new System.Drawing.Point(3, 2);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(299, 66);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "кількість на листі";
            // 
            // b_90
            // 
            this.b_90.Location = new System.Drawing.Point(200, 30);
            this.b_90.Name = "b_90";
            this.b_90.Size = new System.Drawing.Size(31, 23);
            this.b_90.TabIndex = 6;
            this.b_90.Text = "90°";
            this.b_90.UseVisualStyleBackColor = true;
            this.b_90.Click += new System.EventHandler(this.b_90_Click);
            // 
            // b_0
            // 
            this.b_0.Location = new System.Drawing.Point(163, 30);
            this.b_0.Name = "b_0";
            this.b_0.Size = new System.Drawing.Size(31, 23);
            this.b_0.TabIndex = 5;
            this.b_0.Text = "0°";
            this.b_0.UseVisualStyleBackColor = true;
            this.b_0.Click += new System.EventHandler(this.b_0_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(80, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(12, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "x";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(95, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "по Y";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "по X";
            // 
            // nud_y
            // 
            this.nud_y.Location = new System.Drawing.Point(98, 35);
            this.nud_y.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nud_y.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_y.Name = "nud_y";
            this.nud_y.Size = new System.Drawing.Size(49, 20);
            this.nud_y.TabIndex = 1;
            this.nud_y.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nud_y.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nud_x
            // 
            this.nud_x.Location = new System.Drawing.Point(25, 35);
            this.nud_x.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nud_x.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_x.Name = "nud_x";
            this.nud_x.Size = new System.Drawing.Size(49, 20);
            this.nud_x.TabIndex = 0;
            this.nud_x.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nud_x.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // b_max
            // 
            this.b_max.Location = new System.Drawing.Point(237, 30);
            this.b_max.Name = "b_max";
            this.b_max.Size = new System.Drawing.Size(46, 23);
            this.b_max.TabIndex = 7;
            this.b_max.Text = "MAX";
            this.b_max.UseVisualStyleBackColor = true;
            this.b_max.Click += new System.EventHandler(this.b_max_Click);
            // 
            // BindingSimpleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox1);
            this.Name = "BindingSimpleControl";
            this.Size = new System.Drawing.Size(307, 167);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Yofs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Xofs)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_x)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.CheckBox cb_OneCut;
        private System.Windows.Forms.NumericUpDown nud_Yofs;
        private System.Windows.Forms.NumericUpDown nud_Xofs;
        private System.Windows.Forms.CheckBox cb_centerHeight;
        private System.Windows.Forms.CheckBox cb_centerWidth;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button b_90;
        private System.Windows.Forms.Button b_0;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nud_y;
        private System.Windows.Forms.NumericUpDown nud_x;
        private System.Windows.Forms.Button b_max;
    }
}
