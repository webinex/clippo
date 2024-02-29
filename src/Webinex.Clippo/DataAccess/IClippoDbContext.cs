using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Webinex.Clippo;

public interface IClippoDbContext<TMeta, TData>
    where TMeta : class, ICloneable
    where TData : class, ICloneable
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    Task SaveChangesAsync();
}