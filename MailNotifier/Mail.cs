// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com 

using MailNotifier.Shablons;
using S22.Imap;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Interfaces;
using Attachment = System.Net.Mail.Attachment;
using MailMessage = System.Net.Mail.MailMessage;

namespace MailNotifier
{
    public sealed class Mail : IMail
    {
        public event EventHandler<Exception> OnError = delegate { };

        public IMailSettings Settings { get; set; }
        private readonly IUserProfile _profile;
        private IJob _curJob;
        private readonly List<string> _attachmentsList = new List<string>();
        public bool ShowBaloon;
       
        //private AutoResetEvent _reconnectEvent;

        //private CancellationTokenSource _tokenSource;
       
        public MailShablonManager ShablonManager;

        public Mail(IUserProfile userProfile, IMailSettings settings)
        {
            _profile = userProfile;

            Settings = settings;
            Settings.SettingsFolder = userProfile.ProfilePath;
            if (Settings.MailTo == null) Settings.MailTo = new List<string>();

            InitShablonManager();
        }

        private void InitShablonManager()
        {
            ShablonManager = new MailShablonManager(this);
        }
        //public void StopWatching()
        //{
        //    _tokenSource?.Cancel();
        //    _reconnectEvent?.Set();
        //}

        public void Send(object job, string to, string tema, string body)
        {
            SmtpClient smtp = null;

            try
            {
                var fromAddress = new MailAddress(Settings.MailFrom, Settings.MailFrom);
                var toAddress = new MailAddress(to, to);

                smtp = new SmtpClient
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
                OnError(this,e);
                //ExceptionMessage = e.Message;
            }
            finally {
                smtp?.Dispose();
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
                        _profile.Plugins.Mail?.ProcessMessageBeforeSend(message);
                        smtp.Send(message);
                        Logger.Log.Info(this, "Mail", $"\"{message.Subject}\" => \"{to}\"");
                    }
                    catch (Exception e)
                    {
                        Logger.Log.Error(this, "Mail", $"\"{message.Subject}\" => \"{to}\" send error: {e.Message}");
                        OnError(this,e);
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

            SmtpClient smtp = null;

            try
            {
                var fromAddress = new MailAddress(Settings.MailFrom, Settings.MailFrom);
                var toAddress = new MailAddress(to, to);

                var subject = Path.GetFileName(attachmentPath);

                var attachment = new Attachment(attachmentPath);

                smtp = new SmtpClient
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
                OnError(this,e);
            }
            finally
            {
                smtp?.Dispose();
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