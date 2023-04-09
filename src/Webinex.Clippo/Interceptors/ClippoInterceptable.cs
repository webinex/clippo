using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Webinex.Clippo.Features.Services;
using Webinex.Coded;

namespace Webinex.Clippo.Interceptors
{
    internal interface IClippoInterceptable<TClip> : IClippo<TClip>
    {
    }
    
    internal class ClippoInterceptable<TClip> : IClippoInterceptable<TClip>
    {
        private readonly IClippoByIdService<TClip> _byIdService;
        private readonly IClippoGetAllService<TClip> _getAllService;
        private readonly IClippoStoreService<TClip> _storeService;
        private readonly IClippoContentService _contentService;
        private readonly IClippoApplyService<TClip> _applyService;

        public ClippoInterceptable(
            IClippoByIdService<TClip> byIdService,
            IClippoGetAllService<TClip> getAllService,
            IClippoStoreService<TClip> storeService,
            IClippoContentService contentService,
            IClippoApplyService<TClip> applyService)
        {
            _byIdService = byIdService;
            _getAllService = getAllService;
            _storeService = storeService;
            _contentService = contentService;
            _applyService = applyService;
        }

        public async Task<CodedResult<TClip[]>> ByIdAsync(IEnumerable<string> ids)
        {
            ids = ids?.ToArray() ?? throw new ArgumentNullException(nameof(ids));
            return await _byIdService.ByIdAsync(ids);
        }

        public async Task<CodedResult<TClip[]>> GetAllAsync(GetClipsArgs args)
        {
            args = args ?? throw new ArgumentNullException(nameof(args));
            return await _getAllService.GetAllAsync(args);
        }

        public Task<CodedResult<TClip[]>> StoreAsync(IEnumerable<StoreClipArgs> args, CancellationToken cancellationToken = default)
        {
            args = args?.ToArray() ?? throw new ArgumentNullException(nameof(args));
            return _storeService.StoreAsync(args, cancellationToken);
        }

        public Task<CodedResult<ClipContent[]>> ContentAsync(GetContentArgs args, CancellationToken cancellationToken = default)
        {
            args = args ?? throw new ArgumentNullException(nameof(args));
            return _contentService.ContentAsync(args, cancellationToken);
        }

        public Task<CodedResult> ApplyAsync(IEnumerable<IClippoAction> actions)
        {
            actions = actions ?? throw new ArgumentNullException(nameof(actions));
            return _applyService.ApplyAsync(actions);
        }
    }
}