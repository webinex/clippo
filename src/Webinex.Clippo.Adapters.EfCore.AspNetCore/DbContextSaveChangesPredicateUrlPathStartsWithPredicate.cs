using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Webinex.Clippo.Adapters.EfCore.AspNetCore
{
    internal class DbContextSaveChangesPredicateUrlPathStartsWithPredicate : IDbContextSaveChangesPredicate
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DbContextSaveChangesWhenUrlPathStartsWithPredicateSettings _predicateSettings;

        public DbContextSaveChangesPredicateUrlPathStartsWithPredicate(
            IHttpContextAccessor httpContextAccessor,
            DbContextSaveChangesWhenUrlPathStartsWithPredicateSettings predicateSettings)
        {
            _httpContextAccessor = httpContextAccessor;
            _predicateSettings = predicateSettings;
        }

        public Task<bool> InvokeAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var path = httpContext?.Request.Path;
            var result = path?.StartsWithSegments(_predicateSettings.Path) == true;
            return Task.FromResult(result);
        }
    }
}