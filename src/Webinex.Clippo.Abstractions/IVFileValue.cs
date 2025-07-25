using System;

namespace Webinex.Clippo;

public interface IVFileValue<TData>
    where TData : class, ICloneable
{
    public string? Path { get; }
    public string Name { get; }
    public string MimeType { get; }
    public int Bytes { get; }
    public string Ref { get; }
    public TData Data { get; }
}