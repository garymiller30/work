namespace JobSpace.UC
{
    partial class Uc_PreviewControl
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.pb_preview = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_preview)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pb_preview);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(339, 243);
            this.panel1.TabIndex = 0;
            this.panel1.SizeChanged += new System.EventHandler(this.panel1_SizeChanged);
            // 
            // pb_preview
            // 
            this.pb_preview.BackColor = System.Drawing.Color.White;
            this.pb_preview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pb_preview.Location = new System.Drawing.Point(0, 0);
            this.pb_preview.Name = "pb_preview";
            this.pb_preview.Size = new System.Drawing.Size(192, 133);
            this.pb_preview.TabIndex = 0;
            this.pb_preview.TabStop = false;
            this.pb_preview.Paint += new System.Windows.Forms.PaintEventHandler(this.pb_preview_Paint);
            this.pb_preview.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pb_preview_MouseDown);
            this.pb_preview.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pb_preview_MouseMove);
            this.pb_preview.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pb_preview_MouseUp);
            // 
            // Uc_PreviewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "Uc_PreviewControl";
            this.Size = new System.Drawing.Size(339, 243);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pb_preview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pb_preview;
    }
}
