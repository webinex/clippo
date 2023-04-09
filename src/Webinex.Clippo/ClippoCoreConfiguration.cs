using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Webinex.Clippo.Actions;
using Webinex.Clippo.Actions.Activate;
using Webinex.Clippo.Actions.Delete;
using Webinex.Clippo.Actions.SetValues;
using Webinex.Clippo.Adapters.FileSystem;
using Webinex.Clippo.Adapters.Model;
using Webinex.Clippo.Features.Services;
using Webinex.Clippo.Interceptors;
using Webinex.Clippo.Ports.Blob;
using Webinex.Clippo.Ports.Model;

namespace Webinex.Clippo
{
    /// <summary>
    ///     Configuration for clippo library
    /// </summary>
    public interface IClippoCoreConfiguration
    {
        /// <summary>
        ///     Clip type
        /// </summary>
        [NotNull]
        Type ClipType { get; }

        /// <summary>
        ///     Services to be used in extension methods
        /// </summary>
        [NotNull]
        IServiceCollection Services { get; }

        /// <summary>
        ///     Values which can be shared between configuration calls
        /// </summary>
        [NotNull]
        IDictionary<string, object> Values { get; }

        /// <summary>
        ///     Adds File System Blob store
        /// </summary>
        /// <param name="basePath">Fully qualified path to directory to save blobs</param>
        /// <returns><see cref="IClippoCoreConfiguration"/></returns>
        /// <exception cref="InvalidOperationException">Thrown if path not fully qualified</exception>
        IClippoCoreConfiguration AddFileSystemStore([NotNull] string basePath);

        /// <summary>
        ///     Adds model definition
        /// </summary>
        /// <param name="type">Model definition type</param>
        /// <returns><see cref="IClippoCoreConfiguration"/></returns>
        IClippoCoreConfiguration AddModelDefinition([NotNull] Type type);

        /// <summary>
        ///     Allows to configure interceptors
        /// </summary>
        [NotNull]
        IClippoCoreInterceptorsConfiguration Interceptors { get; }
    }

    /// <summary>
    ///     Configuration for clippo interceptors
    /// </summary>
    public interface IClippoCoreInterceptorsConfiguration
    {
        /// <summary>
        ///     Parent configuration
        /// </summary>
        IClippoCoreConfiguration Configuration { get; }

        /// <summary>
        ///     Adds interceptor to request pipeline.
        ///     Interceptors would be called in revers order of their registration.
        ///
        ///     .Add&lt;X&gt;();
        ///     .Add&lt;Y&gt;();
        ///
        ///     Firstly would be called Y, than after calling `next.InvokeAsync` would be called X.
        ///     After X call to `next.InvokeAsync`, would be called impl and control would be returned
        ///     back to X and after `return` statement - back to Y.
        /// </summary>
        /// <param name="interceptorType"></param>
        /// <returns><see cref="IClippoCoreInterceptorsConfiguration"/></returns>
        IClippoCoreInterceptorsConfiguration Add(Type interceptorType);
    }

    internal class ClippoCoreCoreConfiguration<TClip> : IClippoCoreConfiguration, IClippoCoreInterceptorsConfiguration
    {
        public ClippoCoreCoreConfiguration(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));

            services
                .AddScoped<IClippo<TClip>, Clippo<TClip>>()
                .AddScoped<IClippoInterceptable<TClip>, ClippoInterceptable<TClip>>()
                .AddScoped<IClippoApplyService<TClip>, ClippoApplyService<TClip>>()
                .AddScoped<IClippoByIdService<TClip>, ClippoByIdService<TClip>>()
                .AddScoped<IClippoContentService, ClippoContentService<TClip>>()
                .AddScoped<IClippoGetAllService<TClip>, ClippoGetAllService<TClip>>()
                .AddScoped<IClippoStoreService<TClip>, ClippoStoreService<TClip>>()
                .AddScoped<IClippoModel<TClip>, DefinitionClippoModel<TClip>>()
                .AddScoped<IClippoActionHandler<TClip>, ActivateActionHandler<TClip>>()
                .AddScoped<IClippoNewActionHandler<TClip>, ActivateNewActionHandler<TClip>>()
                .AddScoped<IClippoActionHandler<TClip>, DeleteClippoActionHandler<TClip>>()
                .AddScoped<IClippoActionHandler<TClip>, SetValuesClippoActionHandler<TClip>>();
        }

        public Type ClipType => typeof(TClip);

        public IServiceCollection Services { get; }

        public IDictionary<string, object> Values { get; } = new Dictionary<string, object>();

        public IClippoCoreConfiguration Configuration => this;

        public IClippoCoreInterceptorsConfiguration Add(Type interceptorType)
        {
            interceptorType = interceptorType ?? throw new ArgumentNullException(nameof(interceptorType));

            var serviceType = typeof(IClippoInterceptor<TClip>);
            if (!serviceType.IsAssignableFrom(interceptorType))
                throw new ArgumentException($"Might be assignable to type {serviceType.Name}", nameof(interceptorType));

            Services.AddScoped(interceptorType);

            Services.Decorate(
                typeof(IClippoInterceptable<>).MakeGenericType(ClipType),
                typeof(ClippoInterceptorActivator<,>).MakeGenericType(ClipType, interceptorType));

            return this;
        }

        public IClippoCoreInterceptorsConfiguration Interceptors => this;

        public IClippoCoreConfiguration AddFileSystemStore(string basePath)
        {
            basePath = basePath ?? throw new ArgumentNullException(nameof(basePath));
            var fsSettings = new FileSystemBlobSettings(basePath);
            Services.AddSingleton(fsSettings);
            Services.AddScoped<IClippoBlobStore, FileSystemBlobStore>();

            return this;
        }

        public IClippoCoreConfiguration AddModelDefinition(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));
            var serviceType = typeof(IClippoModelDefinition<TClip>);

            if (!serviceType.IsAssignableFrom(type))
                throw new ArgumentException($"Might be assignable to {serviceType.Name}", nameof(type));

            Services.AddScoped(serviceType, type);
            return this;
        }

        public static ClippoCoreCoreConfiguration<TClip> GetOrCreate(IServiceCollection services)
        {
            var instance = services
                .FirstOrDefault(x => x.ImplementationInstance?.GetType() == typeof(ClippoCoreCoreConfiguration<TClip>))
                ?.ImplementationInstance;

            if (instance != null)
            {
                return (ClippoCoreCoreConfiguration<TClip>)instance;
            }

            var newInstance = new ClippoCoreCoreConfiguration<TClip>(services);
            services.AddSingleton(newInstance);
            return newInstance;
        }
    }

    public static class ClippoCoreConfigurationExtensions
    {
        /// <summary>
        ///     Adds model definition
        /// </summary>
        /// <param name="coreConfiguration"><see cref="IClippoCoreInterceptorsConfiguration"/></param>
        /// <typeparam name="T">Model definition type]</typeparam>
        /// <returns><see cref="IClippoCoreConfiguration"/></returns>
        public static IClippoCoreConfiguration AddModelDefinition<T>(
            [NotNull] this IClippoCoreConfiguration coreConfiguration)
        {
            coreConfiguration = coreConfiguration ?? throw new ArgumentNullException(nameof(coreConfiguration));

            return coreConfiguration.AddModelDefinition(typeof(T));
        }

        /// <summary>
        ///     Adds model definition based on <see cref="ClippoModelDefinitionConfiguration{TClip}"/>. Which more
        ///     secure and fluent way.
        /// </summary>
        /// <param name="coreConfiguration"><see cref="IClippoCoreConfiguration"/></param>
        /// <typeparam name="T">Type of Model definition configuration</typeparam>
        /// <returns><see cref="IClippoCoreConfiguration"/></returns>
        /// <exception cref="InvalidOperationException">If <typeparamref name="T"/> not subtype of <see cref="ClippoModelDefinitionConfiguration{TClip}"/></exception>
        public static IClippoCoreConfiguration AddModelDefinitionConfiguration<T>(
            [NotNull] this IClippoCoreConfiguration coreConfiguration)
        {
            coreConfiguration = coreConfiguration ?? throw new ArgumentNullException(nameof(coreConfiguration));

            var expectedType = typeof(ClippoModelDefinitionConfiguration<>).MakeGenericType(coreConfiguration.ClipType);
            if (!expectedType.IsAssignableFrom(typeof(T)))
            {
                throw new InvalidOperationException($"Generic argument might be assignable to {expectedType.Name}");
            }

            var configuration = Activator.CreateInstance<T>()!;
            var definitionProperty = configuration.GetType()
                .GetProperty(nameof(ClippoModelDefinitionConfiguration<object>.Definition),
                    BindingFlags.Instance | BindingFlags.NonPublic);

            var definition = definitionProperty!.GetValue(configuration);

            coreConfiguration.Services.AddSingleton(
                typeof(IClippoModelDefinition<>).MakeGenericType(coreConfiguration.ClipType),
                definition);

            return coreConfiguration;
        }
    }

    public static class ClippoCoreInterceptorsConfigurationExtensions
    {
        /// <summary>
        ///     Adds interceptor to request pipeline.
        ///     Interceptors would be called in revers order of their registration.
        ///
        ///     .Add&lt;X&gt;();
        ///     .Add&lt;Y&gt;();
        ///
        ///     Firstly would be called Y, than after calling `next.InvokeAsync` would be called X.
        ///     After X call to `next.InvokeAsync`, would be called impl and control would be returned
        ///     back to X and after `return` statement - back to Y.
        /// </summary>
        /// <param name="configuration"><see cref="IClippoCoreConfiguration"/></param>
        /// <typeparam name="TInterceptor">Interceptor type</typeparam>
        /// <returns><see cref="IClippoCoreInterceptorsConfiguration"/></returns>
        public static IClippoCoreInterceptorsConfiguration Add<TInterceptor>(
            [NotNull] this IClippoCoreInterceptorsConfiguration configuration)
        {
            configuration = configuration ??
                            throw new ArgumentNullException(nameof(configuration));

            configuration.Add(typeof(TInterceptor));
            return configuration;
        }

        /// <summary>
        ///     Adds interceptor to prevent empty files storing.
        /// </summary>
        /// <param name="configuration"><see cref="IClippoCoreConfiguration"/></param>
        /// <returns><see cref="IClippoCoreConfiguration"/></returns>
        public static IClippoCoreInterceptorsConfiguration AddNoEmpty(
            [NotNull] this IClippoCoreInterceptorsConfiguration configuration)
        {
            configuration = configuration ??
                            throw new ArgumentNullException(nameof(configuration));

            configuration.Add(typeof(NoEmptyInterceptor<>).MakeGenericType(configuration.Configuration.ClipType));
            return configuration;
        }

        /// <summary>
        ///     Adds max size validation interceptor.
        /// </summary>
        /// <param name="configuration"><see cref="IClippoCoreConfiguration"/></param>
        /// <param name="size">Max file size</param>
        /// <returns><see cref="IClippoCoreConfiguration"/></returns>
        public static IClippoCoreInterceptorsConfiguration AddMaxSize(
            [NotNull] this IClippoCoreInterceptorsConfiguration configuration,
            int size)
        {
            configuration = configuration ??
                            throw new ArgumentNullException(nameof(configuration));

            configuration.Configuration.Services.AddSingleton(new MaxSizeSettings(size));
            configuration.Add(typeof(MaxSizeInterceptor<>).MakeGenericType(configuration.Configuration.ClipType));
            return configuration;
        }
    }
}