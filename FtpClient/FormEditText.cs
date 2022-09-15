using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace FtpClient
{
    public sealed partial class FormEditText : KryptonForm
    {
        public string EditText
        {
            get => kryptonTextBox1.Text;
            set => kryptonTextBox1.Text = value;
        }


        public FormEditText()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }
    }
}
