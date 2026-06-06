using System;
using Contacts.Domain.Entities;
using FluentValidation;

namespace Contacts.Application.Validation;

public class ContactValidator : AbstractValidator<Contact>
{
    public ContactValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters.");

        RuleFor(c => c.MobilePhone)
            .NotEmpty().WithMessage("Mobile phone is required.")
            .MaximumLength(20).WithMessage("Mobile phone cannot exceed 20 characters.")
            .Matches(@"^\+?[\d\s\-\(\)]{7,20}$").WithMessage("Mobile phone must be a valid phone number.");

        RuleFor(c => c.JobTitle)
            .MaximumLength(100).WithMessage("Job title cannot exceed 100 characters.");

        RuleFor(c => c.BirthDate)
            .LessThan(DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("Birth date must be in the past.")
            .GreaterThan(new DateOnly(1900, 1, 1))
                .WithMessage("Birth date is not realistic.");
    }
}
