using Interfaces;
using Interfaces.Script;
using PythonEngine.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonEngine
{
    public sealed class PythonScriptEngine : IScriptEngine
    {
        IUserProfile _profile;
        private readonly PythonEngine _pythonEngine;

        public IScriptController Ftp { get; set; }
        public IScriptController JobList { get; set; }
        public IScriptController FileBrowser { get; set; }

        public PythonScriptEngine(IUserProfile profile)
        {
            _pythonEngine = new PythonEngine();
            _profile = profile;
            Ftp = new FtpScriptController(new ScriptControllerSettings(){ 
                Engine = _pythonEngine,
                Profile = profile
                });
            JobList = new JobListScriptController(new ScriptControllerSettings(){ 
                Engine = _pythonEngine,
                Profile = profile
                });
            FileBrowser = new FileBrowserScriptController(new ScriptControllerSettings(){ 
                Engine = _pythonEngine,
                Profile = profile
                });
        }

        /// <summary>
        /// перевірити, чи файл є файлом пітона 🐍
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool IsScriptFile(string path)
        {
            return _pythonEngine.IsScript(path);
        }

    }
}
