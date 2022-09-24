using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace YzNgin;

public interface IYzClientManager
{
    public IEnumerable<Socket> Sockets { get; }
    public IYzClient NewClient(Socket socket);
    public IYzClient? DropClient(Socket socket);
    public IYzClient? GetClient(Socket socket);
}
