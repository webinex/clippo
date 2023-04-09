using System.Collections.Generic;
using System.Threading.Tasks;
using Webinex.Coded;

namespace Webinex.Clippo.Interceptors
{
    /// <inheritdoc />
    public class ClippoInterceptor<TClip> : IClippoInterceptor<TClip>
    {
        /// <inheritdoc />
        public virtual Task<CodedResult<TClip[]>> OnByIdAsync(IEnumerable<string> ids, INext<IEnumerable<string>, CodedResult<TClip[]>> next)
        {
            return next.InvokeAsync(ids);
        }

        /// <inheritdoc />
        public virtual Task<CodedResult<TClip[]>> OnGetAllAsync(GetClipsArgs args, INext<GetClipsArgs, CodedResult<TClip[]>> next)
        {
            return next.InvokeAsync(args);
        }

        /// <inheritdoc />
        public virtual Task<CodedResult<TClip[]>> OnStoreAsync(IEnumerable<StoreClipArgs> args, INext<IEnumerable<StoreClipArgs>, CodedResult<TClip[]>> next)
        {
            return next.InvokeAsync(args);
        }

        /// <inheritdoc />
        public virtual Task<CodedResult<ClipContent[]>> OnContentAsync(GetContentArgs args, INext<GetContentArgs, CodedResult<ClipContent[]>> next)
        {
            return next.InvokeAsync(args);
        }

        /// <inheritdoc />
        public virtual Task<CodedResult> OnApplyAsync(IEnumerable<IClippoAction> actions, INext<IEnumerable<IClippoAction>, CodedResult> next)
        {
            return next.InvokeAsync(actions);
        }
    }
}