using System.Windows.Forms;

namespace ActiveWorks.Forms
{
    public sealed partial class FormWhatNew : Form
    {
        public FormWhatNew(string text)
        {
            InitializeComponent();

            kryptonRichTextBox1.Text = text;
        }
    }
}
