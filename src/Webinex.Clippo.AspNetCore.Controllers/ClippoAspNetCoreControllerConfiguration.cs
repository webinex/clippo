using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Webinex.Clippo.AspNetCore;

public interface IClippoAspNetCoreConfiguration
{
    IClippoAspNetCoreConfiguration UsePolicy(string schema, string policy);
    Type MetaType { get; }
    Type DataType { get; }
    IMvcBuilder MvcBuilder { get; }
}

public interface IClippoAspNetCoreSettings
{
    public string? Schema { get; }
    public string? Policy { get; }
}

internal class ClippoAspNetCoreConfiguration<TMeta, TData> : IClippoAspNetCoreConfiguration,
    IClippoAspNetCoreSettings
    where TMeta : class, ICloneable
    where TData : class, ICloneable
{
    private ClippoAspNetCoreConfiguration(IMvcBuilder mvcBuilder)
    {
        MvcBuilder = mvcBuilder ?? throw new ArgumentNullException(nameof(mvcBuilder));

        MvcBuilder.Services.AddSingleton<IClippoAspNetCoreSettings>(this);
        MvcBuilder.Services.AddScoped<IClippoAspNetCoreService<TMeta, TData>, ClippoAspNetCoreService<TMeta, TData>>();
        MvcBuilder.AddController(typeof(ClippoController<TMeta, TData>));
    }

    public static ClippoAspNetCoreConfiguration<TMeta, TData> GetOrCreate(IMvcBuilder mvcBuilder)
    {
        mvcBuilder = mvcBuilder ?? throw new ArgumentNullException(nameof(mvcBuilder));

        var instance = (ClippoAspNetCoreConfiguration<TMeta, TData>?)mvcBuilder.Services.FirstOrDefault(x =>
                x.ImplementationInstance?.GetType() == typeof(ClippoAspNetCoreConfiguration<TMeta, TData>))
            ?.ImplementationInstance;

        if (instance != null)
            return instance;

        instance = new ClippoAspNetCoreConfiguration<TMeta, TData>(mvcBuilder);
        mvcBuilder.Services.AddSingleton(instance);
        return instance;
    }

    public IClippoAspNetCoreConfiguration UsePolicy(string schema, string policy)
    {
        Schema = schema ?? throw new ArgumentNullException(nameof(schema));
        Policy = policy ?? throw new ArgumentNullException(nameof(policy));
        return this;
    }

    internal void Complete()
    {
        MvcBuilder.Services.TryAddScoped(
            typeof(IClippoAspNetCoreMapper<,>).MakeGenericType(MetaType, DataType),
            typeof(DefaultClippoAspNetCoreMapper<,>).MakeGenericType(MetaType, DataType));
    }

    public Type MetaType { get; } = typeof(TMeta);
    public Type DataType { get; } = typeof(TData);
    public IMvcBuilder MvcBuilder { get; }
    public string? Schema { get; private set; }
    public string? Policy { get; private set; }
}