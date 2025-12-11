using Interfaces;
using System;
using System.Dynamic;
using System.Linq;

namespace PythonEngine
{
    public sealed class ScriptRunParameters : IScriptRunParameters
    {
        public string ScriptPath { get; set; }
        public dynamic Values { get; set; } = new ExpandoObject();
    }
}
