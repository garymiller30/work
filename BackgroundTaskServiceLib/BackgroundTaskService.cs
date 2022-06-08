using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace BackgroundTaskServiceLib
{
    public static class BackgroundTaskService
    {
        static object _lock = new object();
        const int MaxThreads = 10;
        const int DefaultInterval = 2000;
        private static int countThreads;
        private static readonly System.Timers.Timer timer = new System.Timers.Timer(DefaultInterval);
        private static readonly ConcurrentQueue<BackgroundTaskItem> queueTaskItems = new ConcurrentQueue<BackgroundTaskItem>();
        private static readonly List<BackgroundTaskItem> processTask = new List<BackgroundTaskItem>();

        public static event EventHandler<BackgroundTaskItem> OnAdd = delegate { };
        public static event EventHandler<BackgroundTaskItem> OnFinish = delegate { };
        static BackgroundTaskService()
        {
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Start();
        }


        public static ICollection GetAll()
        {
            var list = new List<BackgroundTaskItem>();
            list.AddRange(queueTaskItems.ToList());
            list.AddRange(processTask);
            return list;

        }

        public static BackgroundTaskItem CreateTask(string name, Action action)
        {
            return new BackgroundTaskItem()
            {
                Name = name,
                BackgroundAction = action,
            };
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!queueTaskItems.IsEmpty)
            {
                while (countThreads < MaxThreads)
                {
                    var res = queueTaskItems.TryDequeue(out BackgroundTaskItem taskItem);
                    if (res)
                    {
                        Interlocked.Increment(ref countThreads);
                        taskItem.Finished += OnFinished;

                        CancellationTokenSource tokenSource = new CancellationTokenSource();
                        CancellationToken token = tokenSource.Token;

                        var task = new Task(() => taskItem.Start(), token);
                        taskItem.ParentTask = task;
                        taskItem.CancelationToken = token;
                        lock (_lock)
                        {
                            processTask.Add(taskItem);
                        }

                        task.ConfigureAwait(true);
                        Application.UseWaitCursor = true;

                        task.Start();
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private static void OnFinished(object sender, BackgroundTaskItem e)
        {
            Interlocked.Decrement(ref countThreads);
            lock (_lock)
            {
                processTask.Remove(e);
                if (processTask.Count == 0) Application.UseWaitCursor = false;
            }
            OnFinish(null, e);
        }

        public static void AddTask(BackgroundTaskItem item)
        {
            queueTaskItems.Enqueue(item);
            OnAdd(null, item);
        }
    }
}

