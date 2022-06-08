namespace Interfaces
{
    public interface IBaseSettings
    {
        string MongoDbBaseName { get; set; }
        string MongoDbServer { get; set; }
        string MongoDbUser { get; set; }
        string MongoDbPassword { get; set; }
        int MongoDbPort { get; set; }
        int BaseTimeOut { get; set; }
    }
}
