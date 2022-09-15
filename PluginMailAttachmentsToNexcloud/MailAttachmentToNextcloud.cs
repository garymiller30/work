using ExtensionMethods;
using Interfaces;
using Interfaces.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Windows.Forms;

namespace PluginMailAttachmentsToNextcloud
{
    sealed class MailAttachmentToNextcloud : IPluginMail
    {

        public IUserProfile UserProfile { get; set; }

        private CloudSettings Settings { get; set; } = new CloudSettings();

        //private string _fileSettingsPath;


        public MailAttachmentToNextcloud()
        {
        }

        private void LoadSettings()
        {
            Settings = UserProfile.Plugins.LoadSettings<CloudSettings>();
        }


        public void ProcessMessageBeforeSend(MailMessage message)
        {
            LoadSettings();

            if (message.Attachments.Any())
            {
                // перевірити розмір прилінкованих файлів
                if (message.Attachments.Sum(x => x.ContentStream.Length) > Settings.SizeLimit)
                {
                    CreateFileLink(message);
                }
            }
        }

        private void CreateFileLink(MailMessage message)
        {
            var client = new NextCloudClient(Settings);

            List<Attachment> remove = new List<Attachment>();

            string attachMessage = Properties.Resources.shablon_begin;

            //create remote folder if count of attachments more than 2
            if (message.Attachments.Count > 2)
            {
                var remoteFolder = CreateRemoteFolder(client, message);
                if (remoteFolder != null)
                {

                    foreach (var attachment in message.Attachments)
                    {
                        if (attachment.ContentStream is FileStream fileStream)
                        {
                            var res = client.Upload(remoteFolder, fileStream);
                            if (!res)
                            {
                                throw new Exception($"Can't upload file {fileStream.Name}");
                            }
                            else
                            {
                                remove.Add(attachment);
                            }

                        }
                    }

                    string shareFolderLink = client.GetShareFolderLink(remoteFolder);

                    if (shareFolderLink != string.Empty)
                    {
                        var filelink = Properties.Resources.shablon_attachment.Replace("{link}", shareFolderLink)
                            .Replace("{size}", $"{message.Attachments.Count} файл(а)")
                            .Replace("{filename}", remoteFolder); ;

                        attachMessage += filelink;

                    }
                    else
                    {
                        throw new Exception($"Can't share folder {remoteFolder}");
                    }

                }
                else
                {
                    throw new Exception($"Can't create remote folder");
                }
            }
            else
            {
                foreach (var attachment in message.Attachments)
                {
                    if (attachment.ContentStream is FileStream fileStream)
                    {
                        var lenght = attachment.ContentStream.Length.GetFileSizeInString();
                        var filename = attachment.Name;

                        var link = client.Upload(fileStream);

                        if (!string.IsNullOrEmpty(link))
                        {
                            remove.Add(attachment);

                            var filelink = Properties.Resources.shablon_attachment.Replace("{link}", link)
                                .Replace("{size}", lenght)
                                .Replace("{filename}", filename); ;

                            attachMessage += filelink;

                        }
                        else
                        {
                            throw client.ErrorException;
                        }
                    }
                }
            }




            attachMessage += Properties.Resources.shablon_end;

            message.Body += attachMessage;

            foreach (var attachment in remove)
            {
                attachment.Dispose();
                message.Attachments.Remove(attachment);
            }
        }

        private string CreateRemoteFolder(NextCloudClient client, MailMessage message)
        {
            //generate folder name

            var folder = $"{DateTime.Now.Year}.{DateTime.Now.Month}.{DateTime.Now.Day}_{DateTime.Now.Hour}-{DateTime.Now.Minute}-{DateTime.Now.Second:D2}";
            if (client.CreateFolder(folder))
            {
                return folder;
            }

            return null;
        }

        public string PluginName { get; } = "FileToNextcloud";
        public string PluginDescription { get; } = "Завантажує завеликі файли на сервер nextcloud і вставляє лінки в тіло листа";
        public void ShowSettingsDlg()
        {
            using (var form = new FormSettings(Settings))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    SaveSettings();
                }
            }
        }

        private void SaveSettings()
        {
            UserProfile.Plugins.SaveSettings(Settings);
            //var str = JsonConvert.SerializeObject(_settings);
            //File.WriteAllText(_fileSettingsPath, str,Encoding.Unicode);
        }
    }
}
