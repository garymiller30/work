using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Script
{
    public interface IScriptEngine
    {
        IScriptController Ftp {get;set;}
        IScriptController JobList {get;set;}
        IScriptController FileBrowser{get;set;}

        bool IsScriptFile(string path);
    }
}
