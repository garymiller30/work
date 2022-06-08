using System;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;

namespace MailNotifier.Serialization
{
    [Serializable]
    internal class SerializeableLinkedResource
    {
        String ContentId;
        Uri ContentLink;
        Stream ContentStream;
        SerializeableContentType ContentType;
        TransferEncoding TransferEncoding;

        internal static SerializeableLinkedResource GetSerializeableLinkedResource(LinkedResource lr)
        {
            if (lr == null)
                return null;

            SerializeableLinkedResource slr = new SerializeableLinkedResource();
            slr.ContentId = lr.ContentId;
            slr.ContentLink = lr.ContentLink;

            if (lr.ContentStream != null)
            {
                byte[] bytes = new byte[lr.ContentStream.Length];
                lr.ContentStream.Read(bytes, 0, bytes.Length);
                slr.ContentStream = new MemoryStream(bytes);
            }

            slr.ContentType = SerializeableContentType.GetSerializeableContentType(lr.ContentType);
            slr.TransferEncoding = lr.TransferEncoding;

            return slr;

        }

        internal LinkedResource GetLinkedResource()
        {
            LinkedResource slr = new LinkedResource(ContentStream);
            slr.ContentId = ContentId;
            slr.ContentLink = ContentLink;

            slr.ContentType = ContentType.GetContentType();
            slr.TransferEncoding = TransferEncoding;

            return slr;
        }
    }
}
