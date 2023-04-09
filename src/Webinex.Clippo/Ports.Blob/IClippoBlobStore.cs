using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Webinex.Clippo.Ports.Blob
{
    /// <summary>
    ///     Port for blob storage. You can provide your own implementation if you would like to store
    ///     blobs to stores which have no built-in support.
    /// </summary>
    public interface IClippoBlobStore
    {
        /// <summary>
        ///     Gets file contents
        /// </summary>
        /// <param name="references">Files references</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>File content streams by reference</returns>
        Task<IDictionary<string, Stream>> GetAsync(
            [NotNull] IEnumerable<string> references,
            CancellationToken cancellationToken);

        /// <summary>
        ///     Stores file contents
        /// </summary>
        /// <param name="contents">File content streams</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>File reference by file content</returns>
        Task<IDictionary<Stream, string>> StoreAsync(
            [NotNull] IEnumerable<Stream> contents,
            CancellationToken cancellationToken);
    }
}