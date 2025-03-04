using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Webinex.Clippo;

public static class ClippoInteractorServiceCollectionExtensions
{
    public static IServiceCollection AddClippoInteractor<TMeta, TData>(this IServiceCollection services)
        where TMeta : class, ICloneable
        where TData : class, ICloneable
    {
        services.TryAddScoped(typeof(IClippoInteractor<TMeta, TData>), typeof(ClippoInteractor<TMeta, TData>));
        return services;
    }
}