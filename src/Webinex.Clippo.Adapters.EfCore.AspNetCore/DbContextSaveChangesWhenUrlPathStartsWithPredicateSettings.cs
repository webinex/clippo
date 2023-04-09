using System;
using System.Diagnostics.CodeAnalysis;

namespace Webinex.Clippo.Adapters.EfCore.AspNetCore
{
    internal class DbContextSaveChangesWhenUrlPathStartsWithPredicateSettings
    {
        public DbContextSaveChangesWhenUrlPathStartsWithPredicateSettings([NotNull] string path)
        {
            Path = path ?? throw new ArgumentNullException(nameof(path));
        }

        [NotNull]
        public string Path { get; }
    }
}