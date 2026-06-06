using Contacts.Application.Contracts.Repository;
using Contacts.Infrastructure.Contexts;
using Contacts.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Contacts.Infrastructure.Extensions;

public static class DbExtension
{
    public static void ConfigureContactsSqlContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ContactsContext>(opts =>
            opts.UseNpgsql(configuration.GetConnectionString("contactsConnection")));
    }

    public static void ConfigureUnitOfWork(this IServiceCollection services) =>
        services.AddScoped<IUnitOfWork, UnitOfWork>();

    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ContactsContext>();
        context.Database.Migrate();
    }
}