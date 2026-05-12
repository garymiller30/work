using Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundTaskServiceLib
{
    public enum BackgroundTaskStatus
    {
        Waiting,
        Running,
        Finished,
        Canceled,
        Failed
    }

    public sealed class BackgroundTaskItem
    {
        private static readonly StringComparer FileComparer = StringComparer.InvariantCultureIgnoreCase;

        public string Name { get; set; }
        public CancellationToken CancelationToken { get; set; }
        public CancellationTokenSource CancelationTokenSource { get; set; }
        public Task ParentTask { get; set; }
        public event EventHandler<BackgroundTaskItem> Finished = delegate { };
        public event EventHandler<BackgroundTaskItem> Changed = delegate { };
        public Action BackgroundAction { get; set; }
        public List<string> Files { get; set; } = new List<string>();
        public BackgroundTaskStatus Status { get; private set; } = BackgroundTaskStatus.Waiting;
        public string Details { get; private set; } = string.Empty;

        public bool CanCancel
        {
            get { return CancelationTokenSource != null && !CancelationTokenSource.IsCancellationRequested && Status != BackgroundTaskStatus.Finished; }
        }

        public static List<string> NormalizeFiles(IEnumerable<string> files)
        {
            if (files == null)
            {
                return new List<string>();
            }

            return files
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .Distinct(FileComparer)
                .ToList();
        }

        public void Start()
        {
            try
            {
                SetStatus(BackgroundTaskStatus.Running, Details);
                BackgroundAction();
                if (CancelationToken.IsCancellationRequested)
                {
                    SetStatus(BackgroundTaskStatus.Canceled, "Скасовано");
                }
                else if (Status == BackgroundTaskStatus.Running || Status == BackgroundTaskStatus.Waiting)
                {
                    SetStatus(BackgroundTaskStatus.Finished, "Готово");
                }
            }
            catch (Exception e)
            {
                SetStatus(BackgroundTaskStatus.Failed, e.Message);
                Logger.Log.Error(null, "BackgroundTaskItem", $"[{e.Message}]");
            }
            finally
            {
                Finished(this, this);
            }
        }

        public void Cancel()
        {
            CancelationTokenSource?.Cancel();
            SetStatus(BackgroundTaskStatus.Canceled, "Скасування...");
        }

        public void SetStatus(BackgroundTaskStatus status, string details = null)
        {
            Status = status;
            if (details != null)
            {
                Details = details;
            }

            Changed(this, this);
        }
    }
}
