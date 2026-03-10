using CSScriptLibrary;
using Interfaces;
using Interfaces.Script;
using System;
using System.Collections.Concurrent;
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
        ConcurrentDictionary<string, CachedScript> Cache = new ConcurrentDictionary<string, CachedScript>();
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
            var ext = Path.GetExtension(path);
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

        public IScript GetScript(string path)
        {
            var lastWrite = File.GetLastWriteTimeUtc(path);

            if (!Cache.TryGetValue(path, out var entry) || entry.LastWrite != lastWrite)
            {
                CSScript.EvaluatorConfig.Engine = EvaluatorEngine.CodeDom;
                var script = CSScript.Evaluator.LoadFile<IScript>(path);

                entry = new CachedScript
                {
                    LastWrite = lastWrite,
                    Script = script
                };

                Cache[path] = entry;
            }

            return entry.Script;
        }
    }
}
