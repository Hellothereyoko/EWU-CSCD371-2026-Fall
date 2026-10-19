using System.Text.Json;
using Contacts;

namespace Persistence;

public class JsonDataService(string filePath) : IDataService
{
    public void Save(IEnumerable<IContact> contacts)
    {
        List<ContactDto> dtos = contacts
            .Select(ToDto)
            .ToList();
        
        File.WriteAllText(filePath, JsonSerializer.Serialize(dtos));
    }

    public IContact[] Load()
    {
        if (!File.Exists(filePath))
            return [];
        
        string json = File.ReadAllText(filePath);
        
        List<ContactDto>? dtos = JsonSerializer.Deserialize<List<ContactDto>>(json);

        if (dtos == null)
            return [];
        
        return dtos
            .Select(ToContact)
            .ToArray();
    }

    private static ContactDto ToDto(IContact contact)
    {
        return new ContactDto(
            contact.Id,
            contact.GetType().Name,
            contact.Name.FirstName,
            contact.Name.LastName,
            contact.Name.MiddleName,
            contact.Emails.Emails.ToArray()
        );
    }
    
    private static IContact ToContact(ContactDto dto)
    {
        // create a full name record
        FullName fullName = new(
            dto.FirstName,
            dto.LastName,
            dto.MiddleName
        );

        // create an email record
        ContactEmails emails = new(dto.Emails);

        // return IContact created with those records according to type
        return dto.ContactType switch
        {
            nameof(Student) => new Student(
                fullName,
                emails
            ) { Id = dto.Id },

            nameof(Employee) => new Employee(
                fullName,
                emails
            ) { Id = dto.Id },

            _ => throw new InvalidOperationException(
                $"Unknown contact type: '{dto.ContactType}'.")
        };
    }
}