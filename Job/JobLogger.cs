using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace JobSpace
{
    public static class JobLogger
    {
        //private const string FileName = "log.txt";
        private const string DirName = "Log";

        static JobLogger()
        {
            if (!Directory.Exists(DirName)) Directory.CreateDirectory(DirName);
        }

        public static void WriteLine(string str)
        {
            var fn = $"{DateTime.Now.Date:yy-MM}.txt";

            var path = Path.Combine(DirName, fn);

            File.AppendAllText(path, $"{DateTime.Now:g}\t {$"{str}\n"}", Encoding.UTF8);
        }

        public static void WriteLine(object o)
        {
            WriteLine(o.ToString());
        }

        public static void OpenLogFolderInExlorer()
        {
            Process.Start(DirName);
        }

    }
}
