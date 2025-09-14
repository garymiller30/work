using System;
using System.Collections.Generic;
using Interfaces;
using Interfaces.Enums;
using MailNotifier;

namespace JobSpace.Profiles
{
    [Serializable]
   

    public sealed class MailSettings : IMailSettings
    {
        public string SettingsFolder { get; set; }
        public bool MailNotifyEnable { get; set; }
        public string MailNotifySoundFile { get; set; }
        public string MailFrom { get; set; }
        public string MailFromPassword { get; set; }
        public string MailImapHost { get; set; } = "imap.gmail.com";
        public int MailImapPort { get; set; } = 993;
        public string MailSmtpServer { get; set; } = "smtp.gmail.com";
        public int MailSmtpPort { get; set; } = 587;
        public List<string> MailTo { get; set; } = new List<string>();
        public bool MailAutoRelogon { get; set; }

        public MailConnectTypeEnum MailConnectType { get; set; } = MailConnectTypeEnum.SMTP;
        public string ClientSecretFile { get; set; }

        public bool Validate()
        {
            if (string.IsNullOrEmpty(MailFrom)) return false;
            if (string.IsNullOrEmpty(MailFromPassword)) return false;
            if (string.IsNullOrEmpty(MailImapHost)) return false;
            if (string.IsNullOrEmpty(MailSmtpServer)) return false;
            return true;
        }
    }
}
