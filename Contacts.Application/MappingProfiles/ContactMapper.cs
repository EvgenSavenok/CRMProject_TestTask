using System.Collections.Generic;
using System.Linq;
using Contacts.Application.DTOs;
using Contacts.Domain.Entities;

namespace Contacts.Application.MappingProfiles;

public static class ContactMapper
{
    public static ContactDto EntityToDto(Contact contact)
    {
        return new ContactDto
        {
            Id = contact.ContactId,
            Name = contact.Name,
            MobilePhone = contact.MobilePhone,
            JobTitle = contact.JobTitle,
            BirthDate = contact.BirthDate
        };
    }

    public static IEnumerable<ContactDto> EntitiesToDtos(IEnumerable<Contact> contacts)
    {
        return contacts.Select(EntityToDto);
    }

    public static Contact DtoToEntity(ContactDto dto)
    {
        return new Contact
        {
            Name = dto.Name,
            MobilePhone = dto.MobilePhone,
            JobTitle = dto.JobTitle,
            BirthDate = dto.BirthDate
        };
    }

    public static void ApplyDtoToEntity(ContactDto dto, ref Contact contact)
    {
        contact.Name = dto.Name;
        contact.MobilePhone = dto.MobilePhone;
        contact.JobTitle = dto.JobTitle;
        contact.BirthDate = dto.BirthDate;
    }
}
