using Interfaces.MQ;

namespace PluginRabbitMq
{

    public sealed class MessageMQ
    {
        public MessageMQ()
        {
            
        }

        public MessageEnum Code { get; set; }
        public string Id { get; set; }
        public string QueryId { get; set; }
    }
}
