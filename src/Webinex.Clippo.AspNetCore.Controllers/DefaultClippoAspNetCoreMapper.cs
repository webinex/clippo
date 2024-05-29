using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webinex.Clippo.AspNetCore;

public class DefaultClippoAspNetCoreMapper<TMeta, TData> : IClippoAspNetCoreMapper<TMeta, TData>
    where TMeta : class, ICloneable
    where TData : class, ICloneable
{
    public Task<IReadOnlyCollection<VFolderDto>> MapAsync(IEnumerable<VFolder<TMeta, TData>> folders)
    {
        var result = folders.Select(VFolderDto.FromFolder).ToArray();
        return Task.FromResult<IReadOnlyCollection<VFolderDto>>(result);
    }
}