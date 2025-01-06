namespace JobSpace.UserForms.PDF.ImposItems
{
    partial class PreviewControl
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
            this.pb_preview = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.imposToolsControl1 = new JobSpace.UserForms.PDF.ImposItems.ImposToolsControl();
            ((System.ComponentModel.ISupportInitialize)(this.pb_preview)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pb_preview
            // 
            this.pb_preview.BackColor = System.Drawing.Color.White;
            this.pb_preview.Location = new System.Drawing.Point(3, 3);
            this.pb_preview.Name = "pb_preview";
            this.pb_preview.Size = new System.Drawing.Size(100, 50);
            this.pb_preview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pb_preview.TabIndex = 3;
            this.pb_preview.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.pb_preview);
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(449, 276);
            this.panel2.TabIndex = 4;
            // 
            // imposToolsControl1
            // 
            this.imposToolsControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imposToolsControl1.Location = new System.Drawing.Point(6, 288);
            this.imposToolsControl1.Margin = new System.Windows.Forms.Padding(0);
            this.imposToolsControl1.Name = "imposToolsControl1";
            this.imposToolsControl1.Size = new System.Drawing.Size(446, 63);
            this.imposToolsControl1.TabIndex = 0;
            // 
            // PreviewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.imposToolsControl1);
            this.Controls.Add(this.panel2);
            this.Name = "PreviewControl";
            this.Size = new System.Drawing.Size(455, 351);
            ((System.ComponentModel.ISupportInitialize)(this.pb_preview)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private JobSpace.UserForms.PDF.ImposItems.ImposToolsControl imposToolsControl1;
        private System.Windows.Forms.PictureBox pb_preview;
        private System.Windows.Forms.Panel panel2;
    }
}
