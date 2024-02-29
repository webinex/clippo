using System;

namespace Webinex.Clippo;

internal class VRow<TMeta, TData>
    where TMeta : class, ICloneable
    where TData : class, ICloneable
{
    public string Id { get; protected init; } = null!;
    public VRowType Type { get; protected init; }
    public VFolderId Folder { get; protected set; } = null!;
    public string Version { get; protected set; } = null!;
    public string? Name { get; protected set; }
    public int? Bytes { get; protected set; }
    public string? Ref { get; protected set; }
    public string? MimeType { get; protected set; }
    public TMeta Meta { get; protected set; } = typeof(TMeta) == typeof(VNone) ? (TMeta)(object)new VNone() : null!;
    public TData? Data { get; protected set; } = typeof(TData) == typeof(VNone)
        ? (TData)(object)new VNone()
        : null!;

    protected VRow()
    {
    }

    protected VRow(VRow<TMeta, TData> value)
    {
        Id = value.Id;
        Type = value.Type;
        Folder = value.Folder.Clone();
        Version = value.Version;
        Name = value.Name;
        Bytes = value.Bytes;
        Ref = value.Ref;
        MimeType = value.MimeType;
        Meta = (TMeta)value.Meta.Clone();
        Data = (TData?)value.Data?.Clone();
    }

    public static VRow<TMeta, TData> NewFolder(
        string id,
        string type,
        TMeta meta)
    {
        var vFolderId = new VFolderId(type, id);
        return new VRow<TMeta, TData>
        {
            Id = vFolderId.ToString(),
            Type = VRowType.Folder,
            Version = Guid.NewGuid().ToString(),
            Folder = vFolderId,
            Meta = (TMeta)meta.Clone(),
        };
    }

    public static VRow<TMeta, TData> NewFile(
        VFolderId vFolder,
        string name,
        string mimeType,
        int bytes,
        string @ref,
        TMeta meta,
        TData data)
    {
        return new VRow<TMeta, TData>
        {
            Id = vFolder + "::" + Guid.NewGuid(),
            Version = Guid.NewGuid().ToString(),
            Type = VRowType.File,
            Folder = vFolder.Clone(),
            Name = name,
            Bytes = bytes,
            MimeType = mimeType,
            Ref = @ref,
            Meta = (TMeta)meta.Clone(),
            Data = (TData)data.Clone(),
        };
    }

    public void UpdateFile(string name, int bytes, string @ref, string mimeType, TData data)
    {
        Name = name;
        Bytes = bytes;
        Ref = @ref;
        MimeType = mimeType;
        Data = (TData)data.Clone();
        Version = Guid.NewGuid().ToString();
    }

    public VRow<TMeta, TData> Move(VFolderId id)
    {
        if (Type == VRowType.Folder)
        {
            return new VRow<TMeta, TData>(this)
            {
                Id = id.ToString(),
                Folder = id.Clone(),
                Version = Guid.NewGuid().ToString(),
            };
        }

        return new VRow<TMeta, TData>(this)
        {
            Id = id + "::" + Guid.NewGuid(),
            Folder = id.Clone(),
            Version = Guid.NewGuid().ToString(),
        };
    }
}