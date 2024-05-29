using System;
using System.Collections.Generic;
using System.Linq;

namespace Webinex.Clippo.AspNetCore;

public class VFolderDto
{
    public string Id { get; }
    public string Type { get; }
    public string Version { get; }
    public IReadOnlyCollection<VFileDto> Files { get; }

    public VFolderDto(string type, string id, string version, IReadOnlyCollection<VFileDto> files)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Type = type ?? throw new ArgumentNullException(nameof(type));
        Version = version ?? throw new ArgumentNullException(nameof(version));
        Files = files ?? throw new ArgumentNullException(nameof(files));
    }

    public static VFolderDto FromFolder<TMeta, TData>(VFolder<TMeta, TData> folder)
        where TMeta : class, ICloneable
        where TData : class, ICloneable
    {
        return new VFolderDto(folder.Type, folder.Id, folder.Version, folder.Files.Select(VFileDto.FromFile).ToArray());
    }
}