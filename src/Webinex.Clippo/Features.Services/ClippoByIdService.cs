using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Webinex.Clippo.Ports.Model;
using Webinex.Coded;

namespace Webinex.Clippo.Features.Services
{
    internal interface IClippoByIdService<TClip>
    {
        Task<CodedResult<TClip[]>> ByIdAsync([NotNull] IEnumerable<string> ids, GetClipOptions options = GetClipOptions.None);
    }

    internal class ClippoByIdService<TClip> : IClippoByIdService<TClip>
    {
        private readonly IClippoStore<TClip> _clippoStore;

        public ClippoByIdService(IClippoStore<TClip> clippoStore)
        {
            _clippoStore = clippoStore;
        }

        public async Task<CodedResult<TClip[]>> ByIdAsync(IEnumerable<string> ids, GetClipOptions options = GetClipOptions.None)
        {
            ids = ids?.Distinct().ToArray() ?? throw new ArgumentNullException(nameof(ids));
            var result = await _clippoStore.GetByIdsAsync(ids, options);

            if (result.Length != ids.Count())
                return ClippoCodes.NOT_FOUND.Failed<TClip[]>();

            return CodedResults.Success(result);
        }
    }
}