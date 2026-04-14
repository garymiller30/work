using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace server
{
    class Program
    {
        static void Main(string[] args)
        {
            var _server = new Server();

            _server.RunServer(23030);

        }
    }
}
