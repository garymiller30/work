using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Job.Dlg
{
    public partial class FormSelectBackFile : Form
    {
        public string Back { get; set; }

        public FormSelectBackFile(IEnumerable<string> files)
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;

            var displayFiles = files.Select(x => new DisplayFile(x));
            comboBox1.Items.AddRange(displayFiles.ToArray());
            comboBox1.SelectedIndex = 0;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Back = ((DisplayFile)comboBox1.SelectedItem).FullName;
            DialogResult = DialogResult.OK;
            Close();
        }
    }

    public class DisplayFile
    {
        public string Name { get; set; }
        public string FullName { get; set; }

        public DisplayFile(string file)
        {
            FullName = file;
            Name = Path.GetFileName(file);
        }
    }
}
