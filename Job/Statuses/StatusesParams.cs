using Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Job.Statuses
{
    /// <summary>
    /// при зміні статусу запускати вказану програму
    /// </summary>
    public sealed class StatusesParams : IStatusesParams
    {
        private const string FileName = "OnChangeStatusesParams.xml";

        public IUserProfile UserProfile { get; set; }

        private List<StatusChangeParam> _params;



        public void Load()
        {

            var path = Path.Combine(UserProfile.ProfilePath, FileName);

            _params = File.Exists(path)
                ? Commons.DeserializeXML<List<StatusChangeParam>>(path) ?? new List<StatusChangeParam>()
                : new List<StatusChangeParam>();
        }

        public void Save()
        {
            var dir = UserProfile.ProfilePath;

            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            var path = Path.Combine(dir, FileName);
            Commons.SerializeXML(_params, path);
        }

        /// <summary>
        /// повертає параметр за статусом
        /// </summary>
        /// <param name="status"></param>
        /// <param name="job"></param>
        /// <returns></returns>
        //public StatusChangeParam GetParam(int statusCode)
        //{
        //    var param = _params.FirstOrDefault(x => x.StatusCode == statusCode);
        //    if (param == null)
        //    {
        //        param = new StatusChangeParam(statusCode);
        //        _params.Add(param);
        //    }

        //    return param;
        //}

        public void Run(IJob job)
        {
            var param = GetParam(job.StatusCode);
            if (param.Enable && !string.IsNullOrEmpty(param.ProgramPath))
            {
                var pii = new ProcessStartInfo
                {
                    WorkingDirectory = Path.GetDirectoryName(param.ProgramPath),
                    FileName = param.ProgramPath,
                    Arguments = string.Format(param.CommandLineParams,
                        job.Number,
                        job.Customer)
                };

                var p = Process.Start(pii);
                p.WaitForExit();
                if (p.ExitCode != 0)
                {
                    MessageBox.Show(p.ExitCode.ToString(), "Error Code", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public IStatusChangeParam GetParam(int statusCode)
        {
            var param = _params.FirstOrDefault(x => x.StatusCode == statusCode);
            if (param == null)
            {
                param = new StatusChangeParam(statusCode);
                _params.Add(param);
            }

            return param;
        }
    }
}
