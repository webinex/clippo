using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Webinex.Clippo
{
    /// <summary>
    ///     Stores files as clips
    /// </summary>
    public class StoreClipArgs
    {
        public StoreClipArgs(
            [NotNull] StoreClipContent content,
            IEnumerable<IClippoAction> actions,
            IDictionary<string, object> values)
        {
            Content = content ?? throw new ArgumentNullException(nameof(content));
            Actions = actions ?? Array.Empty<IClippoAction>();
            Values = values ?? new Dictionary<string, object>();
        }

        /// <summary>
        ///     File content
        /// </summary>
        [NotNull] public StoreClipContent Content { get; }

        /// <summary>
        ///     Actions to be applied on newly created entity
        /// </summary>
        [NotNull] public IEnumerable<IClippoAction> Actions { get; }

        /// <summary>
        ///     Additional values
        /// </summary>
        [NotNull] public IDictionary<string, object> Values { get; }
    }
}