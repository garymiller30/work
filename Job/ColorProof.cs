using System;
using Interfaces;
using MongoDB.Bson;

namespace Job
{
    public class ColorProof: IWithId
    {
        public ObjectId Id { get; set; }
        public string Customer { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }

        public DateTime Date { get; set; }
        public bool Payed { get; set; }

        public string Description { get; set; }

        public ColorProof()
        {
            Id = ObjectId.GenerateNewId();
            Date = DateTime.Now;
        }
    }
}
