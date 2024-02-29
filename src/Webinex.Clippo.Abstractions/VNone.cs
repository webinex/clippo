using System;

namespace Webinex.Clippo;

public class VNone : ICloneable
{
    public object Clone()
    {
        return new VNone();
    }
}