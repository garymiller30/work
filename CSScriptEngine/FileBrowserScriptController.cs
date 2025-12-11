using CSScriptLibrary;
using Interfaces;
using Interfaces.Ftp;
using Interfaces.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSScriptEngine
{
    public class FileBrowserScriptController : IScriptController
    {

        private ScriptControllerSettings _settings;

        public FileBrowserScriptController(ScriptControllerSettings settings)
        {
            // Initialize with settings if needed
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
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
            Cursor.Current = Cursors.WaitCursor;
            
            dynamic script = CSScript.Evaluator.LoadFile(parameters.ScriptPath);

            if (script != null) {
                try
                {
                    script.Run(parameters.Values);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            Cursor.Current = Cursors.Default;
        }
    }
}
