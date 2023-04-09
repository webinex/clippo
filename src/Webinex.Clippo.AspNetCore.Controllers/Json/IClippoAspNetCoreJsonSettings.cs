using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Webinex.Clippo.AspNetCore.Controllers.Json
{
    public interface IClippoAspNetCoreJsonSettings
    {
        /// <summary>
        ///     Clippo action types by kind
        /// </summary>
        [NotNull]
        IDictionary<string, Type> Actions { get; }
    }
}