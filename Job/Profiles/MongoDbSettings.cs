using System;
using Interfaces;

namespace Job.Profiles
{
    [Serializable]
    //[XmlRoot("MongoDbSettings")]
    public class MongoDbSettings : IBaseSettings
    {
        public string MongoDbBaseName { get; set; }
        public string MongoDbServer { get; set; }
        public string MongoDbUser { get; set; }
        public string MongoDbPassword { get; set; }
        public int MongoDbPort { get; set; }
        public int BaseTimeOut { get; set; } = 3;
    }
}
