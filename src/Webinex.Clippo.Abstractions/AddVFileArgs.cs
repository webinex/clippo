using System;

namespace Webinex.Clippo;

public class AddVFileArgs<TMeta, TData>
    where TMeta : class, ICloneable
    where TData : class, ICloneable
{
    public VFileState<TData> State { get; }
    public TMeta? Meta { get; set; }

    public AddVFileArgs(VFileState<TData> state, TMeta? meta = null)
    {
        State = state;
        Meta = meta;
    }
}