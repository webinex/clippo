using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

    public async Task<IReadOnlyCollection<VFolder<TMeta, TData>>> GetAllAsync(VFolderQuery query)
    {
        return await _clippo.QueryAsync(query);
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