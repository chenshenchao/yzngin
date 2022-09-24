using System.Net;
using System.Net.Sockets;

namespace YzNgin;

/// <summary>
/// 
/// </summary>
public class YzServer
{
    public delegate void SucceedHandler(IYzClient client);
    public delegate void FailedHandler(Exception exception);

    public event SucceedHandler? Accepted;
    public event FailedHandler? AcceptedFailed;
    public event SucceedHandler? Dispatched;
    public event FailedHandler? DispatchedFailed;


    public string Host { get; init; }
    public int Port { get; init; }
    public IYzPackCoder Coder { get; set; }
    public IYzClientManager Clients { get; set; }

    public YzServer(IYzClientManager clients, IYzPackCoder coder, int port, string host="0.0.0.0")
    {
        Host = host;
        Port = port;
        Coder = coder;
        Clients = clients;
    }

    public async Task Serve()
    {
        var tcp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        var ipAddress = IPAddress.Parse(Host);
        var ipEndPoint = new IPEndPoint(ipAddress, Port);
        tcp.NoDelay = true;
        tcp.Bind(ipEndPoint);
        tcp.Listen(0);

        await Parallel.ForEachAsync(new List<Task>
        {
            tcp.AcceptAsync(),
            DispatchAsync(),
        }, async (t, c) => await t);
    }

    public async Task AcceptAsync(Socket server)
    {
        while(true)
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

    public async Task DispatchAsync()
    {
        while (true)
        {
            try
            {
                var sockets = Clients.Sockets.ToList();
                if (sockets.Count > 0)
                {
                    Socket.Select(sockets, null, null, 1000);
                    await Parallel.ForEachAsync(sockets, OnDispatch);
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

    public async ValueTask OnDispatch(Socket socket, CancellationToken cancellationToken)
    {
        try
        {
            var client = Clients.GetClient(socket);
            var m = await client!.ReceiveAsync(async (buffer) =>
            {
                int count = await socket.ReceiveAsync(buffer, SocketFlags.None);

                return count;
            });
        }
        catch (Exception e)
        {
            socket.Close();
            Clients.DropClient(socket);
            DispatchedFailed?.Invoke(e);
        }
    }
}