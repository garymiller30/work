using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace Job
{
    /// <summary>
    /// Оплата наличкой
    /// </summary>
    public class CachePay
    {
        public ObjectId Id { get; set; }
        /// <summary>
        /// выставленная сумма
        /// </summary>
        public decimal PriceDecimal { get; set; }
        /// <summary>
        /// дата высталения
        /// </summary>
        public DateTime PriceDateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// краткое описание назначения оплаты
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// оплаченная сумма
        /// </summary>
        public List<PayedDate> Payed { get; set; } = new List<PayedDate>();
        
    }
}
