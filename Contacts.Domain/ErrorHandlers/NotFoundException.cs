using System;

namespace Contacts.Domain.ErrorHandlers;

public class NotFoundException(string message) : Exception(message);
