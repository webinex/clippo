using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Webinex.Clippo.AspNetCore;

public interface IClippoAspNetCoreService<TMeta, TData>
    where TMeta : class, ICloneable
    where TData : class, ICloneable
{
    Task<IActionResult> ByIdAsync(VFolderId id);
    Task<IActionResult> SaveAsync(VFolderState<TData> state);
    Task<IActionResult> PatchAsync(VFolderPatch<TData> patch);
}

internal class ClippoAspNetCoreService<TMeta, TData> : IClippoAspNetCoreService<TMeta, TData>
    where TMeta : class, ICloneable
    where TData : class, ICloneable
{
    private readonly IClippoInteractor<TMeta, TData> _interactor;
    private readonly IClippoAspNetCoreMapper<TMeta, TData> _mapper;

    public ClippoAspNetCoreService(
        IClippoInteractor<TMeta, TData> interactor,
        IClippoAspNetCoreMapper<TMeta, TData> mapper)
    {
        _interactor = interactor;
        _mapper = mapper;
    }

    public async Task<IActionResult> ByIdAsync(VFolderId id)
    {
        var result = await _interactor.ByIdAsync(id);
        var dto = result != null
            ? await _mapper.MapAsync(result)
            : null;
        return new OkObjectResult(dto);
    }

    public async Task<IActionResult> SaveAsync(VFolderState<TData> state)
    {
        var result = await _interactor.SaveAsync(state);
        var dto = await _mapper.MapAsync(result);
        return new OkObjectResult(dto);
    }

    public async Task<IActionResult> PatchAsync(VFolderPatch<TData> patch)
    {
        var result = await _interactor.PatchAsync(patch);
        var dto = await _mapper.MapAsync(result);
        return new OkObjectResult(dto);
    }
}