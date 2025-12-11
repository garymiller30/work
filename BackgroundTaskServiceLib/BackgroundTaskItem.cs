using Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundTaskServiceLib
{
    public sealed class BackgroundTaskItem
    {
        public string Name { get; set; }
        public CancellationToken CancelationToken { get; set; }
        public Task ParentTask { get; set; }
        public event EventHandler<BackgroundTaskItem> Finished = delegate { };
        public Action BackgroundAction { get; set; }

        public void Start()
        {
            try
            {
                BackgroundAction();
            }
            catch (Exception e)
            {
                Logger.Log.Error(null, "BackgroundTaskItem", $"[{e.Message}]"); ;
            }
            finally
            {
                Finished(this, this);
            }
        }
    }
}
