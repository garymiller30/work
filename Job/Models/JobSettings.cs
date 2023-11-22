using System;
using Interfaces;

namespace Job.Models
{
    [Serializable]
    public sealed class JobSettings : IJobSettings
    {
        public bool StoreByYear { get; set; }
        public string WorkPath { get; set; }
        public string SignaFileShablon { get; set; } = "{0}_#{1}_{2}";
        public string SignaJobsPath { get; set; }
        public bool UseJobFolder { get;set; } = false;
        public string SubFolderForSignaFile { get;set; } = ".signa";
    }
}
