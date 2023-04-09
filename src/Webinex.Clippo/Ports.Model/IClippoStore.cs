using System.Collections.Generic;
using System.Threading.Tasks;

namespace Webinex.Clippo.Ports.Model
{
    /// <summary>
    ///     Clippo entities store
    /// </summary>
    /// <typeparam name="TClip"></typeparam>
    public interface IClippoStore<TClip>
    {
        Task<TClip[]> GetAllAsync(GetClipsArgs args);

        Task<TClip[]> GetByIdsAsync(IEnumerable<string> ids, GetClipOptions options = GetClipOptions.None);

        Task<TClip[]> AddAsync(IEnumerable<TClip> clips);

        Task<TClip[]> UpdateAsync(IEnumerable<TClip> clips);

        Task DeleteAsync(IEnumerable<TClip> clips);
    }
}