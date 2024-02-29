using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webinex.Clippo;

public interface IClippo<TMeta, TData>
    where TMeta : class, ICloneable
    where TData : class, ICloneable
{
    Task<IReadOnlyCollection<VFile<TMeta, TData>>> FilesByIdAsync(IEnumerable<string> ids);
    Task<IReadOnlyCollection<VFolder<TMeta, TData>>> FoldersByIdAsync(IEnumerable<VFolderId> ids);
    Task<IReadOnlyCollection<VFolder<TMeta, TData>>> QueryAsync(VFolderQuery query);
    Task<IReadOnlyCollection<VFile<TMeta, TData>>> QueryAsync(VFileQuery query);
    Task<IReadOnlyCollection<VFolder<TMeta, TData>>> PatchAsync(IEnumerable<VFolderPatch<TData>> patches);
    Task<IReadOnlyCollection<VFile<TMeta, TData>>> PatchAsync(IEnumerable<VFilePatch<TData>> patches);
    Task<IReadOnlyCollection<VFolder<TMeta, TData>>> AddAsync(IEnumerable<VFolderState<TData>> states);
    Task<IReadOnlyCollection<VFolder<TMeta, TData>>> UpdateAsync(IEnumerable<VFolderState<TData>> states);
    Task<IReadOnlyCollection<VFolder<TMeta, TData>>> SaveAsync(IEnumerable<VFolderState<TData>> states);
    Task<IReadOnlyCollection<VFolder<TMeta, TData>>> DeleteFoldersAsync(IEnumerable<VFolderId> ids);
    Task<IReadOnlyCollection<VFolder<TMeta, TData>>> MoveAsync(IEnumerable<MoveVFolderArgs> args);
}

public static class ClippoExtensions
{
    public static async Task<VFolder<TMeta, TData>?> FolderByIdAsync<TMeta, TData>(
        this IClippo<TMeta, TData> clippo,
        string type,
        string id)
        where TMeta : class, ICloneable
        where TData : class, ICloneable
    {
        var folderId = new VFolderId(id, type);
        return await clippo.FolderByIdAsync(folderId);
    }

    public static async Task<VFolder<TMeta, TData>?> FolderByIdAsync<TMeta, TData>(
        this IClippo<TMeta, TData> clippo,
        VFolderId id)
        where TMeta : class, ICloneable
        where TData : class, ICloneable
    {
        var result = await clippo.FoldersByIdAsync(new[] { id });
        return result.FirstOrDefault();
    }

    public static async Task<VFolder<TMeta, TData>> SaveAsync<TMeta, TData>(
        this IClippo<TMeta, TData> clippo,
        VFolderState<TData> state)
        where TMeta : class, ICloneable
        where TData : class, ICloneable
    {
        var result = await clippo.SaveAsync(new[] { state });
        return result.First();
    }

    public static async Task<VFolder<TMeta, TData>> PatchAsync<TMeta, TData>(
        this IClippo<TMeta, TData> clippo,
        VFolderPatch<TData> state)
        where TMeta : class, ICloneable
        where TData : class, ICloneable
    {
        var result = await clippo.PatchAsync(new[] { state });
        return result.First();
    }

    public static async Task<VFolder<TMeta, TData>> AddAsync<TMeta, TData>(
        this IClippo<TMeta, TData> clippo,
        VFolderState<TData> state)
        where TMeta : class, ICloneable
        where TData : class, ICloneable
    {
        var result = await clippo.AddAsync(new[] { state });
        return result.First();
    }

    public static async Task<VFolder<TMeta, TData>> UpdateAsync<TMeta, TData>(
        this IClippo<TMeta, TData> clippo,
        VFolderState<TData> state)
        where TMeta : class, ICloneable
        where TData : class, ICloneable
    {
        var result = await clippo.UpdateAsync(new[] { state });
        return result.First();
    }
}