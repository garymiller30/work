using MailNotifier.Shablons;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MailNotifier
{
    public partial class FormTemplate : Form
    {
        public MailTemplate Template;

        public FormTemplate(MailTemplate template = null)
        {
            if (template == null)
                Template = new MailTemplate();
            else
            {
                Template = template;
            }
            
            InitializeComponent();
            BindTemplate();
            DialogResult = DialogResult.Cancel;
        }

        private void BindTemplate()
        {
            tb_mail_template_name.Text = Template.ShablonName;
            tb_whom.Text = Template.SendTo;
            tb_header.Text = Template.Header;
            if (Template.Message != null && Template.Message.StartsWith("{\\rtf"))
                rtb_message.Rtf = Template.Message;
            else
                rtb_message.Text = Template.Message;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            Template.ShablonName = tb_mail_template_name.Text;
            Template.SendTo = tb_whom.Text;
            Template.Header = tb_header.Text;
            Template.Message = rtb_message.Rtf;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
