using Interfaces.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSScriptEngine
{
    public class CachedScript
    {
        public DateTime LastWrite;
        public IScript Script;
    }
}
