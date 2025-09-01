using Interfaces;
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

        public string GetHeader(IJob job)
        {
            if (job == null)
                return Header;
            return Header.Replace("$OrderNumber", job.Number);
                   
        }
    }
}
