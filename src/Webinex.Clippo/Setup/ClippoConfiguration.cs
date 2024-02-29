using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Webinex.Clippo;

public interface IClippoConfiguration
{
    Type MetaType { get; }
    Type DataType { get; }
    IServiceCollection Services { get; }
    IClippoConfiguration UseMetaProvider<TProvider>();
    IClippoConfiguration UseDbContext<TDbContext>();
}

internal class ClippoConfiguration<TMeta, TData> : IClippoConfiguration
    where TMeta : class, ICloneable
    where TData : class, ICloneable
{
    public ClippoConfiguration(IServiceCollection services)
    {
        services.AddSingleton(this);
        services.AddScoped<IClippo<TMeta, TData>, Clippo<TMeta, TData>>();
        services.AddSingleton<VFileVRowAskyFieldMap<TMeta, TData>>();
        services.AddSingleton<VFolderVRowAskyFieldMap<TMeta, TData>>();
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    public Type MetaType { get; } = typeof(TMeta);
    public Type DataType { get; } = typeof(TData);
    public IServiceCollection Services { get; }

    public IClippoConfiguration UseMetaProvider<TProvider>()
    {
        if (!typeof(TProvider).IsAssignableTo(typeof(IMetaProvider<TMeta, TData>)))
            throw new InvalidOperationException($"Type might be assignable to {typeof(IMetaProvider<TMeta, TData>).Name}");

        Services.AddTransient<IMetaProvider<TMeta, TData>>(x => (IMetaProvider<TMeta, TData>)x.GetRequiredService(typeof(TProvider)));
        return this;
    }

    public IClippoConfiguration UseDbContext<TDbContext>()
    {
        if (!typeof(TDbContext).IsAssignableTo(typeof(IClippoDbContext<TMeta, TData>)))
            throw new InvalidOperationException($"Type might be assignable to {typeof(IClippoDbContext<TMeta, TData>).Name}");
        
        Services.AddTransient<IClippoDbContext<TMeta, TData>>(x => (IClippoDbContext<TMeta, TData>)x.GetRequiredService(typeof(TDbContext)));
        return this;
    }

    public static ClippoConfiguration<TMeta, TData> GetOrCreate(IServiceCollection services)
    {
        services = services ?? throw new ArgumentNullException(nameof(services));

        var instance = (ClippoConfiguration<TMeta, TData>?)services.FirstOrDefault(x =>
                x.ImplementationInstance?.GetType() == typeof(ClippoConfiguration<TMeta, TData>))
            ?.ImplementationInstance;

        return instance ?? new ClippoConfiguration<TMeta, TData>(services);
    }
}