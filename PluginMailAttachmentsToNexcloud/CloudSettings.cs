using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginMailAttachmentsToNextcloud
{
    public class CloudSettings
    {
        public string Server { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string RemoteFolder { get; set; }
        public decimal SizeLimit { get; set; } = 1024 * 1024 * 10;

        public bool Validate()
        {
            if (string.IsNullOrEmpty(Server)) return false;
            if (string.IsNullOrEmpty(User)) return false;
            if (string.IsNullOrEmpty(Password)) return false;

            return true;
        }
    }
}
