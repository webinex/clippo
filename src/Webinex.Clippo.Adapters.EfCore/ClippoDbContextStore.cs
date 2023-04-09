using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Webinex.Clippo.Ports.Model;

namespace Webinex.Clippo.Adapters.EfCore
{
    /// <summary>
    ///     <see cref="IClippoStore{TClip}"/> for <typeparamref name="TDbContext"/>
    /// </summary>
    /// <typeparam name="TClip">Clip type</typeparam>
    /// <typeparam name="TDbContext">DbContext type</typeparam>
    public class ClippoDbContextStore<TClip, TDbContext> : IClippoStore<TClip>
        where TClip : class
        where TDbContext : DbContext, IClippoDbContext<TClip>
    {
        private readonly TDbContext _dbContext;
        private readonly IClippoModelDefinition<TClip> _model;

        public ClippoDbContextStore(TDbContext dbContext, IClippoModelDefinition<TClip> model)
        {
            _dbContext = dbContext;
            _model = model;
        }

        /// <inheritdoc />
        public virtual async Task<TClip[]> GetAllAsync(GetClipsArgs args)
        {
            args = args ?? throw new ArgumentNullException(nameof(args));
            
            var queryable = _dbContext.Clips.AsQueryable();
            
            if (args.Directory != null)
                queryable = queryable.Where(_model.WhereDirectoryEquals(args.Directory));

            if (args.OwnerType != null)
                queryable = queryable.Where(_model.WhereOwnerTypeEquals(args.OwnerType));

            if (args.OwnerId != null)
                queryable = queryable.Where(_model.WhereOwnerIdEquals(args.OwnerId));

            if (args.Options != GetClipOptions.IncludeInactive)
                queryable = queryable.Where(_model.WhereActive(true));

            return await queryable.ToArrayAsync();
        }

        /// <inheritdoc />
        public virtual async Task<TClip[]> GetByIdsAsync(IEnumerable<string> ids, GetClipOptions options = GetClipOptions.None)
        {
            ids = ids ?? throw new ArgumentNullException(nameof(ids));

            var queryable = _dbContext.Clips.Where(_model.WhereIdIn(ids));
            if (options != GetClipOptions.IncludeInactive)
                queryable = queryable.Where(_model.WhereActive(true));

            return await queryable.ToArrayAsync();
        }

        /// <inheritdoc />
        public virtual Task<TClip[]> AddAsync(IEnumerable<TClip> clipEnumerable)
        {
            var clips = clipEnumerable?.ToArray() ?? throw new ArgumentNullException(nameof(clipEnumerable));   
            
            _dbContext.Clips.AddRange(clips);
            return Task.FromResult(clips.ToArray());
        }

        /// <inheritdoc />
        public virtual Task<TClip[]> UpdateAsync(IEnumerable<TClip> clipEnumerable)
        {
            var clips = clipEnumerable?.ToArray() ?? throw new ArgumentNullException(nameof(clipEnumerable));

            foreach (var clip in clips)
            {
                var entry = _dbContext.Entry(clip);
                if (entry.State == EntityState.Unchanged)
                    _dbContext.Clips.Update(clip);
            }

            return Task.FromResult(clips);
        }

        /// <inheritdoc />
        public virtual Task DeleteAsync(IEnumerable<TClip> clipEnumerable)
        {
            var clips = clipEnumerable?.ToArray() ?? throw new ArgumentNullException(nameof(clipEnumerable));
            
            _dbContext.Clips.RemoveRange(clips);
            return Task.CompletedTask;
        }
    }
}