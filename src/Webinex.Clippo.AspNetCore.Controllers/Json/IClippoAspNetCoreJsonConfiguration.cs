using System;
using System.Diagnostics.CodeAnalysis;

namespace Webinex.Clippo.AspNetCore.Controllers.Json
{
    public interface IClippoAspNetCoreJsonConfiguration
    {
        /// <summary>
        ///     Adds action mapping.
        /// </summary>
        /// <param name="kind">'kind' property value in action json</param>
        /// <param name="type">Action type</param>
        /// <returns><see cref="IClippoAspNetCoreJsonConfiguration"/></returns>
        IClippoAspNetCoreJsonConfiguration AddAction([NotNull] string kind, [NotNull] Type type);
    }
}