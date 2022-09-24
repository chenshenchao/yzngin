using Microsoft.Extensions.DependencyInjection;
using YzNgin;
using YzNgin.Demo;

var ioc = new ServiceCollection();
ioc.AddSingleton<IYzClientManager>(op =>
{
    return new ClientManager();
});

ioc.AddSingleton(op =>
{
    var cm = op.GetRequiredService<IYzClientManager>();
    return new YzServer(cm, null, 44444);
});

var provider = ioc.BuildServiceProvider();
var server = provider.GetRequiredService<YzServer>();
server.Serve().Wait();
