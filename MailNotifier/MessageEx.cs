using System.IO;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters.Binary;
using MailNotifier.Serialization;
using S22.Imap;

namespace MailNotifier
{
    public sealed class MessageEx
    {
        public uint Id { get; }
        public string Date
        {
            get
            {
                var d = _message?.Date();
                return d?.ToString("yyyy.MM.dd");
            }
        }

        public string Time
        {
            get
            {
                var d = _message?.Date();
                return d?.ToString("HH:mm:ss");

            }
        }

        
        public string From => _message.From?.Address;
        public string Subject => _message.Subject;

        public MailMessage OriginalMessage => _message;

        private readonly MailMessage _message;




        public MessageEx(uint id, MailMessage message)
        {
            Id = id;
            _message = message;
            
        }

        public static MessageEx ToMessageEx(uint id, MailMessage message)
        {


            return new MessageEx(id, message);
        }

        public static void Save(MailMessage message, string fileName)
        {

            var serializeMessage = new Serialization.SerializeableMailMessage(message);

            using (var stream = File.Create(fileName))
            {
                var formater = new BinaryFormatter();
                formater.Serialize(stream,serializeMessage);
                stream.Close();
            }

            


            //Assembly assembly = typeof(SmtpClient).Assembly;
            //Type mailWriterType = assembly.GetType("System.Net.Mail.MailWriter");

            //using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
            //{
            //    // Get reflection info for MailWriter contructor
            //    ConstructorInfo mailWriterContructor = mailWriterType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic,null,new Type[] { typeof(Stream) },null);

            //    // Construct MailWriter object with our FileStream
            //    object mailWriter = mailWriterContructor.Invoke(new object[] { fileStream });

            //    // Get reflection info for Send() method on MailMessage
            //    MethodInfo sendMethod = typeof(MailMessage).GetMethod("Send",BindingFlags.Instance | BindingFlags.NonPublic);

            //    // Call method passing in MailWriter
            //    sendMethod.Invoke(message,BindingFlags.Instance | BindingFlags.NonPublic,null,new object[] { mailWriter, true },null);

            //    // Finally get reflection info for Close() method on our MailWriter
            //    MethodInfo closeMethod = mailWriter.GetType().GetMethod("Close",BindingFlags.Instance | BindingFlags.NonPublic);

            //    // Call close method
            //    closeMethod.Invoke(mailWriter,BindingFlags.Instance | BindingFlags.NonPublic,null,new object[] { },null);
            //}

        }

        public static MailMessage Load(string fileName)
        {
            var stream = File.OpenRead( fileName );
            var formatter = new BinaryFormatter();
            var v = (SerializeableMailMessage) formatter.Deserialize( stream );
            stream.Close();
            return v.GetMailMessage();
        }

    }
}
