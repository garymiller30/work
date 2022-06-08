using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;
using Job.Models;

namespace Job.Static
{
    public static class NaturalSorting
    {
        [SuppressUnmanagedCodeSecurity]
        private static class SafeNativeMethods
        {
            [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
            public static extern int StrCmpLogicalW(string psz1, string psz2);
        }

        public sealed class NaturalStringComparer : IComparer<string>
        {
            public int Compare(string a, string b)
            {
                return SafeNativeMethods.StrCmpLogicalW(a, b);
            }
        }

        public class NaturalFileInfoNameComparer : IComparer<FileSystemInfoExt>
        {
            public int Compare(FileSystemInfoExt a, FileSystemInfoExt b)
            {
                return SafeNativeMethods.StrCmpLogicalW(a?.FileInfo.Name, b?.FileInfo.Name);
            }
        }

        public class NaturalComparer : IComparer
        {
            //SortOrder _order;

            public NaturalComparer(SortOrder sortOrder)
            {
                //_order = sortOrder;
            }

            public int Compare(object x, object y)
            {
                if (x is FileSystemInfo a && y is FileSystemInfo b)
                {
                    return SafeNativeMethods.StrCmpLogicalW(a.Name, b.Name);
                }

                return -1;
            }
        }

    }
}
