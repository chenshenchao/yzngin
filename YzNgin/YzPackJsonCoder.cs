using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace YzNgin;

public class YzPackJsonCoder : IYzPackCoder
{
    public static readonly byte[] Magic = Encoding.UTF8.GetBytes("YzNJ");
    public static readonly int HeadSize = Magic.Length + 8;
    public string? NameSpacePrefix { get; set; } = null;

    public object DecodeBody(byte[] bytes, string sign)
    {
        var type = Type.GetType(NameSpacePrefix is null ? sign : $"{NameSpacePrefix}.{sign}");
        var text = Encoding.UTF8.GetString(bytes);
        return JsonSerializer.Deserialize(text, type!)!;
    }

    public byte[] EncodeBody(object value, string sign)
    {
        var type = Type.GetType(NameSpacePrefix is null ? sign : $"{NameSpacePrefix}.{sign}");
        var text = JsonSerializer.Serialize(value, type!);
        return Encoding.UTF8.GetBytes(text);
    }
}
