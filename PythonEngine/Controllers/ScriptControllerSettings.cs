using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonEngine.Controllers
{
    public class ScriptControllerSettings
    {
        public IUserProfile Profile{get;set;}
        public PythonEngine Engine {get;set;}
    }
}
