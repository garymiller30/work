namespace Interfaces
{
    public interface IBaseSettings
    {
        string MongoDbBaseName { get; set; }
        string MongoDbServer { get; set; }
        int BaseTimeOut { get; set; }
    }
}
