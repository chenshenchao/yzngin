using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzNgin;

public class YzPack
{
    public int SignSize { get; init; }
    public int BodySize { get; init; }
    public string Sign { get; init; }
    public object Body { get; init; }

    public YzPack(int signSize, int bodySize, string sign, object body)
    {
        SignSize = signSize;
        BodySize = bodySize;
        Sign = sign;
        Body = body;
    }
}
