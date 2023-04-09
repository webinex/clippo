using System;
using System.Diagnostics.CodeAnalysis;

namespace Webinex.Clippo
{
    /// <summary>
    ///     Activates newly created Clip and links it to Owner
    /// </summary>
    public class ActivateNewClippoAction : IClippoAction
    {
        public ActivateNewClippoAction(
            [NotNull] string ownerId,
            [NotNull] string ownerType,
            [MaybeNull] string directory)
        {
            OwnerId = ownerId ?? throw new ArgumentNullException(nameof(ownerId));
            OwnerType = ownerType ?? throw new ArgumentNullException(nameof(ownerType));
            Directory = directory;
        }

        /// <summary>
        ///     New clip owner identifier
        /// </summary>
        [NotNull]
        public string OwnerId { get; }

        /// <summary>
        ///     New clip owner type
        /// </summary>
        [NotNull]
        public string OwnerType { get; }

        /// <summary>
        ///     New clip directory
        /// </summary>
        [MaybeNull]
        public string Directory { get; }
    }
}