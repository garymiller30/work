// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com 

using FluentFTP;
using FluentFTP.Helpers;
using Interfaces;
using Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace FtpClient
{
    public class Client
    {
        private Exception _exception;

        private FluentFTP.FtpClient _ftpClient;

        public event EventHandler<double> OnDownloadingProcess = delegate { };

        public event EventHandler<double> OnUploadingProgress = delegate { };

        private Encoding _encoding;

        private string _rootDirectory;


        public string CurrentDirectory { get; private set; }


        public Client()
        {

        }

        ~Client()
        {
            _ftpClient?.Dispose();
        }
        public void CreateConnection(string fileServer, string user, string password, string rootDirectory,
            bool activeMode, int codePage)
        {
            // закрити з'єднання, якщо відкрито
            if (_ftpClient != null && _ftpClient.IsConnected) _ftpClient.Disconnect();
            _rootDirectory = rootDirectory;
            CurrentDirectory = rootDirectory;

            _encoding = Encoding.GetEncoding(codePage);

            _ftpClient = new FluentFTP.FtpClient
            {
                Host = fileServer,
                Credentials = new NetworkCredential(user, password),
                Encoding = _encoding,
                DataConnectionType = activeMode
                    ? FtpDataConnectionType.AutoActive
                    : FtpDataConnectionType.AutoPassive
            };

        }

        public IEnumerable<IFtpFileExt> GetDirectories()
        {
            IEnumerable<IFtpFileExt> dirs = new List<IFtpFileExt>();

            try
            {
                _ftpClient.Connect();
                var dirsRaw = _ftpClient.GetListing(CurrentDirectory).Select(x => new FtpFileExt(x));
                dirs = dirsRaw.Where(file => file.IsDir);

            }
            catch (Exception e)
            {
                Log.Error(this, "FtpClient", e.Message);
            }
            finally
            {
                if (_ftpClient.IsConnected) _ftpClient.Disconnect();
            }

            return dirs;
        }

        public void Upload(string file)
        {
            var fn = Path.GetFileName(file);
            var remotePath = CurrentDirectory.GetFtpPath(fn);

            _upload(file, remotePath);

        }

        public void Upload(IFtpFileExt targetFolder, string file)
        {
            var fn = Path.GetFileName(file);
            var remotePath = targetFolder.FullPath.GetFtpPath(fn);

            _upload(file, remotePath);

        }

        public void Upload(UploadFileParam uploadFileParam)
        {
            _upload(uploadFileParam.UploadFile, uploadFileParam.TargetFtpFile);
        }

        private void _upload(string source, string target)
        {
            try
            {
                _ftpClient.Connect();
                _ftpClient.UploadFile(source, target, FtpRemoteExists.Overwrite, false, FtpVerify.None,
                    Progress);// new Progress<double>(x => { OnUploadingProgress(this, x); }));

                //_ftpClient.UploadFile(source, target, FtpExists.Overwrite, false, FtpVerify.None,
                //new Progress<double>(x => { OnUploadingProgress(this, x); }));
            }
            catch (Exception e)
            {
                Log.Error(this, "FtpClient", e.Message);
            }
        }

        private void Progress(FtpProgress obj)
        {
            OnUploadingProgress(this, obj.Progress);
        }

        public void DirectoryUp()
        {
            if (!_rootDirectory.Equals(CurrentDirectory))
            {
                CurrentDirectory = CurrentDirectory.GetFtpDirectoryName();
            }
        }

        public IEnumerable<IFtpFileExt> GetDirectoriesAndFiles(IFtpFileExt dir = null)
        {
            var curDir = dir == null ? CurrentDirectory : (dir.IsDir ? dir.FullPath : CurrentDirectory);

            var dirs = new List<FtpFileExt>();
            var files = new List<FtpFileExt>();

            try
            {
                _ftpClient.Connect();
                var items = _ftpClient.GetListing(curDir).Select(x => new FtpFileExt(x));

                _ftpClient.Disconnect();

                var d = items.Where(file => file.IsDir);

                dirs.AddRange(d);

                var fil = items.Where(file => !file.IsDir);

                files.AddRange(fil);

                dirs.Sort(Comparison);
                files.Sort(Comparison);

                dirs.AddRange(files);

            }
            catch (Exception e)
            {
                Log.Error(this, "FtpClient", e.Message);
            }

            return dirs;
        }

        private static int Comparison(IFtpFileExt ftpFile, IFtpFileExt file)
        {
            return string.Compare(ftpFile.Name, file.Name, StringComparison.Ordinal);
        }

        public IEnumerable<IFtpFileExt> ChangeDirectory(IFtpFileExt ftpFileExt)
        {
            CurrentDirectory = ftpFileExt.FullPath;
            return GetDirectoriesAndFiles();
        }
        internal void DownloadFiles(IEnumerable<string> files, string targetDirectory)
        {
            try
            {
                _ftpClient.Connect();
                _ftpClient.RetryAttempts = 3;

                var cntFiles = files.Count();
                var multiplier = 0;


                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file);



                    // потрібно створити папки
                    var extractPath = Path.GetDirectoryName(file.Substring(_rootDirectory.Length + 1));

                    string targetDir = Path.Combine(targetDirectory, extractPath);

                    Directory.CreateDirectory(targetDir);

                    var tmpFile = $"{Path.Combine(targetDir, fileName)}.download";

                    var multiplier1 = multiplier;
                    _ftpClient.DownloadFile(tmpFile, file, FtpLocalExists.Overwrite, FtpVerify.None, progress =>
                    {
                        double ret = (multiplier1 + progress.Progress) / cntFiles;

                        OnDownloadingProcess(this, ret);
                    });


                    string targetFile;
                    var cnt = 0;
                    var filename = Path.GetFileNameWithoutExtension(fileName);
                    var ext = Path.GetExtension(fileName);
                    // згенеруємо 
                    do
                    {
                        if (cnt == 0)
                        { targetFile = Path.Combine(targetDir, fileName); }
                        else
                        {
                            targetFile = $"{Path.Combine(targetDir, filename)}({cnt}){ext}";
                        }

                        cnt++;

                    } while (File.Exists(targetFile));
                    File.Move(tmpFile, targetFile);

                    multiplier += 100;
                }
            }
            catch (Exception e)
            {
                Log.Error(this, "FtpClient", e.Message);
            }
            finally
            {
                if (_ftpClient.IsConnected) _ftpClient.Disconnect();
            }
        }



        /// <summary>
        ///     получить последнее исключение
        /// </summary>
        /// <returns></returns>
        public Exception GetLastException()
        {
            return _exception;
        }

        internal void DeleteFiles(IEnumerable<IFtpFileExt> files)
        {

            try
            {
                _ftpClient.Connect();


                foreach (var file in files)
                {
                    if (file.IsDir)
                    {
                        _ftpClient.DeleteDirectory(file.FullPath, FtpListOption.Auto);
                    }
                    else
                    {
                        _ftpClient.DeleteFile(file.FullPath);
                    }

                }
            }
            catch (Exception e)
            {
                Log.Error(this, "FtpClient", e.Message);
            }
            finally
            {
                if (_ftpClient.IsConnected) _ftpClient.Disconnect();
            }


        }

        internal void CreateDirectory(string directoryName)
        {
            try
            {
                var path = CurrentDirectory.GetFtpPath(directoryName);

                _ftpClient.Connect();
                _ftpClient.CreateDirectory(path);

                CurrentDirectory = path;
            }
            catch (Exception e)
            {
                Log.Error(this, "FtpClient", e.Message);
            }
            finally
            {
                if (_ftpClient.IsConnected) _ftpClient.Disconnect();
            }
        }

        public DownloadFileParam CreateDownloadFileList(IEnumerable<IFtpFileExt> ftpFileExts)
        {
            var list = new DownloadFileParam
            {
                Server = _ftpClient.Host,
                User = _ftpClient.Credentials.UserName,
                Password = _ftpClient.Credentials.Password,
                CodePage = _ftpClient.Encoding.CodePage,
                ActiveMode = _ftpClient.DataConnectionType == FtpDataConnectionType.AutoActive,
                RootDirectory = CurrentDirectory ?? string.Empty
            };

            foreach (IFtpFileExt ff in ftpFileExts)
            {
                if (!ff.IsDir)
                {
                    list.File.Add(ff);
                }
                else
                {
                    list.File.AddRange(GetFilesFromDir(ff));
                }
            }

            return list;
        }

        private List<IFtpFileExt> GetFilesFromDir(IFtpFileExt dir)
        {
            var list = new List<IFtpFileExt>();

            var files = GetDirectoriesAndFiles(dir);

            foreach (IFtpFileExt file in files)
            {
                if (file.IsDir)
                {
                    list.AddRange(GetFilesFromDir(file));
                }
                else
                {
                    list.Add(file);
                }
            }


            return list;
        }
    }

}