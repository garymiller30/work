using System;
using System.Linq;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using FtpClient;

namespace Job.UserForms
{
    public sealed partial class FormFtpDirectoryList : KryptonForm
    {

        private FtpFileExt _selectedDirectory;
        private readonly Client _client;

        public FormFtpDirectoryList(string server, string user, string password, string rootDirectory, bool activeMode, int codePage)
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;

            _client = new Client();
            _client.CreateConnection(server, user, password, rootDirectory, activeMode, codePage);

            objectListView_Ftp.AddObjects(_client.GetDirectories().ToArray());
            
        }


        public FtpFileExt GetSelectedDirectory()
        {
            return _selectedDirectory;
        }

        private void button_Select_Click(object sender, EventArgs e)
        {
            if (objectListView_Ftp.SelectedObject != null)
            {
                _selectedDirectory = objectListView_Ftp.SelectedObject as FtpFileExt;
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void button_Up_Click(object sender, EventArgs e)
        {
            objectListView_Ftp.ClearObjects();
            _client.DirectoryUp();
            objectListView_Ftp.AddObjects(_client.GetDirectoriesAndFiles().ToArray());
        }
    }
}
