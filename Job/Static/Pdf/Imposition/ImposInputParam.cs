using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Static.Pdf.Imposition
{
    public class ImposInputParam
    {
        public List<string> Files { get; set; } = new List<string>();
        public string JobFolder { get; set; } = string.Empty;
        public IUserProfile UserProfile { get; set; }
    }
}
