using Interfaces;
using JobSpace.Models;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace JobSpace.Profiles
{
    [Serializable]
    public class ProfileSettings : IProfileSettings, INotifyPropertyChanged
    {
        public ExtendedSettings ExtendedSettings { get; set; } = new ExtendedSettings();
        public MailSettings Mail { get; set; } = new MailSettings();
        public MongoDbSettings BaseSettings { get; set; } = new MongoDbSettings();
        public FileBrowserSettings FileBrowser { get; set; } = new FileBrowserSettings();
        public JobSettings JobSettings { get; set; } = new JobSettings();
        public PdfConverterSettings PdfConverterSettings { get; set; } = new PdfConverterSettings();
        public IPdfConverterSettings GetPdfConverterSettings() => PdfConverterSettings;
        public string OLVState { get; set; }
        public string ProfileName { get; set; } = "SomeOne";
        public IBaseSettings GetBaseSettings() => BaseSettings;
        public IMailSettings GetMail() => Mail;
        public IFileBrowserSettings GetFileBrowser() => FileBrowser;
        public IJobSettings GetJobSettings() => JobSettings;
        public JobListSettings JobListSettings { get; set; } = new JobListSettings();
        public IJobListSettings GetJobListSettings() => JobListSettings;
        public decimal CountExplorers { get; set; } = 1;
        public bool HideCategory { get; set; }
        public bool CloseAfterPasteText { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Normalize()
        {
            if (JobListSettings == null) JobListSettings = new JobListSettings();
        }

    }
}
