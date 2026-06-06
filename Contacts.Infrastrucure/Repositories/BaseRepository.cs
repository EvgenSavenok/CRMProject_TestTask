using System.Linq.Expressions;
using Contacts.Application.Contracts.Repository;
using Contacts.Infrastrucure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Infrastrucure.Repositories;

public abstract class BaseRepository<T>(ContactsContext context) : IBaseRepository<T>
    where T : class
{
    public async Task<IEnumerable<T>> FindByConditionAsync(
        Expression<Func<T, bool>> expression,
        CancellationToken cancellationToken,
        params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = context.Set<T>().Where(expression).AsNoTracking();

        foreach (var include in includes)
            query = query.Include(include);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> FindByConditionTrackedAsync(
        Expression<Func<T, bool>> expression,
        CancellationToken cancellationToken,
        params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = context.Set<T>().Where(expression);

        foreach (var include in includes)
            query = query.Include(include);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await context.Set<T>().AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        context.Set<T>().Update(entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        context.Set<T>().Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }
}
