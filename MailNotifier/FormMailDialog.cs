
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Windows.Forms;


namespace MailNotifier
{
    public partial class FormMailDialog : Form
    {
        private Mail _mail;

        public FormMailDialog(Mail mail)
        {
            InitializeComponent();
            _mail = mail;

            _mail.OnNewMail+= MailOnOnNewMail;
            _mail.OnDeleteMail+= MailOnOnDeleteMail;
            _mail.OnError+= MailOnOnError;

            objectListViewInbox.AddObjects(_mail.GetInboxMessages());
        }

        private void MailOnOnError(object sender, EventArgs e)
        {
            
        }

        private void MailOnOnDeleteMail(object sender, uint e)
        {
            var message = objectListViewInbox.Objects?.Cast<MessageEx>().FirstOrDefault(x => x.Id == e);

            if (message != null)
                objectListViewInbox.RemoveObject(message);


        }

        private void MailOnOnNewMail(object sender, MessageEx e)
        {
           objectListViewInbox.AddObject(e);
        }

        private void FormMailDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void objectListViewInbox_Click(object sender, EventArgs e)
        {
            ShowMessageBody(objectListViewInbox.SelectedObject as MessageEx);
            ShowMessageAttachment(objectListViewInbox.SelectedObject as MessageEx);
        }

        private void ShowMessageAttachment(MessageEx messageEx)
        {
            objectListViewAttachment.ClearObjects();

            if (messageEx == null) return;

            if (messageEx.OriginalMessage.Attachments.Count > 0)
            {
                foreach (Attachment attachment in messageEx.OriginalMessage.Attachments)
                {
                    objectListViewAttachment.AddObject(attachment);
                }
            }

        }

        private string _altbody;

        private void ShowMessageBody(MessageEx messageEx)
        {
           if (messageEx == null) return;

            _altbody = string.Empty;



            string appPath = Path.Combine(Application.StartupPath, "Mail",messageEx.Id.ToString());



            if (!File.Exists(Path.Combine(appPath,"0.html")))
            {
                //Directory.CreateDirectory(appPath);

                foreach (var alternateView in messageEx.OriginalMessage.AlternateViews)
                {

                    if (string.Compare(alternateView.ContentType.MediaType, "text/html", StringComparison.Ordinal) == 0)
                    {
                        var byteBuffer = new byte[alternateView.ContentStream.Length];
                        alternateView.ContentStream.Seek(0, SeekOrigin.Begin);
                        _altbody = messageEx.OriginalMessage.BodyEncoding.GetString(byteBuffer, 0, alternateView.ContentStream.Read(byteBuffer, 0, byteBuffer.Length));

                    }
                    else if (string.Compare(alternateView.ContentType.MediaType, "image/png", StringComparison.Ordinal) == 0)
                    {
                        SaveAlternateImage(appPath, alternateView);
                        _altbody =_altbody.Replace($"cid:{alternateView.ContentId}", alternateView.ContentId);
                    }

                }

                File.WriteAllText(Path.Combine(appPath,"0.html"),_altbody);
            }
           
            webBrowser1.Navigate(new Uri(Path.Combine(appPath,"0.html")));
            
        }

        private static void SaveAlternateImage(string appPath, AlternateView alternateView)
        {
            var path = Path.Combine(appPath, $"{alternateView.ContentId}");

            byte[] allBytes = new byte[alternateView.ContentStream.Length];
            _ = alternateView.ContentStream.Read(allBytes, 0, (int) alternateView.ContentStream.Length);

            BinaryWriter writer =
                new BinaryWriter(new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None));
            writer.Write(allBytes);
            writer.Close();
        }

        public void SaveMailAttachment(Attachment attachment,string targetDirectory)
        {
            byte[] allBytes = new byte[attachment.ContentStream.Length];
            _ = attachment.ContentStream.Read(allBytes, 0, (int)attachment.ContentStream.Length);

            string destinationFile = Path.Combine(targetDirectory, attachment.Name);

            BinaryWriter writer = new BinaryWriter(new FileStream(destinationFile, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None));
            writer.Write(allBytes);
            writer.Close();
        }

      

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            RefreshMailbox();
        }

        private void RefreshMailbox()
        {
            objectListViewInbox.ClearObjects();
            objectListViewInbox.AddObjects(_mail.GetInboxMessages());
        }

        private void saveToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListViewAttachment.SelectedObjects != null)

            using (Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog dialog = new CommonOpenFileDialog())
            {
                dialog.IsFolderPicker = true;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    foreach (Attachment attachment in objectListViewAttachment.SelectedObjects)
                    {
                        SaveMailAttachment(attachment, dialog.FileName);
                    }
                }
            }
        }

        private void archiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (MessageEx ex in objectListViewInbox.SelectedObjects)
            {
                _mail.Archive(ex);
                objectListViewInbox.RemoveObject(ex);
            }
        }
    }
}
