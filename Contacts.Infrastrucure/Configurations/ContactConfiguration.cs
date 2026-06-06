using Contacts.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contacts.Infrastrucure.Configurations;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.HasKey(c => c.ContactId);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.MobilePhone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(c => c.JobTitle)
            .HasMaxLength(100);

        builder.Property(c => c.BirthDate)
            .IsRequired();
    }
}
