using Contacts.Application.Constants;
using Contacts.Domain.Constants;
using Contacts.Domain.Entities;
using FluentValidation;

namespace Contacts.Application.Validation;

public class ContactValidator : AbstractValidator<Contact>
{
    public ContactValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage(ContactMessages.NameRequired)
            .MaximumLength(ContactConstants.NameMaxLength).WithMessage(ContactMessages.NameTooLong);

        RuleFor(c => c.MobilePhone)
            .NotEmpty().WithMessage(ContactMessages.MobilePhoneRequired)
            .MaximumLength(ContactConstants.MobilePhoneMaxLength).WithMessage(ContactMessages.MobilePhoneTooLong)
            .Matches(ContactConstants.MobilePhoneRegex).WithMessage(ContactMessages.MobilePhoneInvalid);

        RuleFor(c => c.JobTitle)
            .MaximumLength(ContactConstants.JobTitleMaxLength).WithMessage(ContactMessages.JobTitleTooLong);

        RuleFor(c => c.BirthDate)
            .LessThan(DateOnly.FromDateTime(DateTime.Today)).WithMessage(ContactMessages.BirthDateMustBeInPast)
            .GreaterThan(ContactConstants.MinBirthDate).WithMessage(ContactMessages.BirthDateNotRealistic);
    }
}