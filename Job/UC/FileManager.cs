// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com 

using Interfaces;
using JobSpace.Static;
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

namespace JobSpace.UC
{
    public sealed class FileManager : IFileManager
    {
        public const string TEMP_FOLDER = "temp";
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

        public async Task RefreshAsync(string selectFileName = null)
        {
            try
            {
                List<IFileSystemInfoExt> files;
                if (Settings.ShowAllFilesWithoutDir)
                {
                     await GetAllFilesWithoutDir();
                    return;
                }
                else
                {
                    files = await Task.Run(() => _cache.GetFiles(Settings.CurFolder)).ConfigureAwait(false);
                }

                // Викликайте події на UI-потоці, якщо потрібно
                OnRefreshDirectory(this, files);
                if (!string.IsNullOrEmpty(selectFileName))
                    OnSelectFileName(this, selectFileName);
            }
            catch (Exception ex)
            {
                // Логування або повідомлення про помилку
                OnError(this, ex.Message);
            }
        }

        /// <summary>
        /// Встановити нову папку
        /// </summary>
        /// <param name="rootDir"></param>
        public void SetRootDirectory(string rootDir)
        {
            OnChangeRootDirectory(this, EventArgs.Empty);

            if (string.IsNullOrEmpty(rootDir)) { OnError(this, "rootDir is null or empty"); return; }

            if (Directory.Exists(rootDir))
            {
                Settings.RootFolder = rootDir;
                Settings.CurFolder = rootDir;

                // Fix CS4014: Fire and forget with explicit discard
                _ = RefreshAsync();
            }
            else
            {
                OnError(this, $"SetRootDirectory: {rootDir} not exist");
            }
        }

        public void MoveFileOrDirectoryToCurrentFolder(IFileSystemInfoExt file, string newName)
        {
            if (file.FileInfo.Name.Equals(newName,StringComparison.InvariantCultureIgnoreCase)) return;

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
                    if (FileManager.CopyPaste) //вирізати
                    {
                        _moveFileOrDir(info, Path.Combine(Settings.CurFolder, Path.Combine(Settings.CurFolder, Path.GetFileName(file))));
                    }

                    else
                        FileSystem.CopyDirectory(file, Path.Combine(Settings.CurFolder, Path.GetFileName(file)), UIOption.AllDialogs);

                }
                else
                {
                    var target = Path.Combine(Settings.CurFolder, Path.GetFileName(file));
                    int count = 1;
                    
                    while (File.Exists(target))
                    {
                        target = Path.Combine(Settings.CurFolder, $"{Path.GetFileNameWithoutExtension(file)}({count}){Path.GetExtension(file)}");
                        count++;
                    }

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
                    else FileSystem.DeleteFile(file.FileInfo.FullName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);// File.Delete(file.FileInfo.FullName);
                }
                catch (IOException)
                {
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
            SaveAndLoad.Save(settingFile, Settings);
        }

        public void GetTempFolder()
        {
            //todo: переробити корзину
            var tempPath = Path.Combine(Settings.RootFolder, TEMP_FOLDER);
            if (Directory.Exists(tempPath))
            {
                Settings.CurFolder = tempPath;
                OnChangeDirectory(this, null);
                // Fix CS4014: Fire and forget with explicit discard
                _ = RefreshAsync();
            }
        }

        public void MoveFilesToTrash(IFileSystemInfoExt[] files)
        {

            int cnt = 0;
            string tmpPath;

            do
            {
                cnt++;
                tmpPath = Path.Combine(Settings.RootFolder, TEMP_FOLDER, cnt.ToString("D3"));

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
            _ = RefreshAsync();
        }

        public void DirectoryUp()
        {

            if (Settings.RootFolder == null) return;

            string selectedFileName = Settings.CurFolder;

            if (!Settings.RootFolder.Equals(Settings.CurFolder, StringComparison.InvariantCultureIgnoreCase))
            {
                selectedFileName = Path.GetFileName(Settings.CurFolder);

                Settings.CurFolder = Path.GetDirectoryName(Settings.CurFolder);
            }

            _ = RefreshAsync(selectedFileName);
        }

        public void MoveFolderContentsToHere(IFileSystemInfoExt folder,bool appendFolderName)
        {
            var sourceFolder = folder.FileInfo.FullName;

            if (!sourceFolder.Equals(Settings.RootFolder, StringComparison.InvariantCultureIgnoreCase))
            {
                var destFolder = Path.GetDirectoryName(sourceFolder);

                try
                {
                    MoveDir(sourceFolder, destFolder, appendFolderName);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        void MoveDir(string sourceFolder, string destFolder, bool appendFolderName)
        {
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);

            string folderName ="";
            if (appendFolderName == true)
            {
                folderName = $"{Path.GetFileName(sourceFolder)}_"; 
            }

            // Get Files & Copy
            string[] files = Directory.GetFiles(sourceFolder);
            foreach (string file in files)
            {
                string name = $"{folderName}{Path.GetFileName(file)}";

                // ADD Unique File Name Check to Below!!!!
                string dest = Path.Combine(destFolder, name);

                string destName = $"{folderName}{Path.GetFileNameWithoutExtension(file)}_";
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
                MoveDir(folder, dest, appendFolderName);
            }
            Directory.Delete(sourceFolder);
        }

        // Псевдокод для покращення функції GetAllFilesWithoutDir:
        // 1. Використати async/await для асинхронної роботи з файлами.
        // 2. Уникати зайвого захоплення контексту синхронізації (ConfigureAwait(false)).
        // 3. Перевірити, чи дійсно потрібна змінна files поза Task.Run.
        // 4. Обробити винятки та повідомити про помилку через OnError.
        // 5. Викликати OnRefreshDirectory лише після успішного отримання файлів.

        public async Task GetAllFilesWithoutDir()
        {
            try
            {
                List<IFileSystemInfoExt> files = await Task.Run(() =>
                {
                    var allFiles = _cache.GetAllFiles(Settings.CurFolder);

                    if (Settings.IgnoreFolders != null && Settings.IgnoreFolders.Length > 0)
                    {
                        allFiles = allFiles.Where(f => CheckForIgnoreFolders(f)).ToList();
                    }
                    return allFiles;
                }).ConfigureAwait(false);

                OnRefreshDirectory(this, files);
            }
            catch (Exception ex)
            {
                OnError(this, ex.Message);
            }
        }

        public List<IFileSystemInfoExt> GetDirs()
        {
            List<IFileSystemInfoExt> files = new List<IFileSystemInfoExt>(1);

            files = _cache.GetDirs(Settings.CurFolder);

            return files;
        }

        private bool CheckForIgnoreFolders(IFileSystemInfoExt f)
        {
            if (Settings.IgnoreFolders == null || Settings.IgnoreFolders.Length == 0)
                return true;

            var segments = f.FileInfo.FullName.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            foreach (var ignoreFolder in Settings.IgnoreFolders)
            {
                if (segments.Any(s => s.Equals(ignoreFolder, StringComparison.OrdinalIgnoreCase)))
                {
                    return false;
                }
            }
            return true;
        }

        public void CreateDirectoryInCurrentFolder(string name, Action<string> action)
        {
            throw new NotImplementedException();
        }

        public void MoveTo(IFileSystemInfoExt file, string targetDir)
        {
            _moveFileOrDir(file, targetDir);
        }
    }

}
