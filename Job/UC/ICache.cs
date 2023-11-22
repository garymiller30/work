using Interfaces;
using System;
using System.Collections.Generic;

namespace Job.UC
{
    public interface ICache<T>
    {
        event EventHandler<T> OnChanged;
        event EventHandler<T> OnDeleted;
        event EventHandler<T> OnCreated;
        event EventHandler<T> OnRenamed;
        event System.IO.ErrorEventHandler OnError;


        List<T> GetFiles(string path);
        List<T> GetAllFiles(string path);
        List<T> GetDirs(string path);
        int GetCountFiles();
    }
}
