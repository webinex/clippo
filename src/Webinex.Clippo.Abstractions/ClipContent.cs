using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Webinex.Clippo
{
    /// <summary>
    ///     Clip file content
    /// </summary>
    public class ClipContent
    {
        public ClipContent(
            [NotNull] string id,
            [NotNull] string fileName,
            [NotNull] string mimeType,
            int sizeBytes,
            [NotNull] Stream value)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
            MimeType = mimeType ?? throw new ArgumentNullException(nameof(mimeType));
            SizeBytes = sizeBytes;
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        ///     Clip identifier
        /// </summary>
        [NotNull] public string Id { get; }
        
        /// <summary>
        ///     File name
        /// </summary>
        [NotNull] public string FileName { get; }
        
        /// <summary>
        ///     File mime type
        /// </summary>
        [NotNull] public string MimeType { get; }
        
        /// <summary>
        ///     File size in bytes
        /// </summary>
        public int SizeBytes { get; }
        
        /// <summary>
        ///     File content stream
        /// </summary>
        [NotNull] public Stream Value { get; }
    }
}