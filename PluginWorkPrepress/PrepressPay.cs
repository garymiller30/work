using System;
using Interfaces;

namespace PluginWorkPrepress
{
    public class PrepressPay : IPay
    {
        public DateTime Date { get; set; } = DateTime.Now;
        public decimal Sum { get; set; }
    }
}
