namespace Contacts;

// nullability defines a non-null must be provided for first and last
// if no middle provided, it will default null
public record FullName(
    string FirstName,
    string LastName,
    string? MiddleName = null
);