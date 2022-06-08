using Interfaces;
using Interfaces.Ftp;
using Interfaces.Script;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonEngine.Controllers
{
    public class FtpScriptController : AbstractScriptController
    {
        public FtpScriptController(ScriptControllerSettings settings):base(settings)
        {

        }

        public override string PrepareCommandlineArguments(IJob job, IMenuSendTo menuSendTo)
        {
            throw new NotImplementedException();
        }

        public override string PrepareCommandlineArguments(IJob job, IMenuSendTo menuSendTo, IFileSystemInfoExt fileSystemInfoExt)
        {
            throw new NotImplementedException();
        }

        public override IScriptRunParameters PrepareScriptToStart(IJob job, IMenuSendTo menuSendTo)
        {
            throw new NotImplementedException();
        }

        public override void ProccessScriptByName(IDownloadTicket ticket,string scriptName)
        {
            

        }
    }
}
