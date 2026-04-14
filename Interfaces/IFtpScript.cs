namespace Interfaces
{
    public interface IFtpScript
    {
        bool Enable { get; set; }
        string Name { get; set; }
        string ScriptPath { get; set; }
        string Parameters { get; set; }
    }
}
