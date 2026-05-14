using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Webinex.Clippo;

/// <summary>
///     Service which ASP.NET API interacts with.
///     Useful in decoration for a custom pre-/post-processing
/// </summary>
public interface IClippoInteractor<TMeta, TData>
    where TMeta : class, ICloneable
    where TData : class, ICloneable
{
    Task<IReadOnlyCollection<VFolder<TMeta, TData>>> GetAllAsync(VFolderQuery query);
    Task<VFolder<TMeta, TData>> SaveAsync(VFolderState<TData> state);
    Task<VFolder<TMeta, TData>> PatchAsync(VFolderPatch<TData> patch);
}