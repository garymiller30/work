// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com 


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FtpClient
{
    /// <summary>
    /// закачка на фтп
    /// </summary>
    public class Uploader
    {
        static readonly List<Uploader> _uploadTasks = new List<Uploader>();
        private static bool _isUploading;
        private static readonly object Locker = new object();


        private readonly List<UploadFileParam> _files = new List<UploadFileParam>();
        readonly Client _client;

        private long _totalLenght;

       // private readonly long _totalTransfered;


        public long TotaLenght => _totalLenght;

        public event EventHandler<long> OnStartUpload = delegate { };
        public event EventHandler<string> OnStartUploadFile = delegate { };
        public event EventHandler<long> OnProcessUploading = delegate { };
        public event EventHandler OnFinishUpload = delegate { };

        public object Tag;

        public Uploader()
        {
            _client = new Client();
        }
        public static Uploader AddUploader()
        {
            var uploader = new Uploader();

            lock (Locker)
            {
                _uploadTasks.Add(uploader);
            }
            
            return uploader;
        }

        public static void StartUpload()
        {
            if (!_isUploading && _uploadTasks.Any())
            {
                Task.Run(()=>_uploadTasks[0].Upload());
            }
        }

        public void AddFile(string uploadFile, string targetFtpFile, string server, bool activeMode, int codePage, string user, string pass)
        {
            _files.Add(new UploadFileParam
            {
                UploadFile = uploadFile,
                Password = pass,
                User = user,
                Server = server,
                TargetFtpFile = targetFtpFile,
                ActiveMode = activeMode,
                CodePage = codePage
            });
        }

        private void Upload()
        {

            lock (Locker)
            {
                _isUploading = true;
            }
            // получить размер файлов
            _totalLenght = _files.Sum(x => GetFileLenght(x.UploadFile));

            OnStartUpload(this, _totalLenght); // вызвать событие о начале загрузки

            foreach (var file in _files)
            {
                OnStartUploadFile(this, Path.GetFileName(file.UploadFile));
                _uploadParam(file);
            }

            _files.Clear();
            OnFinishUpload(this, null);

            lock (Locker)
            {
                _uploadTasks.Remove(this); // удалить себя из списка задач

            }

            if (_uploadTasks.Any())
            {
                _uploadTasks[0].Upload();
            }
            else
            {
                lock (Locker)
                {
                    _isUploading = false;
                }
            }
        }

        private long GetFileLenght(string uploadFile)
        {
            return new FileInfo(uploadFile).Length;
        }

        private void _uploadParam(UploadFileParam file)
        {
            
            _client.CreateConnection(file.Server, file.User, file.Password, string.Empty, file.ActiveMode, file.CodePage);
            _client.Upload(file);

        }

    }
}
