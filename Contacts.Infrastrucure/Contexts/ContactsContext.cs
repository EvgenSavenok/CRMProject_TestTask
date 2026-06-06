using Contacts.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Infrastrucure.Contexts;

public class ContactsContext(DbContextOptions<ContactsContext> options) : DbContext(options)
{
    public DbSet<Contact> Contacts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContactsContext).Assembly);
    }
}
