namespace Contacts.Domain.Constants;

public static class ContactConstants
{
    public const int NameMaxLength = 200;
    public const int MobilePhoneMaxLength = 20;
    public const int JobTitleMaxLength = 100;

    public const string MobilePhoneRegex = @"^\+?[\d\s\-\(\)]{7,20}$";

    public static readonly DateOnly MinBirthDate = new(1900, 1, 1);
}