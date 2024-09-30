using Interfaces;
using JobSpace;
using Krypton.Toolkit;
using Microsoft.Scripting.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ActiveWorks.Forms
{
    public partial class FormMoveSignaFileToOrder : KryptonForm
    {
        IUserProfile _profile;
        ObservableCollection<SignaFile> _signaFiles = new ObservableCollection<SignaFile>();

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
            _signaFiles.AddRange(files);
            objectListView1.AddObjects(_signaFiles);

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
            var jobs = _profile.Base.All<JobSpace.Job>("Jobs");
            foreach (var job in jobs)
            {

            }
        }
    }




    public class SignaFile : INotifyPropertyChanged
    {

        FileStatus _status = FileStatus.Idle;

        public SignaFile(string path)
        {
            Path = path;
            Name = System.IO.Path.GetFileNameWithoutExtension(path);
        }

        public string Path { get;set;}
        public string OrderPath { get;set; }
        public string Name { get;set;}
        public FileStatus Status {
            get => _status;
            set {_status = value;
                OnPropertyChanged();
                } } 
        public Color Color { get;set;} = Color.White;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public enum FileStatus
    {
        Idle,
        Moved,
        Skiped
    }
}
