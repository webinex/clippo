using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Webinex.Coded;

namespace Webinex.Clippo.Interceptors
{
    internal class ClippoInterceptorActivator<TClip, TInterceptor> : IClippoInterceptable<TClip>
        where TInterceptor : IClippoInterceptor<TClip>
    {
        private readonly IClippoInterceptable<TClip> _clippo;
        private readonly TInterceptor _interceptor;

        public ClippoInterceptorActivator(IClippoInterceptable<TClip> clippo, TInterceptor interceptor)
        {
            _clippo = clippo;
            _interceptor = interceptor;
        }

        public Task<CodedResult<TClip[]>> ByIdAsync(IEnumerable<string> ids)
        {
            ids = ids ?? throw new ArgumentNullException(nameof(ids));
            var next = new NextDelegate<IEnumerable<string>, CodedResult<TClip[]>>(args => _clippo.ByIdAsync(args));
            return _interceptor.OnByIdAsync(ids, next);
        }

        public Task<CodedResult<TClip[]>> GetAllAsync(GetClipsArgs args)
        {
            args = args ?? throw new ArgumentNullException(nameof(args));
            var next = new NextDelegate<GetClipsArgs, CodedResult<TClip[]>>(a => _clippo.GetAllAsync(a));
            return _interceptor.OnGetAllAsync(args, next);
        }

        public Task<CodedResult<TClip[]>> StoreAsync(IEnumerable<StoreClipArgs> args, CancellationToken cancellationToken = default)
        {
            args = args ?? throw new ArgumentNullException(nameof(args));
            var next = new NextDelegate<IEnumerable<StoreClipArgs>, CodedResult<TClip[]>>(x => _clippo.StoreAsync(x, cancellationToken));
            return _interceptor.OnStoreAsync(args, next);
        }

        public Task<CodedResult<ClipContent[]>> ContentAsync(GetContentArgs args, CancellationToken cancellationToken = default)
        {
            args = args ?? throw new ArgumentNullException(nameof(args));
            var next = new NextDelegate<GetContentArgs, CodedResult<ClipContent[]>>(x => _clippo.ContentAsync(x, cancellationToken));
            return _interceptor.OnContentAsync(args, next);
        }

        public Task<CodedResult> ApplyAsync(IEnumerable<IClippoAction> actions)
        {
            actions = actions ?? throw new ArgumentNullException(nameof(actions));
            var next = new NextDelegate<IEnumerable<IClippoAction>, CodedResult>(x => _clippo.ApplyAsync(x));
            return _interceptor.OnApplyAsync(actions, next);
        }
        
        private class NextDelegate<TArgs, TResult> : INext<TArgs, TResult>
        {
            private readonly Func<TArgs, Task<TResult>> _delegate;

            public NextDelegate(Func<TArgs, Task<TResult>> @delegate)
            {
                _delegate = @delegate;
            }

            public Task<TResult> InvokeAsync(TArgs args)
            {
                return _delegate.Invoke(args);
            }
        }
    }
}