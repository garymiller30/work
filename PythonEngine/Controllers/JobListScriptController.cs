using Interfaces;
using Interfaces.Ftp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonEngine.Controllers
{
    public sealed class JobListScriptController : AbstractScriptController
    {
        public JobListScriptController(ScriptControllerSettings settings) : base(settings)
        {

        }

        public override string PrepareCommandlineArguments(IJob job, IMenuSendTo menuSendTo)
        {
            string args = string.Empty;

            if (menuSendTo.CommandLine.Contains("{1}"))
            {
                args = string.Format(menuSendTo.CommandLine,
                        string.Empty, //0
                        Settings.Profile.Jobs.GetFullPathToWorkFolder(job), //Path.GetDirectoryName(info.FileInfo.FullName),//1
                        string.Empty, //Path.GetFileNameWithoutExtension(info.FileInfo.FullName),//2
                        job.Number, //3
                        job.Customer);//4
            }
           
            return args;
        }

        public override string PrepareCommandlineArguments(IJob job, IMenuSendTo menuSendTo, IFileSystemInfoExt fileSystemInfoExt)
        {
            var args = string.Empty;

            if (menuSendTo.CommandLine.Contains("{0}"))
            {
                args = string.Format(menuSendTo.CommandLine,
                    fileSystemInfoExt.FileInfo.FullName,
                    Path.GetDirectoryName(fileSystemInfoExt.FileInfo.FullName),
                    Path.GetFileNameWithoutExtension(fileSystemInfoExt.FileInfo.FullName),
                    job.Number,
                    job.Customer);
            }
            return args;
        }

        public override IScriptRunParameters PrepareScriptToStart(IJob job, IMenuSendTo menuSendTo)
        {
            var args = PrepareCommandlineArguments(job,menuSendTo);

            var scriptParam = new ScriptRunParameters();

            scriptParam.Values.Order = job;
            scriptParam.Values.Profile = Settings.Profile;
            scriptParam.Values.OrderNumber = job.Number;
            scriptParam.Values.Customer = job.Customer;
            scriptParam.Values.Description = job.Description;
            scriptParam.Values.FileName = args;
            scriptParam.ScriptPath = menuSendTo.Path;

            return scriptParam;
        }

        public override void ProccessScriptByName(IDownloadTicket ticket, string scriptName)
        {
            var script = Settings.Profile.MenuManagers.Utils.Get().FirstOrDefault(x => x.Name.ToLower().Equals(scriptName.ToLower()) && Settings.Profile.ScriptEngine.IsScriptFile(x.Path));
            if (script == null) return;

            var param = PrepareScriptToStart(ticket.Job,script);

            ScriptExecuter.AddToQuery(this, param);
            //Settings.Profile.ScriptEngine.JobList.RunScript(param);
        }
    }
}
