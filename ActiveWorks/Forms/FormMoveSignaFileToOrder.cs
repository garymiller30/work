using Interfaces;
using Krypton.Toolkit;
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

namespace ActiveWorks.Forms
{
    public partial class FormMoveSignaFileToOrder : KryptonForm
    {
        IUserProfile _profile;
        List<SignaFile> _signaFiles;

        public FormMoveSignaFileToOrder(IUserProfile profile)
        {
            InitializeComponent();
            _profile = profile;

            kryptonTextBoxSignaJobPath.Text = _profile.Settings.GetJobSettings().SignaJobsPath;
            kryptonTextBoxSignaJobTemplate.Text = _profile.Settings.GetJobSettings().SignaFileShablon;

        }

        void FindFiles()
        {
            var signaJobsPath = kryptonTextBoxSignaJobPath.Text;
            var template = kryptonTextBoxSignaJobTemplate.Text;

            var files = Directory.GetFiles(signaJobsPath, "*.sdf").Select(x => new SignaFile(x)).ToList();
            objectListView1.AddObjects(files);

        }

        private void kryptonButtonFind_Click(object sender, EventArgs e)
        {
            FindFiles();
        }

        private async void kryptonButtonCheckTemplate_Click(object sender, EventArgs e)
        {
            await Task.Run(CheckTemplates);
        }

        private void CheckTemplates()
        {
            
        }
    }




    public class SignaFile
    {
        public SignaFile(string path)
        {
            Path = path;
            Name = System.IO.Path.GetFileNameWithoutExtension(path);
        }

        public string Path { get;set;}
        public string OrderPath { get;set; }
        public string Name { get;set;}
        public FileStatus Status {get;set;} = FileStatus.Idle;
        public Color Color { get;set;} = Color.White;
    }

    public enum FileStatus
    {
        Idle,
        Moved,
        Skiped
    }
}
