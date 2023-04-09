using System;
using Microsoft.Extensions.DependencyInjection;

namespace Webinex.Clippo
{
    public static class ClippoServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds clippo service
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="configure">Configuration delegate</param>
        /// <typeparam name="TClip">Clip type</typeparam>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddClippo<TClip>(
            this IServiceCollection services,
            Action<IClippoCoreConfiguration> configure)
        {
            services = services ?? throw new ArgumentNullException(nameof(services));
            configure = configure ?? throw new ArgumentNullException(nameof(configure));

            var configuration = ClippoCoreCoreConfiguration<TClip>.GetOrCreate(services);
            configure(configuration);

            return services;
        }
    }
}