using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Webinex.Coded;

namespace Webinex.Clippo.Actions
{
    /// <summary>
    ///     Action handler which executes for every action supplied to <see cref="IClippo{TClip}.ApplyAsync"/>.
    ///     If you would like to execute action only for specific action type, see <see cref="ClippoActionHandler{TClip,TAction}"/>
    /// </summary>
    /// <typeparam name="TClip">Clip type</typeparam>
    public interface IClippoActionHandler<TClip>
    {
        /// <summary>
        ///     Action handler
        /// </summary>
        /// <param name="action">Action payload</param>
        /// <returns>Coded result success or error. When error - actions execution would be interrupted and failed result returned</returns>
        Task<CodedResult> HandleAsync([NotNull] IClippoAction action);
    }
}