using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Job.Models;
using Org.BouncyCastle.Asn1.Mozilla;

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


        // використовується в браузері на колонці "ім'я"
        public sealed class NaturalFileInfoNameComparer : IComparer<FileSystemInfoExt>
        {
            public int Compare(FileSystemInfoExt a, FileSystemInfoExt b)
            {
                return SafeNativeMethods.StrCmpLogicalW(a?.FileInfo.Name, b?.FileInfo.Name);
            }
        }

        public sealed class FileWidthComparer : IComparer
        {
            SortOrder _order;

            public FileWidthComparer(SortOrder order)
            {
                _order = order;
            }

            public int Compare(object x, object y)
            {

                decimal w1= ((FileSystemInfoExt)(x as OLVListItem).RowObject).Format.Width;
                decimal w2 = ((FileSystemInfoExt)(y as OLVListItem).RowObject).Format.Width;
                
                if (_order == SortOrder.Ascending)
                {
                    return _compare(w1,w2);
                }
                else
                {
                    return _compare(w2,w1);
                }


                int _compare(decimal left, decimal right)
                {
                    if (left > right) return 1;
                    if (left < right) return -1;
                    return 0;
                }

            }
        }

        public sealed class FileHeightComparer : IComparer
        {
            SortOrder _order;

            public FileHeightComparer(SortOrder order)
            {
                _order = order;
            }

            public int Compare(object x, object y)
            {

                decimal w1 = ((FileSystemInfoExt)(x as OLVListItem).RowObject).Format.Height;
                decimal w2 = ((FileSystemInfoExt)(y as OLVListItem).RowObject).Format.Height;

                if (_order == SortOrder.Ascending)
                {
                    return _compare(w1, w2);
                }
                else
                {
                    return _compare(w2, w1);
                }


                int _compare(decimal left, decimal right)
                {
                    if (left > right) return 1;
                    if (left < right) return -1;
                    return 0;
                }

            }
        }
        public sealed class FilePagesComparer : IComparer
        {
            SortOrder _order;

            public FilePagesComparer(SortOrder order)
            {
                _order = order;
            }

            public int Compare(object x, object y)
            {

                decimal w1 = ((FileSystemInfoExt)(x as OLVListItem).RowObject).Format.cntPages;
                decimal w2 = ((FileSystemInfoExt)(y as OLVListItem).RowObject).Format.cntPages;

                if (_order == SortOrder.Ascending)
                {
                    return _compare(w1, w2);
                }
                else
                {
                    return _compare(w2, w1);
                }


                int _compare(decimal left, decimal right)
                {
                    if (left > right) return 1;
                    if (left < right) return -1;
                    return 0;
                }

            }
        }


        public sealed class OrderDateComparer: IComparer
        {
            SortOrder _order;

            public OrderDateComparer(SortOrder order)
            {
                _order = order;
            }
            public int Compare(object x, object y)
            {
                DateTime d1 = ((Job)(x as OLVListItem).RowObject).Date;
                DateTime d2 = ((Job)(y as OLVListItem).RowObject).Date;
                if (_order == SortOrder.Ascending)
                {
                    return DateTime.Compare(d1, d2);
                }
                return DateTime.Compare(d2, d1);
            }
        }


        public sealed class FileDateComparer : IComparer
        {
            SortOrder _order;

            public FileDateComparer(SortOrder order)
            {
                _order= order;
            }

            public int Compare(object x, object y)
            {
                DateTime d1 = ((FileSystemInfoExt)(x as OLVListItem).RowObject).FileInfo.LastWriteTime;
                DateTime d2 = ((FileSystemInfoExt)(y as OLVListItem).RowObject).FileInfo.LastWriteTime;

                if (_order == SortOrder.Ascending)
                {
                   return DateTime.Compare(d1, d2);
                }
                else
                {
                    return DateTime.Compare(d2, d1);
                }

            }
        }

        public sealed class FileBleedComparer : IComparer
        {
            SortOrder _order;

            public FileBleedComparer(SortOrder order)
            {
                _order = order;
            }

            public int Compare(object x, object y)
            {

                decimal w1 = ((FileSystemInfoExt)(x as OLVListItem).RowObject).Format.Bleeds;
                decimal w2 = ((FileSystemInfoExt)(y as OLVListItem).RowObject).Format.Bleeds;

                if (_order == SortOrder.Ascending)
                {
                    return _compare(w1, w2);
                }
                else
                {
                    return _compare(w2, w1);
                }


                int _compare(decimal left, decimal right)
                {
                    if (left > right) return 1;
                    if (left < right) return -1;
                    return 0;
                }

            }
        }

        public sealed class FileNameNaturalComparer : IComparer
        {
            SortOrder _order;

            public FileNameNaturalComparer(SortOrder order)
            {
                _order = order;
            }
            public int Compare(object x, object y)
            {
                if (x is OLVListItem a && y is OLVListItem b) {
                    

                    if (_order == SortOrder.Ascending)
                    {
                        return SafeNativeMethods.StrCmpLogicalW(((FileSystemInfoExt)a.RowObject).FileInfo.Name, ((FileSystemInfoExt)b.RowObject).FileInfo.Name);
                    }
                    else
                    {
                       
                        return SafeNativeMethods.StrCmpLogicalW(((FileSystemInfoExt)b.RowObject).FileInfo.Name, ((FileSystemInfoExt)a.RowObject).FileInfo.Name);
                    }
                    

                }
                return -1;
            }
        }

        public sealed class NaturalComparer : IComparer
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
