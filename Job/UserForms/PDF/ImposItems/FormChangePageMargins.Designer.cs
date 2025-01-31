namespace JobSpace.UserForms.PDF.ImposItems
{
    partial class FormChangePageMargins
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormChangePageMargins));
            this.nud_top = new System.Windows.Forms.NumericUpDown();
            this.nud_left = new System.Windows.Forms.NumericUpDown();
            this.nud_right = new System.Windows.Forms.NumericUpDown();
            this.nud_bottom = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.b_ok = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nud_top)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_left)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_right)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_bottom)).BeginInit();
            this.SuspendLayout();
            // 
            // nud_top
            // 
            this.nud_top.DecimalPlaces = 1;
            this.nud_top.Location = new System.Drawing.Point(91, 12);
            this.nud_top.Name = "nud_top";
            this.nud_top.Size = new System.Drawing.Size(58, 20);
            this.nud_top.TabIndex = 0;
            this.nud_top.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // nud_left
            // 
            this.nud_left.DecimalPlaces = 1;
            this.nud_left.Location = new System.Drawing.Point(10, 64);
            this.nud_left.Name = "nud_left";
            this.nud_left.Size = new System.Drawing.Size(58, 20);
            this.nud_left.TabIndex = 1;
            this.nud_left.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // nud_right
            // 
            this.nud_right.DecimalPlaces = 1;
            this.nud_right.Location = new System.Drawing.Point(167, 64);
            this.nud_right.Name = "nud_right";
            this.nud_right.Size = new System.Drawing.Size(58, 20);
            this.nud_right.TabIndex = 2;
            this.nud_right.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // nud_bottom
            // 
            this.nud_bottom.DecimalPlaces = 1;
            this.nud_bottom.Location = new System.Drawing.Point(91, 115);
            this.nud_bottom.Name = "nud_bottom";
            this.nud_bottom.Size = new System.Drawing.Size(58, 20);
            this.nud_bottom.TabIndex = 3;
            this.nud_bottom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
            this.label1.Location = new System.Drawing.Point(135, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 26);
            this.label1.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.Image = ((System.Drawing.Image)(resources.GetObject("label2.Image")));
            this.label2.Location = new System.Drawing.Point(105, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 26);
            this.label2.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.Image = ((System.Drawing.Image)(resources.GetObject("label3.Image")));
            this.label3.Location = new System.Drawing.Point(105, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 26);
            this.label3.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.Image = ((System.Drawing.Image)(resources.GetObject("label4.Image")));
            this.label4.Location = new System.Drawing.Point(74, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 26);
            this.label4.TabIndex = 7;
            // 
            // b_ok
            // 
            this.b_ok.Location = new System.Drawing.Point(74, 161);
            this.b_ok.Name = "b_ok";
            this.b_ok.Size = new System.Drawing.Size(87, 30);
            this.b_ok.TabIndex = 8;
            this.b_ok.Text = "OK";
            this.b_ok.UseVisualStyleBackColor = true;
            this.b_ok.Click += new System.EventHandler(this.b_ok_Click);
            // 
            // FrormChangePageMargins
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(235, 203);
            this.Controls.Add(this.b_ok);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nud_bottom);
            this.Controls.Add(this.nud_right);
            this.Controls.Add(this.nud_left);
            this.Controls.Add(this.nud_top);
            this.Name = "FrormChangePageMargins";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Змінити поля сторінки";
            ((System.ComponentModel.ISupportInitialize)(this.nud_top)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_left)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_right)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_bottom)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nud_top;
        private System.Windows.Forms.NumericUpDown nud_left;
        private System.Windows.Forms.NumericUpDown nud_right;
        private System.Windows.Forms.NumericUpDown nud_bottom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button b_ok;
    }
}