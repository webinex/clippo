using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Webinex.Clippo.Interceptors;
using Webinex.Coded;

namespace Webinex.Clippo.Adapters.EfCore
{
    internal class DbContextSaveChangesInterceptor<TClip, TDbContext> : ClippoInterceptor<TClip>
        where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;
        private readonly IDbContextSaveChangesPredicate _predicate;

        public DbContextSaveChangesInterceptor(TDbContext dbContext, IDbContextSaveChangesPredicate predicate)
        {
            _dbContext = dbContext;
            _predicate = predicate;
        }

        public override async Task<CodedResult<TClip[]>> OnStoreAsync(IEnumerable<StoreClipArgs> args, INext<IEnumerable<StoreClipArgs>, CodedResult<TClip[]>> next)
        {
            var result = await next.InvokeAsync(args);
            if (result.Succeed && await _predicate.InvokeAsync())
                await _dbContext.SaveChangesAsync();

            return result;
        }

        public override async Task<CodedResult> OnApplyAsync(IEnumerable<IClippoAction> actions, INext<IEnumerable<IClippoAction>, CodedResult> next)
        {
            var result = await next.InvokeAsync(actions);
            if (result.Succeed && await _predicate.InvokeAsync())
                await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}