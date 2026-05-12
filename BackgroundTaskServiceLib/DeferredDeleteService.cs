using Logger;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;

namespace BackgroundTaskServiceLib
{
    public static class DeferredDeleteService
    {
        private static readonly object SyncRoot = new object();
        private static readonly StringComparer PathComparer = StringComparer.InvariantCultureIgnoreCase;
        private static readonly List<DeferredDeleteRequest> PendingRequests = new List<DeferredDeleteRequest>();
        private static readonly HashSet<string> EnqueuedPaths = new HashSet<string>(PathComparer);
        private static readonly TimeSpan[] RetryDelays =
        {
            TimeSpan.FromSeconds(5),
            TimeSpan.FromSeconds(15),
            TimeSpan.FromSeconds(30),
            TimeSpan.FromMinutes(1),
            TimeSpan.FromMinutes(5)
        };

        private static bool _initialized;

        public static void Initialize()
        {
            lock (SyncRoot)
            {
                if (_initialized)
                {
                    return;
                }

                _initialized = true;
                foreach (var request in LoadPendingRequests())
                {
                    StartRequest(request, false);
                }
            }
        }

        public static BackgroundTaskItem Enqueue(string path)
        {
            return Enqueue(path, DeferredDeleteMode.RecycleBin);
        }

        public static BackgroundTaskItem EnqueuePermanent(string path)
        {
            return Enqueue(path, DeferredDeleteMode.Permanent);
        }

        public static BackgroundTaskItem Enqueue(string path, DeferredDeleteMode mode)
        {
            Initialize();

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Path is empty", nameof(path));
            }

            var fullPath = System.IO.Path.GetFullPath(path);
            var request = new DeferredDeleteRequest
            {
                Path = fullPath,
                IsDirectory = Directory.Exists(fullPath),
                Mode = mode
            };

            lock (SyncRoot)
            {
                if (EnqueuedPaths.Contains(fullPath))
                {
                    return null;
                }

                return StartRequest(request, true);
            }
        }

        private static BackgroundTaskItem StartRequest(DeferredDeleteRequest request, bool save)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Path) || EnqueuedPaths.Contains(request.Path))
            {
                return null;
            }

            PendingRequests.Add(request);
            EnqueuedPaths.Add(request.Path);
            if (save)
            {
                SavePendingRequests();
            }

            BackgroundTaskItem task = null;
            task = BackgroundTaskService.CreateTask(
                "delete " + System.IO.Path.GetFileName(request.Path),
                () => ProcessDeleteRequest(request, task),
                new[] { request.Path });
            task.SetStatus(BackgroundTaskStatus.Waiting, request.Path);
            BackgroundTaskService.AddTask(task);
            return task;
        }

        private static void ProcessDeleteRequest(DeferredDeleteRequest request, BackgroundTaskItem task)
        {
            while (!task.CancelationToken.IsCancellationRequested)
            {
                if (!File.Exists(request.Path) && !Directory.Exists(request.Path))
                {
                    RemovePendingRequest(request);
                    task.SetStatus(BackgroundTaskStatus.Finished, "Файл вже відсутній");
                    return;
                }

                try
                {
                    request.Attempts++;
                    task.SetStatus(BackgroundTaskStatus.Running, $"Спроба {request.Attempts}: {request.Path}");
                    SavePendingRequests();

                    DeleteNow(request);

                    RemovePendingRequest(request);
                    task.SetStatus(BackgroundTaskStatus.Finished, "Видалено");
                    return;
                }
                catch (IOException e)
                {
                    WaitBeforeRetry(request, task, e);
                }
                catch (UnauthorizedAccessException e)
                {
                    WaitBeforeRetry(request, task, e);
                }
                catch (Exception e)
                {
                    request.LastError = e.Message;
                    SavePendingRequests();
                    task.SetStatus(BackgroundTaskStatus.Failed, e.Message);
                    Log.Error(null, "DeferredDeleteService", $"Cannot delete {request.Path}: {e}");
                    return;
                }
            }

            RemovePendingRequest(request);
            task.SetStatus(BackgroundTaskStatus.Canceled, "Скасовано користувачем");
        }

        private static void DeleteNow(DeferredDeleteRequest request)
        {
            if (request.Mode == DeferredDeleteMode.RecycleBin)
            {
                if (Directory.Exists(request.Path))
                {
                    FileSystem.DeleteDirectory(request.Path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                }
                else
                {
                    FileSystem.DeleteFile(request.Path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                }

                return;
            }

            if (Directory.Exists(request.Path))
            {
                Directory.Delete(request.Path, true);
            }
            else
            {
                File.Delete(request.Path);
            }
        }

        private static void WaitBeforeRetry(DeferredDeleteRequest request, BackgroundTaskItem task, Exception exception)
        {
            request.LastError = exception.Message;
            SavePendingRequests();

            var delay = GetRetryDelay(request.Attempts);
            task.SetStatus(
                BackgroundTaskStatus.Running,
                $"Зайнято. Повтор через {FormatDelay(delay)}. {exception.Message}");

            if (task.CancelationToken.WaitHandle.WaitOne(delay))
            {
                task.SetStatus(BackgroundTaskStatus.Canceled, "Скасування...");
            }
        }

        private static TimeSpan GetRetryDelay(int attempts)
        {
            var index = Math.Max(0, Math.Min(attempts - 1, RetryDelays.Length - 1));
            return RetryDelays[index];
        }

        private static string FormatDelay(TimeSpan delay)
        {
            return delay.TotalMinutes >= 1
                ? $"{(int)delay.TotalMinutes} хв"
                : $"{(int)delay.TotalSeconds} с";
        }

        private static void RemovePendingRequest(DeferredDeleteRequest request)
        {
            lock (SyncRoot)
            {
                PendingRequests.RemoveAll(x => x.Id == request.Id);
                EnqueuedPaths.Remove(request.Path);
                SavePendingRequests();
            }
        }

        private static List<DeferredDeleteRequest> LoadPendingRequests()
        {
            try
            {
                var path = GetStorePath();
                if (!File.Exists(path))
                {
                    return new List<DeferredDeleteRequest>();
                }

                var json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<List<DeferredDeleteRequest>>(json) ?? new List<DeferredDeleteRequest>();
            }
            catch (Exception e)
            {
                Log.Error(null, "DeferredDeleteService", $"Cannot load pending delete queue: {e.Message}");
                return new List<DeferredDeleteRequest>();
            }
        }

        private static void SavePendingRequests()
        {
            try
            {
                var path = GetStorePath();
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
                List<DeferredDeleteRequest> snapshot;
                lock (SyncRoot)
                {
                    snapshot = PendingRequests.ToList();
                }

                var json = JsonSerializer.Serialize(snapshot, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(path, json);
            }
            catch (Exception e)
            {
                Log.Error(null, "DeferredDeleteService", $"Cannot save pending delete queue: {e.Message}");
            }
        }

        private static string GetStorePath()
        {
            var root = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "ActiveWorks");
            return System.IO.Path.Combine(root, "deferred-delete-queue.json");
        }
    }
}
