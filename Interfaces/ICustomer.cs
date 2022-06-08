using System.Collections.Generic;
using MongoDB.Bson;

namespace Interfaces
{
    public interface ICustomer
    {
        ObjectId Id { get; set; }
        string Name { get; set; }

        bool Show { get; set; } 

        List<string> FtpServers { get; set; }
    }
}
