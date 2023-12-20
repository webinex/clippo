using System;
using System.Diagnostics.CodeAnalysis;

namespace Webinex.Clippo.AzureBlob
{
    public class ClippoAzureBlobStoreSettings
    {
        public ClippoAzureBlobStoreSettings(
            [NotNull] string connectionString,
            [NotNull] string container)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            Container = container ?? throw new ArgumentNullException(nameof(container));
        }

        [NotNull]
        public string ConnectionString { get; }

        [NotNull]
        public string Container { get; }
    }
}