using System;

namespace Webinex.Clippo;

public class AddVFileArgs<TMeta, TData> : IVFileValue<TData>
    where TMeta : class, ICloneable
    where TData : class, ICloneable
{
    public string? Id { get; }
    public string Name { get; }
    public int Bytes { get; }
    public string Ref { get; }
    public string MimeType { get; }
    public TData Data { get; }
    public TMeta? Meta { get; }

    public AddVFileArgs(string? id, string name, int bytes, string @ref, string mimeType, TData? data, TMeta? meta = null)
    {
        if (typeof(TData) == typeof(VNone))
            Data = (TData)(object)new VNone();
        else
            Data = data ?? throw new ArgumentNullException(nameof(data));

        Id = id;
        Name = name;
        Ref = @ref;
        MimeType = mimeType;
        Bytes = bytes;
        Meta = meta;
    }
}