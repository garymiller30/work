using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interfaces;

namespace Job.UC
{
    public sealed partial class UcNote : UserControl, INoteControl
    {
        public UcNote()
        {
            InitializeComponent();
        }

        public void SetText(string text)
        {
            if (text.TrimStart().StartsWith(@"{\rtf1", StringComparison.Ordinal))
            {
                kryptonRichTextBox1.Rtf = text;
            }
            else
            {
                kryptonRichTextBox1.Text = text;
            }
        }

        public string GetText()
        {
            return kryptonRichTextBox1.Text;
        }

        public string GetRtf()
        {
            return kryptonRichTextBox1.Rtf;
        }

        private void ToolStripButtonBold_Click(object sender, EventArgs e)
        {
            if (kryptonRichTextBox1.SelectionFont != null)
                kryptonRichTextBox1.SelectionFont = new Font(kryptonRichTextBox1.Font,kryptonRichTextBox1.SelectionFont.Style ^ FontStyle.Bold);
        }

        private void ToolStripButtonItalic_Click(object sender, EventArgs e)
        {
            if (kryptonRichTextBox1.SelectionFont != null)
                kryptonRichTextBox1.SelectionFont = new Font(kryptonRichTextBox1.Font, kryptonRichTextBox1.SelectionFont.Style ^ FontStyle.Italic);
        }

        private void ToolStripButtonStrikethrough_Click(object sender, EventArgs e)
        {
            if (kryptonRichTextBox1.SelectionFont != null)
                kryptonRichTextBox1.SelectionFont = new Font(kryptonRichTextBox1.Font, kryptonRichTextBox1.SelectionFont.Style ^ FontStyle.Strikeout);
        }

        private void ToolStripButtonUnderline_Click(object sender, EventArgs e)
        {
            if (kryptonRichTextBox1.SelectionFont != null)
                kryptonRichTextBox1.SelectionFont = new Font(kryptonRichTextBox1.Font, kryptonRichTextBox1.SelectionFont.Style ^ FontStyle.Underline);
        }

        private void ToolStripButtonReset_Click(object sender, EventArgs e)
        {
            if (kryptonRichTextBox1.SelectionFont != null)
                kryptonRichTextBox1.SelectionFont = new Font(kryptonRichTextBox1.Font, FontStyle.Regular);
        }

        private void ToolStripButtonAlignLeft_Click(object sender, EventArgs e)
        {
            kryptonRichTextBox1.SelectionAlignment = HorizontalAlignment.Left;
        }

        private void ToolStripButtonAlignCenter_Click(object sender, EventArgs e)
        {
            kryptonRichTextBox1.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void ToolStripButtonAlignRight_Click(object sender, EventArgs e)
        {
            kryptonRichTextBox1.SelectionAlignment = HorizontalAlignment.Right;
        }

        private void ToolStripButtonColor_Click(object sender, EventArgs e)
        {
            SelectColorDialog();

        }
        private void SelectColorDialog()
        {
            using (var f = new ColorDialog())
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    toolStripButtonSelectedColor.BackColor = f.Color;
                    SetColor();
                }
            }
        }

        private void SetColor()
        {
            kryptonRichTextBox1.SelectionColor = toolStripButtonSelectedColor.BackColor;
        }

        private void ToolStripButtonSelectedColor_Click(object sender, EventArgs e)
        {
            SetColor();
        }

        private void kryptonRichTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

       
    }
}
