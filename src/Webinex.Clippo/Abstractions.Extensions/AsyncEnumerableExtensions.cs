using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Webinex.Clippo;

internal static class AsyncEnumerableExtensions
{
    public static async Task<List<T>> ToListAsync<T>(
        this IAsyncEnumerable<T> items,
        CancellationToken cancellationToken = default)
    {
        var results = new List<T>();
        await foreach (var item in items.WithCancellation(cancellationToken))
        {
            results.Add(item);
        }

        return results;
    }
}