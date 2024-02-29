using System;
using System.Collections.Generic;
using System.Linq;

namespace Webinex.Clippo;

public class VFolderPatch<TData>
    where TData : class, ICloneable
{
    public VFolderId Id { get; }
    public Optional<string>? Version { get; }
    public IEnumerable<VFilePatch<TData>>? Files { get; }

    public VFolderPatch(
        VFolderId id,
        Optional<string> version,
        IEnumerable<VFilePatch<TData>>? files)
    {
        files = files?.ToArray();

        if (files != null && files.Where(x => x.Id() != null).GroupBy(x => x.Id()).Any(x => x.Count() > 1))
            throw new ArgumentException("Duplicate file IDs are not allowed.");

        Id = id;
        Version = version;
        Files = files;
    }
}

public class VFilePatch<TData>
    where TData : class, ICloneable
{
    public VFileSetPatch<TData>? Set { get; }
    public VFileDeletePatch? Delete { get; }
    public VFileAddPatch<TData>? Add { get; }

    internal string? Id() => Set?.Id ?? Delete?.Id;

    public VFilePatch(
        VFileSetPatch<TData>? set,
        VFileDeletePatch? delete,
        VFileAddPatch<TData>? add)
    {
        var nonNull = new object?[]
        {
            set,
            delete,
            add
        }.Count(x => x != null);

        if (nonNull != 1)
            throw new ArgumentException("Exactly one of Set, Delete, or Add must be specified.");

        Set = set;
        Delete = delete;
        Add = add;
    }
}

public class VFileDeletePatch
{
    public string Id { get; }

    public VFileDeletePatch(string id)
    {
        Id = id;
    }
}

public class VFileAddPatch<TData>
    where TData : class, ICloneable
{
    public string Name { get; }
    public string MimeType { get; }
    public int Bytes { get; }
    public string Ref { get; }
    public TData Data { get; }

    public VFileAddPatch(
        string? id,
        string name,
        string mimeType,
        int bytes,
        string @ref,
        TData data)
    {
        Name = name;
        MimeType = mimeType;
        Bytes = bytes;
        Ref = @ref;
        Data = data;
    }
}

public class VFileSetPatch<TData>
    where TData : class, ICloneable
{
    public string Id { get; }
    public Optional<string>? Name { get; }
    public Optional<string>? MimeType { get; }
    public Optional<int>? Bytes { get; }
    public Optional<string>? Ref { get; }
    public Optional<TData>? Data { get; }

    public VFileSetPatch(
        string id,
        Optional<string>? name = null,
        Optional<string>? mimeType = null,
        Optional<int>? bytes = null,
        Optional<string>? @ref = null,
        Optional<TData>? data = null)
    {
        Id = id;
        Name = name;
        MimeType = mimeType;
        Bytes = bytes;
        Ref = @ref;
        Data = data;
    }
}