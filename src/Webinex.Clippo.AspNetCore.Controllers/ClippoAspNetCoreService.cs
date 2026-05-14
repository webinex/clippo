using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Webinex.Asky;

namespace Webinex.Clippo.AspNetCore;

public interface IClippoAspNetCoreService<TMeta, TData>
    where TMeta : class, ICloneable
    where TData : class, ICloneable
{
    Task<IActionResult> GetAllAsync(VFolderId? id, string? path);
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

    public async Task<IActionResult> GetAllAsync(VFolderId? id, string? path)
    {
        var filterRules = new List<FilterRule>();

        if (id != null)
        {
            filterRules.Add(FilterRule.Eq("id", id.Id));
            filterRules.Add(FilterRule.Eq("type", id.Type));
        }

        if (!string.IsNullOrWhiteSpace(path))
            filterRules.Add(FilterRule.StartsWith("path", path));

        var filterRule = CombineRules(filterRules);

        var query = new VFolderQuery(filterRule);

        var result = await _interactor.GetAllAsync(query);
        var dto = await _mapper.MapAsync(result);
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

    private static FilterRule? CombineRules(ICollection<FilterRule> rules)
    {
        return rules.Count switch
        {
            0 => null,
            1 => rules.Single(),
            _ => FilterRule.And(rules),
        };
    }
}