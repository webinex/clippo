using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Webinex.Clippo.Ports.Blob;

namespace Webinex.Clippo.Adapters.FileSystem
{
    internal class FileSystemBlobStore : IClippoBlobStore
    {
        private static bool _basePathExists;
        private static readonly object BasePathLock = new object();

        private readonly FileSystemBlobSettings _settings;

        public FileSystemBlobStore(FileSystemBlobSettings settings)
        {
            _settings = settings;
        }

        public Task<IDictionary<string, Stream>> GetAsync(
            IEnumerable<string> references,
            CancellationToken cancellationToken)
        {
            references = references?.Distinct().ToArray() ?? throw new ArgumentNullException(nameof(references));

            var result = Get(references, cancellationToken);
            return Task.FromResult<IDictionary<string, Stream>>(new Dictionary<string, Stream>(result));
        }

        private IEnumerable<KeyValuePair<string, Stream>> Get(
            IEnumerable<string> references,
            CancellationToken cancellationToken)
        {
            var result = new LinkedList<KeyValuePair<string, Stream>>();
            foreach (var reference in references)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                Stream stream = File.OpenRead(Path(reference));
                result.AddLast(KeyValuePair.Create(reference, stream));
            }
            
            if (cancellationToken.IsCancellationRequested)
                DisposeAndThrowCancellation(result, cancellationToken);

            return result;
        }

        private void DisposeAndThrowCancellation(
            IEnumerable<KeyValuePair<string, Stream>> entries,
            CancellationToken cancellationToken)
        {
            var exceptions = new LinkedList<Exception>();
                
            foreach (var entry in entries)
            {
                try
                {
                    entry.Value.Dispose();
                }
                catch (Exception ex)
                {
                    exceptions.AddLast(ex);
                }
            }

            if (exceptions.Any())
                throw new AggregateException(exceptions);
                
            cancellationToken.ThrowIfCancellationRequested();
        }

        public Task<IDictionary<Stream, string>> StoreAsync(
            IEnumerable<Stream> contents,
            CancellationToken cancellationToken)
        {
            EnsureBasePathExists();
            contents = contents?.Distinct().ToArray() ?? throw new ArgumentNullException(nameof(contents));

            var result = contents.ToDictionary(x => x, x =>
            {
                cancellationToken.ThrowIfCancellationRequested();

                var reference = Guid.NewGuid().ToString();
                var path = Path(reference);
                using var fsStream = File.OpenWrite(path);
                x.CopyTo(fsStream);
                return reference;
            });

            return Task.FromResult<IDictionary<Stream, string>>(result);
        }

        private void EnsureBasePathExists()
        {
            lock (BasePathLock)
            {
                if (_basePathExists)
                {
                    return;
                }

                if (Directory.Exists(_settings.BasePath))
                {
                    _basePathExists = true;
                    return;
                }

                Directory.CreateDirectory(_settings.BasePath);
                _basePathExists = true;
            }
        }

        private string Path(string reference)
        {
            return System.IO.Path.Combine(_settings.BasePath, reference);
        }
    }
}