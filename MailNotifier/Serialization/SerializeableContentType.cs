using System;
using System.Net.Mime;

namespace MailNotifier.Serialization
{
    [Serializable]
    internal class SerializeableContentType
    {
        String Boundary;
        String CharSet;
        String MediaType;
        String Name;
        SerializeableCollection Parameters;

        internal static SerializeableContentType GetSerializeableContentType(System.Net.Mime.ContentType ct)
        {
            if (ct == null)
                return null;

            SerializeableContentType sct = new SerializeableContentType();

            sct.Boundary = ct.Boundary;
            sct.CharSet = ct.CharSet;
            sct.MediaType = ct.MediaType;
            sct.Name = ct.Name;
            sct.Parameters = SerializeableCollection.GetSerializeableCollection(ct.Parameters);

            return sct;
        }

        internal ContentType GetContentType()
        {

            ContentType sct = new ContentType();

            sct.Boundary = Boundary;
            sct.CharSet = CharSet;
            sct.MediaType = MediaType;
            sct.Name = Name;

            Parameters.SetColletion(sct.Parameters);

            return sct;
        }
    }
}
