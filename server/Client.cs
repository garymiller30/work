using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace server
{
    public class Client : IDisposable
    {
        private readonly NetworkStream _s;

        public Client(TcpClient c)
        {
            _s = c.GetStream();
        }

        public void Dispose()
        {
            _s.Dispose();
        }

        async Task<byte[]> ReadFromStreamAsync(int nbytes)
        {
            var buf = new byte[nbytes];
            var readpos = 0;
            while (readpos < nbytes)
                readpos += await _s.ReadAsync(buf, readpos, nbytes - readpos);
            return buf;
        }

        public async Task ProcessAsync()
        {
            //var actionBuffer = await ReadFromStreamAsync(2);
            //var action = (ActionEnum)BitConverter.ToInt16(actionBuffer, 0);
            //switch (action)
            //{
            //    // логика в зависимости от кода команды
            //    case ActionEnum.Connected:
            //        Console.WriteLine("connected");
            //        break;
            //    case ActionEnum.Disconected:
            //        Console.WriteLine("disconnected");
            //        break;
            //    default:
            //        throw new ArgumentOutOfRangeException();
            //}
        }

        async Task<byte[]> ReadFromStreamAsync(int nbytes, CancellationToken ct)
        {
            var buf = new byte[nbytes];
            var readpos = 0;
            while (readpos < nbytes)
                readpos += await _s.ReadAsync(buf, readpos, nbytes - readpos, ct);
            return buf;
        }

        async Task<string> ReadWithTimeout(int n)
        {
            using (var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000)))
            {
                try
                {
                    return Decode(await ReadFromStreamAsync(n, cts.Token));
                }
                catch (OperationCanceledException)
                {
                    return null;
                }
            }
        }

        private string Decode(byte[] bytes)
        {
            return string.Empty;
        }
    }
}
