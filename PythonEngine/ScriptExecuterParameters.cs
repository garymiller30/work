using Interfaces;
using Interfaces.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonEngine
{
    public sealed class ScriptExecuterParameters
    {
        public IScriptController ScriptController {get;set;}
        public IScriptRunParameters Parameters{get;set;}
    }
}
