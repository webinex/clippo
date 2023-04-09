using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Webinex.Clippo.Adapters.EfCore.AspNetCore
{
    public static class ClippoCoreConfigurationExtensions
    {
        /// <summary>
        ///     Adds interceptor which calls `dbContext.SaveChangesAsync` on succeed apply and store calls
        ///     when request path starts with <paramref name="path"/> segment
        /// </summary>
        /// <param name="coreConfiguration"><see cref="IClippoCoreConfiguration"/></param>
        /// <param name="path">Request path. Might start with /</param>
        /// <returns><see cref="IClippoCoreConfiguration"/></returns>,
        /// <exception cref="InvalidOperationException">Thrown if path not starts with /</exception>
        public static IClippoCoreInterceptorsConfiguration AddSaveChangesWhenUrlPathStartsWith(
            [NotNull] this IClippoCoreInterceptorsConfiguration coreConfiguration,
            [NotNull] string path)
        {
            coreConfiguration = coreConfiguration ?? throw new ArgumentNullException(nameof(coreConfiguration));
            path = path ?? throw new ArgumentNullException(nameof(path));

            if (!path.StartsWith("/"))
                throw new ArgumentException("Might start with /", nameof(path));

            coreConfiguration.Configuration.Services.AddSingleton(
                new DbContextSaveChangesWhenUrlPathStartsWithPredicateSettings(path));
            coreConfiguration.Configuration.Services
                .AddSingleton<IDbContextSaveChangesPredicate, DbContextSaveChangesPredicateUrlPathStartsWithPredicate>();

            return coreConfiguration.AddSaveChanges();
        }
    }
}