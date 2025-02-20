using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Webinex.Clippo;

public static class ClippoInteractorServiceCollectionExtensions
{
    public static IServiceCollection AddClippoInteractor(this IServiceCollection services)
    {
        services.TryAddScoped(typeof(IClippoInteractor<,>), typeof(IClippoInteractor<,>));
        return services;
    }
}