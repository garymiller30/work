using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Plugins
{
    public interface IPluginMail : IPluginBase
    {
        IUserProfile UserProfile { get; set; }
        void ProcessMessageBeforeSend(MailMessage message);
    }
}
