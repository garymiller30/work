// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com 

using Interfaces;
using Job.Static;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using FileSystem = Microsoft.VisualBasic.FileIO.FileSystem;

namespace Job.UC
{
    public sealed class FileManager : IFileManager
    {

        public FileBrowserSettings Settings { get; set; } = new FileBrowserSettings();


        public event EventHandler<IFileSystemInfoExt> OnAddFile = delegate { };
        public event EventHandler<IFileSystemInfoExt> OnDeleteFile = delegate { };
        public event EventHandler<IFileSystemInfoExt> OnChangeFile = delegate { };
        public event EventHandler<IFileSystemInfoExt> OnChangeDirectory = delegate { };
        public event EventHandler OnChangeRootDirectory = delegate { };
        public event EventHandler<List<IFileSystemInfoExt>> OnRefreshDirectory = delegate { };
        public event EventHandler<IFileSystemInfoExt> OnSelectParent = delegate { };
        public event EventHandler<string> OnError = delegate { };
        public event EventHandler<string> OnSelectFileName = delegate { };

        public static bool CopyPaste;


        private readonly ICache<IFileSystemInfoExt> _cache;

        private bool _useNaturalSorting;
        //private string _parentDir;

        public bool UseNaturalSorting
        {
            get => _useNaturalSorting;
            set
            {
                _useNaturalSorting = value;
            }
        }



        public FileManager()
        {
            _cache = new NoCache(new FileWatcher());
            _cache.OnDeleted += _cache_OnDeleted;
            _cache.OnError += CacheOnOnError;
            _cache.OnChanged += CacheOnOnChanged;
            _cache.OnCreated += CacheOnOnCreated;
            _cache.OnRenamed += CacheOnOnRenamed;

        }

        private void CacheOnOnRenamed(object sender, IFileSystemInfoExt e)
        {
            OnChangeFile(this, e);
        }

        private void CacheOnOnCreated(object sender, IFileSystemInfoExt e)
        {

            OnAddFile(this, e);
        }

        private void CacheOnOnChanged(object sender, IFileSystemInfoExt e)
        {
            OnChangeFile(this, e);
        }

        private void CacheOnOnError(object sender, ErrorEventArgs e)
        {
            OnError(this, e.GetException().Message);
        }

        private void _cache_OnDeleted(object sender, IFileSystemInfoExt e)
        {
            OnDeleteFile(this, e);
        }

        public async void RefreshAsync(string selectFileName = null)
        {
            List<IFileSystemInfoExt> files = new List<IFileSystemInfoExt>(1);
            try
            {
                await Task.Run(() =>
                {
                    files = _cache.GetFiles(Settings.CurFolder);
                }).ConfigureAwait(true);
            }
            catch
            {


            }


            OnRefreshDirectory(this, files);
            if (!string.IsNullOrEmpty(selectFileName))
                OnSelectFileName(this, selectFileName);
        }

        /// <summary>
        /// Встановити нову папку
        /// </summary>
        /// <param name="rootDir"></param>
        public void SetRootDirectory(string rootDir)
        {
            OnChangeRootDirectory(this, null);

            if (string.IsNullOrEmpty(rootDir)) { OnError(this, "rootDir is null or empty"); return; }

            if (Directory.Exists(rootDir))
            {

                Settings.RootFolder = rootDir;
                Settings.CurFolder = rootDir;

                RefreshAsync();
            }
            else
            {
                OnError(this, $"SetRootDirectory: {rootDir} not exist");
            }
        }

        public void MoveFileOrDirectoryToCurrentFolder(IFileSystemInfoExt file, string newName)
        {
            _moveFileOrDir(file, Path.Combine(Settings.CurFolder, newName));
        }

        private void _moveFileOrDir(IFileSystemInfoExt source, string target)
        {
            try
            {
                if (source.IsDir)
                {
                    FileSystem.MoveDirectory(source.FileInfo.FullName, target, UIOption.AllDialogs);
                }
                else
                {
                    FileSystem.MoveFile(source.FileInfo.FullName, target, UIOption.AllDialogs);
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, @"Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
        //public void CreateJdf(IEnumerable<IFileSystemInfoExt> files)
        //{

        //    //StopWatcher();

        //    var jdf = new Jdf { PatternSettings = { Separator = "~" } };

        //    jdf.PatternSettings.AddPattern(PatternEnum.Customer);
        //    jdf.PatternSettings.AddPattern(PatternEnum.JobName);
        //    jdf.PatternSettings.AddPattern(PatternEnum.PageNumber);
        //    jdf.PatternSettings.AddPattern(PatternEnum.FrontBack);
        //    jdf.PatternSettings.AddPattern(PatternEnum.Color);

        //    jdf.ShablonPath = @"JDF\JobStart.jdf";

        //    foreach (IFileSystemInfoExt file in files)
        //    {
        //        jdf.AddFile(file.FileInfo.FullName);
        //    }

        //    jdf.CreateJdf(Settings.CurFolder);

        //    Refresh();

        //і}

        public void CreateDirectoryInCurrentFolder(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                try
                {
                    var dir = Path.Combine(Settings.CurFolder, name);
                    Directory.CreateDirectory(dir);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        public void PasteFromClipboard(string[] files)
        {

            foreach (var file in files)
            {

                var info = new FileInfo(file).ToFileSystemInfoExt();

                if (Directory.Exists(file))
                {
                    if (FileManager.CopyPaste) //вырезать
                    {
                        _moveFileOrDir(info, Path.Combine(Settings.CurFolder, Path.Combine(Settings.CurFolder, Path.GetFileName(file))));
                    }

                    else
                        FileSystem.CopyDirectory(file, Path.Combine(Settings.CurFolder, Path.GetFileName(file)), UIOption.AllDialogs);

                }
                else
                {
                    var target = Path.Combine(Settings.CurFolder, Path.GetFileName(file));
                    if (target.Equals(file))
                        continue;


                    if (CopyPaste)
                    {
                        _moveFileOrDir(info, target);
                    }
                    else
                        FileSystem.CopyFile(file, target, UIOption.AllDialogs);
                }
            }
        }

        public void DeleteFilesAndDirectories(IEnumerable<IFileSystemInfoExt> files)
        {

            foreach (IFileSystemInfoExt file in files)
            {
            retry:

                try
                {
                    if (file.IsDir) FileSystem.DeleteDirectory(file.FileInfo.FullName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);// Directory.Delete(file.FileInfo.FullName, true);
                    else FileSystem.DeleteFile(file.FileInfo.FullName,UIOption.OnlyErrorDialogs,RecycleOption.SendToRecycleBin);// File.Delete(file.FileInfo.FullName);
                }
                catch (IOException)
                {
                    //var processes = FileUtil.WhoIsLocking(file.FileInfo.FullName);
                    //var message = processes.Select(x => x.ProcessName).Aggregate((a, n) => $"{a}/n{n}");
                    var message = FileUtil.GetNamesWhoBlock(file.FileInfo.FullName);
                    if (MessageBox.Show($"Файл {file.FileInfo.FullName} заблоковано такими програмами: {message}", "Файл заблоковано", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning) == DialogResult.Retry)
                    {
                        goto retry;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "помилка");
                }

            }

        }



        /// <summary>
        /// завантажити налаштування провідника
        /// </summary>
        public void LoadSettings(string settingFile)
        {
            //_cacheManager.LoadCache(settingFile+".cache");


            var setting = SaveAndLoad.Load<FileBrowserSettings>(settingFile);
            if (setting != null)
            {
                Settings = setting;
                SetRootDirectory(Settings.RootFolder);
            }

        }
        /// <summary>
        /// зберегти налаштування провідника
        /// </summary>
        public void SaveSettings(string settingFile)
        {
            //_cacheManager.SaveCache(settingFile+".cache");

            SaveAndLoad.Save(settingFile, Settings);
        }

        public void GetTempFolder()
        {
            //todo: переробити корзину
            var tempPath = Path.Combine(Settings.RootFolder, "temp");
            if (Directory.Exists(tempPath))
            {
                Settings.CurFolder = tempPath;
                OnChangeDirectory(this, null);
                RefreshAsync();
            }
        }

        public void MoveFilesToTrash(IFileSystemInfoExt[] files)
        {

            int cnt = 0;
            string tmpPath;

            do
            {
                cnt++;
                tmpPath = Path.Combine(Settings.RootFolder, "temp", cnt.ToString("D3"));

            } while (Directory.Exists(tmpPath));

            try
            {
                if (!Directory.Exists(tmpPath)) Directory.CreateDirectory(tmpPath);

                //var filesCopy = files.Clone();

                foreach (IFileSystemInfoExt file in files)
                {
                    _moveFileOrDir(file, Path.Combine(tmpPath, file.FileInfo.Name));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        public List<IFileSystemInfoExt> GetFiles(string path)
        {
            List<IFileSystemInfoExt> files = new List<IFileSystemInfoExt>();

            var f = Directory.GetFiles(path).Select(x => new FileInfo(x).ToFileSystemInfoExt());

            files.AddRange(f);

            return files;
        }


        public static void CopyFiles(string[] files, string target)
        {
            foreach (var file in files)
            {
                if (File.Exists(file))
                    FileSystem.CopyFile(file, Path.Combine(target, Path.GetFileName(file)), UIOption.AllDialogs);
            }
        }

        public int GetCountFiles()
        {
            return _cache.GetCountFiles();
        }

        public void OpenFileOrFolder(IFileSystemInfoExt fileOrDirectory)
        {

            if (fileOrDirectory.IsDir)
            {

                ChangeDirectory(fileOrDirectory);
            }
            else if (File.Exists(fileOrDirectory.FileInfo.FullName))
            {
                try
                {
                    var pi = new ProcessStartInfo(fileOrDirectory.FileInfo.FullName)
                    {
                        WorkingDirectory = Path.GetDirectoryName(fileOrDirectory.FileInfo.FullName)
                    };
                    Process.Start(pi);

                }
                catch (Exception ex)
                {


                    MessageBox.Show(ex.Message, @"Error!", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                }

            }
        }

        private void ChangeDirectory(IFileSystemInfoExt directory)
        {
            OnChangeDirectory(this, directory);
            Settings.CurFolder = directory.FileInfo.FullName.ToLower();
            RefreshAsync();
        }

        public void DirectoryUp()
        {
            if (Settings.RootFolder == null) return;
            if (Settings.RootFolder.Equals(Settings.CurFolder, StringComparison.InvariantCultureIgnoreCase)) return;

            var selectedFileName = Path.GetFileName(Settings.CurFolder);

            Settings.CurFolder = Path.GetDirectoryName(Settings.CurFolder);
            RefreshAsync(selectedFileName);
        }

        public void MoveFolderContentsToHere(IFileSystemInfoExt folder)
        {
            var sourceFolder = folder.FileInfo.FullName;

            if (!sourceFolder.Equals(Settings.RootFolder, StringComparison.InvariantCultureIgnoreCase))
            {
                var destFolder = Path.GetDirectoryName(sourceFolder);

                try
                {
                    MoveDir(sourceFolder, destFolder);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message,"Помилка",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                


            }
        }

        void MoveDir(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);

            // Get Files & Copy
            string[] files = Directory.GetFiles(sourceFolder);
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);

                // ADD Unique File Name Check to Below!!!!
                string dest = Path.Combine(destFolder, name);

                string destName = $"{Path.GetFileNameWithoutExtension(file)}_";
                string ext = Path.GetExtension(file);


                while (File.Exists(dest))
                {
                    destName += "C";
                    dest = Path.Combine(destFolder, $"{destName}{ext}");
                }

                File.Move(file, dest);
            }

            // Get dirs recursively and copy files
            string[] folders = Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(destFolder, name);
                MoveDir(folder, dest);


            }

            Directory.Delete(sourceFolder);
        }

        public async void GetAllFilesWithoutDir()
        {
            List<IFileSystemInfoExt> files = new List<IFileSystemInfoExt>(1);
            try
            {
                await Task.Run(() =>
                {
                    files = _cache.GetAllFiles(Settings.CurFolder);
                }).ConfigureAwait(true);
            }
            catch
            {


            }

            OnRefreshDirectory(this, files);
        }
    }

}
