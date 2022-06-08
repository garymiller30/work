using System;

namespace MailNotifier.Shablons
{
    [Serializable]
    public class MailShablon
    {
        public string ShablonName { get; set; }
        public string SendTo { get; set; }
        public string Header { get; set; }
        public string Message { get; set; }
    }
}
