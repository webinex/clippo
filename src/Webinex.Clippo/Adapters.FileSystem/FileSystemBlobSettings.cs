using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Webinex.Clippo.Adapters.FileSystem
{
    internal class FileSystemBlobSettings
    {
        public FileSystemBlobSettings([NotNull] string basePath)
        {
            BasePath = basePath ?? throw new ArgumentNullException(nameof(basePath));

            if (!Path.IsPathFullyQualified(BasePath))
                throw new ArgumentException("Might be fully qualified", nameof(basePath));
        }

        [NotNull]
        public string BasePath { get; }
    }
}