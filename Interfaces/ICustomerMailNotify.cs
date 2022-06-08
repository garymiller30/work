using MongoDB.Bson;

namespace Interfaces
{
    public interface ICustomerMailNotify
    {
        ObjectId Id { get; set; }
        
        bool Enabled { get; set; }
        
        ObjectId CustomerId { get; set; }
        int StatusCode { get; set; }
        string Tema { get; set; }
        string Body { get; set; }
        string Email { get; set; }
    }
}
