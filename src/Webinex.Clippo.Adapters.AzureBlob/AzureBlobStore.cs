using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Webinex.Clippo.Ports.Blob;

namespace Webinex.Clippo.Adapters.AzureBlob
{
    internal class AzureBlobStore : IClippoBlobStore
    {
        private readonly Lazy<BlobContainerClient> _clientLazy;

        public AzureBlobStore(ClippoAzureBlobStoreSettings settings)
        {
            _clientLazy = new Lazy<BlobContainerClient>(() =>
                new BlobContainerClient(settings.ConnectionString, settings.Container));
        }
    
        public async Task<IDictionary<string, Stream>> GetAsync(IEnumerable<string> references, CancellationToken cancellationToken)
        {
            var result = new Dictionary<string, Stream>();
        
            foreach (var reference in references)
            {
                var blob = _clientLazy.Value.GetBlobClient(reference);
                var info = await blob.DownloadContentAsync(cancellationToken);

                Stream stream = null;
                try
                {
                    stream = info.Value.Content.ToStream();
                }
                catch (Exception exception)
                {
                    try
                    {
                        await DisposeAsync(result.Values);
                    }
                    catch (Exception disposeException)
                    {
                        throw new AggregateException(exception, disposeException);
                    }

                    throw;
                }
            
                result.Add(reference, stream);
            }

            return result;
        }

        public async Task<IDictionary<Stream, string>> StoreAsync(IEnumerable<Stream> contents, CancellationToken cancellationToken)
        {
            var result = new Dictionary<Stream, string>();
        
            foreach (var stream in contents)
            {
                var reference = Guid.NewGuid().ToString("N");
                var blob = _clientLazy.Value.GetBlobClient(reference);
                await blob.UploadAsync(stream, cancellationToken);
                result.Add(stream, reference);
            }

            return result;
        }
        
        private static async Task DisposeAsync(IEnumerable<Stream> streams)
        {
            var exceptions = new LinkedList<Exception>();

            foreach (var stream in streams)
            {
                try
                {
                    await stream.DisposeAsync();
                }
                catch (Exception ex)
                {
                    exceptions.AddLast(ex);
                }

                if (exceptions.Count == 1)
                    throw exceptions.First!.Value;

                if (exceptions.Count > 1)
                    throw new AggregateException(exceptions);
            }
        }
    }
}