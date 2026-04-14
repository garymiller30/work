using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace MailNotifier.Serialization
{
    ///
    /// Serializeable mailmessage object
    ///
    [Serializable]
    public class SerializeableMailMessage
    {
        Boolean IsBodyHtml { get; set; }
        String Body { get; set; }
        SerializeableMailAddress From { get; set; }
        List<SerializeableMailAddress> To = new List<SerializeableMailAddress>();
        List<SerializeableMailAddress> CC = new List<SerializeableMailAddress>();
        List<SerializeableMailAddress> Bcc = new List<SerializeableMailAddress>();
        SerializeableMailAddress ReplyTo { get; set; }
        SerializeableMailAddress Sender { get; set; }
        String Subject { get; set; }
        List<SerializeableAttachment> Attachments = new List<SerializeableAttachment>();
        readonly Encoding BodyEncoding;
        readonly Encoding SubjectEncoding;
        readonly DeliveryNotificationOptions DeliveryNotificationOptions;
        SerializeableCollection Headers;
        readonly MailPriority Priority;
        List<SerializeableAlternateView> AlternateViews = new List<SerializeableAlternateView>();

        ///
        /// Creates a new serializeable mailmessage based on a MailMessage object
        ///
        ///
        public SerializeableMailMessage(MailMessage mm)
        {
            IsBodyHtml = mm.IsBodyHtml;
            Body = mm.Body;
            Subject = mm.Subject;
            From = SerializeableMailAddress.GetSerializeableMailAddress(mm.From);
            To = new List<SerializeableMailAddress>();
            foreach (MailAddress ma in mm.To)
            {
                To.Add(SerializeableMailAddress.GetSerializeableMailAddress(ma));
            }

            CC = new List<SerializeableMailAddress>();
            foreach (MailAddress ma in mm.CC)
            {
                CC.Add(SerializeableMailAddress.GetSerializeableMailAddress(ma));
            }

            Bcc = new List<SerializeableMailAddress>();
            foreach (MailAddress ma in mm.Bcc)
            {
                Bcc.Add(SerializeableMailAddress.GetSerializeableMailAddress(ma));
            }

            Attachments = new List<SerializeableAttachment>();
            foreach (Attachment att in mm.Attachments)
            {
                Attachments.Add(SerializeableAttachment.GetSerializeableAttachment(att));
            }

            BodyEncoding = mm.BodyEncoding;

            DeliveryNotificationOptions = mm.DeliveryNotificationOptions;
            Headers = SerializeableCollection.GetSerializeableCollection(mm.Headers);
            Priority = mm.Priority;
            ReplyTo = SerializeableMailAddress.GetSerializeableMailAddress(mm.ReplyToList.FirstOrDefault());
            Sender = SerializeableMailAddress.GetSerializeableMailAddress(mm.Sender);
            SubjectEncoding = mm.SubjectEncoding;

            foreach (AlternateView av in mm.AlternateViews)
                AlternateViews.Add(SerializeableAlternateView.GetSerializeableAlternateView(av));
        }
        public SerializeableMailMessage()
        { }

        ///
        /// Returns the MailMessge object from the serializeable object
        ///
        ///
        public MailMessage GetMailMessage()
        {
            MailMessage mm = new MailMessage();

            mm.IsBodyHtml = IsBodyHtml;
            mm.Body = Body;
            mm.Subject = Subject;
            if (From != null)
                mm.From = From.GetMailAddress();

            foreach (SerializeableMailAddress ma in To)
            {
                mm.To.Add(ma.GetMailAddress());
            }

            foreach (SerializeableMailAddress ma in CC)
            {
                mm.CC.Add(ma.GetMailAddress());
            }

            foreach (SerializeableMailAddress ma in Bcc)
            {
                mm.Bcc.Add(ma.GetMailAddress());
            }

            foreach (SerializeableAttachment att in Attachments)
            {
                mm.Attachments.Add(att.GetAttachment());
            }

            mm.BodyEncoding = BodyEncoding;

            mm.DeliveryNotificationOptions = DeliveryNotificationOptions;
            Headers.SetColletion(mm.Headers);
            mm.Priority = Priority;
            if (ReplyTo != null)
                mm.ReplyToList.Add(ReplyTo.GetMailAddress());

            if (Sender != null)
                mm.Sender = Sender.GetMailAddress();

            mm.SubjectEncoding = SubjectEncoding;

            foreach (SerializeableAlternateView av in AlternateViews)
                mm.AlternateViews.Add(av.GetAlternateView());

            return mm;
        }

    }
}
