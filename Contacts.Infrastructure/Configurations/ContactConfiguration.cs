using Contacts.Domain.Constants;
using Contacts.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contacts.Infrastructure.Configurations;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.HasKey(c => c.ContactId);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(ContactConstants.NameMaxLength);

        builder.Property(c => c.MobilePhone)
            .IsRequired()
            .HasMaxLength(ContactConstants.MobilePhoneMaxLength);

        builder.Property(c => c.JobTitle)
            .HasMaxLength(ContactConstants.JobTitleMaxLength);

        builder.Property(c => c.BirthDate)
            .IsRequired();
    }
}