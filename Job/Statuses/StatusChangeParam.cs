using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Interfaces;

namespace Job.Statuses
{
    [Serializable]
    public sealed class StatusChangeParam : IStatusChangeParam
    {
        public bool Enable { get; set; } = false;
        [Obsolete]
        public global::Job.Settings.JobStatus Status { get; set; }

        public int StatusCode { get; set; }

        public string ProgramPath { get;set; }
        public string CommandLineParams { get; set; } = string.Empty;

        private StatusChangeParam()
        {

        }

        public StatusChangeParam(int statusCode)
        {
            StatusCode = statusCode;
        }

    }
}
