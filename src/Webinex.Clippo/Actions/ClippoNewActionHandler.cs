using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Webinex.Coded;

namespace Webinex.Clippo.Actions
{
    /// <summary>
    ///     Action handler which executes for <typeparamref name="TAction"/>
    ///     on <see cref="StoreClipArgs.Actions"/> on newly created clips.
    /// </summary>
    /// <typeparam name="TClip">Clip type</typeparam>
    /// <typeparam name="TAction">Action type to handle</typeparam>
    public abstract class ClippoNewActionHandler<TClip, TAction> : IClippoNewActionHandler<TClip>
        where TAction : IClippoAction
    {
        public async Task<CodedResult> HandleNewAsync(TClip clip, IClippoAction action)
        {
            clip = clip ?? throw new ArgumentNullException(nameof(clip));
            action = action ?? throw new ArgumentNullException(nameof(action));

            if (typeof(TAction) != action.GetType())
                return CodedResults.Success();

            return await HandleNewAsync(clip, (TAction)action);
        }

        /// <summary>
        ///     Handler function
        /// </summary>
        /// <param name="clip">Newly created clip</param>
        /// <param name="action">Action payload</param>
        /// <returns>Coded result success or error. When error - actions execution would be interrupted and failed result returned</returns>
        public abstract Task<CodedResult> HandleNewAsync([NotNull] TClip clip, [NotNull] TAction action);
    }
}