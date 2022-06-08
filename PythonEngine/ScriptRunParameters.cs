using Interfaces;
using System;
using System.Dynamic;
using System.Linq;

namespace PythonEngine
{
    public class ScriptRunParameters : IScriptRunParameters
    {
        public string ScriptPath { get; set; }
        //public string ScriptPathArg { get; set; }

        public dynamic Values { get; set; } = new ExpandoObject();
    }
}
