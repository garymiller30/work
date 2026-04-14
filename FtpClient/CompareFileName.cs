// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com 

using System.Collections.Generic;
using Interfaces;
using System;

namespace FtpClient
{
    sealed class CompareFileName : IEqualityComparer<IFtpFileExt>
    {
        public bool Equals(IFtpFileExt f1, IFtpFileExt f2)
        {
            return f2 != null && f1 != null && string.Compare(f1.FullPath, f2.FullPath, StringComparison.Ordinal) == 0;
        }
        public int GetHashCode(IFtpFileExt fi)
        {
            var s = $"{fi.FullPath}";
            return s.GetHashCode();
        }

    }
}
