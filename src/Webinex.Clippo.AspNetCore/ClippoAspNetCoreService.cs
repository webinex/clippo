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
    private readonly IClippo<TMeta, TData> _clippo;
    private readonly IClippoDbContext<TMeta, TData> _dbContext;

    public ClippoAspNetCoreService(IClippo<TMeta, TData> clippo, IClippoDbContext<TMeta, TData> dbContext)
    {
        _clippo = clippo;
        _dbContext = dbContext;
    }

    public async Task<IActionResult> ByIdAsync(VFolderId id)
    {
        var result = await _clippo.FolderByIdAsync(id);
        return new OkObjectResult(result);
    }

    public async Task<IActionResult> SaveAsync(VFolderState<TData> state)
    {
        var result = await _clippo.SaveAsync(state);
        await _dbContext.SaveChangesAsync();
        return new OkObjectResult(result);
    }

    public async Task<IActionResult> PatchAsync(VFolderPatch<TData> patch)
    {
        var result = await _clippo.PatchAsync(patch);
        await _dbContext.SaveChangesAsync();
        return new OkObjectResult(result);
    }
}