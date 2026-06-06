using Contacts.Application.Contracts.Repository;
using Contacts.Infrastrucure.Contexts;

namespace Contacts.Infrastrucure.Repositories;

public class UnitOfWork(ContactsContext context) : IUnitOfWork
{
    private IContactRepository? _contactRepository;

    public IContactRepository ContactRepository
    {
        get
        {
            _contactRepository ??= new ContactRepository(context);
            return _contactRepository;
        }
    }
}
