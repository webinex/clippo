using System;
using Microsoft.Extensions.DependencyInjection;

namespace Webinex.Clippo;

public static class ClippoServiceCollectionExtensions
{
    public static IServiceCollection AddClippo<TMeta, TData>(this IServiceCollection services, Action<IClippoConfiguration> configure)
        where TMeta : class, ICloneable
        where TData : class, ICloneable
    {
        var configuration = ClippoConfiguration<TMeta, TData>.GetOrCreate(services);
        configure(configuration);
        return services;
    }
}