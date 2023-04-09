using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Webinex.Clippo.AspNetCore.Controllers
{
    public interface IClippoAspNetCoreControllerConfiguration
    {
        /// <summary>
        ///     Adds authorization requirements for default <see cref="ClippoControllerBase{TClip,TClipDto}"/>
        /// </summary>
        /// <param name="schema">Authentication schema</param>
        /// <param name="policy">Authorization policy</param>
        /// <returns><see cref="IClippoAspNetCoreControllerConfiguration"/></returns>
        IClippoAspNetCoreControllerConfiguration UsePolicy([NotNull] string schema, [NotNull] string policy);

        /// <summary>
        ///     Clip type
        /// </summary>
        [NotNull]
        Type ClipType { get; }

        /// <summary>
        ///     DTO type
        /// </summary>
        [NotNull]
        Type DtoType { get; }
        
        /// <summary>
        ///     MvcBuilder for extensions
        /// </summary>
        [NotNull]
        IMvcBuilder MvcBuilder { get; }
    }
}