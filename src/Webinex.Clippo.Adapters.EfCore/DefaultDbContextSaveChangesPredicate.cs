using System.Threading.Tasks;

namespace Webinex.Clippo.Adapters.EfCore
{
    internal class DefaultDbContextSaveChangesPredicate : IDbContextSaveChangesPredicate
    {
        public Task<bool> InvokeAsync()
        {
            return Task.FromResult(true);
        }
    }
}