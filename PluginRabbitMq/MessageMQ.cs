using Interfaces.MQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginRabbitMq
{
    [Serializable]
    public sealed class MessageMQ
    {
        public MessageEnum Code { get; set; }
        public string Id { get; set; }
        public string QueryId { get; set; }
    }
}
