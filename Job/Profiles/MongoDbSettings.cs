using System;
using Interfaces;

namespace Job.Profiles
{
    [Serializable]
    //[XmlRoot("MongoDbSettings")]
    public sealed class MongoDbSettings : IBaseSettings
    {
        public string MongoDbBaseName { get; set; }
        public string MongoDbServer { get; set; }
        [Obsolete]
        public string MongoDbUser { get; set; }
        [Obsolete]
        public string MongoDbPassword { get; set; }
        [Obsolete]
        public int MongoDbPort { get; set; }
        public int BaseTimeOut { get; set; } = 3;
    }
}
