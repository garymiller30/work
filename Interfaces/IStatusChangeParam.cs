namespace Interfaces
{
    public interface IStatusChangeParam
    {
        bool Enable { get; set; } 
        int StatusCode { get; set; }
        string ProgramPath { get;set; }
        string CommandLineParams { get; set; } 
    }
}
