using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Webinex.Clippo.Ports.Model;

namespace Webinex.Clippo.Adapters.EfCore
{
    public static class ClippoCoreConfigurationExtensions
    {
        private const string DB_CONTEXT_TYPE_KEY =
            "Webinex.Clippo.Adapters.EfCore.ClippoCoreConfigurationExtensions.DB_CONTEXT_TYPE_KEY";

        /// <summary>
        ///     Adds EFCore DbContext as <see cref="IClippoStore{TClip}"/>
        /// </summary>
        /// <param name="coreConfiguration"><see cref="IClippoCoreConfiguration"/></param>
        /// <typeparam name="TDbContext">Type of <see cref="IClippoDbContext{TClip}"/> implementation</typeparam>
        /// <returns><see cref="IClippoCoreConfiguration"/></returns>
        /// <exception cref="ArgumentException">
        ///     Thrown if <typeparamref name="TDbContext"/> not subtype of <see cref="IClippoDbContext{TClip}"/>
        /// </exception>
        public static IClippoCoreConfiguration AddDbContext<TDbContext>(
            this IClippoCoreConfiguration coreConfiguration)
        {
            coreConfiguration = coreConfiguration ?? throw new ArgumentNullException(nameof(coreConfiguration));

            var services = coreConfiguration.Services;

            if (!typeof(DbContext).IsAssignableFrom(typeof(TDbContext)))
                throw new ArgumentException("Might be assignable to DbContext", nameof(TDbContext));

            if (!typeof(IClippoDbContext<>).MakeGenericType(coreConfiguration.ClipType)
                .IsAssignableFrom(typeof(TDbContext)))
                throw new ArgumentException("Might be assignable to IClippoDbContext", nameof(TDbContext));

            services.AddScoped(
                typeof(IClippoStore<>).MakeGenericType(coreConfiguration.ClipType),
                typeof(ClippoDbContextStore<,>).MakeGenericType(coreConfiguration.ClipType, typeof(TDbContext)));

            coreConfiguration.Values[DB_CONTEXT_TYPE_KEY] = typeof(TDbContext);

            return coreConfiguration;
        }

        /// <summary>
        ///     Adds interceptor which calls `dbContext.SaveChangesAsync` on succeed apply and store calls
        /// </summary>
        /// <param name="coreConfiguration"><see cref="IClippoCoreConfiguration"/></param>
        /// <returns><see cref="IClippoCoreConfiguration"/></returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if called before <see cref="AddDbContext{TDbContext}"/>
        /// </exception>
        public static IClippoCoreInterceptorsConfiguration AddSaveChanges(
            this IClippoCoreInterceptorsConfiguration coreConfiguration)
        {
            coreConfiguration = coreConfiguration ?? throw new ArgumentNullException(nameof(coreConfiguration));

            if (!coreConfiguration.Configuration.Values.TryGetValue(DB_CONTEXT_TYPE_KEY, out var dbContextType))
                throw new InvalidOperationException(
                    $"You might call {nameof(AddDbContext)} before calling {nameof(AddSaveChanges)}");

            var interceptorType = typeof(DbContextSaveChangesInterceptor<,>)
                .MakeGenericType(coreConfiguration.Configuration.ClipType, (Type)dbContextType);

            coreConfiguration.Add(interceptorType);
            coreConfiguration.Configuration.Services.TryAddSingleton<IDbContextSaveChangesPredicate>(
                new DefaultDbContextSaveChangesPredicate());

            return coreConfiguration;
        }
    }
}