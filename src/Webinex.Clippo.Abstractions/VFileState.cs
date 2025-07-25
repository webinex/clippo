using System;

namespace Webinex.Clippo;

public class VFileState<TData> : IVFileValue<TData>
    where TData : class, ICloneable
{
    public string? Id { get; }
    public string? Path { get; }
    public string Name { get; }
    public int Bytes { get; }
    public string Ref { get; }
    public string MimeType { get; }
    public TData Data { get; }

    public VFileState(string? id, string? path, string name, int bytes, string @ref, string mimeType, TData? data)
    {
        if (typeof(TData) == typeof(VNone))
            Data = (TData)(object)new VNone();
        else
            Data = data ?? throw new ArgumentNullException(nameof(data));
        
        Id = id;
        Path = path;
        Name = name;
        Bytes = bytes;
        Ref = @ref;
        MimeType = mimeType;
    }
}