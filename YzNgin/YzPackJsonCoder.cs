using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzNgin;

public class YzPackJsonCoder : IYzPackCoder
{
    public static readonly byte[] Magic = Encoding.UTF8.GetBytes("YzNJ");
    public static readonly int HeadSize = Magic.Length + 8;

    public object DecodeBody(byte[] bytes, string sign)
    {
        
    }

    public byte[] EncodeBody(object value, string sign)
    {
        
    }
}
