using Interfaces;
using Interfaces.Ftp;
using Interfaces.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSScriptEngine
{
    public class JobListScriptController : IScriptController
    {

        public JobListScriptController(ScriptControllerSettings scriptControllerSettings)
        {
            
        }
        public string PrepareCommandlineArguments(IJob job, IMenuSendTo menuSendTo)
        {
            throw new NotImplementedException();
        }

        public string PrepareCommandlineArguments(IJob job, IMenuSendTo menuSendTo, IFileSystemInfoExt fileSystemInfoExt)
        {
            throw new NotImplementedException();
        }

        public IScriptRunParameters PrepareScriptToStart(IJob job, IMenuSendTo menuSendTo)
        {
            throw new NotImplementedException();
        }

        public void ProccessScriptByName(IDownloadTicket ticket, string scriptName)
        {
            throw new NotImplementedException();
        }

        public void RunScript(IScriptRunParameters parameters)
        {
            throw new NotImplementedException();
        }
    }
}
