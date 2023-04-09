using System;
using System.Diagnostics.CodeAnalysis;

namespace Webinex.Clippo
{
    /// <summary>
    ///     Activates Clip and links it to Owner
    /// </summary>
    public class ActivateClippoAction : IClippoAction
    {
        public ActivateClippoAction(
            [NotNull] string id,
            [MaybeNull] string ownerType,
            [MaybeNull] string ownerId,
            [MaybeNull] string directory)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            OwnerType = ownerType;
            OwnerId = ownerId;
            Directory = directory;
        }

        /// <summary>
        ///     Clip to active identifier
        /// </summary>
        [NotNull]
        public string Id { get; }

        /// <summary>
        ///     Clip owner identifier
        /// </summary>
        [MaybeNull]
        public string OwnerId { get; }

        /// <summary>
        ///     Clip owner type
        /// </summary>
        [MaybeNull]
        public string OwnerType { get; }

        /// <summary>
        ///     Clip directory
        /// </summary>
        [MaybeNull]
        public string Directory { get; }

        /// <summary>
        ///     Clones instance of <see cref="ActivateClippoAction"/> and sets specified owner information
        /// </summary>
        /// <param name="ownerType">Owner type</param>
        /// <param name="ownerId">Owner id</param>
        /// <param name="directory">Directory</param>
        /// <returns>New instance</returns>
        public ActivateClippoAction WithOwner([NotNull] string ownerType, [NotNull] string ownerId, string directory)
        {
            ownerType = ownerType ?? throw new ArgumentNullException(nameof(ownerType));
            ownerId = ownerId ?? throw new ArgumentNullException(nameof(ownerId));

            return new ActivateClippoAction(Id, ownerType, ownerId, directory);
        }

        /// <summary>
        ///     True if action valid for activation, false otherwise
        /// </summary>
        public bool Valid => !string.IsNullOrWhiteSpace(Id)
                             && !string.IsNullOrWhiteSpace(OwnerType)
                             && !string.IsNullOrWhiteSpace(OwnerId);
    }
}