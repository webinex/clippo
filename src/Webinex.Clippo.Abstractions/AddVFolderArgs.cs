using System;
using System.Collections.Generic;

namespace Webinex.Clippo;

public class AddVFolderArgs<TMeta, TData>
    where TMeta : class, ICloneable
    where TData : class, ICloneable
{
    public AddVFolderArgs(string id, string type, IEnumerable<AddVFileArgs<TMeta, TData>> files)
    {
        Id = id;
        Type = type;
        Files = files;
    }

    public string Id { get; }
    public string Type { get; }
    public IEnumerable<AddVFileArgs<TMeta, TData>> Files { get; }

    public VFolderId VFolderId() => new(Type, Id);
}