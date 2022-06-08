using Interfaces;
using Job.Models;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Job.Profiles
{
    [Serializable]
    public class ProfileSettings : IProfileSettings, INotifyPropertyChanged
    {

        public ExtendedSettings ExtendedSettings { get; set; } = new ExtendedSettings();
        public MailSettings Mail { get; set; } = new MailSettings();
        public MongoDbSettings BaseSettings { get; set; } = new MongoDbSettings();
        public RabbitMqSettings RabbitMq { get; set; } = new RabbitMqSettings();
        public FileBrowserSettings FileBrowser { get; set; } = new FileBrowserSettings();
        public JobSettings JobSettings { get; set; } = new JobSettings();
        public PdfConverterSettings PdfConverterSettings { get; set; } = new PdfConverterSettings();
        public IPdfConverterSettings GetPdfConverterSettings() => PdfConverterSettings;
        public string OLVState { get; set; }
        public string ProfileName { get; set; } = "SomeOne";
        public IBaseSettings GetBaseSettings() => BaseSettings;

        public IMailSettings GetMail() => Mail;

        public IMQSettings GetRabbitMq() => RabbitMq;

        public IFileBrowserSettings GetFileBrowser() => FileBrowser;
        public IJobSettings GetJobSettings() => JobSettings;

        //public string SignaJobsPath { get; set; }
        public bool CasheEnable { get; set; }
        public decimal CountExplorers { get; set; } = 1;
        public bool ExplorerInRightPanel { get; set; }
        //public string SignaFileShablon { get; set; }
        [Obsolete]
        public bool OpenSignaFileWithoutPrompt { get; set; }
        //public bool StoreByYear { get; set; }
        public bool HideTabPlates { get; set; }
        public bool HideCategory { get; set; }
        public bool CloseAfterPasteText { get; set; }



        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void NotifyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



    }
}
