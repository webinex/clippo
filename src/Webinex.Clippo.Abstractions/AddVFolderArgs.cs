using System;
using System.Collections.Generic;

namespace Webinex.Clippo;

public class AddVFolderArgs<TMeta, TData>
    where TMeta : class, ICloneable
    where TData : class, ICloneable
{
    public AddVFolderArgs(string id, string type, IReadOnlyCollection<AddVFileArgs<TMeta, TData>> files, TMeta? meta = null)
    {
        Id = id;
        Type = type;
        Files = files;
        Meta = meta;
    }

    public string Id { get; }
    public string Type { get; }
    public IReadOnlyCollection<AddVFileArgs<TMeta, TData>> Files { get; }
    public TMeta? Meta { get; }

    public VFolderId VFolderId() => new(Type, Id);
}