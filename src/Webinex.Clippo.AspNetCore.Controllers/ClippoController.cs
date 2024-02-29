using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Webinex.Clippo.AspNetCore;

[Route("/api/clippo")]
public class ClippoController<TMeta, TData> : ControllerBase
    where TMeta : class, ICloneable
    where TData : class, ICloneable
{
    private readonly IClippoAspNetCoreSettings _settings;
    private readonly IClippoAspNetCoreService<TMeta, TData> _clippoAspNetCore;

    public ClippoController(
        IClippoAspNetCoreSettings settings,
        IClippoAspNetCoreService<TMeta, TData> clippoAspNetCore)
    {
        _settings = settings;
        _clippoAspNetCore = clippoAspNetCore;
    }

    [HttpGet("{type}/{id}")]
    public async Task<IActionResult> ByIdAsync([Required] string type, [Required] string id)
    {
        if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(type))
            return BadRequest();

        if (!await AuthorizeAsync())
            return Forbid();

        return await _clippoAspNetCore.ByIdAsync(new VFolderId(type, id));
    }

    [HttpPatch]
    public async Task<IActionResult> PatchAsync([FromBody] VFolderPatch<TData> patch)
    {
        if (!await AuthorizeAsync())
            return Forbid();

        return await _clippoAspNetCore.PatchAsync(patch);
    }
    
    [HttpPut]
    public async Task<IActionResult> SaveAsync([FromBody] VFolderState<TData> state)
    {
        if (!await AuthorizeAsync())
            return Forbid();

        return await _clippoAspNetCore.SaveAsync(state);
    }

    private IAuthorizationService AuthorizationService =>
        HttpContext.RequestServices.GetRequiredService<IAuthorizationService>();

    private IAuthenticationService AuthenticationService =>
        HttpContext.RequestServices.GetRequiredService<IAuthenticationService>();

    private async Task<bool> AuthorizeAsync()
    {
        if (_settings.Policy == null)
            return true;

        var authenticationResult = await AuthenticationService.AuthenticateAsync(HttpContext, _settings.Schema);
        if (!authenticationResult.Succeeded)
            return false;

        var authorizationResult =
            await AuthorizationService.AuthorizeAsync(authenticationResult.Principal!, _settings.Policy);
        return authorizationResult.Succeeded;
    }
}