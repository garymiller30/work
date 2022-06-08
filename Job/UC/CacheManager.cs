//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using Job.Static;

//namespace Job.UC
//{
//    public class CacheManager
//    {
//        private const string TempFolder = "temp";

//        List<DirectoryCache> _directoryCaches = new List<DirectoryCache>();

//        private DirectoryCache _curCache;

//        DirectoryCache IsCached(string path)
//        {
//            return _directoryCaches.FirstOrDefault(x=>x.IsCached(path));
//        }
        

//        public void SaveCache(string cacheFile)
//        {
//            SaveAndLoad.Save(cacheFile,_directoryCaches);
//        }

//        public void LoadCache(string cacheFile)
//        {
//            _directoryCaches = SaveAndLoad.Load<List<DirectoryCache>>(cacheFile) ?? new List<DirectoryCache>();
//            _directoryCaches.ForEach(x=>x.ResetFinishScan());
//        }

//        public void AddToCurrentDirectoryCache(FileSystemInfoExt dirInfo)
//        {
//            //if (!_curCache.Files.Contains(dirInfo))
//             _curCache.Files.Add(dirInfo);
//        }

//        public void RemoveFromCurrentDirectoryCache(FileSystemInfoExt file)
//        {
//            _curCache.Files.Remove(file);
//        }

//        public FileSystemInfoExt FindFileInCurrentDirectoryCache(FileSystemInfoExt fileInfo)
//        {
//            return _curCache.Files?.FirstOrDefault(x => x.FileInfo.FullName.Equals(fileInfo.FileInfo.FullName));
//        }

//        public void CreateCacheFromPath(string curFolder, bool useNaturalSorting)
//        {

//            DirectoryInfo di = new DirectoryInfo(curFolder);

//            var cached = IsCached(curFolder);
            
//            if (cached == null)
//            {
//                cached = new DirectoryCache(curFolder);
//                _directoryCaches.Add(cached);
//            }
//            else // перевіремо, чи писалося щось у цю папку
//            {
//                if (!cached.LastWriteTime.Equals(di.LastWriteTime))
//                {
//                    cached.RescanFiles(); // якщо так, то оновимо інфу
//                }
//            }

//            if (useNaturalSorting) cached.SortingFilesAndDirectories();
//            _curCache = cached;
//        }

//        internal void ForceRefresh(string curFolder)
//        {
//            var cached = IsCached(curFolder);

//            if (cached != null)
//            {
//                _directoryCaches.Remove(cached);
//            }
//        }

//        internal List<FileSystemInfoExt> GetCurrentFolderCache()
//        {
//            return _curCache.Files;
//        }

//        internal int GetCountFiles()
//        {
//            return _curCache?.GetCountFiles() ?? 0;
//        }

//        /*
//                internal void MoveFileOrDir(FileSystemInfoExt source, string target)
//                {
//                    // інфо звідки переносити
//                    var parentDirInfo = Directory.GetParent(source.FileInfo.FullName);
//                    // шукаємо в кеші
//                    var parentdir = _directoryCaches.FirstOrDefault(x => x.IsCached(parentDirInfo.FullName));

//                    // шукаємо файл
//                    var file = parentdir?.Files.FirstOrDefault(x => x.FileInfo.Name.Equals(source.FileInfo.Name));

//                    if (file != null)
//                        parentdir.Files.Remove(file); // є такий - видаляємо з кешу

//                    var targetInfo = new FileInfo(target).ToFileSystemInfoExt();

//                    // якщо кешу ще нема на таргет, то створити
//                    var parentTargetInfo = Directory.GetParent(targetInfo.FileInfo.FullName);

//                    if (!_curCache.IsCached(parentTargetInfo.FullName))
//                    {
//                        var dirCache = _directoryCaches.FirstOrDefault(x => x.IsCached(parentTargetInfo.FullName));

//                        if (dirCache == null) // create new dircaсhe
//                        {
//                            dirCache = new DirectoryCache(parentTargetInfo.FullName);
//                            _directoryCaches.Add(dirCache);
//                        }

//                        dirCache.Files.Add(targetInfo);
//                    }

//                }
//        */

//    }
//}
