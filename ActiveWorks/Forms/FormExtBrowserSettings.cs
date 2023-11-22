using Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ActiveWorks.Forms
{
    public sealed partial class FormExtBrowserSettings : Form
    {
        private readonly IUserProfile _profile;
        readonly List<IFileBrowserControlSettings> _settings = new List<IFileBrowserControlSettings>();
        public FormExtBrowserSettings(IUserProfile currentProfile)
        {
            InitializeComponent();

            _profile = currentProfile;

            foreach (var browser in _profile.FileBrowser.Browsers)
            {
                _settings.Add(browser.GetSettings());
            }

            listBox1.Items.AddRange(_settings.ToArray());
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = listBox1.SelectedItem;
        }
    }
}
