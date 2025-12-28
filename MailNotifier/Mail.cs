// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com 

using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Interfaces;
using MailNotifier.Shablons;
using MimeKit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Attachment = System.Net.Mail.Attachment;
using MailMessage = System.Net.Mail.MailMessage;

namespace MailNotifier
{
    public sealed class Mail : IMail
    {
        public event EventHandler<Exception> OnError = delegate { };

        public IMailSettings Settings { get; set; }
        public readonly IUserProfile Profile;
        private IJob _curJob;
        private List<string> _attachmentsList { get; set; } = new List<string>();
        public bool ShowBaloon;
        List<ToolStripItem> _sendMenuItems;

        public MailShablonManager ShablonManager { get; set; }

        public Mail(IUserProfile userProfile, IMailSettings settings)
        {
            Profile = userProfile;

            Settings = settings;
            Settings.SettingsFolder = userProfile.ProfilePath;
            if (Settings.MailTo == null) Settings.MailTo = new List<string>();

            InitShablonManager();
        }

        private void InitShablonManager()
        {
            ShablonManager = new MailShablonManager(this);
        }

        /// <summary>
        /// відправити повідомлення декільком користувачам
        /// </summary>
        /// <param name="job"></param>
        /// <param name="to"></param>
        /// <param name="tema"></param>
        /// <param name="body"></param>
        /// <param name="attachFiles"></param>
        public void SendToMany(string to, string tema, string body, string[] attachFiles)
        {
            if (Settings.MailConnectType == Interfaces.Enums.MailConnectTypeEnum.SMTP)
            {
                try
                {
                    SendBySMTP(to, tema, body, attachFiles);
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Помилка відправки пошти через SMTP: {e.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (Settings.MailConnectType == Interfaces.Enums.MailConnectTypeEnum.GOOGLE_API)
            {
                try
                {
                    SendByGoogleApi(to, tema, body, attachFiles);
                }
                catch (Exception e)
                {

                    MessageBox.Show($"Помилка відправки пошти через Google API: {e.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                
            }
        }

        private async void SendByGoogleApi(string to, string tema, string body, string[] attachFiles)
        {
            string[] Scopes = {
            GmailService.Scope.GmailSend,
            DriveService.Scope.DriveFile // дозвіл на завантаження в Drive
            };
            string ApplicationName = "ActiveWorks";
            string credPath = Path.Combine(Path.GetDirectoryName(Settings.ClientSecretFile), "token_store");
            UserCredential credential;

            try
            {
                using (var stream = new FileStream(Settings.ClientSecretFile, FileMode.Open, FileAccess.Read))
                {
                    credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.FromStream(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true));
                }

                var driveService = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });

                var service = new GmailService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

                // створити повідомлення  через MimeKit
                var mimeMessage = new MimeMessage();

                var res = Profile.Plugins.Mail?.UploadFiles(attachFiles);

                if (!string.IsNullOrEmpty(res))
                {
                    // додати посилання на файли в тілі листа
                    var sb = new StringBuilder();
                    sb.AppendLine(body);
                    sb.AppendLine(res);
                    mimeMessage.Body = new TextPart("html")
                    {
                        Text = sb.ToString()
                    };
                }
                else
                {
                    await AddAttachesToGoogleDisk(mimeMessage, attachFiles, driveService, body);
                }

                mimeMessage.From.Add(new MailboxAddress(Settings.MailFrom, Settings.MailFrom));
                var sendTo = to.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var s in sendTo)
                {
                    mimeMessage.To.Add(new MailboxAddress(s, s));
                }
                mimeMessage.Subject = tema;

                // 4. Конвертуємо в raw base64
                using (var memory = new MemoryStream())
                {
                    mimeMessage.WriteTo(memory);
                    var raw = System.Convert.ToBase64String(memory.ToArray())
                        .Replace('+', '-')
                        .Replace('/', '_')
                        .Replace("=", "");

                    var gmailMessage = new Google.Apis.Gmail.v1.Data.Message { Raw = raw };

                    // 5. Відправляємо
                    var result = await service.Users.Messages.Send(gmailMessage, "me").ExecuteAsync();

                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Помилка відправки пошти через Google API: {e.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Log.Error(this, "Mail", e.Message);
            }
        }

        private async Task AddAttachesToGoogleDisk(MimeMessage mimeMessage, string[] attachFiles, DriveService driveService,string body)
        {
            // перевірити загальний розмір вкладень, якщо більше 20 Мб - завантажити в Drive, якщо менше - прикріпити до листа

            long totalSize = 0;
            foreach (var file in attachFiles)
            {
                var fi = new FileInfo(file);
                totalSize += fi.Length;
            }

            if (totalSize > 20 * 1024 * 1024)
            {
                var attachGoogleFiles = new List<Google.Apis.Drive.v3.Data.File>();
                // завантажити в Drive і  Робимо файл доступним за посиланням
                foreach (var file in attachFiles)
                {
                    var fi = new FileInfo(file);
                    var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                    {
                        Name = fi.Name,
                    };
                    FilesResource.CreateMediaUpload request;
                    using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {

                        fs.Position = 0;
                        request = driveService.Files.Create(fileMetadata, fs, "application/octet-stream");
                        // Вкажи поля, які хочеш отримати у ResponseBody — робити це потрібно ДО UploadAsync
                        request.Fields = "id, webViewLink, name";
                        IUploadProgress progress = await request.UploadAsync();
                        if (progress.Status == UploadStatus.Completed)
                        {
                            var uploadedFile = request.ResponseBody;
                            if (uploadedFile != null)
                            {
                                attachGoogleFiles.Add(uploadedFile);
                                var permission = new Google.Apis.Drive.v3.Data.Permission()
                                {
                                    Role = "reader",
                                    Type = "anyone"
                                };
                                await driveService.Permissions.Create(permission, uploadedFile.Id).ExecuteAsync();
                            }
                        }
                        else
                        {
                            Logger.Log.Error(this, "Mail", $"Error uploading file {file}: {progress.Exception.Message}");
                        }
                    }

                }
                // додати посилання на файли в тілі листа
                var sb = new StringBuilder();
                sb.AppendLine(body);
                sb.AppendLine("<br/><br/>Файли додані в Google Drive:<br/>");
                foreach (var file in attachGoogleFiles)
                {
                    sb.AppendLine($"<a href=\"https://drive.google.com/file/d/{file.Id}/view?usp=sharing\">{file.Name}</a><br/>");
                }
                mimeMessage.Body = new TextPart("html")
                {
                    Text = _curJob == null ? sb.ToString() : Convert(_curJob, sb.ToString())
                };

            }
            else
            {
                var multipart = new Multipart("mixed");

                // прикріпити до листа
                foreach (var file in attachFiles)
                {
                    var attachment = new MimePart("application", "octet-stream")
                    {
                        Content = new MimeContent(File.OpenRead(file)),
                        ContentDisposition = new MimeKit.ContentDisposition(MimeKit.ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                        FileName = Path.GetFileName(file)
                    };
                    multipart.Add(attachment);
                }


                var textPart = new TextPart("html")
                {
                    Text = body
                };

                multipart.Add(textPart);

                mimeMessage.Body = multipart;
            }

            
        }

        private void SendBySMTP(string to, string tema, string body, string[] attachFiles)
        {
            SmtpClient smtp = null;
            try
            {
                var fromAddress = new MailAddress(Settings.MailFrom, Settings.MailFrom);

                smtp = new SmtpClient
                {
                    Host = Settings.MailSmtpServer,// "smtp.gmail.com",
                    Port = Settings.MailSmtpPort,//587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, Settings.MailFromPassword)
                };

                using (var message = new MailMessage())
                {
                    message.From = fromAddress;
                    message.Subject = _curJob == null ? tema : Convert(_curJob, tema);
                    message.IsBodyHtml = true;
                    message.Body = $"{(_curJob == null ? body : Convert(_curJob, body))}";
                    message.BodyEncoding = Encoding.UTF8;
                    var sendTo = to.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var s in sendTo)
                    {
                        message.To.Add(new MailAddress(s, s));
                    }

                    foreach (var file in attachFiles)
                    {
                        message.Attachments.Add(new Attachment(file));
                    }

                    try
                    {
                        Profile.Plugins.Mail?.ProcessMessageBeforeSend(message);
                        smtp.Send(message);
                        Logger.Log.Info(this, "Mail", $"\"{message.Subject}\" => \"{to}\"");
                    }
                    catch (Exception e)
                    {
                        Logger.Log.Error(this, "Mail", $"\"{message.Subject}\" => \"{to}\" send error: {e.Message}");
                        OnError(this, e);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log.Error(this, "Mail", e.Message);
                OnError(this, e);
            }
            finally
            {
                smtp?.Dispose();
            }
        }

        public static string Convert(object job, string txt)
        {
            //var str = txt.Split(' ');
            var sb = new StringBuilder(txt);

            sb.Replace("$OrderNumber", job.GetType().GetProperty("Number")?.GetValue(job).ToString() ?? String.Empty);

            sb.Replace("$OrderDescription", job.GetType().GetProperty("Description")?.GetValue(job).ToString() ?? String.Empty);

            return sb.ToString();
        }



        public void SendFile(string to, string attachmentPath)
        {
            SendToMany(to, Path.GetFileName(attachmentPath), "", new[] { attachmentPath });
        }

        /// <summary>
        /// create "send mail" menu
        /// </summary>
        /// <param name="ttmClick"></param>
        /// <returns></returns>
        public List<ToolStripItem> GetMenu(EventHandler ttmClick)
        {
            //-----------------------------------------------
            if (_sendMenuItems != null) return _sendMenuItems;

            CreateSendMailMenu(ttmClick);
            return _sendMenuItems;
        }

        private void CreateSendMailMenu(EventHandler ttmClick)
        {
            _sendMenuItems = new List<ToolStripItem>();

            var sel = new ToolStripMenuItem { Text = @"Вибрати..." };
            sel.Click += SelOnClick;
            sel.ForeColor = Color.DarkGreen;
            _sendMenuItems.Add(sel);

            _sendMenuItems.Add(new ToolStripSeparator());
            //------------------------------------------------
            // додамо шаблони, якщо є
            // -----------------------------------------------

            var shablons = ShablonManager.GetTemplates();

            if (shablons.Any())
            {
                foreach (MailTemplate mailShablon in shablons)
                {
                    var ttm = new ToolStripMenuItem
                    {
                        Text = mailShablon.ShablonName,
                        Tag = mailShablon.ShablonName
                    };
                    ttm.Click += OpenMailShablon;
                    _sendMenuItems.Add(ttm);
                }

                _sendMenuItems.Add(new ToolStripSeparator());
            }

            //------------------------------------------------

            foreach (var item in Settings.MailTo)
            {
                var ttm = new ToolStripMenuItem
                {
                    Text = item,
                    Tag = item,
                };
                ttm.Click += ttmClick;
                _sendMenuItems.Add(ttm);
            }
        }

        private void OpenMailShablon(object sender, EventArgs e)
        {
            var dialog = new FormSendMail(this) { StartPosition = FormStartPosition.CenterParent };
            dialog.SetJob(_curJob);
            dialog.InitSendToList(Settings.MailTo);
            dialog.SetAttachmentList(_attachmentsList);
            dialog.SetShablon((string)((ToolStripMenuItem)sender).Tag);
            if (string.IsNullOrEmpty(dialog.GetHeader()))
            {
                if (_attachmentsList.Any())
                {
                    dialog.SetHeader(Path.GetFileNameWithoutExtension(_attachmentsList[0]));
                }
            }
            dialog.Show();
        }

        private void SelOnClick(object sender, EventArgs e)
        {
            var dialog = new FormSendMail(this);

            dialog.InitSendToList(Settings.MailTo);
            dialog.SetAttachmentList(_attachmentsList);
            dialog.Show();
        }

        public void SetAttachmentsList(IEnumerable<string> attach)
        {
            _attachmentsList.Clear();
            _attachmentsList.AddRange(attach);
        }

        public void SetCurJob(IJob job)
        {
            _curJob = job;
        }

        public void ShowSendMailDialog(string mailTo, string header, string body)
        {
            using (var dialog = new FormSendMail(this))
            {
                dialog.InitSendToList(new[] { mailTo });
                dialog.SetHeader(header);
                dialog.SetBody(body);
                dialog.ShowDialog();
            }
        }

        public void ShowSendMailDialog()
        {
            using (var dialog = new FormSendMail(this))
            {
                dialog.InitSendToList(Settings.MailTo);
                dialog.ShowDialog();
            }
        }

        public ICollection GetMailTemplates()
        {
            return ShablonManager.GetTemplates().ToArray();
        }

        public void SetMailTemplates(IEnumerable enumerable)
        {
            ShablonManager.SetMailTemplates(enumerable.Cast<MailTemplate>());
        }
    }
}