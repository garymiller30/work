using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Job
{
    [Serializable]
    [BsonDiscriminator("Jobs")]
    public class Job : IJob
    {
        public ObjectId Id { get; set; }

        [Obsolete]
        public Settings.JobStatus Status { get; set; }

        public int StatusCode { get; set; }
        /// <summary>
        /// дата створення замовлення
        /// </summary>
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime Date { get; set; }
        public string Number { get; set; }
        public string Customer { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Note { get; set; }
        /// <summary>
        /// previously order number
        /// </summary>
        public string PreviousOrder { get; set; }
        /// <summary>
        /// job is busy
        /// </summary>
        public bool IsJobInProcess { get; set; }

        public object GetJob()
        {
            return this;
        }

        [BsonIgnore]
        public int ProgressValue { get;set; }

        [BsonIgnore]
        public object Tag { get; set; }

        public bool UseCustomFolder { get; set; }
        [Obsolete]
        public List<Part> Parts { get; set; } = new List<Part>();
        
        public string Folder { get; set; }
        [Obsolete] public bool IsCashe { get; set; }
        [Obsolete] public bool IsCashePayed { get; set; }
        [Obsolete] public decimal CachePayedSum { get; set; }
        public bool DontCreateFolder { get; set; }

        public ObjectId CategoryId { get; set; }


        public Job()
        {
            Date = DateTime.UtcNow;
            Id = ObjectId.GenerateNewId();
            Number = $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}";
        }

        

        public override int GetHashCode()
        {
            return Id.ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is IJob job)
            {
                return job.Id.Equals(Id);
            }
            return false;
        }
    }
}
