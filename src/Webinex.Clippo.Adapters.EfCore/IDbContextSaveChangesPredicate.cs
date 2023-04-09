using System.Threading.Tasks;

namespace Webinex.Clippo.Adapters.EfCore
{
    /// <summary>
    ///     Predicate to control when .SaveChanges called by .AddSaveChanges() interceptor
    /// </summary>
    public interface IDbContextSaveChangesPredicate
    {
        /// <summary>
        ///     Return true if SaveChanges might be called, false otherwise
        /// </summary>
        /// <returns>True if SaveChanges might be called, false otherwise</returns>
        Task<bool> InvokeAsync();
    }
}