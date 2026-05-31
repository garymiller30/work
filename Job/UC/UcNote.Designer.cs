namespace JobSpace.UC
{
    partial class UcNote
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UcNote));
            kryptonRichTextBox1 = new System.Windows.Forms.RichTextBox();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            toolStripButtonBold = new System.Windows.Forms.ToolStripButton();
            toolStripButtonItalic = new System.Windows.Forms.ToolStripButton();
            toolStripButtonStrikethrough = new System.Windows.Forms.ToolStripButton();
            toolStripButtonUnderline = new System.Windows.Forms.ToolStripButton();
            toolStripButtonReset = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            toolStripButtonAlignLeft = new System.Windows.Forms.ToolStripButton();
            toolStripButtonAlignCenter = new System.Windows.Forms.ToolStripButton();
            toolStripButtonAlignRight = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            toolStripButtonSelectedColor = new System.Windows.Forms.ToolStripButton();
            toolStripButtonColor = new System.Windows.Forms.ToolStripButton();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // kryptonRichTextBox1
            // 
            kryptonRichTextBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            kryptonRichTextBox1.Location = new System.Drawing.Point(0, 29);
            kryptonRichTextBox1.Margin = new System.Windows.Forms.Padding(2);
            kryptonRichTextBox1.Name = "kryptonRichTextBox1";
            kryptonRichTextBox1.Size = new System.Drawing.Size(340, 258);
            kryptonRichTextBox1.TabIndex = 0;
            kryptonRichTextBox1.Text = "";
            kryptonRichTextBox1.LinkClicked += kryptonRichTextBox1_LinkClicked;
            kryptonRichTextBox1.Leave += kryptonRichTextBox1_Leave;
            // 
            // toolStrip1
            // 
            toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonBold, toolStripButtonItalic, toolStripButtonStrikethrough, toolStripButtonUnderline, toolStripButtonReset, toolStripSeparator1, toolStripButtonAlignLeft, toolStripButtonAlignCenter, toolStripButtonAlignRight, toolStripSeparator2, toolStripButtonSelectedColor, toolStripButtonColor });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            toolStrip1.Size = new System.Drawing.Size(340, 25);
            toolStrip1.TabIndex = 1;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonBold
            // 
            toolStripButtonBold.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonBold.Image = (System.Drawing.Image)resources.GetObject("toolStripButtonBold.Image");
            toolStripButtonBold.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonBold.Name = "toolStripButtonBold";
            toolStripButtonBold.Size = new System.Drawing.Size(23, 22);
            toolStripButtonBold.Text = "Bold";
            toolStripButtonBold.Click += ToolStripButtonBold_Click;
            // 
            // toolStripButtonItalic
            // 
            toolStripButtonItalic.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonItalic.Image = (System.Drawing.Image)resources.GetObject("toolStripButtonItalic.Image");
            toolStripButtonItalic.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonItalic.Name = "toolStripButtonItalic";
            toolStripButtonItalic.Size = new System.Drawing.Size(23, 22);
            toolStripButtonItalic.Text = "Italic";
            toolStripButtonItalic.Click += ToolStripButtonItalic_Click;
            // 
            // toolStripButtonStrikethrough
            // 
            toolStripButtonStrikethrough.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonStrikethrough.Image = (System.Drawing.Image)resources.GetObject("toolStripButtonStrikethrough.Image");
            toolStripButtonStrikethrough.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonStrikethrough.Name = "toolStripButtonStrikethrough";
            toolStripButtonStrikethrough.Size = new System.Drawing.Size(23, 22);
            toolStripButtonStrikethrough.Text = "Strikeout";
            toolStripButtonStrikethrough.Click += ToolStripButtonStrikethrough_Click;
            // 
            // toolStripButtonUnderline
            // 
            toolStripButtonUnderline.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonUnderline.Image = (System.Drawing.Image)resources.GetObject("toolStripButtonUnderline.Image");
            toolStripButtonUnderline.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonUnderline.Name = "toolStripButtonUnderline";
            toolStripButtonUnderline.Size = new System.Drawing.Size(23, 22);
            toolStripButtonUnderline.Text = "Underline";
            toolStripButtonUnderline.Click += ToolStripButtonUnderline_Click;
            // 
            // toolStripButtonReset
            // 
            toolStripButtonReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonReset.Image = (System.Drawing.Image)resources.GetObject("toolStripButtonReset.Image");
            toolStripButtonReset.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonReset.Name = "toolStripButtonReset";
            toolStripButtonReset.Size = new System.Drawing.Size(23, 22);
            toolStripButtonReset.Text = "Clear";
            toolStripButtonReset.Click += ToolStripButtonReset_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonAlignLeft
            // 
            toolStripButtonAlignLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonAlignLeft.Image = (System.Drawing.Image)resources.GetObject("toolStripButtonAlignLeft.Image");
            toolStripButtonAlignLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonAlignLeft.Name = "toolStripButtonAlignLeft";
            toolStripButtonAlignLeft.Size = new System.Drawing.Size(23, 22);
            toolStripButtonAlignLeft.Text = "Align left";
            toolStripButtonAlignLeft.Click += ToolStripButtonAlignLeft_Click;
            // 
            // toolStripButtonAlignCenter
            // 
            toolStripButtonAlignCenter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonAlignCenter.Image = (System.Drawing.Image)resources.GetObject("toolStripButtonAlignCenter.Image");
            toolStripButtonAlignCenter.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonAlignCenter.Name = "toolStripButtonAlignCenter";
            toolStripButtonAlignCenter.Size = new System.Drawing.Size(23, 22);
            toolStripButtonAlignCenter.Text = "Align center";
            toolStripButtonAlignCenter.Click += ToolStripButtonAlignCenter_Click;
            // 
            // toolStripButtonAlignRight
            // 
            toolStripButtonAlignRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonAlignRight.Image = (System.Drawing.Image)resources.GetObject("toolStripButtonAlignRight.Image");
            toolStripButtonAlignRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonAlignRight.Name = "toolStripButtonAlignRight";
            toolStripButtonAlignRight.Size = new System.Drawing.Size(23, 22);
            toolStripButtonAlignRight.Text = "Align right";
            toolStripButtonAlignRight.Click += ToolStripButtonAlignRight_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonSelectedColor
            // 
            toolStripButtonSelectedColor.BackColor = System.Drawing.Color.Black;
            toolStripButtonSelectedColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None;
            toolStripButtonSelectedColor.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonSelectedColor.Name = "toolStripButtonSelectedColor";
            toolStripButtonSelectedColor.Size = new System.Drawing.Size(23, 22);
            toolStripButtonSelectedColor.Click += ToolStripButtonSelectedColor_Click;
            // 
            // toolStripButtonColor
            // 
            toolStripButtonColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonColor.Image = (System.Drawing.Image)resources.GetObject("toolStripButtonColor.Image");
            toolStripButtonColor.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonColor.Name = "toolStripButtonColor";
            toolStripButtonColor.Size = new System.Drawing.Size(23, 22);
            toolStripButtonColor.Text = "Select color";
            toolStripButtonColor.Click += ToolStripButtonColor_Click;
            // 
            // UcNote
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(toolStrip1);
            Controls.Add(kryptonRichTextBox1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "UcNote";
            Size = new System.Drawing.Size(340, 289);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox kryptonRichTextBox1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonBold;
        private System.Windows.Forms.ToolStripButton toolStripButtonItalic;
        private System.Windows.Forms.ToolStripButton toolStripButtonStrikethrough;
        private System.Windows.Forms.ToolStripButton toolStripButtonUnderline;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonAlignLeft;
        private System.Windows.Forms.ToolStripButton toolStripButtonAlignCenter;
        private System.Windows.Forms.ToolStripButton toolStripButtonAlignRight;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButtonColor;
        private System.Windows.Forms.ToolStripButton toolStripButtonReset;
        private System.Windows.Forms.ToolStripButton toolStripButtonSelectedColor;
    }
}
