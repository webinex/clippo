using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Webinex.Clippo.AspNetCore.Controllers.Json;

namespace Webinex.Clippo.AspNetCore.Controllers
{
    public static class ClippoMvcBuilderExtensions
    {
        /// <summary>
        ///     Adds Clippo JSON converters for actions
        /// </summary>
        /// <param name="mvcBuilder"><see cref="IMvcBuilder"/></param>
        /// <param name="configure">Ability to configure JSON options</param>
        /// <returns></returns>
        public static IMvcBuilder AddClippoJson(
            [NotNull] this IMvcBuilder mvcBuilder,
            [MaybeNull] Action<IClippoAspNetCoreJsonConfiguration> configure = null)
        {
            mvcBuilder = mvcBuilder ?? throw new ArgumentNullException(nameof(mvcBuilder));
            var configuration = ClippoAspNetCoreJsonConfiguration.GetOrCreate(mvcBuilder);
            configure?.Invoke(configuration);

            return mvcBuilder;
        }

        /// <summary>
        ///     Adds default Clippo controller to request pipeline
        /// </summary>
        /// <param name="mvcBuilder"><see cref="IMvcBuilder"/></param>
        /// <param name="configure">Ability to configure controller</param>
        /// <typeparam name="TClip">Type of Clip</typeparam>
        /// <typeparam name="TDto">Type of DTO</typeparam>
        /// <returns><see cref="IMvcBuilder"/></returns>
        public static IMvcBuilder AddClippoController<TClip, TDto>(
            [NotNull] this IMvcBuilder mvcBuilder,
            [MaybeNull] Action<IClippoAspNetCoreControllerConfiguration> configure = null)
        {
            mvcBuilder = mvcBuilder ?? throw new ArgumentNullException(nameof(mvcBuilder));
            var configuration = ClippoAspNetCoreControllerConfiguration<TClip, TDto>.GetOrCreate(mvcBuilder);
            configure?.Invoke(configuration);

            return mvcBuilder;
        }

        /// <summary>
        ///     Adds default Clippo controller to request pipeline.
        ///     Uses DTO type same as <typeparamref name="TClip"/>
        /// </summary>
        /// <param name="mvcBuilder"><see cref="IMvcBuilder"/></param>
        /// <param name="configure">Ability to configure controller</param>
        /// <typeparam name="TClip">Type of Clip</typeparam>
        /// <returns><see cref="IMvcBuilder"/></returns>
        public static IMvcBuilder AddClippoController<TClip>(
            [NotNull] this IMvcBuilder mvcBuilder,
            [MaybeNull] Action<IClippoAspNetCoreControllerConfiguration> configure = null)
        {
            return AddClippoController<TClip, TClip>(mvcBuilder, configure);
        }
    }
}