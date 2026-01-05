using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginFileshareWeb
{
    public class LinkInfo
    {
        public string Name {  get; set; }
        public string Url { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is LinkInfo other)
            {
                return this.Url == other.Url;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Url.GetHashCode();
        }
    }
}
