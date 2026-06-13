using System;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF
{
    public partial class FormPdfAssistantSettings : Form
    {
        public PdfAssistantSettings Settings { get; private set; }

        public FormPdfAssistantSettings(PdfAssistantSettings settings)
        {
            InitializeComponent();
            Settings = settings;

            txtApiUrl.Text = settings.ApiUrl;
            txtApiKey.Text = settings.ApiKey;
            txtModelName.Text = settings.ModelName;
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            Settings.ApiUrl = txtApiUrl.Text.Trim();
            Settings.ApiKey = txtApiKey.Text.Trim();
            Settings.ModelName = txtModelName.Text.Trim();
        }
    }
}
