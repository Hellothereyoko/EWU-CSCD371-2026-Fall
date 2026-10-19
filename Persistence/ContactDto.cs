namespace Persistence;

public record ContactDto
(
    Guid Id,
    string ContactType,
    string FirstName,
    string LastName,
    string? MiddleName,
    string[] Emails
);