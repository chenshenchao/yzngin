using System;
using System.Net.Sockets;

namespace YzNgin.Demo;

public class Client : IYzClient
{
    private Socket sock;

    public Socket Sock => sock;

    public Client(Socket socket)
    {
        sock = socket;
    }

    public async Task<object?> ReceiveAsync(Func<Memory<byte>, ValueTask<int>> write)
    {
        await Task.Yield();
        return null;
    }
}
