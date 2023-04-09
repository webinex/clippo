using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Webinex.Clippo.Interceptors;
using Webinex.Coded;

namespace Webinex.Clippo
{
    internal class Clippo<TClip> : IClippo<TClip>
    {
        private readonly IClippoInterceptable<TClip> _interceptable;

        public Clippo(IClippoInterceptable<TClip> interceptable)
        {
            _interceptable = interceptable;
        }

        public Task<CodedResult<TClip[]>> ByIdAsync(IEnumerable<string> ids)
        {
            ids = ids?.ToArray() ?? throw new ArgumentNullException(nameof(ids));
            return _interceptable.ByIdAsync(ids);
        }

        public Task<CodedResult<TClip[]>> GetAllAsync(GetClipsArgs args)
        {
            args = args ?? throw new ArgumentNullException(nameof(args));
            return _interceptable.GetAllAsync(args);
        }

        public Task<CodedResult<TClip[]>> StoreAsync(IEnumerable<StoreClipArgs> args, CancellationToken cancellationToken = default)
        {
            args = args?.ToArray() ?? throw new ArgumentNullException(nameof(args));
            return _interceptable.StoreAsync(args, cancellationToken);
        }

        public Task<CodedResult<ClipContent[]>> ContentAsync(GetContentArgs args, CancellationToken cancellationToken = default)
        {
            args = args ?? throw new ArgumentNullException(nameof(args));
            return _interceptable.ContentAsync(args, cancellationToken);
        }

        public Task<CodedResult> ApplyAsync(IEnumerable<IClippoAction> actions)
        {
            actions = actions ?? throw new ArgumentNullException(nameof(actions));
            return _interceptable.ApplyAsync(actions);
        }
    }
}