using Interfaces.Ftp;
using System;
using System.Linq;

namespace Interfaces.Script
{
    public interface IScriptController
    {
        void RunScript(IScriptRunParameters parameters);
        IScriptRunParameters PrepareScriptToStart(IJob job, IMenuSendTo menuSendTo);
        string PrepareCommandlineArguments(IJob job, IMenuSendTo menuSendTo);
        string PrepareCommandlineArguments(IJob job, IMenuSendTo menuSendTo, IFileSystemInfoExt fileSystemInfoExt);
        void ProccessScriptByName(IDownloadTicket ticket, string scriptName);
    }
}
