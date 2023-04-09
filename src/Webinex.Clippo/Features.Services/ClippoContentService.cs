using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Webinex.Clippo.Ports.Blob;
using Webinex.Clippo.Ports.Model;
using Webinex.Coded;

namespace Webinex.Clippo.Features.Services
{
    internal interface IClippoContentService
    {
        Task<CodedResult<ClipContent[]>> ContentAsync(
            GetContentArgs args,
            CancellationToken cancellationToken = default);
    }

    internal class ClippoContentService<TClip> : IClippoContentService
    {
        private readonly IClippoStore<TClip> _clippoStore;
        private readonly IClippoModel<TClip> _clippoModel;
        private readonly IClippoBlobStore _clippoBlobStore;

        public ClippoContentService(
            IClippoStore<TClip> clippoStore,
            IClippoModel<TClip> clippoModel,
            IClippoBlobStore clippoBlobStore)
        {
            _clippoStore = clippoStore;
            _clippoModel = clippoModel;
            _clippoBlobStore = clippoBlobStore;
        }

        public async Task<CodedResult<ClipContent[]>> ContentAsync(
            GetContentArgs args,
            CancellationToken cancellationToken = default)
        {
            var clipsResult = await ClipsAsync(args.Ids, args.Options);
            if (!clipsResult.Succeed)
                return clipsResult.Cast<ClipContent[]>();

            var validationResult = ValidateHasReference(clipsResult.Payload);
            if (!validationResult.Succeed)
                return validationResult.Cast<ClipContent[]>();

            var contents = await ContentsAsync(clipsResult.Payload, cancellationToken);
            return CodedResults.Success(contents);
        }

        private async Task<CodedResult<TClip[]>> ClipsAsync(string[] ids, GetClipOptions options)
        {
            var clips = await _clippoStore.GetByIdsAsync(ids, options);

            if (clips.Length != ids.Length)
                return ClippoCodes.NOT_FOUND.Failed<TClip[]>();

            return CodedResults.Success(clips);
        }

        private CodedResult ValidateHasReference(TClip[] clips)
        {
            foreach (var clip in clips)
            {
                if (_clippoModel.GetRef(clip) == null)
                    return ClippoCodes.NO_REF.Failed(_clippoModel.GetId(clip));
            }

            return CodedResults.Success();
        }

        private async Task<ClipContent[]> ContentsAsync(TClip[] clips, CancellationToken cancellationToken)
        {
            var references = clips.Select(x => _clippoModel.GetRef(x)).Distinct().ToArray();
            var streamsByRef = await _clippoBlobStore.GetAsync(references, cancellationToken);

            return clips.Select(x => Map(x, streamsByRef[_clippoModel.GetRef(x)])).ToArray();
        }

        private ClipContent Map(TClip clip, Stream stream)
        {
            var fileName = _clippoModel.GetFileName(clip);
            var sizeBytes = _clippoModel.GetSizeBytes(clip);
            var mimeType = _clippoModel.GetMimeType(clip);
            var id = _clippoModel.GetId(clip);
            return new ClipContent(id, fileName, mimeType, sizeBytes, stream);
        }
    }
}