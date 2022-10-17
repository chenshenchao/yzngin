using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzNgin.Demo.Services;

public class TcpService : BackgroundService
{
    public YzTcpServer Server { get; init; }
    public ILogger<TcpService> Logger { get; init; }

    public TcpService(
        IConfiguration configuration,
        ILogger<TcpService> logger,
        IYzPackCoder coder,
        IYzClientManager clients
    )
    {
        int port = configuration.GetValue<int>("Tcp:Port");
        Logger = logger;
        Server = new YzTcpServer(clients, coder, port);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Logger.LogInformation("Yz tcp server start.");
        await Server.ServeAsync(stoppingToken);
        Logger.LogInformation("Yz tcp server end.");
    }
}
