using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzNgin;

public class YzPackBuffer
{
    public byte[] Data { get; private set; }
    public int Size { get; private set; }
    public int Capacity { get { return Data.Length; } }
    public int Remain { get { return Capacity - Size; } }

    public YzPackBuffer(byte[] data, int length)
    {
        Data = data;
        Size = length;
    }
    public YzPackBuffer() : this(new byte[1024], 0) { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="write"></param>
    /// <param name="min"></param>
    /// <returns></returns>
    public async ValueTask<int> WriteAsync(Func<Memory<byte>, ValueTask<int>> write, int min = 1024)
    {
        if (Remain <= min)
        {
            var ns = (Capacity + min);
            var nb = new byte[ns];
            Array.Copy(Data, 0, nb, 0, Size);
            Data = nb;
        }
        var r = await write(new Memory<byte>(Data, Size, Remain));
        Size += r;
        return r;
    }
}
