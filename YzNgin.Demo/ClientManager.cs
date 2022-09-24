using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace YzNgin.Demo;

internal class ClientManager : IYzClientManager
{
    private ConcurrentDictionary<Socket, Client> clients;

    public ClientManager()
    {
        clients = new ConcurrentDictionary<Socket, Client>();
    }

    public IEnumerable<Socket> Sockets => clients.Values.Select(c => c.Sock);

    public IYzClient? DropClient(Socket socket)
    {
        Client? client;
        return clients.TryRemove(socket, out client) ? client : null;
    }

    public IYzClient? GetClient(Socket socket)
    {
        Client? client;
        return clients.TryGetValue(socket, out client) ? client : null;
    }

    public IYzClient NewClient(Socket socket)
    {
        Client client = new Client(socket);
        clients.TryAdd(socket, client);
        return client;
    }
}
