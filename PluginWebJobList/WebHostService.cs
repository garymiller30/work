using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Server.Kestrel;
using Microsoft.AspNetCore.WebSockets;

public class WebHostService
{
    private IWebHost _host;
    private readonly JobUpdateNotifier _notifier;

    public WebHostService(JobUpdateNotifier notifier)
    {
        _notifier = notifier;
    }

    public void Start()
    {
        _host = new WebHostBuilder()
            .UseKestrel() // FIX: Now resolved by correct namespace
            .UseUrls("http://localhost:5000")
            .ConfigureServices(services => { })
            .Configure(app =>
            {
                app.UseWebSockets();

                app.Map("/ws", wsApp =>
                {
                    wsApp.Run(async ctx =>
                    {
                        if (!ctx.WebSockets.IsWebSocketRequest)
                        {
                            ctx.Response.StatusCode = 400;
                            return;
                        }

                        var socket = await ctx.WebSockets.AcceptWebSocketAsync();

                        _notifier.AddClient(socket, "ClientA"); // тест

                        var buffer = new byte[1024];

                        while (socket.State == WebSocketState.Open)
                        {
                            var result = await socket.ReceiveAsync(
                                new ArraySegment<byte>(buffer),
                                System.Threading.CancellationToken.None);

                            if (result.MessageType == WebSocketMessageType.Close)
                                break;
                        }

                        _notifier.RemoveClient(socket);
                    });
                });

                app.Run(async ctx =>
                {
                    await ctx.Response.WriteAsync("Server running");
                });
            })
            .Build();

        _host.Start();
    }

    public void Stop()
    {
        _host?.Dispose();
    }
}