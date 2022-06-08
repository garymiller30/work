using System.Collections.Generic;

namespace Interfaces
{
    
    public interface IMailSettings
    {
        string SettingsFolder { get; set; }

        bool MailNotifyEnable { get; set; }
        string MailNotifySoundFile { get; set; }
        string MailFrom { get; set; }
        string MailFromPassword { get; set; }
        string MailImapHost { get; set; }
        int MailImapPort { get; set; }
        string MailSmtpServer { get; set; }
        int MailSmtpPort { get; set; }
        List<string> MailTo { get; set; } 
        bool MailAutoRelogon { get; set; }

        bool Validate();
    }
}