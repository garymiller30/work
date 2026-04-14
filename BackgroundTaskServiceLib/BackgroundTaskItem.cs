using Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundTaskServiceLib
{
    public sealed class BackgroundTaskItem
    {
        private static readonly StringComparer FileComparer = StringComparer.InvariantCultureIgnoreCase;

        public string Name { get; set; }
        public CancellationToken CancelationToken { get; set; }
        public Task ParentTask { get; set; }
        public event EventHandler<BackgroundTaskItem> Finished = delegate { };
        public Action BackgroundAction { get; set; }
        public List<string> Files { get; set; } = new List<string>();

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
                BackgroundAction();
            }
            catch (Exception e)
            {
                Logger.Log.Error(null, "BackgroundTaskItem", $"[{e.Message}]");
            }
            finally
            {
                Finished(this, this);
            }
        }
    }
}
