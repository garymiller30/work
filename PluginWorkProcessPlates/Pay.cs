using System;
using Interfaces;

namespace PluginWorkProcessPlates
{
    public sealed class Pay : IPay
    {
        public DateTime Date { get; set; } = DateTime.Now;
        public decimal Sum { get; set; }
    }
}
