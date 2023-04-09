using Microsoft.EntityFrameworkCore;

namespace Webinex.Clippo.Adapters.EfCore
{
    /// <summary>
    ///     Clippo DbContext
    /// </summary>
    /// <typeparam name="TClip">Clip type</typeparam>
    public interface IClippoDbContext<TClip>
        where TClip : class
    {
        public DbSet<TClip> Clips { get; }
    }
}