using System;
using System.IO;
using System.Net.Mail;
using System.Text;

namespace MailNotifier.Serialization
{
    [Serializable]
    internal class SerializeableAttachment
    {
        String ContentId;
        SerializeableContentDisposition ContentDisposition;
        SerializeableContentType ContentType;
        Stream ContentStream;
        System.Net.Mime.TransferEncoding TransferEncoding;
        String Name;
        Encoding NameEncoding;

        internal static SerializeableAttachment GetSerializeableAttachment(Attachment att)
        {
            if (att == null)
                return null;

            SerializeableAttachment saa = new SerializeableAttachment();
            saa.ContentId = att.ContentId;
            saa.ContentDisposition = SerializeableContentDisposition.GetSerializeableContentDisposition(att.ContentDisposition);

            if (att.ContentStream != null)
            {
                byte[] bytes = new byte[att.ContentStream.Length];
                att.ContentStream.Read(bytes, 0, bytes.Length);

                saa.ContentStream = new MemoryStream(bytes);
            }

            saa.ContentType = SerializeableContentType.GetSerializeableContentType(att.ContentType);
            saa.Name = att.Name;
            saa.TransferEncoding = att.TransferEncoding;
            saa.NameEncoding = att.NameEncoding;
            return saa;
        }

        internal Attachment GetAttachment()
        {
            Attachment saa = new Attachment(ContentStream, Name);
            saa.ContentId = ContentId;
            this.ContentDisposition.SetContentDisposition(saa.ContentDisposition);

            saa.ContentType = ContentType.GetContentType();
            saa.Name = Name;
            saa.TransferEncoding = TransferEncoding;
            saa.NameEncoding = NameEncoding;
            return saa;
        }
    }
}
