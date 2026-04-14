using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using Interfaces.Plugins;

namespace Plugins
{
    public sealed class MailController : IMailPluginController
    {
        readonly List<IPluginMail> _plugins = new List<IPluginMail>();

        public bool HasPlugins => _plugins.Count > 0;

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

        public string UploadFiles(string[] files)
        {
            foreach (var plugin in _plugins)
            {
                var res = plugin.UploadFiles(files);

                if (!string.IsNullOrEmpty(res))
                {
                    return res;
                }
            }
            return string.Empty;
        }
    }
}
