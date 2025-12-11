using Interfaces;
using Interfaces.Script;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSScriptEngine
{
    public sealed class CSScriptEngine : IScriptEngine
    {
        IUserProfile _profile;

        public IScriptController Ftp { get; set; }
        public IScriptController JobList { get; set; }
        public IScriptController FileBrowser { get; set; }


        public CSScriptEngine(IUserProfile profile)
        {
            _profile = profile;

            Ftp = new FtpScriptController(new ScriptControllerSettings()
            {
                Engine = this,
                Profile = profile
            });
            JobList = new JobListScriptController(new ScriptControllerSettings()
            {
                Engine = this,
                Profile = profile
            });
            FileBrowser = new FileBrowserScriptController(new ScriptControllerSettings()
            {
                Engine = this,
                Profile = profile
            });

        }
        public bool IsScriptFile(string path)
        {
            var ext =Path.GetExtension(path);
            if (string.IsNullOrEmpty(ext))
                return false;

            if (ext.StartsWith(".cs", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
