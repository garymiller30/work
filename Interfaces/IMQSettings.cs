namespace Interfaces
{
    public interface IMQSettings
    {
        bool UseRabbitMq { get; set; }
        string RabbitServer { get; set; }
        string RabbitUser { get; set; }
        string RabbitPassword { get; set; }
        string RabbitVirtualHost { get; set; }
    }
}