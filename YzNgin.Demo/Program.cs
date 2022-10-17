using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using YzNgin;
using YzNgin.Demo;
using YzNgin.Demo.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(cd =>
    {
        cd.SetBasePath(Directory.GetCurrentDirectory());
        cd.AddEnvironmentVariables(prefix: "YZ_ROBOT_");
    })
    .ConfigureLogging((hc, cl) =>
    {
        cl.AddFile(hc.Configuration.GetSection("LoggingFile"));
        cl.AddConsole();
        cl.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning);
    })
    .ConfigureServices((hc, services) =>
    {
        services.AddSingleton<IYzClientManager>(op => new ClientManager());
        services.AddSingleton<IYzPackCoder>(op => new YzPackJsonCoder());
        services.AddHostedService<TcpService>();
        services.AddHostedService<UdpService>();
    })
    .UseConsoleLifetime()
    .Build();

host.Run();
