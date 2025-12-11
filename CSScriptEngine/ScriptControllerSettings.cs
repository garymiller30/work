using Interfaces;
using Interfaces.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSScriptEngine
{
    public class ScriptControllerSettings
    {
        public IUserProfile Profile { get; set; }
        public IScriptEngine Engine { get;set; }
    }
}
