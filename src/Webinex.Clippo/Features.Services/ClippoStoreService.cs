using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Webinex.Clippo.Ports.Blob;
using Webinex.Clippo.Ports.Model;
using Webinex.Coded;

namespace Webinex.Clippo.Features.Services
{
    internal interface IClippoStoreService<TClip>
    {
        Task<CodedResult<TClip[]>> StoreAsync(IEnumerable<StoreClipArgs> args,
            CancellationToken cancellationToken = default);
    }

    internal class ClippoStoreService<TClip> : IClippoStoreService<TClip>
    {
        private readonly IClippoStore<TClip> _clippoStore;
        private readonly IClippoModel<TClip> _clippoModel;
        private readonly IClippoBlobStore _clippoBlobStore;
        private readonly IClippoApplyService<TClip> _clippoApplyService;

        public ClippoStoreService(
            IClippoStore<TClip> clippoStore,
            IClippoModel<TClip> clippoModel,
            IClippoBlobStore clippoBlobStore,
            IClippoApplyService<TClip> clippoApplyService)
        {
            _clippoStore = clippoStore;
            _clippoModel = clippoModel;
            _clippoBlobStore = clippoBlobStore;
            _clippoApplyService = clippoApplyService;
        }

        public async Task<CodedResult<TClip[]>> StoreAsync(
            IEnumerable<StoreClipArgs> args,
            CancellationToken cancellationToken = default)
        {
            args = args?.ToArray() ?? throw new ArgumentNullException(nameof(args));

            var streams = args.Select(x => x.Content.Value).ToArray();
            var referenceByStream = await _clippoBlobStore.StoreAsync(streams, cancellationToken);

            var models = await NewModelsAsync(args.ToArray(), referenceByStream);
            var created = await _clippoStore.AddAsync(models);
            return CodedResults.Success(created);
        }

        private async Task<TClip[]> NewModelsAsync(StoreClipArgs[] args, IDictionary<Stream, string> referenceByStream)
        {
            var results = new LinkedList<TClip>();
            foreach (var arg in args)
            {
                results.AddLast(await NewModelAsync(arg, referenceByStream[arg.Content.Value]));
            }

            return results.ToArray();
        }

        private async Task<TClip> NewModelAsync(StoreClipArgs args, string reference)
        {
            var model = _clippoModel.New(args.Values);

            _clippoModel.SetRef(model, reference);
            _clippoModel.SetFileName(model, args.Content.FileName);
            _clippoModel.SetMimeType(model, args.Content.MimeType);
            _clippoModel.SetSizeBytes(model, (int)args.Content.Value.Length);

            await _clippoApplyService.ApplyNewAsync(model, args.Actions);

            return model;
        }
    }
}