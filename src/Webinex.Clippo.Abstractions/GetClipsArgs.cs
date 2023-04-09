using System;
using System.Diagnostics.CodeAnalysis;

namespace Webinex.Clippo
{
    /// <summary>
    ///     Clips search payload
    /// </summary>
    public class GetClipsArgs
    {
        internal GetClipsArgs(
            string ownerType = null,
            string ownerId = null,
            string directory = null,
            GetClipOptions options = GetClipOptions.None)
        {
            OwnerId = ownerId;
            OwnerType = ownerType;
            Directory = directory;
            Options = options;
        }

        [MaybeNull] public string OwnerId { get; }
        [MaybeNull] public string OwnerType { get; }
        [MaybeNull] public string Directory { get; }

        public GetClipOptions Options { get; }

        /// <summary>
        ///     Creates new instance of <see cref="GetClipsArgs"/> with search arguments for directory
        /// </summary>
        /// <param name="directory">Directory</param>
        /// <param name="options">Get options</param>
        /// <returns>Instance</returns>
        public static GetClipsArgs ByDirectory([NotNull] string directory, GetClipOptions options = GetClipOptions.None) =>
            new GetClipsArgs(directory: directory ?? throw new ArgumentNullException(nameof(directory)), options: options);

        /// <summary>
        ///     Creates new instance of <see cref="GetClipsArgs"/> with search arguments for specific owner
        /// </summary>
        /// <param name="ownerType">Owner type</param>
        /// <param name="ownerId">Owner identifier</param>
        /// <param name="directory">Optional directory</param>
        /// <param name="options">Get options</param>
        /// <returns>Instance</returns>
        public static GetClipsArgs
            ByOwner([NotNull] string ownerType, [NotNull] string ownerId, string directory = null, GetClipOptions options = GetClipOptions.None) => new GetClipsArgs(
            ownerType ?? throw new ArgumentNullException(nameof(ownerType)),
            ownerId ?? throw new ArgumentNullException(nameof(ownerId)),
            directory,
            options);
    }
}