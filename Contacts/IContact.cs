namespace Contacts;

/// <summary>
/// This interface defines the structure for a contact, including properties for first name, middle name, last name, and record type. Any class implementing this interface must provide implementations for these properties.
/// </summary>
public interface IContact
{
    String FirstName {get; set;}
    String MiddleName {get; set;}
    String LastName {get; set;}
    String RecordType {get; set;}
}
