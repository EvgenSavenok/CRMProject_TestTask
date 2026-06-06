using Contacts.Application.Constants;
using Contacts.Application.Contracts.Repository;
using Contacts.Application.DTOs;
using Contacts.Application.Interfaces.UseCases;
using Contacts.Application.MappingProfiles;
using Contacts.Application.RequestFeatures;
using Contacts.Domain.Entities;
using Contacts.Domain.ErrorHandlers;
using FluentValidation;

namespace Contacts.Application.UseCases;

public class ContactUseCase(
    IUnitOfWork unitOfWork,
    IValidator<Contact> validator)
    : IContactUseCase
{
    public async Task<PagedResult<ContactDto>> GetAllAsync(
        ContactQueryParameters parameters,
        CancellationToken cancellationToken)
    {
        var paged = await unitOfWork.ContactRepository.GetAllContactsAsync(parameters, cancellationToken);

        return new PagedResult<ContactDto>
        {
            Items = ContactMapper.EntitiesToDtos(paged.Items),
            TotalCount = paged.TotalCount,
            PageNumber = paged.PageNumber,
            PageSize = paged.PageSize
        };
    }

    public async Task<ContactDto> GetByIdAsync(int contactId, CancellationToken cancellationToken)
    {
        var contact = await unitOfWork.ContactRepository.GetContactByIdAsync(contactId, cancellationToken)
                      ?? throw new NotFoundException(string.Format(ContactMessages.ContactNotFound, contactId));

        return ContactMapper.EntityToDto(contact);
    }

    public async Task CreateAsync(ContactDto contactDto, CancellationToken cancellationToken)
    {
        var entity = ContactMapper.DtoToEntity(contactDto);

        var result = await validator.ValidateAsync(entity, cancellationToken);
        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }

        await unitOfWork.ContactRepository.CreateAsync(entity, cancellationToken);
    }

    public async Task UpdateAsync(int contactId, ContactDto contactDto, CancellationToken cancellationToken)
    {
        var entity = await unitOfWork.ContactRepository.GetTrackedContactByIdAsync(contactId, cancellationToken)
                     ?? throw new NotFoundException(string.Format(ContactMessages.ContactNotFound, contactId));

        ContactMapper.ApplyDtoToEntity(contactDto, ref entity);

        var result = await validator.ValidateAsync(entity, cancellationToken);
        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }

        await unitOfWork.ContactRepository.UpdateAsync(entity, cancellationToken);
    }

    public async Task DeleteAsync(int contactId, CancellationToken cancellationToken)
    {
        var entity = await unitOfWork.ContactRepository.GetContactByIdAsync(contactId, cancellationToken)
                     ?? throw new NotFoundException(string.Format(ContactMessages.ContactNotFound, contactId));

        await unitOfWork.ContactRepository.DeleteAsync(entity, cancellationToken);
    }
}