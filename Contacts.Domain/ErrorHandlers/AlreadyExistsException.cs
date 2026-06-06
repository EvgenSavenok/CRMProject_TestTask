using System;

namespace Contacts.Domain.ErrorHandlers;

public class AlreadyExistsException(string message) : Exception(message);
