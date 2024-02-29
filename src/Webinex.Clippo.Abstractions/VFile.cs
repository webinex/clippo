using System;
using System.IO;

namespace Webinex.Clippo;

public class VFile<TMeta, TData>
    where TMeta : class, ICloneable
    where TData : class, ICloneable
{
    public string Id { get; }
    public VFolderId Folder { get; }
    public string Name { get; }
    public int Bytes { get; }
    public string Ref { get; }
    public string MimeType { get; }
    public TMeta Meta { get; }
    public TData Data { get; }

    public VFile(
        string id,
        VFolderId folder,
        string name,
        string mimeType,
        int bytes,
        string @ref,
        TMeta meta,
        TData data)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Bytes = bytes;
        Ref = @ref ?? throw new ArgumentNullException(nameof(@ref));
        Meta = meta ?? throw new ArgumentNullException(nameof(meta));
        Data = data ?? throw new ArgumentNullException(nameof(data));
        Folder = folder;
        MimeType = mimeType ?? throw new ArgumentNullException(nameof(mimeType));
    }
}