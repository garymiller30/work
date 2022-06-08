using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Interfaces
{
    public interface IMail
    {
        IMailSettings Settings { get; set; }

        void StopWatching();
        List<ToolStripItem> GetMenu(EventHandler ttmClick);
        void ShowSendMailDialog(string mailTo, string header, string body);
        void SendFile(string to, string attachmentPath);
        void SetCurJob(IJob job);
        void SetAttachmentsList(IEnumerable<string> attach);
        void ShowSendMailDialog();
        void SendToMany(string to, string tema, string body, string[] attachFiles);
    }
}
