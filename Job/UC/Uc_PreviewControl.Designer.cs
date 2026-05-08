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
            components = new System.ComponentModel.Container();
            panel1 = new System.Windows.Forms.Panel();
            pb_preview = new System.Windows.Forms.PictureBox();
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            tsmi_copy_image = new System.Windows.Forms.ToolStripMenuItem();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pb_preview).BeginInit();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            panel1.AutoScroll = true;
            panel1.Controls.Add(pb_preview);
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Margin = new System.Windows.Forms.Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(396, 280);
            panel1.TabIndex = 0;
            panel1.SizeChanged += panel1_SizeChanged;
            // 
            // pb_preview
            // 
            pb_preview.BackColor = System.Drawing.Color.White;
            pb_preview.ContextMenuStrip = contextMenuStrip1;
            pb_preview.Location = new System.Drawing.Point(0, 0);
            pb_preview.Margin = new System.Windows.Forms.Padding(0);
            pb_preview.Name = "pb_preview";
            pb_preview.Size = new System.Drawing.Size(180, 180);
            pb_preview.TabIndex = 0;
            pb_preview.TabStop = false;
            pb_preview.Paint += pb_preview_Paint;
            pb_preview.MouseDown += pb_preview_MouseDown;
            pb_preview.MouseMove += pb_preview_MouseMove;
            pb_preview.MouseUp += pb_preview_MouseUp;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmi_copy_image });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(202, 26);
            // 
            // tsmi_copy_image
            // 
            tsmi_copy_image.Name = "tsmi_copy_image";
            tsmi_copy_image.Size = new System.Drawing.Size(201, 22);
            tsmi_copy_image.Text = "копіювати зображення";
            tsmi_copy_image.Click += tsmi_copy_image_Click;
            // 
            // Uc_PreviewControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(panel1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "Uc_PreviewControl";
            Size = new System.Drawing.Size(396, 280);
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pb_preview).EndInit();
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pb_preview;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmi_copy_image;
    }
}
