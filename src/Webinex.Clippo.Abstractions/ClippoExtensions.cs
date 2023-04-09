using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Webinex.Coded;

namespace Webinex.Clippo
{
    public static class ClippoExtensions
    {
        /// <summary>
        ///     Applies actions and replaces <see cref="ActivateClippoAction"/> owner to
        ///     <paramref name="ownerType"/>, <paramref name="ownerId"/> and <paramref name="directory"/>
        /// </summary>
        /// <param name="clippo">Clippo instance</param>
        /// <param name="actions">Actions to apply</param>
        /// <param name="ownerType">Owner Type to set in <see cref="ActivateClippoAction"/></param>
        /// <param name="ownerId">Owner Id to set in <see cref="ActivateClippoAction"/></param>
        /// <param name="directory">Directory to set in <see cref="ActivateClippoAction"/></param>
        /// <typeparam name="TClip">Clip type</typeparam>
        /// <returns><see cref="CodedResult"/></returns>
        public static async Task<CodedResult> ApplyAsync<TClip>(
            [NotNull] this IClippo<TClip> clippo,
            [NotNull] IEnumerable<IClippoAction> actions,
            [NotNull] string ownerType,
            [NotNull] string ownerId,
            string directory = null)
        {
            clippo = clippo ?? throw new ArgumentNullException(nameof(clippo));
            actions = actions ?? throw new ArgumentNullException(nameof(actions));
            ownerType = ownerType ?? throw new ArgumentNullException(nameof(ownerType));
            ownerId = ownerId ?? throw new ArgumentNullException(nameof(ownerId));

            actions = actions.Select(action =>
            {
                if (action is ActivateClippoAction activate)
                    return activate.WithOwner(ownerType, ownerId, directory);

                return action;
            }).ToArray();

            return await clippo.ApplyAsync(actions);
        }

        /// <summary>
        ///     Gets single Clip by identifier
        /// </summary>
        /// <param name="clippo">Clippo instance</param>
        /// <param name="id">Clippo identifier</param>
        /// <typeparam name="TClip">Clip type</typeparam>
        /// <returns>Coded result containing found clip or error</returns>
        public static async Task<CodedResult<TClip>> ByIdAsync<TClip>(
            [NotNull] this IClippo<TClip> clippo,
            [NotNull] string id)
        {
            clippo = clippo ?? throw new ArgumentNullException(nameof(clippo));
            id = id ?? throw new ArgumentNullException(nameof(id));

            var result = await clippo.ByIdAsync(new[] { id });
            var resultPayload = result.Payload != null
                ? result.Payload.Single()
                : default;

            return new CodedResult<TClip>(result.Failure, resultPayload);
        }

        /// <summary>
        ///     Get clips by identifiers
        /// </summary>
        /// <param name="clippo">Clippo instance</param>
        /// <param name="args">Search arguments</param>
        /// <typeparam name="TClip">Clip type</typeparam>
        /// <returns>Coded result containing found clips or error</returns>
        public static async Task<CodedResult<TClip[]>> GetAllValuesAsync<TClip>(
            [NotNull] this IClippo<TClip> clippo,
            [NotNull] GetClipsArgs args)
        {
            clippo = clippo ?? throw new ArgumentNullException(nameof(clippo));
            args = args ?? throw new ArgumentNullException(nameof(args));

            var result = await clippo.GetAllAsync(args);
            return new CodedResult<TClip[]>(result.Failure, result.Payload?.ToArray());
        }

        /// <summary>
        ///     Stores file as clip and invokes <see cref="IClippoNewActionHandlers"/> for <see cref="StoreClipArgs.Actions"/>
        /// </summary>
        /// <param name="clippo">Clippo instance</param>
        /// <param name="args">Store arguments</param>
        /// <typeparam name="TClip">Clip type</typeparam>
        /// <returns>Coded result containing newly created clip or error</returns>
        public static async Task<CodedResult<TClip>> StoreAsync<TClip>(
            [NotNull] this IClippo<TClip> clippo,
            [NotNull] StoreClipArgs args)
        {
            clippo = clippo ?? throw new ArgumentNullException(nameof(clippo));
            args = args ?? throw new ArgumentNullException(nameof(args));

            var result = await clippo.StoreAsync(new[] { args });
            var resultPayload = result.Payload != null
                ? result.Payload.Single()
                : default;

            return new CodedResult<TClip>(result.Failure, resultPayload);
        }

        /// <summary>
        ///     Gets content of clip
        /// </summary>
        /// <param name="clippo">Clippo instance</param>
        /// <param name="id">Clip identifier</param>
        /// <param name="options">Get options</param>
        /// <typeparam name="TClip">Type of clip</typeparam>
        /// <returns>Coded result containing clip file content or error</returns>
        public static async Task<CodedResult<ClipContent>> ContentAsync<TClip>(
            [NotNull] this IClippo<TClip> clippo,
            [NotNull] string id,
            GetClipOptions options = GetClipOptions.None)
        {
            clippo = clippo ?? throw new ArgumentNullException(nameof(clippo));
            id = id ?? throw new ArgumentNullException(nameof(id));

            var result = await clippo.ContentAsync(new GetContentArgs(new[] { id }, options));
            return new CodedResult<ClipContent>(result.Failure, result.Payload?.SingleOrDefault());
        }

        /// <summary>
        ///     Applies action
        /// </summary>
        /// <param name="clippo">Clippo instance</param>
        /// <param name="action">Action to be applied</param>
        /// <typeparam name="TClip">Clip type</typeparam>
        /// <returns>Coded result succeed or containing error</returns>
        public static async Task<CodedResult> ApplyAsync<TClip>(
            [NotNull] this IClippo<TClip> clippo,
            [NotNull] IClippoAction action)
        {
            clippo = clippo ?? throw new ArgumentNullException(nameof(clippo));
            action = action ?? throw new ArgumentNullException(nameof(action));

            return await clippo.ApplyAsync(new[] { action });
        }

        /// <summary>
        ///     Applies delete action for <paramref name="id"/>
        /// </summary>
        /// <param name="clippo">Clippo instance</param>
        /// <param name="id">Clip to delete identifier</param>
        /// <typeparam name="TClip">Clip type</typeparam>
        /// <returns>Coded result succeed or containing error</returns>
        public static async Task<CodedResult> DeleteAsync<TClip>(
            [NotNull] this IClippo<TClip> clippo,
            [NotNull] string id)
        {
            clippo = clippo ?? throw new ArgumentNullException(nameof(clippo));
            id = id ?? throw new ArgumentNullException(nameof(id));

            return await clippo.ApplyAsync(new DeleteClipAction(id));
        }

        /// <summary>
        ///     Deletes multiple clips by identifiers
        /// </summary>
        /// <param name="clippo">Clippo instance</param>
        /// <param name="ids">Clip to delete identifiers</param>
        /// <typeparam name="TClip">Clip type</typeparam>
        /// <returns>Coded result succeed or containing error</returns>
        public static async Task<CodedResult> DeleteAsync<TClip>(
            [NotNull] this IClippo<TClip> clippo,
            [NotNull] IEnumerable<string> ids)
        {
            clippo = clippo ?? throw new ArgumentNullException(nameof(clippo));
            ids = ids?.Distinct().ToArray() ?? throw new ArgumentNullException(nameof(ids));

            return await clippo.ApplyAsync(ids.Select(id => new DeleteClipAction(id)));
        }
    }
}