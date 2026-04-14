using Interfaces;
using Interfaces.Ftp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonEngine.Controllers
{
    public sealed class FileBrowserScriptController : AbstractScriptController
    {
        public FileBrowserScriptController(ScriptControllerSettings settings):base(settings)
        {

        }

        public override string PrepareCommandlineArguments(IJob job, IMenuSendTo menuSendTo)
        {
            throw new NotImplementedException();
            // var args = string.Format(menuSendTo.CommandLine,
            //                                    string.Empty,
            //                                    _fileManager.Settings.CurFolder,
            //                                    string.Empty,
            //                                    job?.Number,
            //                                    job?.Customer,
            //                                    job?.Description);
            //return args;
        }

        public override string PrepareCommandlineArguments(IJob job, IMenuSendTo menuSendTo, IFileSystemInfoExt fileSystemInfoExt)
        {
            throw new NotImplementedException();
        }

        public override IScriptRunParameters PrepareScriptToStart(IJob job, IMenuSendTo menuSendTo)
        {
            throw new NotImplementedException();
        }

        public override void ProccessScriptByName(IDownloadTicket ticket, string scriptName)
        {
            var script = Settings.Profile.MenuManagers.Utils.Get().FirstOrDefault(x => x.Name.ToLower().Equals(scriptName.ToLower()) && Settings.Profile.ScriptEngine.IsScriptFile(x.Path));
            if (script == null) return;

            var param = PrepareScriptToStart(ticket.Job, script);

            ScriptExecuter.AddToQuery(this, param);
        }
    }
}
