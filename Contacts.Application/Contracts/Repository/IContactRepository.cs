using System.Threading;
using System.Threading.Tasks;
using Contacts.Application.RequestFeatures;
using Contacts.Domain.Entities;

namespace Contacts.Application.Contracts.Repository;

public interface IContactRepository : IBaseRepository<Contact>
{
    Task<Contact?> GetContactByIdAsync(int contactId, CancellationToken cancellationToken);
    Task<Contact?> GetTrackedContactByIdAsync(int contactId, CancellationToken cancellationToken);
    Task<PagedResult<Contact>> GetAllContactsAsync(ContactQueryParameters parameters, CancellationToken cancellationToken);
}
