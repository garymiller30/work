using System;
using System.Windows.Forms;

namespace JobSpace.CustomForms
{
    public sealed partial class FormEnterText : Form
    {
        public string SelectedText { get; set; } = string.Empty;
        public FormEnterText()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
           
        }

        public FormEnterText(string text) : this()
        {
            textBox1.Text = text;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            SelectedText = textBox1.Text;

        }
    }
}
