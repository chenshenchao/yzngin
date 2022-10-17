using System.Net;
using System.Net.Sockets;

namespace YzNgin;

/// <summary>
/// 
/// </summary>
public class YzTcpServer
{

    public event SucceedHandler? Accepted;
    public event FailedHandler? AcceptedFailed;
    public event SucceedHandler? Dispatched;
    public event FailedHandler? DispatchedFailed;


    public string Host { get; init; }
    public int Port { get; init; }
    public IYzPackCoder Coder { get; set; }
    public IYzClientManager Clients { get; set; }

    public YzTcpServer(IYzClientManager clients, IYzPackCoder coder, int port, string host="0.0.0.0")
    {
        Host = host;
        Port = port;
        Coder = coder;
        Clients = clients;
    }

    /// <summary>
    /// 启动服务
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    public async Task ServeAsync(CancellationToken stoppingToken)
    {
        var tcp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        var ipAddress = IPAddress.Parse(Host);
        var ipEndPoint = new IPEndPoint(ipAddress, Port);
        tcp.NoDelay = true;
        tcp.Bind(ipEndPoint);
        tcp.Listen(0);

        await Parallel.ForEachAsync(new List<Task>
        {
            AcceptAsync(tcp, stoppingToken),
            DispatchAsync(stoppingToken),
        }, stoppingToken, async (t, c) => await t);
    }


    /// <summary>
    /// 接受链接
    /// </summary>
    /// <param name="server"></param>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    public async Task AcceptAsync(Socket server, CancellationToken stoppingToken)
    {
        while(!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var socket = await server.AcceptAsync();
                var client = Clients.NewClient(socket);
                Accepted?.Invoke(client);
            }
            catch (Exception e)
            {
                AcceptedFailed?.Invoke(e);
            }
        }
    }

    /// <summary>
    /// 调度接收信息
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    public async Task DispatchAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var sockets = Clients.Sockets.ToList();
                if (sockets.Count > 0)
                {
                    Socket.Select(sockets, null, null, 1000);
                    await Parallel.ForEachAsync(sockets, stoppingToken, OnDispatch);
                }
                else
                {
                    await Task.Yield();
                }
            }
            catch (Exception e)
            {
                DispatchedFailed?.Invoke(e);
            }
        }
    }


    /// <summary>
    /// 调度处理程序
    /// </summary>
    /// <param name="socket"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async ValueTask OnDispatch(Socket socket, CancellationToken cancellationToken)
    {
        try
        {
            var client = Clients.GetClient(socket);
            var m = await client!.ReceiveAsync(async (buffer) =>
            {
                int count = await socket.ReceiveAsync(buffer, SocketFlags.None, cancellationToken);

                return count;
            });
            Dispatched?.Invoke(client);
        }
        catch (Exception e)
        {
            socket.Close();
            Clients.DropClient(socket);
            DispatchedFailed?.Invoke(e);
        }
    }
}