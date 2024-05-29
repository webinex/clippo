using System;
using System.Linq;
using System.Threading.Tasks;

namespace Webinex.Clippo.AspNetCore;

public static class ClippoAspNetCoreMapperExtensions
{
    public static async Task<VFolderDto> MapAsync<TMeta, TData>(this IClippoAspNetCoreMapper<TMeta, TData> mapper, VFolder<TMeta, TData> folder)
        where TMeta : class, ICloneable
        where TData : class, ICloneable
    {
        var result = await mapper.MapAsync(new[] { folder });
        return result.Single();
    }
}