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
    public partial class FormEditLink : Form
    {
        public LinkInfo LinkInfo { get; set; }

        public FormEditLink()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        public FormEditLink(LinkInfo linkInfo):this()
        {
            LinkInfo = linkInfo;
            tb_name.Text = LinkInfo.Name;
            tb_link.Text = LinkInfo.Url;
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (LinkInfo == null)
            {
                LinkInfo = new LinkInfo();
            }

            LinkInfo.Name = tb_name.Text;
            LinkInfo.Url = tb_link.Text;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
