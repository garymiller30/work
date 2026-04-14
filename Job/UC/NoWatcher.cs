using System.IO;

namespace JobSpace.UC
{
    public sealed class NoWatcher : IWatcher
    {
#pragma warning disable CS0067 // The event 'NoWatcher.OnChanged' is never used
        public event FileSystemEventHandler OnChanged;
#pragma warning restore CS0067 // The event 'NoWatcher.OnChanged' is never used
#pragma warning disable CS0067 // The event 'NoWatcher.OnDeleted' is never used
        public event FileSystemEventHandler OnDeleted;
#pragma warning restore CS0067 // The event 'NoWatcher.OnDeleted' is never used
#pragma warning disable CS0067 // The event 'NoWatcher.OnCreated' is never used
        public event FileSystemEventHandler OnCreated;
#pragma warning restore CS0067 // The event 'NoWatcher.OnCreated' is never used
#pragma warning disable CS0067 // The event 'NoWatcher.OnRenamed' is never used
        public event RenamedEventHandler OnRenamed;
#pragma warning restore CS0067 // The event 'NoWatcher.OnRenamed' is never used
#pragma warning disable CS0067 // The event 'NoWatcher.OnError' is never used
        public event ErrorEventHandler OnError;
#pragma warning restore CS0067 // The event 'NoWatcher.OnError' is never used

        public void SetWatchFolder(string folder)
        {

        }

        public void Stop()
        {

        }
    }
}
