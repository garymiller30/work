using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin_RabbitMQ
{
    public sealed class PluginRabbitMQSettings
    {
        public string Server { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }

        public bool isValid()
        {
            return Server != null && User != null && Password != null && VirtualHost != null;
        }

    }
}
