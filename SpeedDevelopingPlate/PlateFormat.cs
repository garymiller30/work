using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedDevelopingPlate
{
    public class PlateFormat : EventArgs
    {
        public PlateFormat(string manufacturer, string format)
        {
            Manufacturer = manufacturer;
            Format = format;
        }

        public Guid Id { get; }= Guid.NewGuid();

        public string Manufacturer { get; set; }

        public string PlateType { get; set; }
        public string Format { get; set; }

        public List<PlateSpeed> Speeds { get; set; } = new List<PlateSpeed>();

        public void Add(decimal temperature, int speed)
        {
            var s = new PlateSpeed(temperature,speed);
            Speeds.Add(s);

        }

        public PlateSpeed GetLastPlateSpeed()
        {
            return Speeds.LastOrDefault();
        }

        public decimal GetLastTemperature => Speeds.LastOrDefault()?.Temperature ?? 0;
        public int GetLastSpeed => Speeds.LastOrDefault()?.Speed ?? 0;
        public DateTime GetLastDate => Speeds.LastOrDefault()?.ChangeDate ?? DateTime.Now;
        //public decimal GetLastTemperature()
        //{
        //    var o = Speeds.LastOrDefault();
        //    return o?.Temperature ?? 0;
        //}

        //public int GetLastSpeed()
        //{
        //    var o = Speeds.LastOrDefault();
        //    return o?.Speed ?? 0;
        //}

        //public DateTime GetLastDate()
        //{
        //    var o = Speeds.LastOrDefault();
        //    return o?.ChangeDate ?? DateTime.Now;
        //}

    }
}
