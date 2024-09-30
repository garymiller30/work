using System;
using System.Windows.Forms;
using Interfaces;
using JobSpace.Profiles;
using Ookii.Dialogs.WinForms;

namespace JobSpace.CustomForms
{
    public sealed partial class FormEditStatus : Form
    {
        private readonly Profile _profile;
        private readonly IJobStatus _jobStatus;

        public FormEditStatus(Profile profile, IJobStatus jobStatus)
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
            _jobStatus = jobStatus;
            _profile = profile;
           
            Bind();
        }

        private void Bind()
        {
            textBox1.Text = _jobStatus.Name;
            BingImage();
        }

        private void BingImage()
        {
            if (_jobStatus.Img != null)
            {
                pictureBox1.Image = _jobStatus.Img;
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;

            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                _jobStatus.Name = textBox1.Text;
                Close();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            using (var f = new VistaOpenFileDialog())
            {
                f.Multiselect = false;
                f.CheckFileExists = true;
                f.Filter = "*.bmp|*.png";
                if (f.ShowDialog() == DialogResult.OK)
                {
                    _profile.StatusManager.SetImage(_jobStatus, f.FileName);
                    BingImage();
                }
            }
        }
    }
}
