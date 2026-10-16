namespace Contacts;

// see notes on Student record class
public record Employee(FullName Name, string Email) : IContact
{
    public Guid Id { get; init; } = Guid.NewGuid();
    
    public virtual bool Equals(Employee? other)
    {
        return other is not null
               && Name == other.Name
               && Email == other.Email;
    }
    
    public override int GetHashCode() => HashCode.Combine(Name, Email);
}