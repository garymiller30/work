using System.Drawing;
using Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JobSpace.Statuses
{
    public sealed class JobStatus : IJobStatus, IWithId
    {
        public object Id { get; set; } = ObjectId.GenerateNewId();

        public string Name { get; set; }

        public int Code { get; set; }

        /// <summary>
        /// default status for new orders
        /// </summary>
        public bool IsDefault { get; set; }

        public string ImgBase64 { get; set; }

        [BsonIgnore]
        public Image Img { get; set; }
    }
}
