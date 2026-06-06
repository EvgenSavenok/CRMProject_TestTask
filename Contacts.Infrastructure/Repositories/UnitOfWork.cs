using Contacts.Application.Contracts.Repository;
using Contacts.Infrastructure.Contexts;

namespace Contacts.Infrastructure.Repositories;

public class UnitOfWork(ContactsContext context) : IUnitOfWork
{
    public IContactRepository ContactRepository
    {
        get
        {
            field ??= new ContactRepository(context);
            return field;
        }
    }
}