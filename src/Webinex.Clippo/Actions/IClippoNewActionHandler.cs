using System.Threading.Tasks;
using Webinex.Coded;

namespace Webinex.Clippo.Actions
{
    /// <summary>
    ///     Action handler which executes for every action supplied on <see cref="StoreClipArgs.Actions"/> on newly created clips.
    ///     If you would like to execute action only for specific action type, see <see cref="ClippoNewActionHandler{TClip,TAction}"/>
    /// </summary>
    /// <typeparam name="TClip">Clip type</typeparam>
    public interface IClippoNewActionHandler<TClip>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="action"></param>
        /// <returns>Coded result success or error. When error - actions execution would be interrupted and failed result returned</returns>
        Task<CodedResult> HandleNewAsync(TClip clip, IClippoAction action);
    }
}