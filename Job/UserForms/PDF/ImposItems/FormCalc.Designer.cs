namespace JobSpace.UserForms.PDF.ImposItems
{
    partial class FormCalc
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
            this.tb_input = new System.Windows.Forms.TextBox();
            this.tb_result = new System.Windows.Forms.TextBox();
            this.btn_calc = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tb_input
            // 
            this.tb_input.Location = new System.Drawing.Point(12, 11);
            this.tb_input.Name = "tb_input";
            this.tb_input.Size = new System.Drawing.Size(148, 20);
            this.tb_input.TabIndex = 0;
            // 
            // tb_result
            // 
            this.tb_result.Location = new System.Drawing.Point(198, 11);
            this.tb_result.Name = "tb_result";
            this.tb_result.ReadOnly = true;
            this.tb_result.Size = new System.Drawing.Size(49, 20);
            this.tb_result.TabIndex = 1;
            // 
            // btn_calc
            // 
            this.btn_calc.Location = new System.Drawing.Point(166, 9);
            this.btn_calc.Name = "btn_calc";
            this.btn_calc.Size = new System.Drawing.Size(26, 23);
            this.btn_calc.TabIndex = 2;
            this.btn_calc.Text = "=";
            this.btn_calc.UseVisualStyleBackColor = true;
            this.btn_calc.Click += new System.EventHandler(this.btn_calc_Click);
            // 
            // FormCalc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 43);
            this.Controls.Add(this.btn_calc);
            this.Controls.Add(this.tb_result);
            this.Controls.Add(this.tb_input);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormCalc";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "калькулятор";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_input;
        private System.Windows.Forms.TextBox tb_result;
        private System.Windows.Forms.Button btn_calc;
    }
}