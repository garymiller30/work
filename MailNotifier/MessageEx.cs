using System.IO;
using System.Net.Mail;
using System.Text.Json;
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
            var serializeMessage = new SerializeableMailMessage(message);
            var options = new JsonSerializerOptions
            {
                WriteIndented = false,
                IncludeFields = true,
                PropertyNameCaseInsensitive = true
            };
            var json = JsonSerializer.Serialize(serializeMessage, options);
            File.WriteAllText(fileName, json);
        }

        public static MailMessage Load(string fileName)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                IncludeFields = true
            };
            var json = File.ReadAllText(fileName);
            var v = JsonSerializer.Deserialize<SerializeableMailMessage>(json, options);
            return v?.GetMailMessage();
        }

    }
}
