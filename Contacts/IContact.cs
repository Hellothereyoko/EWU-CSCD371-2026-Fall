namespace Contacts;

/// <summary>
/// This interface defines the structure for a contact, including properties for first name, middle name, last name, and record type. Any class implementing this interface must provide implementations for these properties.
/// </summary>
public interface IContact
{
    Guid Id { get; } // easiest way to demo ID considerations - could be replaced with something more readable / defined
    FullName Name { get; }
    string Email { get; } // probably replaced with List of emails at least for EC. Else replace with some ContactInfo record to possibly add phone numbers or more?
}
