using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzNgin;

public interface IYzPackCoder
{
    public object DecodeBody(byte[] bytes, string sign);
    public byte[] EncodeBody(object value, string sign);
}
