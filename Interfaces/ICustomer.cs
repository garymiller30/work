using System.Collections.Generic;

namespace Interfaces
{
    public interface ICustomer
    {
        object Id { get; set; }
        string Name { get; set; }

        bool Show { get; set; } 

        List<string> FtpServers { get; set; }
    }
}
