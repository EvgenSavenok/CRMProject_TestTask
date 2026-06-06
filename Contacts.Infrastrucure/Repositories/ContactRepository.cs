using Contacts.Application.Contracts.Repository;
using Contacts.Application.RequestFeatures;
using Contacts.Domain.Entities;
using Contacts.Infrastrucure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Infrastrucure.Repositories;

public class ContactRepository(ContactsContext context)
    : BaseRepository<Contact>(context), IContactRepository
{
    public async Task<Contact?> GetContactByIdAsync(int contactId, CancellationToken cancellationToken)
    {
        return await context.Contacts
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.ContactId == contactId, cancellationToken);
    }

    public async Task<Contact?> GetTrackedContactByIdAsync(int contactId, CancellationToken cancellationToken)
    {
        return await context.Contacts
            .FirstOrDefaultAsync(c => c.ContactId == contactId, cancellationToken);
    }

    public async Task<PagedResult<Contact>> GetAllContactsAsync(
        ContactQueryParameters parameters,
        CancellationToken cancellationToken)
    {
        IQueryable<Contact> query = context.Contacts.AsNoTracking();

        var totalCount = await query.CountAsync(cancellationToken);

        query = query.Paging(parameters.PageNumber, parameters.PageSize);

        var items = await query.ToListAsync(cancellationToken);

        return new PagedResult<Contact>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize
        };
    }
}
