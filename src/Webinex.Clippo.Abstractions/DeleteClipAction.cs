using System;
using System.Diagnostics.CodeAnalysis;

namespace Webinex.Clippo
{
    /// <summary>
    ///     Deletes clip by identifier
    /// </summary>
    public class DeleteClipAction : IClippoAction
    {
        public DeleteClipAction([NotNull] string id)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }

        /// <summary>
        ///     Clip to delete identifier
        /// </summary>
        [NotNull]
        public string Id { get; }
    }
}