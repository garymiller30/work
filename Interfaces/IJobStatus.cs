using System.Drawing;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Interfaces
{
    public interface IJobStatus
    {
        ObjectId Id { get; set; }
        
        string Name { get; set; }
        
        int Code { get; set; }
        
        bool IsDefault { get; set; }
        
        string ImgBase64 { get; set; }
        
        [BsonIgnore]
        Image Img { get; set; }


    }
}
