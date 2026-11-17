namespace Assignment8.ResilientImporter;

public sealed class Person
{
    public Person(int id, string firstName, string lastName, string email, Address address)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(id));
        ArgumentException.ThrowIfNullOrWhiteSpace(firstName, nameof(firstName));
        ArgumentException.ThrowIfNullOrWhiteSpace(lastName, nameof(lastName));
        ArgumentException.ThrowIfNullOrWhiteSpace(email, nameof(email));
        ArgumentNullException.ThrowIfNull(address, nameof(address));

        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Address = address;
    }

    public int Id { get; }

    public string FirstName { get; }

    public string LastName { get; }

    public string Email { get; }

    public Address Address { get; }
}
