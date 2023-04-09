using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Webinex.Clippo
{
    /// <summary>
    ///     Store clip file content
    /// </summary>
    public class StoreClipContent
    {
        public StoreClipContent(
            [NotNull] string fileName,
            [NotNull] string mimeType,
            [NotNull] Stream value)
        {
            FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
            MimeType = mimeType ?? throw new ArgumentNullException(nameof(mimeType));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        ///     File name
        /// </summary>
        [NotNull]
        public string FileName { get; }

        /// <summary>
        ///     Mime type
        /// </summary>
        [NotNull]
        public string MimeType { get; }

        /// <summary>
        ///     File content stream
        /// </summary>
        [NotNull]
        public Stream Value { get; }
    }
}