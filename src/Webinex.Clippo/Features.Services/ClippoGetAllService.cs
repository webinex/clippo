using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Webinex.Clippo.Ports.Model;
using Webinex.Coded;

namespace Webinex.Clippo.Features.Services
{
    internal interface IClippoGetAllService<TClip>
    {
        Task<CodedResult<TClip[]>> GetAllAsync([NotNull] GetClipsArgs args);
    }

    internal class ClippoGetAllService<TClip> : IClippoGetAllService<TClip>
    {
        private readonly IClippoStore<TClip> _clippoStore;

        public ClippoGetAllService(IClippoStore<TClip> clippoStore)
        {
            _clippoStore = clippoStore;
        }

        public async Task<CodedResult<TClip[]>> GetAllAsync(GetClipsArgs args)
        {
            var results = await _clippoStore.GetAllAsync(args);
            return CodedResults.Success(results);
        }
    }
}