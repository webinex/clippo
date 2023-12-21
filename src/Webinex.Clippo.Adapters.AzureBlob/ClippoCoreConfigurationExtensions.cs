using System;
using Microsoft.Extensions.DependencyInjection;
using Webinex.Clippo.Ports.Blob;

namespace Webinex.Clippo.Adapters.AzureBlob
{
    public static class ClippoCoreConfigurationExtensions
    {
        /// <summary>
        ///     Adds Azure Storage Blob store
        /// </summary>
        public static IClippoCoreConfiguration AddAzureBlobStore(
            this IClippoCoreConfiguration @this,
            string connectionString,
            string container)
        {
            connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            container = container ?? throw new ArgumentNullException(nameof(container));

            var settings = new ClippoAzureBlobStoreSettings(connectionString, container);
            @this.Services.AddScoped(_ => settings);
            @this.Services.AddScoped<IClippoBlobStore, AzureBlobStore>();

            return @this;
        }
    }
}