// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com 

using MailNotifier.Shablons;
using S22.Imap;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interfaces;
using Interfaces.Plugins;
using Attachment = System.Net.Mail.Attachment;
using MailMessage = System.Net.Mail.MailMessage;

namespace MailNotifier
{
    public class Mail : IMail
    {
        public IMailSettings Settings { get; set; }
        private readonly IUserProfile _profile;
        private IJob _curJob;
        private readonly List<string> _attachmentsList = new List<string>();
        //public bool Enable = false;
        public bool ShowBaloon;
        //public string Mp3FilePath { get; set; }
        private readonly string _mailPath;
        //private readonly List<NotifyIcon> _notifyIcons = new List<NotifyIcon>();
        //private Task _watchingTask;

        public List<MessageEx> GetInboxMessages()
        {
            var uids = _client.Search(SearchCondition.All(), "Inbox");

            var list = new List<MessageEx>();

            foreach (var uid in uids)
            {
                //search in local folder
                var messageFolder = Path.Combine(_mailPath, uid.ToString());
                var emailFile = Path.Combine(messageFolder, uid.ToString());

                MailMessage message;

                if (Directory.Exists(messageFolder))
                {
                    if (File.Exists(emailFile))
                    {
                        //todo: load from file
                        message = MessageEx.Load(emailFile);
                    }
                    else
                    {
                        message = _client.GetMessage(uid, FetchOptions.Normal);
                        MessageEx.Save(message, emailFile);
                    }
                }
                else
                {
                    Directory.CreateDirectory(messageFolder);
                    message = _client.GetMessage(uid, FetchOptions.Normal);
                    MessageEx.Save(message, emailFile);
                }

                list.Add(MessageEx.ToMessageEx(uid, message));
            }

            return list;
        }

        private ImapClient _client;
        private AutoResetEvent _reconnectEvent;

        public event EventHandler OnError = delegate { };

        public event EventHandler<MessageEx> OnNewMail = delegate { };
        public event EventHandler<List<MessageEx>> OnNewMails = delegate { };


        public event EventHandler<uint> OnDeleteMail = delegate { };

        private CancellationTokenSource _tokenSource;
        private CancellationToken _token;

        public static string ExceptionMessage;

        public MailShablonManager ShablonManager;
        //private FormMailDialog _mailDialog;

        public Mail(IUserProfile userProfile, IMailSettings settings)
        {
            _profile = userProfile;
            _mailPath = Path.Combine(Application.StartupPath, "Mail");

            Settings = settings;
            Settings.SettingsFolder = userProfile.ProfilePath;
            if (Settings.MailTo == null) Settings.MailTo = new List<string>();

            InitShablonManager();
            StartWatching();
        }

        private void InitShablonManager()
        {
            ShablonManager = new MailShablonManager(this);
        }

        private void StartWatching()
        {
            if (!Validate()) return;

            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;
            _reconnectEvent = new AutoResetEvent(false);

            //_watchingTask =
            Task.Run(() =>
            {
                retry:
                var result = CreateClient();
                if (!result)
                {
                    _reconnectEvent.WaitOne();

                    if (!_token.IsCancellationRequested)
                    {
                        if (Settings.MailAutoRelogon)
                        {
                            goto retry;
                        }

                        var res = MessageBox.Show("Disconnect. Retry?", "Mail notifycation", MessageBoxButtons.YesNo);
                        if (res == DialogResult.Yes) goto retry;
                    }
                    _reconnectEvent.Close();
                    _client?.Dispose();
                }

            }, _token).ConfigureAwait(false);

        }

        private bool Validate()
        {
            return Settings.Validate();
        }

        public void StopWatching()
        {
            _tokenSource?.Cancel();
            _reconnectEvent?.Set();
        }

        private bool CreateClient()
        {
            _client?.Dispose();

            try
            {
                _client = new ImapClient(
                    Settings.MailImapHost,
                    Settings.MailImapPort,
                    Settings.MailFrom,
                    Settings.MailFromPassword,
                    AuthMethod.Login, true);

                // We should make sure IDLE is actually supported by the server.
                if (_client.Supports("IDLE") == false)
                {
                    Debug.WriteLine("Server does not support IMAP IDLE");
                    return false;
                }

                // We want to be informed when new messages arrive.
                _client.NewMessage += OnNewMessage;
                _client.IdleError += Client_IdleError;
                _client.MessageDeleted += Client_MessageDeleted;
                return true;
            }
            catch (Exception e)
            {
                ExceptionMessage = e.Message;
                Logger.Log.Error(this, "Mail", e.Message);
                OnError(this, null);

            }

            return false;
        }

        private void Client_MessageDeleted(object sender, IdleMessageEventArgs e)
        {
            OnDeleteMail(this, e.MessageUID);
        }

        private void Client_IdleError(object sender, IdleErrorEventArgs e)
        {
            _reconnectEvent.Set();
        }

        private void OnNewMessage(object sender, IdleMessageEventArgs e)
        {
            var uids = e.Client.Search(SearchCondition.Unseen());

            try
            {
                object messages;
                if (uids.Count() > 1)
                {
                    messages = new List<MessageEx>();

                    foreach (var uid in uids)
                    {
                        var message = getMailMessage(uid);
                        ((List<MessageEx>)messages).Add(MessageEx.ToMessageEx(uid, message));
                    }
                    OnNewMails(this, (List<MessageEx>)messages);

                }
                else
                {
                    var uid = uids.First();
                    messages = getMailMessage(uid);
                    OnNewMail(this, MessageEx.ToMessageEx(uid, (MailMessage)messages));
                }

                _profile.Plugins.PlaySound(AvailableSound.Mail_NewMessage, messages);
            }
            catch (Exception exception)
            {
                Logger.Log.Error(this, "Mail notifier", exception.Message);
                Debug.WriteLine(exception);

            }



            //foreach (var uid in uids)
            //{
            //    ////var message = e.Client.GetMessage(uid);
            //    //var messageFolder = Path.Combine(_mailPath, uid.ToString());
            //    //var emailFile = Path.Combine(messageFolder, uid.ToString());
            //    //Directory.CreateDirectory(messageFolder);
            //    try
            //    {
            //        var message = _client.GetMessage(uid, FetchOptions.Normal,seen:false);
            //        //_client.RemoveMessageFlags(uid, null, MessageFlag.Seen);
            //        OnNewMail(this, MessageEx.ToMessageEx(uid, message));
            //        _profile.Plugins.PlaySound(AvailableSound.Mail_NewMessage,message);
            //    }
            //    catch (Exception exception)
            //    {
            //        Logger.Log.Error(this,"Mail notifier",exception.Message);
            //        Debug.WriteLine(exception);

            //    }

            //    //MessageEx.Save(message,emailFile);


            //}

        }

        private MailMessage getMailMessage(uint uid)
        {
            return _client.GetMessage(uid, FetchOptions.Normal, seen: false);
        }

        internal void Archive(MessageEx ex)
        {
            _client.SetMessageFlags(ex.Id, "inbox", MessageFlag.Deleted);
        }

        //public int GetUnseenMessagesCount()
        //{
        //    var uids = _client.Search(SearchCondition.Unseen(), "Inbox");
        //    return uids.Count();
        //}

        //private void Ni_MouseClick(object sender, MouseEventArgs e)
        //{
        //    ((NotifyIcon)sender).ShowBalloonTip(30000);
        //}

        //private void NiOnClick(object sender, EventArgs eventArgs)
        //{
        //    ((NotifyIcon)sender).Visible = true;
        //    ((NotifyIcon)sender).ShowBalloonTip(30000);
        //}

        //private void NiOnBalloonTipClosed(object sender, EventArgs eventArgs)
        //{
        //    ((NotifyIcon)sender).Dispose();
        //}

        public void Send(object job, string to, string tema, string body)
        {
            ExceptionMessage = null;

            try
            {
                var fromAddress = new MailAddress(Settings.MailFrom, Settings.MailFrom);
                var toAddress = new MailAddress(to, to);

                var smtp = new SmtpClient
                {
                    Host = Settings.MailSmtpServer, // "smtp.gmail.com",
                    Port = Settings.MailSmtpPort, //587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, Settings.MailFromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress))
                {
                    message.Subject = Convert(job, tema);
                    message.Body = Convert(job, body);

                    smtp.Send(message);
                    Logger.Log.Info(this, "Mail", $"\"{message.Subject}\" => \"{to}\"");
                }
            }
            catch (Exception e)
            {
                Logger.Log.Error(this, "Mail", e.Message);
                ExceptionMessage = e.Message;
            }
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
            ExceptionMessage = null;
            try
            {
                var fromAddress = new MailAddress(Settings.MailFrom, Settings.MailFrom);

                var smtp = new SmtpClient
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
                        _profile.Plugins.Mail?.ProcessMessageBeforeSend(message);
                        smtp.Send(message);
                        Logger.Log.Info(this, "Mail", $"\"{message.Subject}\" => \"{to}\"");
                    }
                    catch (Exception e)
                    {
                        Logger.Log.Error(this, "Mail", $"\"{message.Subject}\" => \"{to}\" send error: {e.Message}");
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log.Error(this, "Mail", e.Message);
                ExceptionMessage = e.Message;
            }
        }

        private static string Convert(object job, string txt)
        {
            //var str = txt.Split(' ');
            var sb = new StringBuilder(txt);

            sb.Replace("$OrderNumber", job.GetType().GetProperty("Number")?.GetValue(job).ToString() ?? String.Empty);

            sb.Replace("$OrderDescription", job.GetType().GetProperty("Description")?.GetValue(job).ToString() ?? String.Empty);

            return sb.ToString();
        }

        /*
                public void Send(string to, string attachmentPath)
                {
                    ExceptionMessage = null;

                    try
                    {
                        var fromAddress = new MailAddress(MailSettings.UserName, MailSettings.UserName);
                        var toAddress = new MailAddress(to, to);

                        var subject = "звіт " + Path.GetFileName(attachmentPath);

                        var attachment = new Attachment(attachmentPath);

                        var smtp = new SmtpClient
                        {
                            Host = MailSettings.SmtpServer,
                            Port = MailSettings.SmtpPort,
                            EnableSsl = true,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential(fromAddress.Address, MailSettings.Password)
                        };
                        using (var message = new MailMessage(fromAddress, toAddress)
                        {
                            Subject = subject,
                            Attachments = { attachment }
                        })
                        {
                            smtp.Send(message);
                        }

                        Logger.Log.Info(this,"Mail",$"\"{subject}\" => \"{to}\"");
                    }
                    catch (Exception e)
                    {
                        Logger.Log.Error(this,"Mail",e.Message);
                        ExceptionMessage = e.Message;
                    }
                }
        */

        public void SendFile(string to, string attachmentPath)
        {
            ExceptionMessage = null;
            try
            {
                var fromAddress = new MailAddress(Settings.MailFrom, Settings.MailFrom);
                var toAddress = new MailAddress(to, to);

                var subject = Path.GetFileName(attachmentPath);

                var attachment = new Attachment(attachmentPath);

                var smtp = new SmtpClient
                {
                    Host = Settings.MailSmtpServer,
                    Port = Settings.MailSmtpPort,
                    EnableSsl = true,

                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, Settings.MailFromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress) {Subject = subject, Attachments = { attachment }})
                {
                    message.IsBodyHtml = true;
                    _profile.Plugins.Mail?.ProcessMessageBeforeSend(message);
                    smtp.Send(message);
                }

                Logger.Log.Info(this, "Mail", $"\"{subject}\" => \"{to}\"");
            }
            catch (Exception e)
            {
                Logger.Log.Error(this, "Mail", e.Message);
                ExceptionMessage = e.Message;
            }
        }

        /// <summary>
        /// create "send mail" menu
        /// </summary>
        /// <param name="ttmClick"></param>
        /// <returns></returns>
        public List<ToolStripItem> GetMenu(EventHandler ttmClick)
        {
            //-----------------------------------------------
            var l = new List<ToolStripItem>();

            var sel = new ToolStripMenuItem { Text = @"Вибрати..." };
            sel.Click += SelOnClick;
            sel.ForeColor = Color.DarkGreen;
            l.Add(sel);

            l.Add(new ToolStripSeparator());
            //------------------------------------------------
            // додамо шаблони, якщо є
            // -----------------------------------------------

            var shablons = ShablonManager.GetShablons();

            if (shablons.Any())
            {
                foreach (MailShablon mailShablon in shablons)
                {
                    var ttm = new ToolStripMenuItem
                    {
                        Text = mailShablon.ShablonName,
                        Tag = mailShablon.ShablonName
                    };
                    ttm.Click += OpenMailShablon;
                    l.Add(ttm);
                }

                l.Add(new ToolStripSeparator());
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
                l.Add(ttm);
            }


            return l;
        }

        private void OpenMailShablon(object sender, EventArgs e)
        {

            var dialog = new FormSendMail(this){ StartPosition = FormStartPosition.CenterParent};
            
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

        //public void ShowMailDialog()
        //{
        //    //_mailDialog.Show();
        //}

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
    }
}