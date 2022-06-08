using System;
using MongoDB.Bson;

namespace Job
{
    /// <summary>
    /// оплата 
    /// </summary>
    public class PayedDate
    {
        public ObjectId Id { get; set; }
        /// <summary>
        /// сколько оплачено
        /// </summary>
        public decimal Sum { get; set; }
        /// <summary>
        /// когда оплачено
        /// </summary>
        public DateTime Date { get; set; } = new DateTime();
    }
}
