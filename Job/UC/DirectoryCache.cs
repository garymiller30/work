//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using Job.Static;

//namespace Job.UC
//{



//    [Serializable]
//    public class DirectoryCache
//    {
//        private readonly string _path;

//        public List<FileSystemInfoExt> Files { get; private set; } = new List<FileSystemInfoExt>();

//        private bool _isFinishScan = true;

//        public DirectoryCache(string path)
//        {
//            _path = path;
//            GetFiles();
//        }

//        public void RescanFiles()
//        {
//            Files.Clear();
            
//            GetFiles();
//        }

//        public void ResetFinishScan()
//        {
//            _isFinishScan = true;
//        }
//        public DateTime LastWriteTime { get; private set; }

//        internal bool IsCached(string path)
//        {
//            return _path.Equals(path);
//        }

//        private void GetFiles()
//        {
//            if (_isFinishScan)
//            {
//                _isFinishScan = false;
//                var pathInfo = new DirectoryInfo(_path);
//                if (pathInfo.Exists)
//                {
//                    LastWriteTime = pathInfo.LastWriteTime;

//                    var files = pathInfo.GetFileSystemInfos();
//                    Files.AddRange(files.Select(Converter.ConvertToFileSystemInfoExt));

//                }

//                _isFinishScan = true;
//            }
//        }

//        public void SortingFilesAndDirectories()
//        {
//            var comparer = new NaturalSorting.NaturalFileInfoNameComparer();

//            var dirs = Files.Where(x=>x.IsDir).ToList();
//            dirs.Sort(comparer);

//            var files = Files.Where(x => !x.IsDir).ToList();

//            files.Sort(comparer);
//            dirs.AddRange(files);

//            Files = dirs;
//        }
//        /// <summary>
//        /// Перевірити чи це файл або папка
//        /// </summary>
//        /// <param name="fileSystemInfo"></param>
//        /// <returns></returns>
//        //public static bool IsDir(FileSystemInfoExt fileSystemInfo)
//        //{
//        //    return (fileSystemInfo.FileInfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory;
//        //}

//        public override int GetHashCode()
//        {
//            return _path.GetHashCode();
//        }

//        public override bool Equals(object obj)
//        {
//            return obj != null && _path.Equals(obj.ToString(),StringComparison.InvariantCultureIgnoreCase);
//        }

//        internal int GetCountFiles()
//        {
//            return Files.Count(x => !x.IsDir);
//        }
//    }

   
//}
