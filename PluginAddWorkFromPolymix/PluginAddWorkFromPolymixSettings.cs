using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginAddWorkFromPolymix
{
    [Serializable]
    public sealed class PluginAddWorkFromPolymixSettings
    {
        public string ServerAddress { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string BaseName { get; set; }
        //public int KindId { get; set; } = 33;
        public Dictionary<string,string> CusomerNames { get; set; }= new Dictionary<string,string>();

        public List<int> StatusFilter { get; set; } = new List<int>();
        public List<int> KindFilter { get; set; } = new List<int>();
    }
}
