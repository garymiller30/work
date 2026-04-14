namespace Interfaces
{
    public interface IJobSettings
    {
        bool StoreByYear { get; set; }
        string WorkPath { get; set; }
        string SignaFileShablon { get; set; }
        string SignaJobsPath { get; set; }
        bool UseJobFolder { get; set; }
        string SubFolderForSignaFile { get; set; }
    }
}
