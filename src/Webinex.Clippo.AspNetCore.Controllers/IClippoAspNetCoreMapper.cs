using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Webinex.Clippo.AspNetCore;

public interface IClippoAspNetCoreMapper<TMeta, TData>
    where TMeta : class, ICloneable
    where TData : class, ICloneable
{
    Task<IReadOnlyCollection<VFolderDto>> MapAsync(IEnumerable<VFolder<TMeta, TData>> folders);
}