namespace Contacts.Application.Constants;

public static class ContactMessages
{
    public const string NameRequired = "Name is required.";
    public const string NameTooLong = "Name cannot exceed 1000 characters.";

    public const string MobilePhoneRequired = "Mobile phone is required.";
    public const string MobilePhoneTooLong = "Mobile phone cannot exceed 20 characters.";
    public const string MobilePhoneInvalid = "Mobile phone must be a valid phone number.";

    public const string JobTitleTooLong = "Job title cannot exceed 100 characters.";

    public const string BirthDateMustBeInPast = "Birth date must be in the past.";
    public const string BirthDateNotRealistic = "Birth date is not realistic.";

    public const string ContactNotFound = "Contact with id {0} not found.";
}