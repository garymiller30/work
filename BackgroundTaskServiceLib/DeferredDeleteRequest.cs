using System;

namespace BackgroundTaskServiceLib
{
    public sealed class DeferredDeleteRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Path { get; set; }
        public bool IsDirectory { get; set; }
        public DeferredDeleteMode Mode { get; set; } = DeferredDeleteMode.RecycleBin;
        public int Attempts { get; set; }
        public string LastError { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
