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
            contact.Email
        );
    }
    
    private static IContact ToContact(ContactDto dto)
    {
        FullName fullName = new(
            dto.FirstName,
            dto.LastName,
            dto.MiddleName
        );

        return dto.ContactType switch
        {
            nameof(Student) => new Student(
                fullName,
                dto.Email
            ) { Id = dto.Id },

            nameof(Employee) => new Employee(
                fullName,
                dto.Email
            ) { Id = dto.Id },

            _ => throw new InvalidOperationException(
                $"Unknown contact type: '{dto.ContactType}'.")
        };
    }
}