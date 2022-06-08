using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PluginAddWorkFromPolymix.Model
{
    public class OrderState
    {
        public int Code { get;set; }
        public string Name { get;set; }
        
        public Image Img { get;set; }
    }
}
