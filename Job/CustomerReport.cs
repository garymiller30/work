using MongoDB.Bson;

namespace JobSpace
{
    public class CustomerReport
    {
        /// <summary>
        /// номер замовлення
        /// </summary>
        public string Number { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public PlateFormat PlateFormats { get; set; }
        public int PlateCount { get; set; }
        public string Date { get; set; }
        public ObjectId FormOwner { get; set; }

        public int Brak { get; set; }

        public int PaperProofs { get; set; }

        public Job AssignedJob { get; set; }

       // public string Owner => Managers.PlateOwners.GetOwnerNameById(FormOwner);

        /// <summary>
        /// отримати в квадратних метрах
        /// </summary>
        /// <returns></returns>
        public decimal GetSquareMeter()
        {
            return (decimal) PlateFormats.Width*PlateFormats.Height*PlateCount/1000000;
        }
    }
}
