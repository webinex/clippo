using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Webinex.Coded;

namespace Webinex.Clippo.Actions
{
    /// <summary>
    ///     Action handler which executes for every action of type <typeparamref name="TAction"/>
    ///     supplied to <see cref="IClippo{TClip}.ApplyAsync"/>
    /// </summary>
    /// <typeparam name="TClip">Clip type</typeparam>
    /// <typeparam name="TAction">Action type to handle</typeparam>
    public abstract class ClippoActionHandler<TClip, TAction> : IClippoActionHandler<TClip>
        where TAction : IClippoAction
    {
        public async Task<CodedResult> HandleAsync(IClippoAction action)
        {
            action = action ?? throw new ArgumentNullException(nameof(action));

            if (typeof(TAction) != action.GetType())
                return CodedResults.Success();

            return await HandleAsync((TAction)action);
        }

        /// <summary>
        ///     Handler function
        /// </summary>
        /// <param name="action">Action payload</param>
        /// <returns>Coded result success or error. When error - actions execution would be interrupted and failed result returned</returns>
        protected abstract Task<CodedResult> HandleAsync([NotNull] TAction action);
    }
}