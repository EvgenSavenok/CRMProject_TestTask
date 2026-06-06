namespace Contacts.Application.Contracts.Repository;

public interface IUnitOfWork
{
    IContactRepository ContactRepository { get; }
}
