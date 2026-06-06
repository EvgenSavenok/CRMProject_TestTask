using System;

namespace Contacts.Domain.Entities;

public class Contact
{
    public int ContactId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string MobilePhone { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public DateOnly BirthDate { get; set; }
}
