using System;
using Microsoft.Extensions.DependencyInjection;

namespace Webinex.Clippo.AspNetCore;

public static class ClippoMvcBuilderExtensions
{
    public static IMvcBuilder AddClippoAspNetCore<TMeta, TData>(
        this IMvcBuilder mvcBuilder,
        Action<IClippoAspNetCoreConfiguration>? configure = null)
        where TMeta : class, ICloneable
        where TData : class, ICloneable
    {
        mvcBuilder = mvcBuilder ?? throw new ArgumentNullException(nameof(mvcBuilder));
        var configuration = ClippoAspNetCoreConfiguration<TMeta, TData>.GetOrCreate(mvcBuilder);
        configure?.Invoke(configuration);
        configuration.Complete();

        return mvcBuilder;
    }
}