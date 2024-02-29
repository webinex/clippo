using System;
using System.Collections.Generic;

namespace Webinex.Clippo;

public class VFolderState<TData>
    where TData : class, ICloneable
{
    public string Id { get; protected set; }
    public string Type { get; protected set; }
    public Optional<string>? Version { get; protected set; }
    public IEnumerable<VFileState<TData>> Files { get; protected set; }

    public VFolderId VFolderId() => new(Type, Id);

    public VFolderState(string id, string type, Optional<string>? version, IEnumerable<VFileState<TData>> files)
    {
        Id = id;
        Type = type;
        Files = files;
        Version = version;
    }

    protected VFolderState(VFolderState<TData> value)
    {
        Id = value.Id;
        Type = value.Type;
        Files = value.Files;
        Version = value.Version;
    }

    public VFolderState<TData> WithId(VFolderId id)
    {
        return new VFolderState<TData>(this)
        {
            Id = id.Id,
            Type = id.Type,
        };
    }

    public VFolderState<TData> WithId(string type, string id)
    {
        return WithId(new VFolderId(type, id));
    }
}