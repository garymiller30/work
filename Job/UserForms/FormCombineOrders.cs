using System.Windows.Forms;

namespace JobSpace.UserForms
{
    public sealed partial class FormCombineOrders : Form
    {
        public string OrderNumber => textBox1.Text;


        public FormCombineOrders()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }
    }
}
