
namespace Interfaces
{
    public interface ICustomerMailNotify
    {
        object Id { get; set; }
        
        bool Enabled { get; set; }
        
        object CustomerId { get; set; }
        int StatusCode { get; set; }
        string Tema { get; set; }
        string Body { get; set; }
        string Email { get; set; }
    }
}
