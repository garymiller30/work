// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com 

using Interfaces;
using System;
using System.Collections.Generic;

namespace FtpClient
{
    public sealed class CompareForChanges : IEqualityComparer<IFtpFileExt>
    {
        public bool Equals(IFtpFileExt x, IFtpFileExt y)
        {
            return Compare(x, y);
        }

        public int GetHashCode(IFtpFileExt obj)
        {
            var s = $"{obj.FullPath}{obj.LastModified}{obj.Size}";
            return s.GetHashCode();
        }

        public static bool Compare(IFtpFileExt x, IFtpFileExt y)
        {
            return y != null && x != null && string.Compare(x.FullPath, y.FullPath, StringComparison.Ordinal) == 0 && x.Size == y.Size &&
                   x.LastModified == y.LastModified;
        }
    }
}
