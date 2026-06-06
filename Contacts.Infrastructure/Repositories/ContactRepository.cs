using Contacts.Application.Contracts.Repository;
using Contacts.Application.RequestFeatures;
using Contacts.Domain.Entities;
using Contacts.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Infrastructure.Repositories;

public class ContactRepository(ContactsContext context)
    : BaseRepository<Contact>(context), IContactRepository
{
    private readonly ContactsContext _context = context;

    public async Task<Contact?> GetContactByIdAsync(int contactId, CancellationToken cancellationToken)
    {
        return await _context.Contacts
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.ContactId == contactId, cancellationToken);
    }

    public async Task<Contact?> GetTrackedContactByIdAsync(int contactId, CancellationToken cancellationToken)
    {
        return await _context.Contacts
            .FirstOrDefaultAsync(c => c.ContactId == contactId, cancellationToken);
    }

    public async Task<PagedResult<Contact>> GetAllContactsAsync(
        ContactQueryParameters parameters,
        CancellationToken cancellationToken)
    {
        var query = _context.Contacts.AsNoTracking();

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