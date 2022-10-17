using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzNgin.Demo.Services;

public class UdpService : BackgroundService
{
    public ILogger<UdpService> Logger { get; init; }

    public UdpService(IConfiguration configuration, ILogger<UdpService> logger, IYzClientManager clients)
    {
        Logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Logger.LogInformation("Yz udp server start.");
        await Task.Yield();
        Logger.LogInformation("Yz udp server end.");
    }
}
