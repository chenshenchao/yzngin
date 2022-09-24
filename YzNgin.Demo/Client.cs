using System;
using System.Net.Sockets;

namespace YzNgin.Demo;

internal class Client : IYzClient
{
    private Socket sock;

    public Socket Sock => sock;

    public Client(Socket socket)
    {
        sock = socket;
    }

    public Task<object?> ReceiveAsync(Func<Memory<byte>, ValueTask<int>> write)
    {
        throw new NotImplementedException();
    }
}
