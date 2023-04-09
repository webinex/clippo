using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Webinex.Coded;

namespace Webinex.Clippo
{
    /// <summary>
    ///     Primary facade for interaction with Clippo.
    ///     All calls to this service can be intercepted.
    /// </summary>
    /// <typeparam name="TClip">Clip type</typeparam>
    public interface IClippo<TClip>
    {
        /// <summary>
        ///     Gets clips by identifiers
        /// </summary>
        /// <param name="ids">Clips identifiers</param>
        /// <returns>Coded result containing found clips or error</returns>
        Task<CodedResult<TClip[]>> ByIdAsync([NotNull] IEnumerable<string> ids);
        
        /// <summary>
        ///     Gets all clips by search arguments
        /// </summary>
        /// <param name="args">Search arguments</param>
        /// <returns>Coded result containing clips or error</returns>
        Task<CodedResult<TClip[]>> GetAllAsync([NotNull] GetClipsArgs args);
        
        /// <summary>
        ///     Stores files as clips and invokes <see cref="IClippoNewActionHandlers"/> for <see cref="StoreClipArgs.Actions"/>
        /// </summary>
        /// <param name="args">Files information to save</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Coded result containing newly created clips or error</returns>
        Task<CodedResult<TClip[]>> StoreAsync([NotNull] IEnumerable<StoreClipArgs> args, CancellationToken cancellationToken = default);
        
        /// <summary>
        ///     Gets content of clips
        /// </summary>
        /// <param name="args">Clip search arguments</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Coded result containing found clips content or error</returns>
        Task<CodedResult<ClipContent[]>> ContentAsync([NotNull] GetContentArgs args, CancellationToken cancellationToken = default);
        
        /// <summary>
        ///     Applies <paramref name="actions"/>
        /// </summary>
        /// <param name="actions">Actions to be applied</param>
        /// <returns>Coded result containing success or error</returns>
        Task<CodedResult> ApplyAsync([NotNull] IEnumerable<IClippoAction> actions);
    }
}