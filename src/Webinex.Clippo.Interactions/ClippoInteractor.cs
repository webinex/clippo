using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Webinex.Asky;

namespace Webinex.Clippo;

public class ClippoInteractor<TMeta, TData> : IClippoInteractor<TMeta, TData> where TMeta : class, ICloneable where TData : class, ICloneable
{
    private readonly IClippo<TMeta, TData> _clippo;
    private readonly IClippoDbContext<TMeta, TData> _dbContext;

    public ClippoInteractor(IClippo<TMeta, TData> clippo, IClippoDbContext<TMeta, TData> dbContext)
    {
        _clippo = clippo;
        _dbContext = dbContext;
    }

    public async Task<VFolder<TMeta, TData>?> ByIdAsync(VFolderId id)
    {
        return await _clippo.FolderByIdAsync(id);
    }

    public async Task<IReadOnlyCollection<VFolder<TMeta, TData>>> ByPathAsync(string path)
    {
        var filterRule = FilterRule.StartsWith("path", path);
        return await _clippo.QueryAsync(new VFolderQuery(filterRule));
    }

    public async Task<VFolder<TMeta, TData>> SaveAsync(VFolderState<TData> state)
    {
        var result = await _clippo.SaveAsync(state);
        await _dbContext.SaveChangesAsync();
        return result;
    }

    public async Task<VFolder<TMeta, TData>> PatchAsync(VFolderPatch<TData> patch)
    {
        var result = await _clippo.PatchAsync(patch);
        await _dbContext.SaveChangesAsync();
        return result;
    }
}