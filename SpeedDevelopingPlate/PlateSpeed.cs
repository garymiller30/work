using System;

namespace SpeedDevelopingPlate
{
    public class PlateSpeed
    {
        public PlateSpeed(decimal temperature, int speed)
        {
            Temperature = temperature;
            Speed = speed;
            ChangeDate = DateTime.Now;
        }

        public Guid Id { get; } = Guid.NewGuid();
        public decimal Temperature { get; }

        public int Speed { get; }

        public DateTime ChangeDate { get; set; }
    }
}