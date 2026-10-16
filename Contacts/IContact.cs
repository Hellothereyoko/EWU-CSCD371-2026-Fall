namespace Contacts;

public interface IContact
{
    Guid Id { get; }
    FullName Name { get; }
    string Email { get; }
}

public record FullName(
    string FirstName,
    string LastName,
    string? MiddleName = null
);

public record Student(FullName Name, string Email) : IContact
{
    public Guid Id { get; init; } = Guid.NewGuid();
    
    // NOTE: equality is only defined between Student Instances
    // any comparison against non-Student type is false
    public virtual bool Equals(Student? other)
    {
        // intentionally leaving ID property out of equality check
        return other is not null
               && Name == other.Name
               && Email == other.Email;
    }

    // again, intentionally leaving ID property out of hash calc
    // allows two Student records where only ID's differ to have same Hash
    public override int GetHashCode() => HashCode.Combine(Name, Email);
}

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
