using System.IO;
using System.Windows.Forms;

namespace ActiveWorks
{
    public partial class FormEditFolder : Form
    {
        public FormEditFolder()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        public FormEditFolder(string name) : this()
        {
            textBox_Name.Text = name;

            if (name != null) textBox_Name.Select(0, Path.GetFileNameWithoutExtension(name).Length);
        }
    }
}
