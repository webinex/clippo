using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Webinex.Clippo.Ports.Model
{
    public static class ClippoStoreExtensions
    {
        public static async Task<TClip> ByIdAsync<TClip>(
            [NotNull] this IClippoStore<TClip> store,
            [NotNull] string id,
            GetClipOptions options = GetClipOptions.None)
        {
            store = store ?? throw new ArgumentNullException(nameof(store));
            id = id ?? throw new ArgumentNullException(nameof(id));

            var result = await store.GetByIdsAsync(new[] { id }, options);
            return result.SingleOrDefault();
        }

        public static async Task<TClip> UpdateAsync<TClip>(
            [NotNull] this IClippoStore<TClip> store,
            [NotNull] TClip clip)
        {
            store = store ?? throw new ArgumentNullException(nameof(store));
            clip = clip ?? throw new ArgumentNullException(nameof(clip));

            var result = await store.UpdateAsync(new[] { clip });
            return result.Single();
        }

        public static async Task DeleteAsync<TClip>(
            [NotNull] this IClippoStore<TClip> store,
            [NotNull] TClip clip)
        {
            store = store ?? throw new ArgumentNullException(nameof(store));
            clip = clip ?? throw new ArgumentNullException(nameof(clip));

            await store.DeleteAsync(new[] { clip });
        }
    }
}