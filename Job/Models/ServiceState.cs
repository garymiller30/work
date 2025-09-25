using Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Models
{
    public sealed class ServiceState : IServiceState
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string Tooltip { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Image Image { get; set; } = null;
    }
}
