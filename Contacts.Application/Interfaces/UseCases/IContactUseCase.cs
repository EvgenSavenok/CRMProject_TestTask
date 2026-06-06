using System.Threading;
using System.Threading.Tasks;
using Contacts.Application.DTOs;
using Contacts.Application.RequestFeatures;

namespace Contacts.Application.Interfaces.UseCases;

public interface IContactUseCase
{
    Task<PagedResult<ContactDto>> GetAllAsync(ContactQueryParameters parameters, CancellationToken cancellationToken);
    Task<ContactDto> GetByIdAsync(int contactId, CancellationToken cancellationToken);
    Task CreateAsync(ContactDto contactDto, CancellationToken cancellationToken);
    Task UpdateAsync(int contactId, ContactDto contactDto, CancellationToken cancellationToken);
    Task DeleteAsync(int contactId, CancellationToken cancellationToken);
}
