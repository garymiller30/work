using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace BackgroundTaskServiceLib
{
    public static class BackgroundTaskService
    {
        private static readonly object _lock = new object();
        private static readonly StringComparer FileComparer = StringComparer.InvariantCultureIgnoreCase;
        private const int DefaultInterval = 2000;
        private static int countThreads;
        private static readonly System.Timers.Timer timer = new System.Timers.Timer(DefaultInterval);
        private static readonly List<BackgroundTaskItem> queueTaskItems = new List<BackgroundTaskItem>();
        private static readonly List<BackgroundTaskItem> processTask = new List<BackgroundTaskItem>();

        public static event EventHandler<BackgroundTaskItem> OnAdd = delegate { };
        public static event EventHandler<BackgroundTaskItem> OnFinish = delegate { };
        public static event EventHandler OnAllFinish = delegate { };

        static BackgroundTaskService()
        {
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Start();
        }


        public static ICollection GetAll()
        {
            var list = new List<BackgroundTaskItem>();

            lock (_lock)
            {
                list.AddRange(queueTaskItems);
                list.AddRange(processTask);
            }

            return list;
        }

        public static BackgroundTaskItem CreateTask(string name, Action action, IEnumerable<string> files = null)
        {
            return new BackgroundTaskItem()
            {
                Name = name,
                BackgroundAction = action,
                Files = BackgroundTaskItem.NormalizeFiles(files),
            };
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            TryStartTasks();
        }

        private static void TryStartTasks()
        {
            List<BackgroundTaskItem> tasksToStart;

            lock (_lock)
            {
                if (queueTaskItems.Count == 0)
                {
                    return;
                }

                var runningFiles = new HashSet<string>(
                    processTask.SelectMany(x => x.Files ?? Enumerable.Empty<string>()),
                    FileComparer);

                var previousTaskFiles = new HashSet<string>(FileComparer);
                var remainingTasks = new List<BackgroundTaskItem>(queueTaskItems.Count);
                tasksToStart = new List<BackgroundTaskItem>();

                foreach (var taskItem in queueTaskItems)
                {
                    var taskFiles = taskItem.Files ?? new List<string>();
                    var hasConflict = taskFiles.Any(file => runningFiles.Contains(file) || previousTaskFiles.Contains(file));

                    if (hasConflict)
                    {
                        remainingTasks.Add(taskItem);
                    }
                    else
                    {
                        tasksToStart.Add(taskItem);
                        processTask.Add(taskItem);
                    }

                    foreach (var file in taskFiles)
                    {
                        previousTaskFiles.Add(file);
                    }
                }

                queueTaskItems.Clear();
                queueTaskItems.AddRange(remainingTasks);

                if (tasksToStart.Count > 0)
                {
                    Application.UseWaitCursor = true;
                }
            }

            foreach (var taskItem in tasksToStart)
            {
                StartTask(taskItem);
            }
        }

        private static void StartTask(BackgroundTaskItem taskItem)
        {
            Interlocked.Increment(ref countThreads);
            taskItem.Finished += OnFinished;

            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            var task = new Task(() => taskItem.Start(), token);

            taskItem.ParentTask = task;
            taskItem.CancelationToken = token;

            task.ConfigureAwait(true);
            task.Start();
        }

        private static void OnFinished(object sender, BackgroundTaskItem e)
        {
            Interlocked.Decrement(ref countThreads);

            var hasTasks = false;
            lock (_lock)
            {
                processTask.Remove(e);
                hasTasks = queueTaskItems.Count > 0 || processTask.Count > 0;

                if (!hasTasks)
                {
                    Application.UseWaitCursor = false;
                }
            }

            OnFinish(null, e);

            if (hasTasks)
            {
                TryStartTasks();
            }
            else
            {
                OnAllFinish(null, null);
            }
        }

        public static void AddTask(BackgroundTaskItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            item.Files = BackgroundTaskItem.NormalizeFiles(item.Files);

            lock (_lock)
            {
                queueTaskItems.Add(item);
                Application.UseWaitCursor = true;
            }

            OnAdd(null, item);
            TryStartTasks();
        }
    }
}

