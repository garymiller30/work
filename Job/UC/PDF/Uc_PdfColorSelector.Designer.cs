namespace JobSpace.UC.PDF
{
    partial class Uc_PdfColorSelector
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
            btn_select_color = new System.Windows.Forms.Button();
            panel_color = new System.Windows.Forms.Panel();
            label_selected_color = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // btn_select_color
            // 
            btn_select_color.Location = new System.Drawing.Point(3, 5);
            btn_select_color.Name = "btn_select_color";
            btn_select_color.Size = new System.Drawing.Size(112, 23);
            btn_select_color.TabIndex = 0;
            btn_select_color.Text = "вибрати колір";
            btn_select_color.UseVisualStyleBackColor = true;
            btn_select_color.Click += btn_select_color_Click;
            // 
            // panel_color
            // 
            panel_color.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel_color.Location = new System.Drawing.Point(121, 5);
            panel_color.Name = "panel_color";
            panel_color.Size = new System.Drawing.Size(135, 23);
            panel_color.TabIndex = 1;
            // 
            // label_selected_color
            // 
            label_selected_color.Location = new System.Drawing.Point(3, 40);
            label_selected_color.Name = "label_selected_color";
            label_selected_color.Size = new System.Drawing.Size(253, 23);
            label_selected_color.TabIndex = 2;
            label_selected_color.Text = "не вибрано";
            label_selected_color.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Uc_PdfColorSelector
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(label_selected_color);
            Controls.Add(panel_color);
            Controls.Add(btn_select_color);
            Name = "Uc_PdfColorSelector";
            Size = new System.Drawing.Size(259, 71);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button btn_select_color;
        private System.Windows.Forms.Panel panel_color;
        private System.Windows.Forms.Label label_selected_color;
    }
}
