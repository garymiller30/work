using System;
using System.Windows.Forms;

namespace MailNotifier
{

    public partial class FormGetName : Form
    {
        public string ShablonName { get; set; }

        public FormGetName()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                DialogResult = DialogResult.OK;
                ShablonName = textBox1.Text;
            }
        }
    }
}
