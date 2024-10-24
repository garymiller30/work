﻿using System;
using Interfaces;

namespace JobSpace.Profiles
{
    [Serializable]
    //[XmlRoot("RabbitMqSettings")]
    public sealed class RabbitMqSettings : IMQSettings
    {
        public bool UseRabbitMq { get; set; }
        public string RabbitServer { get; set; }
        public string RabbitUser { get; set; }
        public string RabbitPassword { get; set; }
        public string RabbitVirtualHost { get; set; }
    }
}
