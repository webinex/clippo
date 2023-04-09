using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webinex.Coded;

namespace Webinex.Clippo.Interceptors
{
    internal class NoEmptyInterceptor<TClip> : ClippoInterceptor<TClip>
    {
        public override async Task<CodedResult<TClip[]>> OnStoreAsync(IEnumerable<StoreClipArgs> args, INext<IEnumerable<StoreClipArgs>, CodedResult<TClip[]>> next)
        {
            args = args.ToArray();

            foreach (var arg in args)
            {
                if (arg.Content.Value.Length == 0)
                    return ClippoFailures.NoEmpty(arg.Content.FileName).ToResult<TClip[]>();
            }
            
            return await base.OnStoreAsync(args, next);
        }
    }
}