using Interfaces;
using Interfaces.Ftp;
using Interfaces.Script;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PythonEngine.Controllers
{
    public abstract class AbstractScriptController : IScriptController
    {
        private readonly ScriptScope _scope;
        protected ScriptControllerSettings Settings { get; set; }
        public AbstractScriptController(ScriptControllerSettings settings)
        {
            Settings = settings;
            _scope = settings.Engine.CreateScope();
        }

        public void RunScript(IScriptRunParameters parameters)
        {
            
            Cursor.Current = Cursors.WaitCursor;
            SetVariablesToScope(_scope,parameters.Values);
            Settings.Engine.ExecuteScript(_scope, parameters);
            Cursor.Current = Cursors.Default;
        }

        void SetVariablesToScope(ScriptScope scope, dynamic vars)
        {
            foreach (var property in (IDictionary<string, object>)vars)
            {
                Settings.Engine.SetVariable(scope, property.Key, property.Value);
            }

            Settings.Engine.SetVariable(scope, "Profile", Settings.Profile);
        }

        public abstract string PrepareCommandlineArguments(IJob job, IMenuSendTo menuSendTo);
        public abstract string PrepareCommandlineArguments(IJob job, IMenuSendTo menuSendTo, IFileSystemInfoExt fileSystemInfoExt);

        public abstract IScriptRunParameters PrepareScriptToStart(IJob job, IMenuSendTo menuSendTo);

        public abstract void ProccessScriptByName(IDownloadTicket ticket, string scriptName);
    }
}
