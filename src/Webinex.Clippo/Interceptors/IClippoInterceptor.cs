using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Webinex.Coded;

namespace Webinex.Clippo.Interceptors
{
    /// <summary>
    ///     Forward control to next pipeline member
    /// </summary>
    /// <typeparam name="TArgs">Argument type</typeparam>
    /// <typeparam name="TResult">Result type</typeparam>
    public interface INext<TArgs, TResult>
    {
        /// <summary>
        ///     Forward control to next pipeline member
        /// </summary>
        /// <param name="args">Arguments</param>
        /// <returns>Next pipeline members result</returns>
        Task<TResult> InvokeAsync(TArgs args);
    }

    /// <summary>
    ///     Interceptors behavior similar to middlewares. They would be executed
    ///     in reverse order they added and same order after calling `next.InvokeAsync(args)`.
    ///     If interceptor returns failed CodedResult, than request pipeline interrupted and CodedResult returned.  
    ///     Default ClippoController would return failed status code with serialized `x-coded-failure` header.
    ///
    ///     If you would like to intercept only part of methods, you can use <see cref="ClippoInterceptor{TClip}"/>
    /// </summary>
    /// <typeparam name="TClip">Clip type</typeparam>
    public interface IClippoInterceptor<TClip>
    {
        /// <summary>
        ///     Interceptor for <see cref="IClippo{TClip}.ByIdAsync"/>
        /// </summary>
        Task<CodedResult<TClip[]>> OnByIdAsync(
            [NotNull] IEnumerable<string> ids,
            [NotNull] INext<IEnumerable<string>, CodedResult<TClip[]>> next);

        
        /// <summary>
        ///     Interceptor for <see cref="IClippo{TClip}.GetAllAsync"/>
        /// </summary>
        Task<CodedResult<TClip[]>> OnGetAllAsync(
            [NotNull] GetClipsArgs args,
            [NotNull] INext<GetClipsArgs, CodedResult<TClip[]>> next);

        /// <summary>
        ///     Interceptor for <see cref="IClippo{TClip}.StoreAsync"/>
        /// </summary>
        Task<CodedResult<TClip[]>> OnStoreAsync(
            [NotNull] IEnumerable<StoreClipArgs> args,
            [NotNull] INext<IEnumerable<StoreClipArgs>, CodedResult<TClip[]>> next);

        /// <summary>
        ///     Interceptor for <see cref="IClippo{TClip}.ContentAsync"/>
        /// </summary>
        Task<CodedResult<ClipContent[]>> OnContentAsync(
            [NotNull] GetContentArgs args,
            [NotNull] INext<GetContentArgs, CodedResult<ClipContent[]>> next);

        /// <summary>
        ///     Interceptor for <see cref="IClippo{TClip}.ApplyAsync"/>
        /// </summary>
        Task<CodedResult> OnApplyAsync(
            [NotNull] IEnumerable<IClippoAction> actions,
            [NotNull] INext<IEnumerable<IClippoAction>, CodedResult> next);
    }
}