using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webinex.Coded;

namespace Webinex.Clippo.Interceptors
{
    internal class MaxSizeInterceptor<TClip> : ClippoInterceptor<TClip>
    {
        private readonly MaxSizeSettings _maxSizeSettings;

        public MaxSizeInterceptor(MaxSizeSettings maxSizeSettings)
        {
            _maxSizeSettings = maxSizeSettings;
        }

        public override async Task<CodedResult<TClip[]>> OnStoreAsync(
            IEnumerable<StoreClipArgs> args,
            INext<IEnumerable<StoreClipArgs>, CodedResult<TClip[]>> next)
        {
            args = args.ToArray();

            foreach (var arg in args)
            {
                if (arg.Content.Value.Length > _maxSizeSettings.Value)
                    return ClippoCodes.MAX.Failed<TClip[]>((arg.Content.FileName, arg.Content.Value.Length));
            }

            return await base.OnStoreAsync(args, next);
        }
    }
}