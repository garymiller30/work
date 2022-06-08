namespace Job.UC
{
    public interface IWatcher
    {
        event System.IO.FileSystemEventHandler OnChanged;
        event System.IO.FileSystemEventHandler OnDeleted;
        event System.IO.FileSystemEventHandler OnCreated;
        event System.IO.RenamedEventHandler OnRenamed;
        event System.IO.ErrorEventHandler OnError;

        void SetWatchFolder(string folder);
        void Stop();
    }
}
