using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IScriptRunParameters
    {
        dynamic Values { get;set; }
        string ScriptPath { get; set; }
        //string ScriptPathArg { get; set; }
    }
}
