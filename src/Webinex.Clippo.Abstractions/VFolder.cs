using System;
using System.Collections.Generic;

namespace Webinex.Clippo;

public class VFolder<TMeta, TData>
    where TMeta : class, ICloneable
    where TData : class, ICloneable
{
    public string Id { get; }
    public string Type { get; }
    public string Version { get; }
    public string? Path { get; }
    public IReadOnlyCollection<VFile<TMeta, TData>> Files { get; }

    public VFolder(string type, string id, string version, string? path, IReadOnlyCollection<VFile<TMeta, TData>> files)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Type = type ?? throw new ArgumentNullException(nameof(type));
        Version = version ?? throw new ArgumentNullException(nameof(version));
        Path = path;
        Files = files ?? throw new ArgumentNullException(nameof(files));
    }
}