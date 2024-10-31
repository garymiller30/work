namespace JobSpace.UserForms.PDF.ImposItems
{
    partial class MarkColorControl
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_selectPantone = new System.Windows.Forms.Button();
            this.tb_name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cb_isSpot = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.nud_k = new System.Windows.Forms.NumericUpDown();
            this.nud_y = new System.Windows.Forms.NumericUpDown();
            this.nud_m = new System.Windows.Forms.NumericUpDown();
            this.nud_c = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cb_isOverprint = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_k)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_m)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_c)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_selectPantone);
            this.groupBox1.Controls.Add(this.tb_name);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cb_isSpot);
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(289, 43);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Пантон";
            // 
            // btn_selectPantone
            // 
            this.btn_selectPantone.Location = new System.Drawing.Point(249, 14);
            this.btn_selectPantone.Name = "btn_selectPantone";
            this.btn_selectPantone.Size = new System.Drawing.Size(34, 23);
            this.btn_selectPantone.TabIndex = 3;
            this.btn_selectPantone.Text = "...";
            this.btn_selectPantone.UseVisualStyleBackColor = true;
            this.btn_selectPantone.Click += new System.EventHandler(this.btn_selectPantone_Click);
            // 
            // tb_name
            // 
            this.tb_name.Location = new System.Drawing.Point(60, 14);
            this.tb_name.Name = "tb_name";
            this.tb_name.Size = new System.Drawing.Size(183, 20);
            this.tb_name.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "ім\'я";
            // 
            // cb_isSpot
            // 
            this.cb_isSpot.AutoSize = true;
            this.cb_isSpot.Location = new System.Drawing.Point(7, 20);
            this.cb_isSpot.Name = "cb_isSpot";
            this.cb_isSpot.Size = new System.Drawing.Size(15, 14);
            this.cb_isSpot.TabIndex = 0;
            this.cb_isSpot.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.nud_k);
            this.groupBox2.Controls.Add(this.nud_y);
            this.groupBox2.Controls.Add(this.nud_m);
            this.groupBox2.Controls.Add(this.nud_c);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(4, 53);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(289, 53);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "CMYK";
            // 
            // nud_k
            // 
            this.nud_k.DecimalPlaces = 1;
            this.nud_k.Location = new System.Drawing.Point(233, 18);
            this.nud_k.Name = "nud_k";
            this.nud_k.Size = new System.Drawing.Size(50, 20);
            this.nud_k.TabIndex = 7;
            this.nud_k.Click += new System.EventHandler(this.nud_c_Click);
            this.nud_k.Enter += new System.EventHandler(this.nud_c_Click);
            // 
            // nud_y
            // 
            this.nud_y.DecimalPlaces = 1;
            this.nud_y.Location = new System.Drawing.Point(163, 18);
            this.nud_y.Name = "nud_y";
            this.nud_y.Size = new System.Drawing.Size(50, 20);
            this.nud_y.TabIndex = 6;
            this.nud_y.Click += new System.EventHandler(this.nud_c_Click);
            this.nud_y.Enter += new System.EventHandler(this.nud_c_Click);
            // 
            // nud_m
            // 
            this.nud_m.DecimalPlaces = 1;
            this.nud_m.Location = new System.Drawing.Point(94, 18);
            this.nud_m.Name = "nud_m";
            this.nud_m.Size = new System.Drawing.Size(50, 20);
            this.nud_m.TabIndex = 5;
            this.nud_m.Click += new System.EventHandler(this.nud_c_Click);
            this.nud_m.Enter += new System.EventHandler(this.nud_c_Click);
            // 
            // nud_c
            // 
            this.nud_c.DecimalPlaces = 1;
            this.nud_c.Location = new System.Drawing.Point(21, 18);
            this.nud_c.Name = "nud_c";
            this.nud_c.Size = new System.Drawing.Size(50, 20);
            this.nud_c.TabIndex = 4;
            this.nud_c.Click += new System.EventHandler(this.nud_c_Click);
            this.nud_c.Enter += new System.EventHandler(this.nud_c_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(217, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "K";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(148, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Y";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(79, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "M";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "C";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cb_isOverprint);
            this.groupBox3.Location = new System.Drawing.Point(4, 109);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(289, 32);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            // 
            // cb_isOverprint
            // 
            this.cb_isOverprint.AutoSize = true;
            this.cb_isOverprint.Location = new System.Drawing.Point(10, 9);
            this.cb_isOverprint.Name = "cb_isOverprint";
            this.cb_isOverprint.Size = new System.Drawing.Size(67, 17);
            this.cb_isOverprint.TabIndex = 0;
            this.cb_isOverprint.Text = "overprint";
            this.cb_isOverprint.UseVisualStyleBackColor = true;
            // 
            // MarkColorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "MarkColorControl";
            this.Size = new System.Drawing.Size(298, 146);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_k)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_m)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_c)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_selectPantone;
        private System.Windows.Forms.TextBox tb_name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cb_isSpot;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown nud_k;
        private System.Windows.Forms.NumericUpDown nud_y;
        private System.Windows.Forms.NumericUpDown nud_m;
        private System.Windows.Forms.NumericUpDown nud_c;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox cb_isOverprint;
    }
}
