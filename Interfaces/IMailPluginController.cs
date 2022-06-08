using Interfaces.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IMailPluginController
    {
        void ProcessMessageBeforeSend(MailMessage message);
        void Add(IPluginMail obj);
        IEnumerable<IPluginBase> GetPluginBase();
    }
}
