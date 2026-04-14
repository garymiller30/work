// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com 

using Interfaces.Ftp;
using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace FtpClient
{
    public static class DownloadService
    {
        private const int MaxThread = 2;
        private static int _countThreads;
        private const int DefaultInterval = 2000;
        private static readonly ConcurrentQueue<Downloader> DownloadersQueue = new ConcurrentQueue<Downloader>();
        private static readonly Timer DownloadTimer = new Timer(DefaultInterval);


        static DownloadService()
        {
            DownloadTimer.Elapsed += DownloadTimerOnElapsed;
            DownloadTimer.AutoReset = true;
            DownloadTimer.Start();

        }


        
        private static void DownloadTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            if (!DownloadersQueue.IsEmpty)
            {
                while (_countThreads < MaxThread)
                {
                    var res = DownloadersQueue.TryDequeue(out Downloader downloader);
                    if (res)
                    {
                        Interlocked.Increment(ref _countThreads);
                        downloader.FinishDownload+= DownloaderOnFinishDownload;
                       Task.Run(()=>downloader.Download());
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private static void DownloaderOnFinishDownload(object sender, IDownloadTicket e)
        {
            Interlocked.Decrement(ref _countThreads);
        }

        public static void AddToQuery(Downloader downloader)
        {
            DownloadersQueue.Enqueue(downloader);
        }
    }
}
