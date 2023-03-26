using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using ExtensionMethods;
using Job.Static;

namespace Job.UC
{
    public sealed partial class UcTexBox : TextBox
    {
        public UcTexBox()
        {
            InitializeComponent();
            //KeyPress += TextBox_Description_KeyPress;
        }

        public UcTexBox(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
            //KeyPress += TextBox_Description_KeyPress;
        }
        protected override void WndProc(ref Message m)
        {
            // Trap WM_PASTE:
            if (m.Msg == 0x302 && Clipboard.ContainsText())
            {
                SelectedText = Clipboard.GetText();
                return;
            }
            base.WndProc(ref m);
        }

        //private void TextBox_Description_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (e.KeyChar == (char)Keys.Delete || e.KeyChar == (char)'\b')
        //        e.Handled = false;
        //    else if (ModifierKeys.HasFlag(Keys.Control) && e.KeyChar == 22)
        //    {
        //        var clipboard = Clipboard.GetDataObject();
        //        if (clipboard != null)
        //        {
        //            if (clipboard.GetDataPresent(DataFormats.StringFormat))
        //            {
        //                string fn = (string)clipboard.GetData(DataFormats.StringFormat);

        //                try
        //                {
        //                    fn = Path.GetFileNameWithoutExtension(fn);
        //                }
        //                catch { }

                       
        //                InsertTextToTextbox(this, fn);
        //                //InsertTextToTextbox(textBox_Description,Path.GetFileNameWithoutExtension( (string) clipboard.GetData(DataFormats.StringFormat)).Transliteration());
        //                e.Handled = true;
        //            }

        //        }
        //    }
        //    else
        //    {
        //        InsertTextToTextbox(this, e.KeyChar);
        //        e.Handled = true;
        //    }
        //}
        private void InsertTextToTextbox(TextBox textBox, string txt)
        {
            var selStart = textBox.SelectionStart;

            if (textBox.SelectionLength > 0)
                textBox.Text = textBox.Text.Remove(this.SelectionStart,
                    textBox.SelectionLength);
            textBox.Text = textBox.Text.Insert(selStart, txt);
            textBox.SelectionStart = selStart + txt.Length;
        }
    }
}
