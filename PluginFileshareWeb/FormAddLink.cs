using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PluginFileshareWeb
{
    public partial class FormAddLink : Form
    {

        public LinkInfo LinkInfo { get;set; } = new LinkInfo();
        public FormAddLink()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0 || textBox2.Text.Length == 0) {return; }

            LinkInfo.Name = textBox1.Text;
            LinkInfo.Url = textBox2.Text;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
