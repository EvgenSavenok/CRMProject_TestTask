using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Contacts.Application.Contracts.Repository;

public interface IBaseRepository<T>
{
    Task<IEnumerable<T>> FindByConditionAsync(
        Expression<Func<T, bool>> expression,
        CancellationToken cancellationToken,
        params Expression<Func<T, object>>[] includes);

    Task<IEnumerable<T>> FindByConditionTrackedAsync(
        Expression<Func<T, bool>> expression,
        CancellationToken cancellationToken,
        params Expression<Func<T, object>>[] includes);

    Task CreateAsync(T entity, CancellationToken cancellationToken);
    Task UpdateAsync(T entity, CancellationToken cancellationToken);
    Task DeleteAsync(T entity, CancellationToken cancellationToken);
}
