using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Webinex.Asky;
using Webinex.Coded;

namespace Webinex.Clippo;

internal class Clippo<TMeta, TData> : IClippo<TMeta, TData>
    where TMeta : class, ICloneable
    where TData : class, ICloneable
{
    private readonly IClippoDbContext<TMeta, TData> _dbContext;
    private readonly VFolderVRowAskyFieldMap<TMeta, TData> _vFolderVRowFieldMap;
    private readonly VFileVRowAskyFieldMap<TMeta, TData> _vFileVRowFieldMap;
    private readonly IMetaProvider<TMeta, TData> _metaProvider;

    public Clippo(
        IClippoDbContext<TMeta, TData> dbContext,
        VFolderVRowAskyFieldMap<TMeta, TData> vFolderVRowFieldMap,
        VFileVRowAskyFieldMap<TMeta, TData> vFileVRowFieldMap,
        IMetaProvider<TMeta, TData> metaProvider)
    {
        _dbContext = dbContext;
        _vFolderVRowFieldMap = vFolderVRowFieldMap;
        _vFileVRowFieldMap = vFileVRowFieldMap;
        _metaProvider = metaProvider;
    }

    public async Task<IReadOnlyCollection<VFolder<TMeta, TData>>> QueryAsync(VFolderQuery query)
    {
        var rows = await RowsQueryable(query, track: false).AsNoTracking().ToArrayAsync();
        return Map(rows);
    }

    private IQueryable<VRow<TMeta, TData>> RowsQueryable(VFolderQuery query, bool track)
    {
        var queryable = _dbContext.Set<VRow<TMeta, TData>>().AsQueryable();

        if (!track)
            queryable = queryable.AsNoTracking();

        if (query.FilterRule != null)
            queryable = queryable.Where(_vFolderVRowFieldMap, query.FilterRule);

        if (query.SortRule != null)
            queryable = queryable.SortBy(_vFolderVRowFieldMap, query.SortRule);

        if (query.PagingRule != null)
            queryable = queryable.PageBy(query.PagingRule);

        return queryable;
    }

    private IQueryable<VRow<TMeta, TData>> RowsQueryableById(IEnumerable<VFolderId> ids, bool track)
    {
        ids = ids.ToArray();

        var byIdFilters = ids.Select(
            x => FilterRule.And(
                FilterRule.Eq("id", x.Id),
                FilterRule.Eq("type", x.Type))).ToArray();
        
        var filterRule = byIdFilters.Length > 1
            ? FilterRule.Or(byIdFilters)
            : byIdFilters.Single();

        return RowsQueryable(new VFolderQuery(filterRule: filterRule), track);
    }

    private VFolder<TMeta, TData>[] Map(VRow<TMeta, TData>[] rows)
    {
        var byId = rows.GroupBy(x => x.Folder.Clone());
        return byId.Select(MapFolder).ToArray();
    }

    private VFolder<TMeta, TData> MapFolder(IEnumerable<VRow<TMeta, TData>> rows)
    {
        rows = rows.ToArray();
        var folderRow = rows.First(r => r.Type == VRowType.Folder);
        var fileRows = rows.Where(r => r.Type == VRowType.File).ToArray();
        var files = fileRows.Select(MapFile).ToArray();

        return new VFolder<TMeta, TData>(folderRow.Folder.Type, folderRow.Folder.Id, folderRow.Version, files);
    }

    private VFile<TMeta, TData> MapFile(VRow<TMeta, TData> x)
    {
        return new VFile<TMeta, TData>(
            x.Id,
            x.Folder.Clone(),
            x.Name!,
            x.MimeType!,
            x.Bytes!.Value,
            x.Ref!,
            x.Meta,
            x.Data!);
    }

    public async Task<IReadOnlyCollection<VFile<TMeta, TData>>> QueryAsync(VFileQuery query)
    {
        var queryable = _dbContext.Set<VRow<TMeta, TData>>()
            .Where(x => x.Type == VRowType.File)
            .AsNoTracking();

        if (query.FilterRule != null)
            queryable = queryable.Where(_vFileVRowFieldMap, query.FilterRule);

        if (query.SortRule != null)
            queryable = queryable.SortBy(_vFileVRowFieldMap, query.SortRule);

        if (query.PagingRule != null)
            queryable = queryable.PageBy(query.PagingRule);

        var rows = await queryable.ToArrayAsync();
        return rows.Select(MapFile).ToArray();
    }

    public async Task<IReadOnlyCollection<VFile<TMeta, TData>>> FilesByIdAsync(
        IEnumerable<string> ids)
    {
        ids = ids.Distinct().ToArray();
        if (!ids.Any()) return Array.Empty<VFile<TMeta, TData>>();

        var rows = await _dbContext
            .Set<VRow<TMeta, TData>>()
            .Where(x => ids.Contains(x.Id))
            .ToArrayAsync();

        return rows.Select(MapFile).ToArray();
    }

    public async Task<IReadOnlyCollection<VFolder<TMeta, TData>>> FoldersByIdAsync(
        IEnumerable<VFolderId> ids)
    {
        ids = ids.Distinct().ToArray();
        if (!ids.Any()) return Array.Empty<VFolder<TMeta, TData>>();

        var result = await RowsQueryableById(ids, track: false).AsNoTracking().ToArrayAsync();
        return Map(result);
    }

    public Task<IReadOnlyCollection<VFolder<TMeta, TData>>> PatchAsync(IEnumerable<VFolderPatch<TData>> patches)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<VFile<TMeta, TData>>> PatchAsync(IEnumerable<VFilePatch<TData>> patches)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyCollection<VFolder<TMeta, TData>>> AddAsync(IEnumerable<VFolderState<TData>> states)
    {
        states = states.ToArray();
        var meta = await _metaProvider.GetAsync();
        return await AddAsync(states, meta);
    }

    private async Task<IReadOnlyCollection<VFolder<TMeta, TData>>> AddAsync(
        IEnumerable<VFolderState<TData>> states,
        TMeta meta)
    {
        var rows = states.SelectMany(x => x.ToRows(meta)).ToArray();
        await _dbContext.Set<VRow<TMeta, TData>>().AddRangeAsync(rows);
        return Map(rows);
    }

    public async Task<IReadOnlyCollection<VFolder<TMeta, TData>>> UpdateAsync(IEnumerable<VFolderState<TData>> states)
    {
        states = states.ToArray();
        var ids = states.Select(x => new VFolderId(x.Type, x.Id)).Distinct().ToArray();
        var rows = await RowsQueryableById(ids, track: true).ToArrayAsync();
        var meta = await _metaProvider.GetAsync();
        return await UpdateAsync(states, rows, meta);
    }

    private async Task<IReadOnlyCollection<VFolder<TMeta, TData>>> UpdateAsync(
        IEnumerable<VFolderState<TData>> states,
        VRow<TMeta, TData>[] rows,
        TMeta meta)
    {
        states = states.ToArray();
        var byFolderId = rows.GroupBy(x => x.Folder.Clone());

        var result = new LinkedList<VFolder<TMeta, TData>>();

        foreach (var group in byFolderId)
        {
            var folderState = states.First(x => x.Id == group.Key.Id && x.Type == group.Key.Type);
            result.AddLast(await UpdateAsync(group.ToArray(), folderState, meta));
        }

        return result;
    }

    public async Task<IReadOnlyCollection<VFolder<TMeta, TData>>> SaveAsync(IEnumerable<VFolderState<TData>> states)
    {
        states = states.ToArray();
        var ids = states.Select(x => new VFolderId(x.Type, x.Id)).Distinct().ToArray();
        var rows = await RowsQueryableById(ids, track: true).ToArrayAsync();
        var vFolderIdInRows = rows.Select(x => x.Folder.Clone()).Distinct().ToArray();
        var meta = await _metaProvider.GetAsync();
        
        var @new = states.Where(x => !vFolderIdInRows.Contains(x.VFolderId())).ToArray();
        var created = await AddAsync(@new, meta);

        var update = states.Except(@new).ToArray();
        var updated = await UpdateAsync(update, rows, meta);
        return created.Concat(updated).ToArray();
    }

    private async Task<VFolder<TMeta, TData>> UpdateAsync(VRow<TMeta, TData>[] rows, VFolderState<TData> state, TMeta meta)
    {
        var folderRow = rows.First(r => r.Type == VRowType.Folder);
        var fileRows = rows.Where(r => r.Type == VRowType.File).ToArray();

        if (state.Version?.HasValue == true && folderRow.Version != state.Version.Value)
            throw CodedException.Conflict();

        var newFiles = state.Files.Where(x => x.Id == null).ToArray();
        var newFileRows = newFiles.Select(x => x.ToRow(folderRow.Folder, meta)).ToArray();
        await _dbContext.Set<VRow<TMeta, TData>>().AddRangeAsync(newFileRows);

        var deleteFileRows = fileRows.Where(x => state.Files.All(sf => sf.Id != x.Id)).ToArray();
        _dbContext.Set<VRow<TMeta, TData>>().RemoveRange(deleteFileRows);

        var updateFileRows = fileRows.Where(x => state.Files.Any(sf => sf.Id == x.Id)).ToArray();

        foreach (var updateFileRow in updateFileRows)
        {
            var fState = state.Files.First(x => x.Id == updateFileRow.Id);
            updateFileRow.UpdateFile(fState.Name, fState.Bytes, fState.Ref, fState.MimeType, fState.Data);
        }

        var result = new[] { folderRow }.Concat(newFileRows).Concat(updateFileRows).ToArray();
        return MapFolder(result);
    }

    public async Task<IReadOnlyCollection<VFolder<TMeta, TData>>> DeleteFoldersAsync(IEnumerable<VFolderId> ids)
    {
        var rows = await RowsQueryableById(ids, track: true).ToArrayAsync();
        _dbContext.Set<VRow<TMeta, TData>>().RemoveRange(rows);
        return Map(rows);
    }

    public async Task<IReadOnlyCollection<VFolder<TMeta, TData>>> MoveAsync(IEnumerable<MoveVFolderArgs> args)
    {
        args = args.Distinct().ToArray();
        var ids = args.Select(x => x.FromId).ToArray();
        var rows = await RowsQueryableById(ids, track: true).ToArrayAsync();

        foreach (var group in rows.GroupBy(x => x.Folder.Clone()))
        {
            var arg = args.First(x => x.FromId == group.Key);
            var @new = group.Select(x => x.Move(arg.ToId)).ToArray();
            await _dbContext.Set<VRow<TMeta, TData>>().AddRangeAsync(@new);
            _dbContext.Set<VRow<TMeta, TData>>().RemoveRange(group.ToArray());
        }

        return Map(rows);
    }
}