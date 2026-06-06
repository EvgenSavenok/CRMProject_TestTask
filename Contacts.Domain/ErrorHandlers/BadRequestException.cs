using System;

namespace Contacts.Domain.ErrorHandlers;

public class BadRequestException(string message) : Exception(message);
