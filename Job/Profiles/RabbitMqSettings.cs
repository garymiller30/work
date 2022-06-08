using System;
using Interfaces;

namespace Job.Profiles
{
    [Serializable]
    //[XmlRoot("RabbitMqSettings")]
    public class RabbitMqSettings : IMQSettings
    {
        public bool UseRabbitMq { get; set; }
        public string RabbitServer { get; set; }
        public string RabbitUser { get; set; }
        public string RabbitPassword { get; set; }
        public string RabbitVirtualHost { get; set; }
    }
}
