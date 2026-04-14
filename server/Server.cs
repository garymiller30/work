using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace server
{
    public class Server
    {
        readonly HashSet<Task> _activeClientTasks = new HashSet<Task>();

        public async  void RunServer(int port)
        {
            var tcpListener = TcpListener.Create(port);
            tcpListener.Start();
           
            while (true)
            {
                var tcpClient = await tcpListener.AcceptTcpClientAsync();
                Console.WriteLine("[Server] Client has connected");
                await processClient(tcpClient); // await не нужен
               
            }
            //await Task.WaitAll(_activeClientTasks); // нужна копия
        }

        async Task processClient(TcpClient c)
        {
            using (var client = new Client(c))
            {
                Task task = null;
                try
                {
                    task = client.ProcessAsync();
                    _activeClientTasks.Add(task);
                    await task;
                }
                finally
                {
                    if (task != null)
                        _activeClientTasks.Remove(task);
                }
            }
        }

       
    }
}
