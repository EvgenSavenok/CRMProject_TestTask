using Contacts.Application.Validation;
using Contacts.Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Contacts.Infrastructure.Extensions;

public static class ValidationExtension
{
    public static void AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<Contact>, ContactValidator>();
    }
}