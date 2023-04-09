using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Webinex.Clippo
{
    /// <summary>
    ///     Get content predicate arguments
    /// </summary>
    public class GetContentArgs
    {
        public GetContentArgs(IEnumerable<string> ids, GetClipOptions options = GetClipOptions.None)
        {
            Ids = ids?.Distinct().ToArray() ?? throw new ArgumentNullException(nameof(ids));
            Options = options;
        }
        
        /// <summary>
        ///     Clips identifiers
        /// </summary>
        [NotNull]
        public string[] Ids { get; }
        
        /// <summary>
        ///     Get options
        /// </summary>
        public GetClipOptions Options { get; }
    }
}