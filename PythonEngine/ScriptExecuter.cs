using Interfaces;
using Interfaces.Script;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace PythonEngine
{
    public static class ScriptExecuter
    {
        
        private static int _countThreads;
        private const int DefaultInterval = 2000;
        private static readonly ConcurrentQueue<ScriptExecuterParameters> ExecuteQueue = new ConcurrentQueue<ScriptExecuterParameters>();
        private static readonly System.Timers.Timer DownloadTimer = new System.Timers.Timer(DefaultInterval);

        static ScriptExecuter()
        {
            DownloadTimer.Elapsed += DownloadTimerOnElapsed;
            DownloadTimer.AutoReset = true;
            DownloadTimer.Start();
        }

        private static void DownloadTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            if (!ExecuteQueue.IsEmpty)
            {
                if (_countThreads == 0)
                {
                    var res = ExecuteQueue.TryDequeue(out ScriptExecuterParameters downloader);
                    if (res)
                    { 
                        Interlocked.Increment(ref _countThreads);

                        var bw = new BackgroundWorker();
                        bw.DoWork += new DoWorkEventHandler(delegate (object s, DoWorkEventArgs ev)
                        {
                            downloader.ScriptController.RunScript(downloader.Parameters);
                         });

                        bw.RunWorkerCompleted+= new RunWorkerCompletedEventHandler(delegate(object se,RunWorkerCompletedEventArgs arg){ 
                             Interlocked.Decrement(ref _countThreads);
                            });
                        
                        bw.RunWorkerAsync();
                    }
                }
            }
        }

        public static void AddToQuery(ScriptExecuterParameters executerParameters)
        {
            ExecuteQueue.Enqueue(executerParameters);
        }

        public static void AddToQuery(IScriptController scriptController, IScriptRunParameters parameters)
        {
            AddToQuery(new ScriptExecuterParameters()
            {
                ScriptController = scriptController,
                Parameters = parameters,
            });
        }

    }
}
