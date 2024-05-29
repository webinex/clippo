using System;

namespace Webinex.Clippo.AspNetCore;

public class VFileDto
{
    public string Id { get; }
    public VFolderId Folder { get; }
    public string Name { get; }
    public int Bytes { get; }
    public string Ref { get; }
    public string MimeType { get; }
    public object Meta { get; }
    public object Data { get; }

    public VFileDto(
        string id,
        VFolderId folder,
        string name,
        string mimeType,
        int bytes,
        string @ref,
        object meta,
        object data)
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
    
    public static VFileDto FromFile<TMeta, TData>(VFile<TMeta, TData> file)
        where TMeta : class, ICloneable
        where TData : class, ICloneable
    {
        return new VFileDto(
            file.Id,
            file.Folder,
            file.Name,
            file.MimeType,
            file.Bytes,
            file.Ref,
            file.Meta,
            file.Data
        );
    }
}