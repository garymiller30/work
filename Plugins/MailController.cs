using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using Interfaces.Plugins;

namespace Plugins
{
    public class MailController : IMailPluginController
    {
        readonly List<IPluginMail> _plugins = new List<IPluginMail>();

        public void ProcessMessageBeforeSend(MailMessage message)
        {
            _plugins.ForEach(x=>x.ProcessMessageBeforeSend(message));
        }

        public void Add(IPluginMail obj)
        {
            _plugins.Add(obj);
        }

        public IEnumerable<IPluginBase> GetPluginBase()
        {
            return _plugins;
        }
    }
}
