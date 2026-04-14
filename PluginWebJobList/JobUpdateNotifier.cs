using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using JobSpace;

public class JobUpdateNotifier
{
    private readonly Func<List<Job>> _getJobs;

    // всі підключені клієнти
    private readonly ConcurrentDictionary<WebSocket, string> _clients = new ConcurrentDictionary<WebSocket, string>();

    public JobUpdateNotifier(Func<List<Job>> getJobs)
    {
        _getJobs = getJobs;
    }

    // ➕ підключення клієнта
    public void AddClient(WebSocket socket, string customer)
    {
        _clients[socket] = customer;
    }

    // ➖ відключення
    public void RemoveClient(WebSocket socket)
    {
        _clients.TryRemove(socket, out _);
    }

    // 🚀 головне: викликаєш при зміні jobs
    public async Task NotifyAsync()
    {
        var jobs = _getJobs();

        var tasks = _clients.Select(async pair =>
        {
            var socket = pair.Key;
            var customer = pair.Value;

            if (socket.State != WebSocketState.Open)
                return;

            var filtered = jobs
                .Where(j => j.Customer == customer)
                .Select(j => new
                {
                    j.Number,
                    j.Description,
                    j.StatusCode,
                    j.ProgressValue
                })
                .ToList();

            var json = JsonSerializer.Serialize(filtered);
            var bytes = Encoding.UTF8.GetBytes(json);

            try
            {
                await socket.SendAsync(
                    new ArraySegment<byte>(bytes),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None);
            }
            catch
            {
                RemoveClient(socket);
            }
        });

        await Task.WhenAll(tasks);
    }
}