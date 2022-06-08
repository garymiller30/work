using System.IO;

namespace Job.UC
{
    public class NoWatcher : IWatcher
    {
        public event FileSystemEventHandler OnChanged;
        public event FileSystemEventHandler OnDeleted;
        public event FileSystemEventHandler OnCreated;
        public event RenamedEventHandler OnRenamed;
        public event ErrorEventHandler OnError;

        public void SetWatchFolder(string folder)
        {

        }

        public void Stop()
        {

        }
    }
}
